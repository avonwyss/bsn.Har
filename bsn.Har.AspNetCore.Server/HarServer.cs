using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting.Server;
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ValueTask<HarResponse> ProcessAsync(HarRequest request, Action<FeatureCollection> setRequestFeatures = null) {
			return (this.handler ?? throw new InvalidOperationException("Server is not started")).ProcessAsync(request, setRequestFeatures);
		}

		public void Dispose() { }
	}
}
