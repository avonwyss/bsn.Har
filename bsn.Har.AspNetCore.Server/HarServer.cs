using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;

namespace bsn.Har.AspNetCore.Server {
	public class HarServer: IServer {
		private readonly HarServerOptions options;
		private HarHandler handler;

		public HarServer(IServiceProvider serviceProvider, IOptions<HarServerOptions> options) {
			this.options = options.Value;
		}

		public Task StartAsync<TContext>(IHttpApplication<TContext> application, CancellationToken cancellationToken) {
			this.handler = new HarHandler<TContext>(application);
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken) {
			this.handler = null;
			return Task.CompletedTask;
		}

		public IFeatureCollection Features { get; } = new FeatureCollection();

		public ValueTask<HarResponse> Process(HarRequest request) {
			return (this.handler ?? throw new InvalidOperationException("Server is not started")).Process(request);
		}

		public void Dispose() { }
	}
}
