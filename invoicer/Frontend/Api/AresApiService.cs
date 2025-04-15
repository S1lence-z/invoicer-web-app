using System.Text.Json;
using Application.AresApiModels;
using Application.ServiceInterfaces;
using Domain.Interfaces;
using Frontend.Models;

namespace Frontend.Api
{
	public class AresApiService : IAresApiService
	{
		private readonly HttpClient _httpClient;
		private readonly string _urlPath = "api/ares";
		private static readonly JsonSerializerOptions jsonSerializeOptions = new() { PropertyNameCaseInsensitive = true, IncludeFields = true };

		public AresApiService(EnvironmentConfig config)
		{
			_httpClient = new HttpClient { BaseAddress = new Uri(config.ApiBaseUrl) };
		}

		public async Task<IResult<IAresApiResponse>> GetEntityInformationByIcoAsync(string ico)
		{
			var response = await _httpClient.GetAsync($"{_urlPath}/{ico}");	
			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				SubjectInformation? data = JsonSerializer.Deserialize<SubjectInformation>(content, jsonSerializeOptions);
				return AresApiResult<SubjectInformation>.Success(data!);
			}
			var errorContent = await response.Content.ReadAsStringAsync();
			AresApiResult<AresApiErrorResponse>? errorData = JsonSerializer.Deserialize<AresApiResult<AresApiErrorResponse>>(errorContent, jsonSerializeOptions);
			return AresApiResult<AresApiErrorResponse>.Failure(errorData?.Data, "Unknown Error", (int)response.StatusCode);
		}
	}
}
