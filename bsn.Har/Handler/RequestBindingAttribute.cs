using System;

namespace bsn.Har.Handler {
	[AttributeUsage(AttributeTargets.Property|AttributeTargets.Field)]
	public class RequestBindingAttribute: Attribute {
		public RequestBindingAttribute(BindingSource source) {
			this.Source = source;
		}

		public RequestBindingAttribute(BindingSource source, string name): this(source) {
			this.Name = name;
		}

		public BindingSource Source {
			get;
		}

		public string Name {
			get;
			set;
		}

		public bool Required {
			get;
			set;
		}

		public int Priority {
			get;
			set;
		}
	}
}