using Microsoft.AspNetCore.Http.Features;

namespace bsn.Har.AspNetCore.Server {
	internal class HttpBodyControlFeature: IHttpBodyControlFeature {
		public HttpBodyControlFeature(bool allowSynchronousIO) {
			this.AllowSynchronousIO = allowSynchronousIO;
		}

		public bool AllowSynchronousIO {
			get;
			set;
		}
	}
}
