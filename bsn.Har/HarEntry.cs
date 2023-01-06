using System;
using System.Net;

namespace bsn.Har {
	public class HarEntry: HarEntity {
		public class HarCache: HarEntity {
			public class HarCacheInfo: HarEntity {
				public DateTime? Expires {
					get;
					set;
				}

				public DateTime LastAccess {
					get;
					set;
				}

				public string ETag {
					get;
					set;
				}

				public int HitCount {
					get;
					set;
				}
			}
		}

		public class HarTimings: HarEntity {
			public int Blocked {
				get;
				set;
			} = -1;

			public int Dns {
				get;
				set;
			} = -1;

			public int Connect {
				get;
				set;
			} = -1;

			public int Send {
				get;
				set;
			}

			public int Wait {
				get;
				set;
			}

			public int Receive {
				get;
				set;
			}

			public int? Ssl {
				get;
				set;
			} = -1;
		}

		public string Pageref {
			get;
			set;
		}

		public DateTime StartedDateTime {
			get;
			set;
		}

		public int Time {
			get;
			set;
		}

		public HarRequest Request {
			get;
			set;
		}

		public HarResponse Response {
			get;
			set;
		}

		public HarCache Cache {
			get;
			set;
		}

		public HarTimings Timings {
			get;
			set;
		}

		public IPAddress ServerIPAddress {
			get;
			set;
		}

		public string Connection {
			get;
			set;
		}
	}
}