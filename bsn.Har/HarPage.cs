using System;

namespace bsn.Har {
	public class HarPage: HarEntity {
		public class HarTimings: HarEntity {
			public int OnContentLoad {
				get;
				set;
			} = -1;

			public int OnLoad {
				get;
				set;
			} = -1;
		}

		public DateTime StartedDateTime {
			get;
			set;
		}

		public string Id {
			get;
			set;
		}

		public string Title {
			get;
			set;
		}

		public HarTimings PageTimings {
			get;
			set;
		} = new HarTimings();
	}
}