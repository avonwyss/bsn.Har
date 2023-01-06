using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace bsn.Har.Multipart {
	public static class MultipartParser {
		private enum ParseState {
			Preamble,
			DelimiterFound,
			ExpectDelimiterClose,
			ExpectDelimiterNewline,
			HeaderLine,
			ExpectHeaderNewline,
			Body
		}

		public static string Unquote(string value) {
			if (value != null && value.Length >= 2 && value[0] == '"' && value[value.Length-1] == '"') {
				return value.Substring(1, value.Length-2).Replace("\"\"", "\"");
			}
			return value;
		}

		private static readonly Dictionary<string, Action<string, HarRequest.HarPostData.HarParam>> headerProcessor = new Dictionary<string, Action<string, HarRequest.HarPostData.HarParam>>(StringComparer.OrdinalIgnoreCase) {
				{
						"Content-Disposition", (header, param) => {
							var headerValue = ContentDispositionHeaderValue.Parse(header);
							if (string.Equals(headerValue.DispositionType, "form-data", StringComparison.OrdinalIgnoreCase)) {
								param.Name = Unquote(headerValue.Name);
								param.FileName = Unquote(headerValue.FileName ?? headerValue.FileNameStar);
							} else {
								throw new InvalidOperationException("Invalid content-disposition: "+header);
							}
						}
				}, {
						"Content-Type", (header, param) => {
							var headerValue = MediaTypeHeaderValue.Parse(header);
							param.ContentType = headerValue.MediaType;
						}
				},
				{ "Content-Transfer-Encoding", (header, param) => { } }
		};

		public static IEnumerable<HarRequest.HarPostData.HarParam> Parse(Stream stream, string boundary) {
			static IEnumerable<byte> StreamBytes(Stream stream) {
				var buffer = new byte[4080];
				int read;
				do {
					read = stream.Read(buffer, 0, buffer.Length);
					for (var i = 0; i < read; i++) {
						yield return buffer[i];
					}
				} while (read > 0);
			}

			static InvalidOperationException ExpectedException(char current, params char[] chars) {
				return new InvalidOperationException("Expected "+string.Join(" | ", chars.Select(ch => char.IsControl(ch) || char.IsWhiteSpace(ch) ? "0x"+((int)ch).ToString("x2").ToUpperInvariant() : ch.ToString()))+" but found "+current);
			}

			var boundaryBytes = Encoding.UTF8.GetBytes("\r\n--"+boundary);
			HarRequest.HarPostData.HarParam param = default;
			var header = new StringBuilder();
			using var body = new MemoryStream();
			var charset = Encoding.ASCII;
			var state = ParseState.Preamble;
			var boundaryIndex = 2;
			foreach (var current in StreamBytes(stream)) {
				switch (state) {
				case ParseState.Preamble:
					if (current == boundaryBytes[boundaryIndex]) {
						if (++boundaryIndex == boundaryBytes.Length) {
							state = ParseState.DelimiterFound;
						}
					} else {
						boundaryIndex = current == boundaryBytes[2] ? 3 : 2;
					}
					break;
				case ParseState.DelimiterFound:
					switch ((char)current) {
					case '-':
						state = ParseState.ExpectDelimiterClose;
						break;
					case '\r':
						state = ParseState.ExpectDelimiterNewline;
						break;
					default:
						throw ExpectedException('-', '\r');
					}
					break;
				case ParseState.ExpectDelimiterClose:
					if (current == '-') {
						// end reached, the epilogue is to be ignored
						yield break;
					}
					throw ExpectedException('-');
				case ParseState.ExpectDelimiterNewline:
					if (current == '\n') {
						state = ParseState.HeaderLine;
						header.Clear();
						param = new HarRequest.HarPostData.HarParam();
					} else {
						throw ExpectedException('\n');
					}
					break;
				case ParseState.HeaderLine:
					if (current == '\r') {
						state = ParseState.ExpectHeaderNewline;
					} else {
						header.Append((char)current);
					}
					break;
				case ParseState.ExpectHeaderNewline:
					if (current == '\n') {
						var headerLine = header.ToString().Split(new[] { ':' }, 2);
						if (headerLine.Length == 1 && string.IsNullOrWhiteSpace(headerLine[0])) {
							boundaryIndex = 0;
							body.SetLength(0);
							state = ParseState.Body;
						} else if (headerLine.Length == 2) {
							if (!headerProcessor.TryGetValue(headerLine[0].Trim(), out var processor)) {
								throw new InvalidOperationException("Header not allowed: "+headerLine);
							}
							processor(headerLine[1].Trim(), param);
							state = ParseState.HeaderLine;
							header.Clear();
						} else {
							throw new InvalidOperationException("Header format error: "+headerLine);
						}
					} else {
						throw ExpectedException('\n');
					}
					break;
				case ParseState.Body:
					body.WriteByte(current);
					if (current == boundaryBytes[boundaryIndex]) {
						if (++boundaryIndex == boundaryBytes.Length) {
							var bodyLength = (int)(body.Length-boundaryBytes.Length);
							if (HarExtensions.IsTextMimeType(param.ContentType ?? "text/plain")) {
								param.Value = charset.GetString(body.GetBuffer(), 0, bodyLength);
								if (string.Equals(param.Name, "_charset_", StringComparison.OrdinalIgnoreCase)) {
									charset = Encoding.GetEncoding(param.Value);
								}
							} else {
								param.Encoding = "base64";
								param.Value = Convert.ToBase64String(body.GetBuffer(), 0, bodyLength, Base64FormattingOptions.None);
							}
							yield return param;
							param = null;
							state = ParseState.DelimiterFound;
						}
					} else {
						boundaryIndex = current == boundaryBytes[0] ? 1 : 0;
					}
					break;
				default:
					throw new InvalidOperationException("Internal error (invalid state).");
				}
			}
			throw new InvalidOperationException("Unexpected end of data");
		}
	}
}
