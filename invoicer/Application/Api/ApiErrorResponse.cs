namespace Application.Api
{
	public class ApiErrorResponse
	{
		public string Message { get; set; } = string.Empty;
		public string Details { get; set; } = string.Empty;
		public int StatusCode { get; set; }

		private ApiErrorResponse(string message, string details, int statusCode)
		{
			Message = message;
			Details = details;
			StatusCode = statusCode;
		}

		public static ApiErrorResponse Create(string message, string details, int statusCode)
		{
			return new ApiErrorResponse(message, details, statusCode);
		}
	}
}
