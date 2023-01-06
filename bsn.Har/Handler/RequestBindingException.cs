using System;
using System.Net.Http.Headers;
using System.Runtime.Serialization;

namespace bsn.Har.Handler {
	[Serializable]
	public class RequestBindingException: Exception {
		public RequestBindingException(string message): base(message) { }

		public RequestBindingException(string message, Exception innerException): base(message, innerException) { }

		protected RequestBindingException(SerializationInfo info, StreamingContext context): base(info, context) { }
	}
}
