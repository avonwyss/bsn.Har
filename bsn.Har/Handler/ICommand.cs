namespace bsn.Har.Handler {
	public interface ICommand<out TResponse> {
		TResponse Execute();
	}
}