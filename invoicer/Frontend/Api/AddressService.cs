using Domain.Models;
using Domain.ServiceInterfaces;
using Frontend.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Frontend.Api
{
	public class AddressService : IAddressService
	{
		private readonly HttpClient _httpClient;

		public AddressService(EnvironmentConfig config)
		{
			_httpClient = new HttpClient { BaseAddress = new Uri(config.ApiBaseUrl) };
		}

		public async Task<Address> CreateAsync(Address obj)
		{
			Console.WriteLine(_httpClient.BaseAddress + "api/Address");
			var response = await _httpClient.PostAsJsonAsync("api/Address", obj);
			return await response.Content.ReadFromJsonAsync<Address>();
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var response = await _httpClient.DeleteAsync($"api/Address/{id}");
			return response.IsSuccessStatusCode;
		}

		public async Task<IList<Address>> GetAllAsync()
		{
			return await _httpClient.GetFromJsonAsync<IList<Address>>("api/Address");
		}

		public async Task<Address?> GetByIdAsync(int id)
		{
			return await _httpClient.GetFromJsonAsync<Address>($"api/Address/{id}");
		}

		public async Task<Address> UpdateAsync(int id, Address obj)
		{
			var response = await _httpClient.PutAsJsonAsync($"api/Address/{id}", obj);
			return await response.Content.ReadFromJsonAsync<Address>();
		}
	}
}