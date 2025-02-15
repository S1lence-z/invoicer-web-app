using Backend.Services.AresApiService.Responses;

namespace Backend.Services.AresApiService.Models
{
	public interface IResult<out TData> where TData : IAresApiResponse
	{
		TData? Data { get; }
		string? ErrorMessage { get; }
		bool IsSuccess { get; }
		int StatusCode { get; }
	}
}
