using System;

namespace bsn.Har.Handler {
	[Flags]
	public enum BindingSource {
		None=0,
		Path=1,
		Header=2,
		Query=4,
		Content=8,
		NonContent=Path|Header|Query,
		Any=NonContent|Content
	}
}