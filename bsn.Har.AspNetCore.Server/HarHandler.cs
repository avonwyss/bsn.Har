using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;

namespace bsn.Har.AspNetCore.Server {
	internal abstract class HarHandler {
		public abstract ValueTask<HarResponse> Process(HarRequest request);
	}

	internal class HarHandler<TContext>: HarHandler {
		private readonly IHttpApplication<TContext> application;

		public HarHandler(IHttpApplication<TContext> application) {
			this.application = application;
		}

		public override async ValueTask<HarResponse> Process(HarRequest request) {
			var features = new FeatureCollection();
			features.Set<IHttpRequestFeature>(new HarHttpRequestFeature(request));
			var response = new HarHttpResponseFeature();
			features.Set<IHttpResponseFeature>(response);
			var exception = default(Exception);
			var context = this.application.CreateContext(features);
			try {
				await this.application.ProcessRequestAsync(context).ConfigureAwait(false);
			} catch (Exception ex) {
				exception = ex;
			} finally {
				this.application.DisposeContext(context, exception);
			}
			return await response.ToHarResponse().ConfigureAwait(false);
		}
	}
}
