namespace Shared.Interfaces
{
	public interface IResult<out TData>
	{
		TData? Data { get; }
		string? ErrorMessage { get; }
		bool IsSuccess { get; }
		int StatusCode { get; }
	}
}
