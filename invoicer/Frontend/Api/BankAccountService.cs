using System.Net.Http.Json;
using Application.ServiceInterfaces;
using Application.DTOs;
using Frontend.Utils;

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
			return await response.Content.ReadFromJsonAsync<BankAccountDto>();
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var reponse = await _httpClient.DeleteAsync($"{_urlPath}/{id}");
			return reponse.IsSuccessStatusCode;
		}

		public async Task<IList<BankAccountDto>> GetAllAsync()
		{
			return await _httpClient.GetFromJsonAsync<IList<BankAccountDto>>(_urlPath) ?? [];
		}

		public async Task<BankAccountDto?> GetByIdAsync(int id)
		{
			return await _httpClient.GetFromJsonAsync<BankAccountDto>($"{_urlPath}/{id}");
		}

		public async Task<BankAccountDto?> UpdateAsync(int id, BankAccountDto obj)
		{
			var response = await _httpClient.PutAsJsonAsync($"{_urlPath}/{id}", obj);
			return await response.Content.ReadFromJsonAsync<BankAccountDto>();
		}
	}
}
