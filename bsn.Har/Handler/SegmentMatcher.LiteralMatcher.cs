using System;
using System.Collections.Immutable;

namespace bsn.Har.Handler {
	public abstract partial class SegmentMatcher {
		private sealed class LiteralMatcher: SegmentMatcher {
			private readonly string literal;
			private readonly StringComparison comparison;

			public LiteralMatcher(string literal, StringComparison comparison) {
				this.literal = literal;
				this.comparison = comparison;
			}

			protected internal override bool TryMatch(string segment, ref IImmutableDictionary<string, object> arguments) {
				return string.Equals(this.literal, segment, this.comparison);
			}

			public override bool Equals(SegmentMatcher other) {
				return other is LiteralMatcher matcher && matcher.comparison == this.comparison && string.Equals(matcher.literal, this.literal, this.comparison);
			}
		}
	}
}
