using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bsn.Har.Json {
	public class CookieConverter: JsonConverter {
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			if (value is Cookie cookie) {
				writer.WriteStartObject();
				writer.WritePropertyName("name");
				writer.WriteValue(cookie.Name);
				writer.WritePropertyName("value");
				writer.WriteValue(cookie.Value);
				if (!string.IsNullOrEmpty(cookie.Path)) {
					writer.WritePropertyName("path");
					writer.WriteValue(cookie.Path);
				}
				if (!string.IsNullOrEmpty(cookie.Domain)) {
					writer.WritePropertyName("domain");
					writer.WriteValue(cookie.Domain);
				}
				if (cookie.Expires != DateTime.MinValue) {
					writer.WritePropertyName("expires");
					writer.WriteValue(cookie.Expires.ToString("O"));
				}
				writer.WritePropertyName("httpOnly");
				writer.WriteValue(cookie.HttpOnly);
				writer.WritePropertyName("secure");
				writer.WriteValue(cookie.Secure);
				if (!string.IsNullOrEmpty(cookie.Comment)) {
					writer.WritePropertyName("comment");
					writer.WriteValue(cookie.Comment);
				}
				writer.WriteEndObject();
			} else {
				writer.WriteNull();
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
			if (objectType != typeof(Cookie)) {
				throw new NotSupportedException("Only Cookies can be deserialized.");
			}
			switch (reader.TokenType) {
			case JsonToken.Null:
				return null;
			case JsonToken.StartObject:
				var result = new Cookie();
				bool hasToken = reader.Read();
				while (hasToken) {
					switch (reader.TokenType) {
					case JsonToken.EndObject:
						return result;
					case JsonToken.PropertyName:
						switch (reader.Value.ToString().ToLowerInvariant()) {
						case "name":
							result.Name = reader.ReadAsString() ?? "";
							break;
						case "value":
							result.Value = reader.ReadAsString();
							break;
						case "path":
							result.Path = reader.ReadAsString();
							break;
						case "domain":
							result.Domain = reader.ReadAsString();
							break;
						case "expires":
							result.Expires = reader.ReadAsDateTime().GetValueOrDefault(DateTime.MinValue);
							break;
						case "httpOnly":
							result.HttpOnly = reader.ReadAsBoolean().GetValueOrDefault();
							break;
						case "secure":
							result.Secure = reader.ReadAsBoolean().GetValueOrDefault();
							break;
						case "comment":
							result.Comment = reader.ReadAsString();
							break;
						default:
							JToken.ReadFrom(reader);
							hasToken = reader.TokenType != JsonToken.None;
							continue;
						}
						hasToken = reader.Read();
						break;
					default:
						hasToken = false;
						break;
					}
				}
				break;
			}
			throw new InvalidOperationException("Unexpected JSON token: "+reader.TokenType);
		}

		public override bool CanConvert(Type objectType) {
			return objectType == typeof(Cookie);
		}
	}
}
