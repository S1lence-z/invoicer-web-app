using System.Net.Http.Json;
using Application.ServiceInterfaces;
using Application.DTOs;
using Application.InvoicePdfGenerator;
using Application.PdfGenerator;
using Frontend.Utils;
using Application.Api;
using System.Net.Http.Headers;

namespace Frontend.Api
{
	public class InvoiceService(HttpClient httpClient) : IInvoiceService
	{
		private readonly string _urlPath = "api/Invoice";

		public async Task<InvoiceDto> CreateAsync(InvoiceDto obj)
		{
			var response = await httpClient.PostAsJsonAsync(_urlPath, obj);
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
			var response = await httpClient.DeleteAsync($"{_urlPath}/{id}");
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

		public async Task<IPdfGenerationResult> ExportInvoicePdfAsync(int id, string lang)
		{
			HttpResponseMessage response = await httpClient.GetAsync($"{_urlPath}/{id}/export-pdf?lang={Uri.UnescapeDataString(lang)}");
			if (!response.IsSuccessStatusCode)
				return PdfGenerationResult.Failure("Failed to generate PDF", (int)response.StatusCode);

			string? receivedFileName = null;

			ContentDispositionHeaderValue? contentDisposition = response.Content.Headers.ContentDisposition;
			if (contentDisposition is not null)
				receivedFileName = contentDisposition.FileNameStar ?? contentDisposition.FileName;

			if (string.IsNullOrEmpty(receivedFileName))
				receivedFileName = $"Invoice_{id}.pdf";

			byte[] pdfFile = await response.Content.ReadAsByteArrayAsync();

			return PdfGenerationResult.Success(pdfFile, receivedFileName);
		}

		public async Task<IList<InvoiceDto>> GetAllAsync()
		{
			var response = await httpClient.GetAsync(_urlPath);
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
			var response = await httpClient.GetAsync($"{_urlPath}/{id}");
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

		public async Task<InvoiceDto> GetNewInvoiceInformationAsync(int entityId)
		{
			var response = await httpClient.GetAsync($"{_urlPath}/entity/{entityId}/new");
			if (response.IsSuccessStatusCode)
				return await response.Content.ReadFromJsonAsync<InvoiceDto>() ?? throw new Exception("Failed to deserialize InvoiceDto");
			else
			{
				var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
				if (errorResponse is not null)
					throw new ApiException(errorResponse);
				else
					throw new Exception($"Failed to retrieve new Invoice information for entity with id {entityId}: {response.ReasonPhrase}");
			}
		}

		public async Task<InvoiceDto> UpdateAsync(int id, InvoiceDto obj)
		{
			var response = await httpClient.PutAsJsonAsync($"{_urlPath}/{id}", obj);
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
