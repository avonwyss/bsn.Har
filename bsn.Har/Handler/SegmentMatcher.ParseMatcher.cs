using System.Collections.Immutable;

namespace bsn.Har.Handler {
	public abstract partial class SegmentMatcher {
		public delegate bool TryParse<T>(string s, out T result);

		private sealed class ParseMatcher<T>: SegmentMatcher {
			private readonly string key;
			private readonly TryParse<T> tryParse;

			public ParseMatcher(string key, TryParse<T> tryParse) {
				this.key = key;
				this.tryParse = tryParse;
			}

			protected internal override bool TryMatch(string segment, ref IImmutableDictionary<string, object> arguments) {
				if (this.tryParse(segment, out var result)) {
					if (!string.IsNullOrEmpty(this.key)) {
						arguments = arguments.Add(this.key, result);
					}
					return true;
				}
				return false;
			}

			public override bool Equals(SegmentMatcher other) {
				return other is ParseMatcher<T> matcher && matcher.key == this.key && matcher.tryParse.Equals(this.tryParse);
			}
		}
	}
}
