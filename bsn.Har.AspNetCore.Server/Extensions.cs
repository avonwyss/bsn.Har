using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;

namespace bsn.Har.AspNetCore.Server {
	public static class Extensions {
		public static IWebHostBuilder UseHarServer(this IWebHostBuilder hostBuilder, Action<HarServerOptions> options) {
			return hostBuilder.ConfigureServices(services => {
				services.Configure(options);
				services.AddSingleton<IServer, HarServer>();
			});
		}
	}
}
