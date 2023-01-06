using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace bsn.Har.Handler {
	public abstract partial class SegmentMatcher {
		private sealed class PatternMatcher: SegmentMatcher {
			private static readonly Regex rxPattern = new Regex(@"\G(([^{}]+|\{\{|\}\})+|\{(?<key>\w+)(:(?<pattern>([^\\{}[]|\.|\{[^\}]*\}|\[([^\]\\]|\\.)*\])))?\})", RegexOptions.CultureInvariant|RegexOptions.Compiled|RegexOptions.Compiled|RegexOptions.ExplicitCapture|RegexOptions.Singleline);
			
			private readonly Regex pattern;
			private readonly KeyValuePair<string, SegmentMatcher>[] matchers;

			public PatternMatcher(string pattern, params Func<string, SegmentMatcher>[] matcherFactory) {
				var builder = new StringBuilder(pattern.Length*2);
				var matchers = new Dictionary<string, SegmentMatcher>(StringComparer.Ordinal);
				builder.Append('^');
				using (var enumerator = ((IEnumerable<Func<string, SegmentMatcher>>)matcherFactory).GetEnumerator()) {
					for (Match match = rxPattern.Match(pattern); match.Success; match = match.NextMatch()) {
						var keyGroup = match.Groups["key"];
						if (keyGroup.Success) {
							var patternGroup = match.Groups["pattern"];
							builder.Append("(?<").Append(keyGroup.Value).Append('>').Append(patternGroup.Success ? patternGroup.Value : ".*?").Append(')');
							matchers.Add(keyGroup.Value, enumerator.MoveNext() && enumerator.Current != null ? enumerator.Current(keyGroup.Value) : new AnyMatcher(keyGroup.Value));
						} else {
							builder.Append(Regex.Escape(match.Value));
						}
					}
				}
				builder.Append('$');
				this.pattern = new Regex(builder.ToString(), RegexOptions.Compiled|RegexOptions.CultureInvariant|RegexOptions.ExplicitCapture);
				this.matchers = matchers.ToArray();
			}

			protected internal override bool TryMatch(string segment, ref IImmutableDictionary<string, object> arguments) {
				var match = pattern.Match(segment);
				if (match.Success) {
					var newArguments = arguments;
					foreach (var matcher in matchers) {
						var group = match.Groups[matcher.Key];
						if (!group.Success || !matcher.Value.TryMatch(group.Value, ref newArguments)) {
							return false;
						}
					}
					arguments = newArguments;
					return true;
				}
				return false;
			}

			public override bool Equals(SegmentMatcher other) {
				return other is PatternMatcher matcher && string.Equals(matcher.pattern.ToString(), pattern.ToString(), StringComparison.Ordinal);
			}
		}
	}
}
