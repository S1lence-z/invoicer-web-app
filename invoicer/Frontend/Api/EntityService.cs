using System.Net.Http.Json;
using Frontend.Utils;
using Shared.Api;
using Shared.DTOs;
using Application.ServiceInterfaces;

namespace Frontend.Api
{
	public class EntityService(HttpClient httpClient) : IEntityService
	{
		private readonly string _urlPath = "api/Entity";

		public async Task<EntityDto> CreateAsync(EntityDto obj)
		{
			var response = await httpClient.PostAsJsonAsync(_urlPath, obj);
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
			var response = await httpClient.DeleteAsync($"{_urlPath}/{id}");
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
			var response = await httpClient.GetAsync(_urlPath);
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
			var response = await httpClient.GetAsync($"{_urlPath}/{id}");
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
			var response = await httpClient.PutAsJsonAsync($"{_urlPath}/{id}", obj);
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
