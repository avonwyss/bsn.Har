using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

namespace bsn.Har.Handler {
	public class RequestData {
		private readonly IImmutableDictionary<string, object> pathArguments;
		private readonly IImmutableDictionary<string, string> queryArguments;

		public RequestData(HarRequest request, IImmutableDictionary<string, object> pathArguments) {
			this.Request = request;
			this.pathArguments = pathArguments;
			request.ParseQueryString();
			queryArguments = ImmutableDictionary<string, string>.Empty
					.WithComparers(StringComparer.OrdinalIgnoreCase)
					.AddRange(request.QueryString.Select(q => new KeyValuePair<string, string>(q.Name, q.Value)));
		}

		public HarRequest Request {
			get;
		}

		protected virtual T ChangeType<T>(object value) {
			return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
		}

		public bool TryGetFromPath<T>(string key, out T value) {
			if (pathArguments.TryGetValue(key, out var pathValue)) {
				value = pathValue is T typedValue 
						? typedValue 
						: ChangeType<T>(pathValue);
				return true;
			}
			value = default;
			return false;
		}

		public bool TryGetFromQuery<T>(string key, out T value) {
			if (queryArguments.TryGetValue(key, out var queryValue)) {
				value = queryValue is T typedValue 
						? typedValue 
						: ChangeType<T>(queryValue);
				return true;
			}
			value = default;
			return false;
		}

		public bool TryGetFromHeader<T>(string key, out T value) {
			var header = Request.Headers.SingleOrDefault(p => string.Equals(p.Name, key, StringComparison.OrdinalIgnoreCase));
			if (header != null) {
				value = header.Value is T typedValue 
						? typedValue 
						: ChangeType<T>(header.Value);
				return true;
			}
			value = default;
			return false;
		}

		public virtual bool TryGetFromContent<T>(string key, out T value) {
			if (Request.ParsePostData()) {
				var param = Request.PostData.Params.SingleOrDefault(p => string.Equals(p.Name, key, StringComparison.OrdinalIgnoreCase));
				if (param != null) {
					value = param.Value is T typedValue 
							? typedValue 
							: ChangeType<T>(param.Value);
					return true;
				}
			}
			value = default;
			return false;
		}
	}
}