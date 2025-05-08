using Application.ServiceInterfaces;
using Application.DTOs;
using System.Net.Http.Json;
using Frontend.Utils;
using Application.Api;

namespace Frontend.Api
{
	public class EntityService : IEntityService
	{
		private readonly HttpClient _httpClient;
		private readonly string _urlPath = "api/Entity";

		public EntityService(EnvironmentConfig config)
		{
			_httpClient = new HttpClient { BaseAddress = new Uri(config.ApiBaseUrl) };
		}

		public async Task<EntityDto> CreateAsync(EntityDto obj)
		{
			var response = await _httpClient.PostAsJsonAsync(_urlPath, obj);
			if (response.IsSuccessStatusCode)
				return await response.Content.ReadFromJsonAsync<EntityDto>() ?? throw new Exception("Failed to deserialize EntityDto");
			else
			{
				var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
				if (errorResponse is not null)
					throw new ApiException(errorResponse);
				else
					throw new Exception($"Failed to create Entity: {response.ReasonPhrase}");
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
					throw new Exception($"Failed to delete Entity with id {id}: {response.ReasonPhrase}");
			}
		}

		public async Task<IList<EntityDto>> GetAllAsync()
		{
			var response = await _httpClient.GetAsync(_urlPath);
			if (response.IsSuccessStatusCode)
				return await response.Content.ReadFromJsonAsync<IList<EntityDto>>() ?? [];
			else
			{
				var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
				if (errorResponse is not null)
					throw new ApiException(errorResponse);
				else
					throw new Exception($"Failed to retrieve Entities: {response.ReasonPhrase}");
			}
		}

		public async Task<EntityDto> GetByIdAsync(int id)
		{
			var response = await _httpClient.GetAsync($"{_urlPath}/{id}");
			if (response.IsSuccessStatusCode)
				return await response.Content.ReadFromJsonAsync<EntityDto>() ?? throw new Exception("Failed to deserialize EntityDto");
			else
			{
				var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
				if (errorResponse is not null)
					throw new ApiException(errorResponse);
				else
					throw new Exception($"Failed to retrieve Entity with id {id}: {response.ReasonPhrase}");
			}
		}

		public async Task<EntityDto> UpdateAsync(int id, EntityDto obj)
		{
			var response = await _httpClient.PutAsJsonAsync($"{_urlPath}/{id}", obj);
			if (response.IsSuccessStatusCode)
				return await response.Content.ReadFromJsonAsync<EntityDto>() ?? throw new Exception("Failed to deserialize EntityDto");
			else
			{
				var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
				if (errorResponse is not null)
					throw new ApiException(errorResponse);
				else
					throw new Exception($"Failed to update Entity with id {id}: {response.ReasonPhrase}");
			}
		}
	}
}
