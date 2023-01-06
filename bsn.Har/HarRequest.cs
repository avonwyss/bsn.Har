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
				if (Text == null) {
					throw new InvalidOperationException(Params == null ? "No content available" : "Content has already been parsed");
				}
				return new MemoryStream(Encoding == "base64" 
						? Convert.FromBase64String(Text) 
						: System.Text.Encoding.UTF8.GetBytes(Text), true);
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
			QueryString.Clear();
			foreach (var item in ParseQueryString(Url.Query)) {
				QueryString.Add(new HarNameValue() {
						Name = item.Key,
						Value = item.Value
				});
			}
		}

		public void ApplyQueryString() {
			var builder = new UriBuilder(Url);
			builder.Query = QueryString == null || QueryString.Count == 0
					? ""
					: '?'+String.Join("&", QueryString.Select(nv => nv.Value == null ? Uri.EscapeDataString(nv.Name) : Uri.EscapeDataString(nv.Name)+'='+Uri.EscapeDataString(nv.Value)));
			Url = builder.Uri;
		}

		public bool ParsePostData() {
			if (!string.IsNullOrEmpty(PostData?.Text)) {
				var mimeType = MediaTypeHeaderValue.Parse(PostData.MimeType);
				if (string.Equals(mimeType.MediaType, "multipart/form-data", StringComparison.OrdinalIgnoreCase)) {
					using var stream = PostData.GetContentStream();
					PostData.Params = MultipartParser.Parse(stream, MultipartParser.Unquote(mimeType.Parameters.Single(p => string.Equals(p.Name, "boundary", StringComparison.OrdinalIgnoreCase)).Value)).ToList();
				} else if (string.Equals(mimeType.MediaType, "application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase)) {
					PostData.Params = ParseQueryString(PostData.Text)
							.Select(p => new HarPostData.HarParam() {
								Name = p.Key,
								Value = p.Value
					}).ToList();
				}
			}
			if (PostData?.Params != null) {
				PostData.Text = null;
				PostData.Encoding = null;
				return true;
			}
			return false;
		}

		public void SetBinaryContent(string mimeType, byte[] content) {
			SetBinaryContent(mimeType, content, 0, content?.Length ?? -1);
		}

		public void SetBinaryContent(string mimeType, byte[] content, int offset, int length) {
			if (content == null || length < 0) {
				BodySize = -1;
				PostData = null;
			} else {
				BodySize = length;
				PostData = new HarPostData() {
						MimeType = mimeType,
						Text = Convert.ToBase64String(content, offset, length, Base64FormattingOptions.None),
						Encoding = "base64"
				};
			}
		}
	}
}