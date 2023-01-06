using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace bsn.Har {
	public class HarResponse: HarEntity {
		public class HarContent: HarEntity {
			public static implicit operator HarContent(string text) {
				return new HarContent() {
						Encoding = null,
						Text = text
				};
			}

			public static implicit operator HarContent(byte[] binary) {
				return new HarContent() {
						Encoding = "base64",
						Text = Convert.ToBase64String(binary, Base64FormattingOptions.None)
				};
			}

			public static implicit operator HarContent(Stream binary) {
				var memoryStream = binary as MemoryStream;
				var ownStream = memoryStream == null;
				if (ownStream) {
					// Create our own buffer stream
					memoryStream = new MemoryStream(binary.CanSeek ? (int)binary.Length : 1024*1024);
				}
				try {
					if (ownStream) {
						binary.CopyTo(memoryStream);
						memoryStream.Seek(0, SeekOrigin.Begin);
					}
					return new HarContent() {
							Encoding = "base64",
							Text = Convert.ToBase64String(memoryStream.GetBuffer(), (int)memoryStream.Position, (int)(memoryStream.Length-memoryStream.Position), Base64FormattingOptions.None)
					};
				} finally {
					if (ownStream) {
						memoryStream.Dispose();
					} else {
						// Since the buffer was directly accessed, pretend we did read the stream
						memoryStream.Seek(0, SeekOrigin.End);
					}
				}
			}

			public int Size {
				get;
				set;
			}

			public int? Compression {
				get;
				set;
			}

			public string MimeType {
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

			public byte[] ToByteArray() {
				return Encoding == "base64"
						? Convert.FromBase64String(Text)
						: System.Text.Encoding.UTF8.GetBytes(Text);
			}
		}

		public HttpStatusCode Status {
			get;
			set;
		}

		public string StatusText {
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

		public HarContent Content {
			get;
			set;
		}

		public Uri RedirectURL {
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
	}
}
