using Domain.Models;
using Domain.ServiceInterfaces;
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

		public async Task<Entity?> CreateAsync(Entity obj)
		{
			var response = await _httpClient.PostAsJsonAsync(_urlPath, obj);
			return await response.Content.ReadFromJsonAsync<Entity>();
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var response = await _httpClient.DeleteAsync($"{_urlPath}/{id}");
			return response.IsSuccessStatusCode;
		}

		public async Task<IList<Entity>> GetAllAsync()
		{
			return await _httpClient.GetFromJsonAsync<IList<Entity>>(_urlPath) ?? [];
		}

		public async Task<Entity?> GetByIdAsync(int id)
		{
			return await _httpClient.GetFromJsonAsync<Entity>($"{_urlPath}/{id}");
		}

		public async Task<Entity?> UpdateAsync(int id, Entity obj)
		{
			var response = await _httpClient.PutAsJsonAsync($"{_urlPath}/{id}", obj);
			return await response.Content.ReadFromJsonAsync<Entity>();
		}
	}
}
