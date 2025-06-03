namespace Application.Interfaces
{
	public interface IApiResult<out TData>
	{
		TData? Data { get; }
		string? ErrorMessage { get; }
		bool IsSuccess { get; }
		int StatusCode { get; }
	}
}
