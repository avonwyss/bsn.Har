using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace bsn.Har.Json {
	public class HttpMethodConverter: JsonConverter {
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
			if (value is HttpMethod method) {
				writer.WriteValue(method.Method);
			} else {
				writer.WriteNull();
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
			if (reader.TokenType == JsonToken.String) {
				switch (((string)reader.Value).ToUpperInvariant()) {
				case "GET":
					return HttpMethod.Get;
				case "POST":
					return HttpMethod.Post;
				case "PUT":
					return HttpMethod.Put;
				case "DELETE":
					return HttpMethod.Delete;
				case "HEAD":
					return HttpMethod.Head;
				case "OPTIONS":
					return HttpMethod.Options;
				case "TRACE":
					return HttpMethod.Trace;
				default:
					return new HttpMethod((string)reader.Value);
				}
			}
			throw new InvalidOperationException("Unexpected token "+reader.TokenType);
		}

		public override bool CanConvert(Type objectType) {
			return objectType == typeof(HttpMethod);
		}
	}
}
