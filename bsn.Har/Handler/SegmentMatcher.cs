using System;
using System.Collections.Immutable;
using System.Globalization;

namespace bsn.Har.Handler {
	public abstract partial class SegmentMatcher: IEquatable<SegmentMatcher> {
		public static implicit operator SegmentMatcher(string literal) {
			return Literal(literal);
		}

		public static SegmentMatcher Any(string key) {
			return new AnyMatcher(key);
		}

		public static SegmentMatcher Pattern(string pattern, params Func<string, SegmentMatcher>[] matcherFactory) {
			return new PatternMatcher(pattern, matcherFactory);
		}

		public static SegmentMatcher Literal(string literal, StringComparison comparison = StringComparison.InvariantCultureIgnoreCase) {
			return new LiteralMatcher(literal, comparison);
		}

		public static SegmentMatcher Parse<T>(string key, TryParse<T> tryParse) {
			return new ParseMatcher<T>(key, tryParse);
		}

		public static SegmentMatcher Number<T>(string key, TryParseNumber<T> tryParse, NumberStyles style = NumberStyles.Any, NumberFormatInfo format = null) {
			return new NumberMatcher<T>(key, tryParse, style, format);
		}

		public static SegmentMatcher Wildcard(string key) {
			return new WildcardMatcher(key);
		}

		public static bool TryGetWildcard(SegmentMatcher matcher, out string key) {
			if (matcher is WildcardMatcher wildcard) {
				key = wildcard.Key;
				return true;
			}
			key = null;
			return false;
		}

		protected internal abstract bool TryMatch(string segment, ref IImmutableDictionary<string, object> arguments);

		public abstract bool Equals(SegmentMatcher other);
	}
}
