using System;

namespace bsn.Har.Handler {
	[Flags]
	public enum RequestFlags {
		None=0,
		ParseQueryString = 1,
		ParsePostData = 2,
		Parse = ParseQueryString|ParsePostData
	}
}
