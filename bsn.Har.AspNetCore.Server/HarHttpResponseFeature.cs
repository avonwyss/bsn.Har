using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace bsn.Har.AspNetCore.Server {
	public class HarHttpResponseFeature: IHttpResponseFeature {
		private readonly List<(bool OnCompleted, Func<object, Task> Callback, object State)> callbacks = new(0);
		private readonly MemoryStream body = new();
		private bool hasStarted;
		private int statusCode;
		private string reasonPhrase;
		private IHeaderDictionary headers;

		public HarHttpResponseFeature() {
			this.Body = this.body;
			this.Headers = new HeaderDictionary();
			this.StatusCode = 200;
		}

		public Stream Body {
			get;
			set;
		}

		public bool HasStarted => this.hasStarted;

		public void OnStarting(Func<object, Task> callback, object state) {
			this.callbacks.Add((false, callback, state));
		}

		public void OnCompleted(Func<object, Task> callback, object state) {
			this.callbacks.Add((true, callback, state));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void AssertNotStarted() {
			if (this.hasStarted) {
				throw new InvalidOperationException("Response has already been started");
			}
		}

		public int StatusCode {
			get => this.statusCode;
			set {
				this.AssertNotStarted();
				this.statusCode = value;
			}
		}

		public string ReasonPhrase {
			get => this.reasonPhrase;
			set {
				this.AssertNotStarted();
				this.reasonPhrase = value ?? throw new ArgumentNullException(nameof(value));;
			}
		}

		public IHeaderDictionary Headers {
			get => this.headers;
			set {
				this.AssertNotStarted();
				this.headers = value ?? throw new ArgumentNullException(nameof(value));
			}
		}

		private async ValueTask InvokeCallbacks(bool complete) {
			foreach (var (onCompleted, callback, state) in this.callbacks) {
				if (onCompleted == complete) {
					await callback(state).ConfigureAwait(false);
				}
			}
		}

		public async ValueTask<HarResponse> ToHarResponse() {
			this.AssertNotStarted();
			this.hasStarted = true;
			await this.InvokeCallbacks(false).ConfigureAwait(false);
			var response = new HarResponse() {
					Status = (HttpStatusCode)this.StatusCode
			};
			foreach (var header in this.Headers) {
				response.Headers.Add(new HarNameValue() {
						Name = header.Key,
						Value = header.Value
				});
			}
			if (this.body.Length > 0) {
				this.body.Seek(0, SeekOrigin.Begin);
				response.Content = this.body;
			}
			await this.InvokeCallbacks(true).ConfigureAwait(false);
			return response;
		}
	}
}
