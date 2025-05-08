using Microsoft.AspNetCore.Mvc;
using Application.ServiceInterfaces;
using Application.DTOs;
using Application.PdfGenerator;
using Application.Api;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class InvoiceController(IInvoiceService invoiceService) : ControllerBase
	{
		[HttpGet("{id:int}", Name = "GetInvoiceById")]
		[ProducesResponseType(typeof(InvoiceDto), 200)]
		[ProducesResponseType(typeof(ApiErrorResponse), 404)]
		[ProducesResponseType(typeof(ApiErrorResponse), 500)]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				InvoiceDto? invoice = await invoiceService.GetByIdAsync(id);
				return Ok(invoice);
			}
			catch (KeyNotFoundException e)
			{
				return NotFound(ApiErrorResponse.Create("Invoice not found", e.Message, 404));
			}
			catch (Exception e)
			{
				return StatusCode(500, ApiErrorResponse.Create("Internal server error", e.Message, 500));
			}
		}

		[HttpGet(Name = "GetAllInvoices")]
		[ProducesResponseType(typeof(IList<InvoiceDto>), 200)]
		[ProducesResponseType(typeof(ApiErrorResponse), 500)]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				IList<InvoiceDto> invoiceList = await invoiceService.GetAllAsync();
				return Ok(invoiceList);
			}
			catch (Exception e)
			{
				return StatusCode(500, ApiErrorResponse.Create("Internal server error", e.Message, 500));
			}
		}

		[HttpPost(Name = "PostInvoice")]
		[ProducesResponseType(typeof(InvoiceDto), 201)]
		[ProducesResponseType(typeof(ApiErrorResponse), 400)]
		[ProducesResponseType(typeof(ApiErrorResponse), 500)]
		public async Task<IActionResult> Post([FromBody] InvoiceDto invoice)
		{
			if (invoice.Items is null || invoice.Items.Count == 0)
			{
				return BadRequest(ApiErrorResponse.Create("The invoice must contain at least one item", "Bad Request", 400));
			}

			try
			{
				InvoiceDto? createdInvoice = await invoiceService.CreateAsync(invoice);
				return CreatedAtRoute("GetInvoiceById", new { id = createdInvoice!.Id }, createdInvoice);
			}
			catch (ArgumentException e)
			{
				return BadRequest(ApiErrorResponse.Create("Invalid request", e.Message, 400));
			}
			catch (KeyNotFoundException e)
			{
				return NotFound(ApiErrorResponse.Create("Invoice not found", e.Message, 404));
			}
			catch (Exception e)
			{
				return StatusCode(500, ApiErrorResponse.Create(e.Message, e.Message, 500));
			}
		}

		[HttpPut("{id:int}", Name = "PutInvoice")]
		[ProducesResponseType(typeof(InvoiceDto), 200)]
		[ProducesResponseType(typeof(ApiErrorResponse), 400)]
		[ProducesResponseType(typeof(ApiErrorResponse), 404)]
		[ProducesResponseType(typeof(ApiErrorResponse), 500)]
		public async Task<IActionResult> Put(int id, [FromBody] InvoiceDto invoice)
		{
			if (invoice.Items is null || invoice.Items.Count == 0)
			{
				return BadRequest(ApiErrorResponse.Create("The invoice must contain at least one item", "Bad Request", 400));
			}

			try
			{
				InvoiceDto? updatedInvoice = await invoiceService.UpdateAsync(id, invoice);
				return Ok(updatedInvoice);
			}
			catch (ArgumentException e)
			{
				return BadRequest(ApiErrorResponse.Create(e.Message, e.Message, 400));
			}
			catch (KeyNotFoundException e)
			{
				return NotFound(ApiErrorResponse.Create(e.Message, e.Message, 404));
			}
			catch (Exception e)
			{
				return StatusCode(500, ApiErrorResponse.Create("Internal server error", e.Message, 500));
			}
		}

		[HttpDelete("{id:int}", Name = "DeleteInvoice")]
		[ProducesResponseType(typeof(string), 200)]
		[ProducesResponseType(typeof(ApiErrorResponse), 404)]
		[ProducesResponseType(typeof(ApiErrorResponse), 500)]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				bool wasDeleted = await invoiceService.DeleteAsync(id);
				if (!wasDeleted)
				{
					return NotFound(ApiErrorResponse.Create("Invoice not found", "Not Found", 404));
				}
				return Ok($"Invoice with id {id} deleted");
			}
			catch (ArgumentException e)
			{
				return NotFound(ApiErrorResponse.Create(e.Message, e.Message, 404));
			}
			catch (Exception e)
			{
				return StatusCode(500, ApiErrorResponse.Create("Internal server error", e.Message, 500));
			}
		}

		[HttpGet("{id:int}/export-pdf", Name = "ExportInvoicePdf")]
		[ProducesResponseType(typeof(byte[]), 200)]
		[ProducesResponseType(typeof(ApiErrorResponse), 404)]
		[ProducesResponseType(typeof(ApiErrorResponse), 400)]
		[ProducesResponseType(typeof(ApiErrorResponse), 500)]
		public async Task<IActionResult> ExportPdf(int id)
		{
			try
			{
				IPdfGenerationResult invoiceResult = await invoiceService.ExportInvoicePdfAsync(id);
				if (!invoiceResult.IsSuccess)
				{
					return StatusCode(500, ApiErrorResponse.Create("An error occurred while exporting the invoice to PDF", invoiceResult.ErrorMessage!, invoiceResult.StatusCode));
				}
				byte[] pdfFile = invoiceResult.Data!;
				return File(pdfFile, "application/pdf", $"invoice_{id}.pdf");
			}
			catch (ArgumentException e)
			{
				return NotFound(ApiErrorResponse.Create("Invoice not found", e.Message, 404));
			}
			catch (InvalidOperationException e)
			{
				return BadRequest(ApiErrorResponse.Create("Invalid operation", e.Message, 400));
			}
			catch (Exception e)
			{
				return StatusCode(500, ApiErrorResponse.Create("Internal server error", e.Message, 500));
			}
		}

		[HttpGet("entity/{entityId:int}/new", Name = "GetNewInvoiceInformation")]
		[ProducesResponseType(typeof(InvoiceDto), 200)]
		[ProducesResponseType(typeof(ApiErrorResponse), 404)]
		[ProducesResponseType(typeof(ApiErrorResponse), 500)]
		public async Task<IActionResult> GetNewInvoiceInformation(int entityId)
		{
			try
			{
				InvoiceDto? invoice = await invoiceService.GetNewInvoiceInformationAsync(entityId);
				return Ok(invoice);
			}
			catch (KeyNotFoundException e)
			{
				return NotFound(ApiErrorResponse.Create("Invoice not found", e.Message, 404));
			}
			catch (Exception e)
			{
				return StatusCode(500, ApiErrorResponse.Create("Internal server error", e.Message, 500));
			}
		}
	}
}
