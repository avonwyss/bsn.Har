using System;
using System.Collections.Immutable;

namespace bsn.Har.Handler {
	public abstract partial class SegmentMatcher {
		private sealed class AnyMatcher: SegmentMatcher {
			private readonly string key;

			public AnyMatcher(string key) {
				this.key = key;
			}

			protected internal override bool TryMatch(string segment, ref IImmutableDictionary<string, object> arguments) {
				if (!string.IsNullOrEmpty(this.key)) {
					arguments = arguments.Add(this.key, segment);
				}
				return true;
			}

			public override bool Equals(SegmentMatcher other) {
				return other is AnyMatcher matcher && matcher.key == this.key;
			}
		}
	}
}
