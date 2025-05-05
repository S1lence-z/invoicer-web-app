using System.Net.Http.Json;
using Application.ServiceInterfaces;
using Application.DTOs;
using Frontend.Utils;
using Application.Api;

namespace Frontend.Api
{
	public class BankAccountService : IBankAccountService
	{
		private readonly HttpClient _httpClient;
		private readonly string _urlPath = "api/BankAccount";

		public BankAccountService(EnvironmentConfig config)
		{
			_httpClient = new HttpClient { BaseAddress = new Uri(config.ApiBaseUrl) };
		}

		public async Task<BankAccountDto?> CreateAsync(BankAccountDto obj)
		{
			var response = await _httpClient.PostAsJsonAsync(_urlPath, obj);
			if (response.IsSuccessStatusCode)
				return await response.Content.ReadFromJsonAsync<BankAccountDto>();
			else
			{
				var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
				if (errorResponse is not null)
					throw new ApiException(errorResponse);
				else
					throw new Exception($"Failed to create Bank Account: {response.ReasonPhrase}");
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
					throw new Exception($"Failed to delete Bank Account with id {id}: {response.ReasonPhrase}");
			}
		}

		public async Task<IList<BankAccountDto>> GetAllAsync()
		{
			var response = await _httpClient.GetAsync(_urlPath);
			if (response.IsSuccessStatusCode)
				return await response.Content.ReadFromJsonAsync<IList<BankAccountDto>>() ?? [];
			else
			{
				var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
				if (errorResponse is not null)
					throw new ApiException(errorResponse);
				else
					throw new Exception($"Failed to get Bank Accounts: {response.ReasonPhrase}");
			}
		}

		public async Task<BankAccountDto?> GetByIdAsync(int id)
		{
			var response = await _httpClient.GetAsync($"{_urlPath}/{id}");
			if (response.IsSuccessStatusCode)
				return await response.Content.ReadFromJsonAsync<BankAccountDto>();
			else
			{
				var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
				if (errorResponse is not null)
					throw new ApiException(errorResponse);
				else
					throw new Exception($"Failed to retrieve Bank Account with id {id}: {response.ReasonPhrase}");
			}
		}

		public async Task<BankAccountDto?> UpdateAsync(int id, BankAccountDto obj)
		{
			var response = await _httpClient.PutAsJsonAsync($"{_urlPath}/{id}", obj);
			if (response.IsSuccessStatusCode)
				return await response.Content.ReadFromJsonAsync<BankAccountDto>();
			else
			{
				var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
				if (errorResponse is not null)
					throw new ApiException(errorResponse);
				else
					throw new Exception($"Failed to update Bank Account with id {id}: {response.ReasonPhrase}");
			}
		}
	}
}
