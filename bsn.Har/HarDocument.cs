using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

using bsn.Har.Json;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace bsn.Har {
	public class HarDocument: HarEntity {
		public static JsonSerializer Serializer {
			get;
		} = JsonSerializer.Create(new JsonSerializerSettings {
				DateFormatHandling = DateFormatHandling.IsoDateFormat,
				NullValueHandling = NullValueHandling.Ignore,
				CheckAdditionalContent = false,
				Formatting = Formatting.None,
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Converters = new List<JsonConverter> {
						new CookieConverter(),
						new HttpMethodConverter()
				}
		});

		public HarLog Log {
			get;
			set;
		}

		[JsonIgnore]
		public override string Comment {
			get => null;
			set => throw new InvalidOperationException("Comment not supported on document");
		}
	}
}
