using System.Net.Http.Json;
using Frontend.Utils;
using Shared.Api;
using Shared.DTOs;
using Application.ServiceInterfaces;

namespace Frontend.Api
{
	public class NumberingSchemeService(HttpClient httpClient) : INumberingSchemeService
	{
		private readonly string _urlPath = "api/NumberingScheme";

		public async Task<NumberingSchemeDto> CreateAsync(NumberingSchemeDto obj)
		{
			var response = await httpClient.PostAsJsonAsync(_urlPath, obj);
			if (response.IsSuccessStatusCode)
				return await response.Content.ReadFromJsonAsync<NumberingSchemeDto>() ?? throw new Exception("Failed to deserialize NumberingSchemeDto");
			else
			{
				var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
				if (errorResponse is not null)
					throw new ApiException(errorResponse);
				else
					throw new Exception($"Failed to create Invoice Numbering Scheme: {response.ReasonPhrase}");
			}
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var response = await httpClient.DeleteAsync($"{_urlPath}/{id}");
			if (response.IsSuccessStatusCode)
				return true;
			else
			{
				var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
				if (errorResponse is not null)
					throw new ApiException(errorResponse);
				else
					throw new Exception($"Failed to delete Invoice Numbering Scheme with id {id}: {response.ReasonPhrase}");
			}
		}

		public async Task<IList<NumberingSchemeDto>> GetAllAsync()
		{
			var response = await httpClient.GetAsync(_urlPath);
			if (response.IsSuccessStatusCode)
				return await response.Content.ReadFromJsonAsync<IList<NumberingSchemeDto>>() ?? [];
			else
			{
				var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
				if (errorResponse is not null)
					throw new ApiException(errorResponse);
				else
					throw new Exception($"Failed to retrieve Invoice Numbering Schemes: {response.ReasonPhrase}");
			}
		}

		public async Task<NumberingSchemeDto> GetByIdAsync(int id)
		{
			var response = await httpClient.GetAsync($"{_urlPath}/{id}");
			if (response.IsSuccessStatusCode)
				return await response.Content.ReadFromJsonAsync<NumberingSchemeDto>() ?? throw new Exception("Failed to deserialize NumberingSchemeDto");
			else
			{
				var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
				if (errorResponse is not null)
					throw new ApiException(errorResponse);
				else
					throw new Exception($"Failed to retrieve Invoice Numbering Scheme with id {id}: {response.ReasonPhrase}");
			}
		}

		public async Task<NumberingSchemeDto> GetDefaultNumberingSchemeAsync()
		{
			var response = await httpClient.GetAsync($"{_urlPath}/default");
			if (response.IsSuccessStatusCode)
				return await response.Content.ReadFromJsonAsync<NumberingSchemeDto>() ?? throw new Exception("Failed to deserialize NumberingSchemeDto");
			else
			{
				var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
				if (errorResponse is not null)
					throw new ApiException(errorResponse);
				else
					throw new Exception($"Failed to retrieve Default Invoice Numbering Scheme: {response.ReasonPhrase}");
			}
		}

		public async Task<NumberingSchemeDto> UpdateAsync(int id, NumberingSchemeDto obj)
		{
			var response = await httpClient.PutAsJsonAsync($"{_urlPath}/{id}", obj);
			if (response.IsSuccessStatusCode)
				return await response.Content.ReadFromJsonAsync<NumberingSchemeDto>() ?? throw new Exception("Failed to deserialize NumberingSchemeDto");
			else
			{
				var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
				if (errorResponse is not null)
					throw new ApiException(errorResponse);
				else
					throw new Exception($"Failed to update Invoice Numbering Scheme with id {id}: {response.ReasonPhrase}");
			}
		}
	}
}
