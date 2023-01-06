using System;
using System.Collections.Immutable;
using System.Globalization;

namespace bsn.Har.Handler {
	public abstract partial class SegmentMatcher {
		public delegate bool TryParseNumber<T>(string s, NumberStyles styles, IFormatProvider provider, out T result); 
		
		private sealed class NumberMatcher<T>: SegmentMatcher {
			private readonly string key;
			private readonly TryParseNumber<T> tryParse;
			private readonly NumberStyles styles;
			private readonly IFormatProvider provider;

			public NumberMatcher(string key, TryParseNumber<T> tryParse, NumberStyles styles, IFormatProvider provider) {
				this.key = key;
				this.tryParse = tryParse;
				this.styles = styles;
				this.provider = provider ?? CultureInfo.InvariantCulture;
			}

			protected internal override bool TryMatch(string segment, ref IImmutableDictionary<string, object> arguments) {
				if (tryParse(segment, styles, provider, out T result)) {
					if (!string.IsNullOrEmpty(key)) {
						arguments = arguments.Add(key, result);
					}
					return true;
				}
				return false;
			}

			public override bool Equals(SegmentMatcher other) {
				return other is NumberMatcher<T> matcher && matcher.key == key && matcher.tryParse.Equals(tryParse) && matcher.styles == styles && matcher.provider == provider;
			}
		}
	}
}
