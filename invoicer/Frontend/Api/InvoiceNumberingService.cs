using System.Net.Http.Json;
using Application.DTOs;
using Application.ServiceInterfaces;
using Frontend.Utils;
using Application.Api;

namespace Frontend.Api
{
	public class InvoiceNumberingService : IInvoiceNumberingService
	{
		private readonly HttpClient _httpClient;
		private readonly string _urlPath = "api/InvoiceNumbering";

		public InvoiceNumberingService(EnvironmentConfig config)
		{
			_httpClient = new HttpClient { BaseAddress = new Uri(config.ApiBaseUrl) };
		}

		public async Task<NumberingSchemeDto> CreateAsync(NumberingSchemeDto obj)
		{
			var response = await _httpClient.PostAsJsonAsync(_urlPath, obj);
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
			var response = await _httpClient.DeleteAsync($"{_urlPath}/{id}");
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
			var response = await _httpClient.GetAsync(_urlPath);
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
			var response = await _httpClient.GetAsync($"{_urlPath}/{id}");
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

		public async Task<NumberingSchemeDto> GetDefaultNumberScheme()
		{
			var response = await _httpClient.GetAsync($"{_urlPath}/default");
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
			var response = await _httpClient.PutAsJsonAsync($"{_urlPath}/{id}", obj);
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

		public Task<string> GetNextInvoiceNumberAsync(int entityId, DateTime generationDate)
		{
			throw new NotImplementedException("GetNextInvoiceNumberAsync method is not implemented.");
		}

		public async Task<string> PeekNextInvoiceNumberAsync(int entityId, DateTime generationDate)
		{
			var response = await _httpClient.GetAsync($"{_urlPath}/PeekNextInvoiceNumber/{entityId}");
			if (response.IsSuccessStatusCode)
				return await response.Content.ReadAsStringAsync();
			else
			{
				var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
				if (errorResponse is not null)
					throw new ApiException(errorResponse);
				else
					throw new Exception($"Failed to peek next Invoice Number: {response.ReasonPhrase}");
			}
		}
	}
}
