using System.Net.Http.Json;
using Application.ServiceInterfaces;
using Application.DTOs;
using Domain.Interfaces;
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

		public async Task<InvoiceDto?> CreateAsync(InvoiceDto obj)
		{
			var response = await _httpClient.PostAsJsonAsync(_urlPath, obj);
			return await response.Content.ReadFromJsonAsync<InvoiceDto>();
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

		public async Task<IList<InvoiceDto>> GetAllAsync()
		{
			return await _httpClient.GetFromJsonAsync<IList<InvoiceDto>>(_urlPath) ?? [];
		}

		public async Task<InvoiceDto?> GetByIdAsync(int id)
		{
			return await _httpClient.GetFromJsonAsync<InvoiceDto>($"{_urlPath}/{id}");
		}

		public async Task<InvoiceDto?> UpdateAsync(int id, InvoiceDto obj)
		{
			var response = await _httpClient.PutAsJsonAsync($"{_urlPath}/{id}", obj);
			return await response.Content.ReadFromJsonAsync<InvoiceDto>();
		}
	}
}
