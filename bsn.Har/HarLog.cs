using System.Collections.Generic;

namespace bsn.Har {
	public class HarLog: HarEntity {
		public string Version {
			get;
			set;
		} = "1.2";

		public HarNameVersion Creator {
			get;
			set;
		}

		public HarNameVersion Browser {
			get;
			set;
		}

		public List<HarPage> Pages {
			get;
			set;
		} = new List<HarPage>();

		public List<HarEntry> Entries {
			get;
			set;
		} = new List<HarEntry>();
	}
}