using System.Net.Http.Json;
using Application.ServiceInterfaces;
using Application.DTOs;
using Application.InvoicePdfGenerator;
using Application.PdfGenerator;
using Frontend.Utils;
using Application.Api;

namespace Frontend.Api
{
	public class InvoiceService : IInvoiceService
	{
		private readonly HttpClient _httpClient;
		private readonly string _urlPath = "api/Invoice";

		public InvoiceService(EnvironmentConfig config)
		{
			_httpClient = new HttpClient { BaseAddress = new Uri(config.ApiBaseUrl) };
		}

		public async Task<InvoiceDto> CreateAsync(InvoiceDto obj)
		{
			var response = await _httpClient.PostAsJsonAsync(_urlPath, obj);
			if (response.IsSuccessStatusCode)
				return await response.Content.ReadFromJsonAsync<InvoiceDto>() ?? throw new Exception("Failed to deserialize InvoiceDto");
			else
			{
				var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
				if (errorResponse is not null)
					throw new ApiException(errorResponse);
				else
					throw new Exception($"Failed to create Invoice: {response.ReasonPhrase}");
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
					throw new Exception($"Failed to delete Invoice with id {id}: {response.ReasonPhrase}");
			}
		}

		public async Task<IPdfGenerationResult> ExportInvoicePdf(int id)
		{
			HttpResponseMessage response = await _httpClient.GetAsync($"{_urlPath}/{id}/export-pdf");

			if (!response.IsSuccessStatusCode)
				return PdfGenerationResult.Failure("Failed to generate PDF", (int)response.StatusCode);

			byte[] pdfFile = await response.Content.ReadAsByteArrayAsync();

			return PdfGenerationResult.Success(pdfFile);
		}

		public async Task<IList<InvoiceDto>> GetAllAsync()
		{
			var response = await _httpClient.GetAsync(_urlPath);
			if (response.IsSuccessStatusCode)
				return await response.Content.ReadFromJsonAsync<IList<InvoiceDto>>() ?? new List<InvoiceDto>();
			else
			{
				var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
				if (errorResponse is not null)
					throw new ApiException(errorResponse);
				else
					throw new Exception($"Failed to retrieve Invoices: {response.ReasonPhrase}");
			}
		}

		public async Task<InvoiceDto> GetByIdAsync(int id)
		{
			var response = await _httpClient.GetAsync($"{_urlPath}/{id}");
			if (response.IsSuccessStatusCode)
				return await response.Content.ReadFromJsonAsync<InvoiceDto>() ?? throw new Exception("Failed to deserialize InvoiceDto");
			else
			{
				var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
				if (errorResponse is not null)
					throw new ApiException(errorResponse);
				else
					throw new Exception($"Failed to retrieve Invoice with id {id}: {response.ReasonPhrase}");
			}
		}

		public async Task<InvoiceDto> UpdateAsync(int id, InvoiceDto obj)
		{
			var response = await _httpClient.PutAsJsonAsync($"{_urlPath}/{id}", obj);
			if (response.IsSuccessStatusCode)
				return await response.Content.ReadFromJsonAsync<InvoiceDto>() ?? throw new Exception("Failed to deserialize InvoiceDto");
			else
			{
				var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
				if (errorResponse is not null)
					throw new ApiException(errorResponse);
				else
					throw new Exception($"Failed to update Invoice with id {id}: {response.ReasonPhrase}");
			}
		}
	}
}
