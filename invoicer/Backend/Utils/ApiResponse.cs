using Domain.Interfaces;

namespace Backend.Utils
{
	/// <summary>
	/// Represents a standardized API response.
	/// </summary>
	/// <typeparam name="T">The type of the data payload.</typeparam>
	public class ApiResponse<T> : IResult<T>
	{
		/// <summary>
		/// Indicates whether the operation was successful (typically true for 2xx status codes).
		/// </summary>
		public bool IsSuccess { get; private set; }

		/// <summary>
		/// The data payload for successful responses (or potentially structured error details).
		/// Will be default(T) for simple failure responses created by methods like NotFound/BadRequest.
		/// </summary>
		public T? Data { get; private set; }

		/// <summary>
		/// The primary error message for failure responses.
		/// Will be null for successful responses.
		/// </summary>
		public string? ErrorMessage { get; private set; }

		/// <summary>
		/// The HTTP status code associated with this response.
		/// </summary>
		public int StatusCode { get; private set; }

		// Private constructor remains the same
		private ApiResponse(bool isSuccess, T? data, string? errorMessage, int statusCode)
		{
			IsSuccess = isSuccess;
			Data = data;
			ErrorMessage = errorMessage;
			StatusCode = statusCode;
		}

		/// <summary>
		/// Creates a success response with a 200 OK status.
		/// </summary>
		/// <param name="data">The data payload.</param>
		/// <returns>An ApiResponse indicating success (200 OK).</returns>
		public static ApiResponse<T> Ok(T data)
		{
			return new ApiResponse<T>(true, data, null, StatusCodes.Status200OK);
		}

		/// <summary>
		/// Creates a success response with a 201 Created status.
		/// </summary>
		/// <param name="data">The data payload (typically the created resource).</param>
		/// <returns>An ApiResponse indicating success (201 Created).</returns>
		public static ApiResponse<T> Created(T data)
		{
			return new ApiResponse<T>(true, data, null, StatusCodes.Status201Created);
		}

		/// <summary>
		/// Creates a failure response with a 404 Not Found status.
		/// </summary>
		/// <param name="errorMessage">The error message.</param>
		/// <returns>An ApiResponse indicating failure (404 Not Found).</returns>
		public static ApiResponse<T> NotFound(string errorMessage)
		{
			// For failure, Data should be the default value for type T
			return new ApiResponse<T>(false, default, errorMessage, StatusCodes.Status404NotFound);
		}

		/// <summary>
		/// Creates a failure response with a 400 Bad Request status.
		/// </summary>
		/// <param name="errorMessage">The error message.</param>
		/// <returns>An ApiResponse indicating failure (400 Bad Request).</returns>
		public static ApiResponse<T> BadRequest(string errorMessage)
		{
			// For failure, Data should be the default value for type T
			return new ApiResponse<T>(false, default, errorMessage, StatusCodes.Status400BadRequest);
		}

		/// <summary>
		/// Creates a failure response with a 500 Internal Server Error status.
		/// </summary>
		/// <param name="errorMessage">The error message.</param>
		/// <returns>An ApiResponse indicating failure (500 Internal Server Error).</returns>
		public static ApiResponse<T> InternalServerError(string errorMessage)
		{
			// For failure, Data should be the default value for type T
			return new ApiResponse<T>(false, default, errorMessage, StatusCodes.Status500InternalServerError);
		}
	}
}
