using System.Net.Http.Json;
using Domain.Interfaces;
using Domain.Models;
using Domain.ServiceInterfaces;
using Frontend.Models;

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

		public async Task<Invoice?> CreateAsync(Invoice obj)
		{
			var response = await _httpClient.PostAsJsonAsync(_urlPath, obj);
			return await response.Content.ReadFromJsonAsync<Invoice>();
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var response = await _httpClient.DeleteAsync($"{_urlPath}/{id}");
			return response.IsSuccessStatusCode;
		}

		public async Task<IPdfGenerationResult> ExportInvoicePdf(int id)
		{
			HttpResponseMessage response = await _httpClient.GetAsync($"{_urlPath}/{id}/export-pdf");

			if (!response.IsSuccessStatusCode)
			{
				return PdfGenerationResult.Failure("Failed to generate PDF", (int)response.StatusCode);
			}

			byte[] pdfFile = await response.Content.ReadAsByteArrayAsync();

			return PdfGenerationResult.Success(pdfFile);
		}

		public async Task<IList<Invoice>> GetAllAsync()
		{
			return await _httpClient.GetFromJsonAsync<IList<Invoice>>(_urlPath) ?? [];
		}

		public async Task<Invoice?> GetByIdAsync(int id)
		{
			return await _httpClient.GetFromJsonAsync<Invoice>($"{_urlPath}/{id}");
		}

		public async Task<Invoice?> UpdateAsync(int id, Invoice obj)
		{
			var response = await _httpClient.PutAsJsonAsync($"{_urlPath}/{id}", obj);
			return await response.Content.ReadFromJsonAsync<Invoice>();
		}
	}
}
