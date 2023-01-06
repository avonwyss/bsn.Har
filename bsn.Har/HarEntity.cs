using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bsn.Har {
	public abstract class HarEntity {
		public virtual string Comment {
			get;
			set;
		}

		[JsonExtensionData]
		public IDictionary<string, JToken> AdditionalData;

		public override string ToString() {
			using (var writer = new StringWriter()) {
				HarDocument.Serializer.Serialize(writer, this);
				return writer.ToString();
			}
		}
	}
}