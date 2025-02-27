using System.Text.Json;
using Domain.AresApiModels;
using Domain.Interfaces;
using Domain.ServiceInterfaces;
using Frontend.Models;

namespace Frontend.Api
{
	public class AresApiService : IAresApiService
	{
		private readonly HttpClient _httpClient;
		private readonly string _urlPath = "api/ares";
		private static readonly JsonSerializerOptions jsonSerializeOptions = new() { PropertyNameCaseInsensitive = true };

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
				var data = JsonSerializer.Deserialize<SubjectInformation>(content, jsonSerializeOptions);
				return AresApiResult<SubjectInformation>.Success(data!);
			}
			var errorContent = await response.Content.ReadAsStringAsync();
			var errorData = JsonSerializer.Deserialize<AresApiErrorResponse>(errorContent, jsonSerializeOptions);
			return AresApiResult<AresApiErrorResponse>.Failure(errorData, errorData?.Popis ?? "Unknown Error", (int)response.StatusCode);
		}
	}
}
