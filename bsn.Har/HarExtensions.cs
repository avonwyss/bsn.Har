using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace bsn.Har {
	public static class HarExtensions {
		private static readonly Regex rxTextMimeType = new Regex(@"^(text/|application/((.+\+)xml|json|x-www-form-urlencoded)\b)", RegexOptions.ExplicitCapture|RegexOptions.CultureInvariant|RegexOptions.Compiled);

		public static bool IsTextMimeType(string mimeType) {
			return rxTextMimeType.IsMatch(mimeType);
		}
	}
}
