using Application.ServiceInterfaces;
using Application.DTOs;
using Frontend.Models;
using System.Net.Http.Json;

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

		public async Task<EntityDto?> CreateAsync(EntityDto obj)
		{
			var response = await _httpClient.PostAsJsonAsync(_urlPath, obj);
			return await response.Content.ReadFromJsonAsync<EntityDto>();
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var response = await _httpClient.DeleteAsync($"{_urlPath}/{id}");
			return response.IsSuccessStatusCode;
		}

		public async Task<IList<EntityDto>> GetAllAsync()
		{
			return await _httpClient.GetFromJsonAsync<IList<EntityDto>>(_urlPath) ?? [];
		}

		public async Task<EntityDto?> GetByIdAsync(int id)
		{
			return await _httpClient.GetFromJsonAsync<EntityDto>($"{_urlPath}/{id}");
		}

		public async Task<EntityDto?> UpdateAsync(int id, EntityDto obj)
		{
			var response = await _httpClient.PutAsJsonAsync($"{_urlPath}/{id}", obj);
			return await response.Content.ReadFromJsonAsync<EntityDto>();
		}
	}
}
