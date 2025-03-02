using System.Net.Http.Json;
using Domain.Models;
using Domain.ServiceInterfaces;
using Frontend.Models;

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

		public async Task<BankAccount?> CreateAsync(BankAccount obj)
		{
			var response = await _httpClient.PostAsJsonAsync(_urlPath, obj);
			return await response.Content.ReadFromJsonAsync<BankAccount>();
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var reponse = await _httpClient.DeleteAsync($"{_urlPath}/{id}");
			return reponse.IsSuccessStatusCode;
		}

		public async Task<IList<BankAccount>> GetAllAsync()
		{
			return await _httpClient.GetFromJsonAsync<IList<BankAccount>>(_urlPath);
		}

		public async Task<BankAccount?> GetByIdAsync(int id)
		{
			return await _httpClient.GetFromJsonAsync<BankAccount>($"{_urlPath}/{id}");
		}

		public async Task<BankAccount> UpdateAsync(int id, BankAccount obj)
		{
			var response = await _httpClient.PutAsJsonAsync($"{_urlPath}/{id}", obj);
			return await response.Content.ReadFromJsonAsync<BankAccount>();
		}
	}
}
