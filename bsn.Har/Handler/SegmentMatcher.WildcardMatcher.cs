using System;
using System.Collections.Immutable;

namespace bsn.Har.Handler {
	public abstract partial class SegmentMatcher {
		private sealed class WildcardMatcher: SegmentMatcher {
			public WildcardMatcher(string key) {
				this.Key = key;
			}

			public string Key {
				get;
			}

			protected internal override bool TryMatch(string segment, ref IImmutableDictionary<string, object> arguments) {
				throw new NotSupportedException("Wildcard cannot match segments");
			}

			public override bool Equals(SegmentMatcher other) {
				throw new NotSupportedException("Wildcard cannot match segments");
			}
		}
	}
}
