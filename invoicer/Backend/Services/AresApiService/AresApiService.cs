using System.Net;
using System.Text.Json;
using Backend.Services.AresApiService.Models;
using Backend.Services.AresApiService.Responses;
using Refit;

namespace Backend.Services.AresApiService
{
	public class AresApiService
	{
		private readonly IAresApi _aresApi;
		private const string baseUrl = "https://ares.gov.cz/ekonomicke-subjekty-v-be/rest";
		private static readonly JsonSerializerOptions jsonSerializeOptions = new() { PropertyNameCaseInsensitive = true };

	public AresApiService()
		{
			_aresApi = RestService.For<IAresApi>(baseUrl);
		}

		public async Task<IResult<IAresApiResponse>> GetEntityInformationByIcoAsync(string ico)
		{
			try
			{
				HttpResponseMessage response = await _aresApi.GetEntityInformationByIcoAsync(ico);
				string content = await response.Content.ReadAsStringAsync();
				if (response.IsSuccessStatusCode)
				{
					SubjectInformation? data = JsonSerializer.Deserialize<SubjectInformation>(content, jsonSerializeOptions);
					return Result<SubjectInformation>.Success(data!);
				}
				AresApiErrorResponse? errorData = JsonSerializer.Deserialize<AresApiErrorResponse>(content, jsonSerializeOptions);
				return HandleErrorResponse(response.StatusCode, errorData!);
			}
			catch (Exception ex)
			{
				return Result<IAresApiResponse>.Failure($"Unexpected error: {ex.Message}", 500);
			}
		}

		private static IResult<IAresApiResponse> HandleErrorResponse(HttpStatusCode statusCode, AresApiErrorResponse errorResponse)
		{
			string message = $"{(int)statusCode}: {errorResponse?.Popis ?? "Unknown Error"}";
			return Result<AresApiErrorResponse>.Failure(message, (int)statusCode);
		}
	}
}
