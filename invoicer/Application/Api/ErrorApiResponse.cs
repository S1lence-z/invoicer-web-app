namespace Application.Api
{
	public class ErrorApiResponse
	{
		public string Message { get; set; } = string.Empty;
		public string Details { get; set; } = string.Empty;
		public int StatusCode { get; set; }

		private ErrorApiResponse(string message, string details, int statusCode)
		{
			Message = message;
			Details = details;
			StatusCode = statusCode;
		}

		public static ErrorApiResponse Create(string message, string details, int statusCode)
		{
			return new ErrorApiResponse(message, details, statusCode);
		}
	}
}
