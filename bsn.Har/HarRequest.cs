using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.RegularExpressions;

using bsn.Har.Multipart;

namespace bsn.Har {
	public class HarRequest: HarEntity {
		private static readonly Regex rxQuery = new Regex("(?:[?&]|^)(?<name>[^&=]+)(=(?<value>[^&]*))?", RegexOptions.Compiled|RegexOptions.CultureInvariant|RegexOptions.ExplicitCapture);

		private static IEnumerable<KeyValuePair<string, string>> ParseQueryString(string query) {
			for (var match = rxQuery.Match(query); match.Success; match = match.NextMatch()) {
				yield return new KeyValuePair<string, string>(Uri.UnescapeDataString(match.Groups["name"].Value), match.Groups["value"].Success ? Uri.UnescapeDataString(match.Groups["value"].Value) : null);
			}
		}

		public class HarPostData: HarEntity {
			public class HarParam: HarNameValue {
				public string FileName {
					get;
					set;
				}

				public string ContentType {
					get;
					set;
				}

				public string Encoding {
					get;
					set;
				}
			}

			public string MimeType {
				get;
				set;
			}

			public List<HarParam> Params {
				get;
				set;
			}

			public string Text {
				get;
				set;
			}

			public string Encoding {
				get;
				set;
			}

			public Stream GetContentStream() {
				if (this.Text == null) {
					throw new InvalidOperationException(this.Params == null ? "No content available" : "Content has already been parsed");
				}
				return new MemoryStream(this.Encoding == "base64" 
						? Convert.FromBase64String(this.Text) 
						: System.Text.Encoding.UTF8.GetBytes(this.Text), true);
			}
		}

		public HttpMethod Method {
			get;
			set;
		}

		public Uri Url {
			get;
			set;
		}

		public string HttpVersion {
			get;
			set;
		}

		public IList<Cookie> Cookies {
			get;
			set;
		} = new List<Cookie>();

		public HarHeaderCollection Headers {
			get;
			set;
		} = new HarHeaderCollection();

		public IList<HarNameValue> QueryString {
			get;
			set;
		} = new List<HarNameValue>();

		public HarPostData PostData {
			get;
			set;
		}

		public int HeadersSize {
			get;
			set;
		} = -1;

		public int BodySize {
			get;
			set;
		} = -1;

		public void ParseQueryString() {
			this.QueryString.Clear();
			foreach (var item in ParseQueryString(this.Url.Query)) {
				this.QueryString.Add(new HarNameValue() {
						Name = item.Key,
						Value = item.Value
				});
			}
		}

		public void ApplyQueryString() {
			var builder = new UriBuilder(this.Url);
			builder.Query = this.QueryString == null || this.QueryString.Count == 0
					? ""
					: '?'+string.Join("&", this.QueryString.Select(nv => nv.Value == null ? Uri.EscapeDataString(nv.Name) : Uri.EscapeDataString(nv.Name)+'='+Uri.EscapeDataString(nv.Value)));
			this.Url = builder.Uri;
		}

		public bool ParsePostData() {
			if (!string.IsNullOrEmpty(this.PostData?.Text)) {
				var mimeType = MediaTypeHeaderValue.Parse(this.PostData.MimeType);
				if (string.Equals(mimeType.MediaType, "multipart/form-data", StringComparison.OrdinalIgnoreCase)) {
					using var stream = this.PostData.GetContentStream();
					this.PostData.Params = MultipartParser.Parse(stream, MultipartParser.Unquote(mimeType.Parameters.Single(p => string.Equals(p.Name, "boundary", StringComparison.OrdinalIgnoreCase)).Value)).ToList();
				} else if (string.Equals(mimeType.MediaType, "application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase)) {
					this.PostData.Params = ParseQueryString(this.PostData.Text)
							.Select(p => new HarPostData.HarParam() {
								Name = p.Key,
								Value = p.Value
					}).ToList();
				}
			}
			if (this.PostData?.Params != null) {
				this.PostData.Text = null;
				this.PostData.Encoding = null;
				return true;
			}
			return false;
		}

		public void SetBinaryContent(string mimeType, byte[] content) {
			this.SetBinaryContent(mimeType, content, 0, content?.Length ?? -1);
		}

		public void SetBinaryContent(string mimeType, byte[] content, int offset, int length) {
			if (content == null || length < 0) {
				this.BodySize = -1;
				this.PostData = null;
			} else {
				this.BodySize = length;
				this.PostData = new HarPostData() {
						MimeType = mimeType,
						Text = Convert.ToBase64String(content, offset, length, Base64FormattingOptions.None),
						Encoding = "base64"
				};
			}
		}
	}
}
