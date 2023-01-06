using System;
using System.Collections.Immutable;

namespace bsn.Har.Handler {
	public static class Command {
		public static Func<HarRequest, IImmutableDictionary<string, object>, TState, HarResponse> Handler<TCommand, TResponse, TState>(Func<HarRequest, IImmutableDictionary<string, object>, TState, TCommand> createCommand, Func<TResponse, HarResponse> createResponse)
				where TCommand: ICommand<TResponse> {
			return (request, pathArguments, state) => {
				var command = createCommand(request, pathArguments, state);
				var result = command.Execute();
				return createResponse(result);
			};
		}
	}
}
