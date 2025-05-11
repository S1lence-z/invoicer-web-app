using Shared.Api;

namespace Frontend.Utils
{
	public class ApiException(ApiErrorResponse errorResponse) : Exception(errorResponse.Message)
	{
		public ApiErrorResponse Error { get; } = errorResponse;

		public override string ToString()
		{
			return Error.ToString();
		}
	}
}
