using Backend.Services.AresApiService.Responses;

namespace Backend.Services.AresApiService.Models
{
	public class Result<TData> : IResult<TData> where TData : IAresApiResponse
	{
		public TData? Data { get; }
		public bool IsSuccess { get; }
		public string? ErrorMessage { get; }
		public int StatusCode { get; }

		private Result(TData? data, bool isSuccess, string? errorMessage, int errorCode)
		{
			Data = data;
			IsSuccess = isSuccess;
			ErrorMessage = errorMessage;
			StatusCode = errorCode;
		}

		public static Result<TData> Success(TData data) => new(data, true, null, 200);
		public static Result<TData> Failure(string errorMessage, int statusCode) => new(default, false, errorMessage, statusCode);
	}

}
