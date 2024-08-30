using System;
using System.IO;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace bsn.Har.AspNetCore.Server {
	public class HarHttpRequestFeature: IHttpRequestFeature {
		public HarHttpRequestFeature(HarRequest request) {
			this.Method = request.Method.Method;
			this.Protocol = request.HttpVersion;
			this.Path = request.Url.AbsolutePath;
			this.QueryString = request.Url.Query;
			this.RawTarget = request.Url.OriginalString;
			this.Scheme = request.Url.Scheme;
			this.Body = string.IsNullOrEmpty(request.PostData?.Text) ? Stream.Null : request.PostData.GetContentStream();
			this.headers = new HeaderDictionary(request.Headers.Count);
			foreach (var header in request.Headers) {
				this.Headers.Add(header.Name, header.Value);
			}
		}

		private IHeaderDictionary headers;

		public string Protocol {
			get;
			set;
		}

		public string Scheme {
			get;
			set;
		}

		public string Method {
			get;
			set;
		}

		public string PathBase {
			get;
			set;
		}

		public string Path {
			get;
			set;
		}

		public string QueryString {
			get;
			set;
		}

		public string RawTarget {
			get;
			set;
		}

		public IHeaderDictionary Headers {
			get => this.headers;
			set => this.headers = value ?? throw new ArgumentNullException(nameof(value));
		}

		public Stream Body {
			get;
			set;
		}
	}
}
