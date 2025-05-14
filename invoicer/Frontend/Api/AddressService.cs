using System.Net.Http.Json;
using Frontend.Utils;
using Shared.Api;
using Shared.DTOs;
using Application.ServiceInterfaces;

namespace Frontend.Api
{
	public class AddressService(HttpClient httpClient) : IAddressService
	{
		private readonly string _urlPath = "api/Address";

		public async Task<AddressDto> CreateAsync(AddressDto obj)
		{
			var response = await httpClient.PostAsJsonAsync(_urlPath, obj);
			if (response.IsSuccessStatusCode)
				return await response.Content.ReadFromJsonAsync<AddressDto>() ?? throw new Exception("Failed to deserialize AddressDto");
			else
			{
				var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
				if (errorResponse is not null)
					throw new ApiException(errorResponse);
				else
					throw new Exception($"Failed to create Address: {response.ReasonPhrase}");
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
					throw new Exception($"Failed to delete Address with id {id}: {response.ReasonPhrase}");
			}
		}

		public async Task<IList<AddressDto>> GetAllAsync()
		{
			var response = await httpClient.GetAsync(_urlPath);
			if (response.IsSuccessStatusCode)
				return await response.Content.ReadFromJsonAsync<IList<AddressDto>>() ?? [];
			else
			{
				var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
				if (errorResponse is not null)
					throw new ApiException(errorResponse);
				else
					throw new Exception($"Failed to retrieve Addresses: {response.ReasonPhrase}");
			}
		}

		public async Task<AddressDto> GetByIdAsync(int id)
		{
			var response = await httpClient.GetAsync($"{_urlPath}/{id}");
			if (response.IsSuccessStatusCode)
				return await response.Content.ReadFromJsonAsync<AddressDto>() ?? throw new Exception("Failed to deserialize AddressDto");
			else
			{
				var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
				if (errorResponse is not null)
					throw new ApiException(errorResponse);
				else
					throw new Exception($"Failed to retrieve Address with id {id}: {response.ReasonPhrase}");
			}
		}

		public async Task<AddressDto> UpdateAsync(int id, AddressDto obj)
		{
			var response = await httpClient.PutAsJsonAsync($"{_urlPath}/{id}", obj);
			if (response.IsSuccessStatusCode)
				return await response.Content.ReadFromJsonAsync<AddressDto>() ?? throw new Exception("Failed to deserialize AddressDto");
			else
			{
				var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
				if (errorResponse is not null)
					throw new ApiException(errorResponse);
				else
					throw new Exception($"Failed to update Address with id {id}: {response.ReasonPhrase}");
			}
		}
	}
}