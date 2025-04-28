using Application.ServiceInterfaces;
using Application.DTOs;
using System.Net.Http.Json;
using Frontend.Utils;

namespace Frontend.Api
{
	public class AddressService : IAddressService
	{
		private readonly HttpClient _httpClient;
		private readonly string _urlPath = "api/Address";

		public AddressService(EnvironmentConfig config)
		{
			_httpClient = new HttpClient { BaseAddress = new Uri(config.ApiBaseUrl) };
		}

		public async Task<AddressDto?> CreateAsync(AddressDto obj)
		{
			var response = await _httpClient.PostAsJsonAsync(_urlPath, obj);
			return await response.Content.ReadFromJsonAsync<AddressDto>();
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var response = await _httpClient.DeleteAsync($"{_urlPath}/{id}");
			return response.IsSuccessStatusCode;
		}

		public async Task<IList<AddressDto>> GetAllAsync()
		{
			var reponse = await _httpClient.GetFromJsonAsync<IList<AddressDto>>(_urlPath);
			return reponse ?? [];
		}

		public async Task<AddressDto?> GetByIdAsync(int id)
		{
			return await _httpClient.GetFromJsonAsync<AddressDto>($"{_urlPath}/{id}");
		}

		public async Task<AddressDto?> UpdateAsync(int id, AddressDto obj)
		{
			var response = await _httpClient.PutAsJsonAsync($"{_urlPath}/{id}", obj);
			return await response.Content.ReadFromJsonAsync<AddressDto>();
		}
	}
}