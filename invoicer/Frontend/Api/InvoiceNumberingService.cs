using System.Net.Http.Json;
using Application.DTOs;
using Application.ServiceInterfaces;
using Frontend.Utils;
using Frontend.Models;
using Application.Api;

namespace Frontend.Api
{
	public class InvoiceNumberingService : IInvoiceNumberingService
	{
		private readonly HttpClient _httpClient;
		private readonly string _urlPath = "api/InvoiceNumbering";

		public InvoiceNumberingService(EnvironmentConfig config)
		{
			_httpClient = new HttpClient { BaseAddress = new Uri(config.ApiBaseUrl) };
		}

		public async Task<InvoiceNumberSchemeDto?> CreateAsync(InvoiceNumberSchemeDto obj)
		{
			try
			{
				var response = await _httpClient.PostAsJsonAsync(_urlPath, obj);
				if (response.IsSuccessStatusCode)
					return await response.Content.ReadFromJsonAsync<InvoiceNumberSchemeDto>();
				else
				{
					var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
					if (errorResponse is not null)
						throw new ApiException(errorResponse);
					else
						throw new Exception($"Failed to create Invoice Numbering Scheme: {response.ReasonPhrase}");
				}
			}
			catch (ApiException)
			{
				throw;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<bool> DeleteAsync(int id)
		{
			try
			{
				var response = await _httpClient.DeleteAsync($"{_urlPath}/{id}");
				if (response.IsSuccessStatusCode)
					return true;
				else
					throw new Exception($"Failed to delete Invoice Numbering Scheme with id {id}");
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<IList<InvoiceNumberSchemeDto>> GetAllAsync()
		{
			try
			{
				var response = await _httpClient.GetAsync(_urlPath);
				if (response.IsSuccessStatusCode)
					return await response.Content.ReadFromJsonAsync<IList<InvoiceNumberSchemeDto>>() ?? [];
				else
				{
					var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
					if (errorResponse is not null)
						throw new ApiException(errorResponse);
					else
						throw new Exception($"Failed to retrieve Invoice Numbering Schemes: {response.ReasonPhrase}");
				}
			}
			catch (ApiException)
			{
				throw;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<InvoiceNumberSchemeDto?> GetByIdAsync(int id)
		{
			try
			{
				var response = await _httpClient.GetAsync($"{_urlPath}/{id}");
				if (response.IsSuccessStatusCode)
					return await response.Content.ReadFromJsonAsync<InvoiceNumberSchemeDto>();
				else
				{
					var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
					if (errorResponse is not null)
						throw new ApiException(errorResponse);
					else
						throw new Exception($"Failed to retrieve Invoice Numbering Scheme with id {id}: {response.ReasonPhrase}");
				}
			}
			catch (ApiException)
			{
				throw;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<InvoiceNumberSchemeDto> GetDefaultNumberScheme()
		{
			try
			{
				var response = await _httpClient.GetAsync($"{_urlPath}/default");
				if (response.IsSuccessStatusCode)
					return await response.Content.ReadFromJsonAsync<InvoiceNumberSchemeDto>() ?? throw new KeyNotFoundException("Default Invoice Numbering Scheme not found");
				else
				{
					var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
					if (errorResponse is not null)
						throw new ApiException(errorResponse);
					else
						throw new Exception($"Failed to retrieve Default Invoice Numbering Scheme: {response.ReasonPhrase}");
				}
			}
			catch (ApiException)
			{
				throw;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<InvoiceNumberSchemeDto?> UpdateAsync(int id, InvoiceNumberSchemeDto obj)
		{
			try
			{
				var response = await _httpClient.PutAsJsonAsync($"{_urlPath}/{id}", obj);
				if (response.IsSuccessStatusCode)
					return await response.Content.ReadFromJsonAsync<InvoiceNumberSchemeDto>();
				else
				{
					var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
					if (errorResponse is not null)
						throw new ApiException(errorResponse);
					else
						throw new Exception($"Failed to update Invoice Numbering Scheme with id {id}: {response.ReasonPhrase}");
				}
			}
			catch (ApiException)
			{
				throw;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public Task<string> GetNextInvoiceNumberAsync(int entityId, DateTime generationDate)
		{
			throw new NotImplementedException("GetNextInvoiceNumberAsync method is not implemented.");
		}
	}
}
