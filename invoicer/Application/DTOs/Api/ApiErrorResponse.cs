namespace Application.DTOs.Api
{
	public class ApiErrorResponse(string message, string details, int statusCode)
	{
		public string Message { get; set; } = message;
		public string Details { get; set; } = details;
		public int StatusCode { get; set; } = statusCode;

		public static ApiErrorResponse Create(string message, string details, int statusCode)
		{
			return new ApiErrorResponse(message, details, statusCode);
		}

		public override string ToString()
		{
			return $"API Error: {Message}, Details: {Details}, StatusCode: {StatusCode}";
		}
	}
}
