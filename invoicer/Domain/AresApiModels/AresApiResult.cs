using Domain.Interfaces;

namespace Domain.AresApiModels
{
	public class AresApiResult<TData> : IResult<TData> where TData : IAresApiResponse
	{
		public TData? Data { get; }
		public bool IsSuccess { get; }
		public string? ErrorMessage { get; }
		public int StatusCode { get; }

		private AresApiResult(TData? data, bool isSuccess, string? errorMessage, int errorCode)
		{
			Data = data;
			IsSuccess = isSuccess;
			ErrorMessage = errorMessage;
			StatusCode = errorCode;
		}

		public static AresApiResult<TData> Success(TData data) => new(data, true, null, 200);
		public static AresApiResult<TData> Failure(TData? errorData, string errorMessage, int statusCode) => new(errorData, false, errorMessage, statusCode);
	}

}
