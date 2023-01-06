using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace bsn.Har.Handler {
	public sealed class SegmentDispatcher<TState> {
		private readonly RequestDispatcher<TState> requestDispatcher;
		private readonly List<(SegmentMatcher matcher, SegmentDispatcher<TState> handler)> matchers = new List<(SegmentMatcher matcher, SegmentDispatcher<TState> handler)>();
		private readonly List<(string method, Func<HarRequest, IImmutableDictionary<string, object>, TState, HarResponse> handler, string wildcard)> methods = new List<(string method, Func<HarRequest, IImmutableDictionary<string, object>, TState, HarResponse> handler, string wildcard)>();

		public SegmentDispatcher(RequestDispatcher<TState> requestDispatcher) {
			this.requestDispatcher = requestDispatcher;
		}

		internal bool TryHandle(HarRequest request, IImmutableStack<string> segments, IImmutableDictionary<string, object> pathArguments, TState state, out HarResponse response, ICollection<string> allowedMethods) {
			if (!segments.IsEmpty) {
				foreach (var matcher in matchers) {
					var newPathArguments = pathArguments;
					if (matcher.matcher.TryMatch(segments.Peek(), ref newPathArguments) && matcher.handler.TryHandle(request, segments.Pop(), newPathArguments, state, out response, allowedMethods)) {
						return true;
					}
				}
			}
			foreach (var method in methods) {
				var isWildcard = string.IsNullOrEmpty(method.wildcard);
				if (segments.IsEmpty || !isWildcard) {
					if (string.Equals(method.method, request.Method.Method, StringComparison.OrdinalIgnoreCase)) {
						if (!isWildcard) {
							pathArguments = pathArguments.Add(method.wildcard, string.Join("/", segments.Select(Uri.EscapeDataString)));
						}
						response = method.handler(request, pathArguments, state);
						return true;
					}
					allowedMethods.Add(method.method);
				}
			}
			response = null;
			return false;
		}

		public SegmentDispatcher<TState> Segment(SegmentMatcher matcher) {
			foreach (var tuple in matchers) {
				if (tuple.matcher.Equals(matcher)) {
					return tuple.handler;
				}
			}
			var handler = new SegmentDispatcher<TState>(requestDispatcher);
			matchers.Add((matcher, handler));
			return handler;
		}

		public void Add(HttpMethod method, Func<HarRequest, IImmutableDictionary<string, object>, TState, HarResponse> handler, string wildcard) {
			methods.Add((method.Method, handler, wildcard));
		}
	}

	public class RequestDispatcher<TState> {
		private const HttpStatusCode HttpStatusCode_UnprocessableEntity = (HttpStatusCode)422;
		private static Regex rxCamelToSpace = new Regex(@"(?<=\p{Ll})(?=\p{Lu})", RegexOptions.Compiled|RegexOptions.ExplicitCapture|RegexOptions.CultureInvariant|RegexOptions.ExplicitCapture);

		private readonly SegmentDispatcher<TState> root;

		public RequestDispatcher() {
			root = new SegmentDispatcher<TState>(this);
		}

		public void RegisterRoute(Func<HarRequest, IImmutableDictionary<string, object>, TState, HarResponse> handler, HttpMethod method, params SegmentMatcher[] route) {
			if (method == null) {
				throw new ArgumentNullException(nameof(method));
			}
			if (handler == null) {
				throw new ArgumentNullException(nameof(handler));
			}
			var dispatcher = root;
			string wildcard = null;
			foreach (var matcher in route) {
				if (wildcard != null) {
					throw new ArgumentException("Wildcard may only appear once as last segment");
				}
				if (!SegmentMatcher.TryGetWildcard(matcher, out wildcard)) {
					dispatcher = dispatcher.Segment(matcher);
				}
			}
			dispatcher.Add(method, handler, wildcard);
		}

		private static HarResponse GenericResponse(HttpStatusCode status, string additionalHtml = "") {
			var statusText = status == HttpStatusCode_UnprocessableEntity ? "Unprocessable Entity" : rxCamelToSpace.Replace(status.ToString(), " ");
			return new HarResponse() {
					Status = status,
					StatusText = statusText,
					Content = new HarResponse.HarContent() {
							MimeType = "text/html",
							Text = $@"<html>
	<head>
		<title>{statusText}</title>
	</head>
	<body>
		<h1>{statusText}</h1>
		{additionalHtml}
	</body>
</html>"
					}
			};
		}

		protected internal virtual HarResponse NotFound(HarRequest request) {
			return GenericResponse(HttpStatusCode.NotFound);
		}

		protected internal virtual HarResponse MethodNotAllowed(HarRequest request, IEnumerable<string> allowedMethods) {
			var response = GenericResponse(HttpStatusCode.MethodNotAllowed);
			response.Headers.Add(new HarNameValue() {
					Name = "Allow",
					Value = string.Join(", ", allowedMethods)
			});
			return response;
		}

		protected internal virtual HarResponse Exception(HarRequest request, Exception exception) {
			if (exception is RequestBindingException) {
				return GenericResponse(request.PostData != null ? HttpStatusCode_UnprocessableEntity : HttpStatusCode.Conflict, $"<p>{WebUtility.HtmlEncode(exception.Message)}</p>");
			}
			return GenericResponse(HttpStatusCode.InternalServerError, $"<pre>{WebUtility.HtmlEncode(exception.ToString())}</pre>");
		}

		public HarResponse Dispatch(HarRequest request, TState state) {
			try {
				var segments = ImmutableStack<string>.Empty;
				var urlSegments = request.Url.Segments;
				for (var i = urlSegments.Length-1; i > 0; i--) {
					segments = segments.Push(Uri.UnescapeDataString(urlSegments[i].TrimEnd('/').Replace('+', ' ')));
				}
				var allowedMethods = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				return root.TryHandle(request, segments, ImmutableDictionary<string, object>.Empty, state, out var response, allowedMethods)
						? response
						: allowedMethods.Count == 0
								? NotFound(request)
								: MethodNotAllowed(request, allowedMethods.Select(a => a.ToUpperInvariant()).OrderBy(a => a));
			} catch (Exception ex) {
				return Exception(request, ex);
			}
		}
	}
}
