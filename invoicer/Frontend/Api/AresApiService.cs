using System.Text.Json;
using Application.Interfaces;
using Application.ExternalServiceInterfaces;
using Application.AresApi;

namespace Frontend.Api
{
	public class AresApiService(HttpClient httpClient) : IAresApiService
	{
		private readonly string _urlPath = "api/ares";
		private static readonly JsonSerializerOptions jsonSerializeOptions = new() { PropertyNameCaseInsensitive = true, IncludeFields = true };

		public async Task<IApiResult<IAresApiResponse>> GetEntityInformationByIcoAsync(string ico)
		{
			var response = await httpClient.GetAsync($"{_urlPath}/{ico}");	
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
