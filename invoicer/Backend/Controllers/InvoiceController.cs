using Domain.ServiceInterfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;
using Backend.Utils;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class InvoiceController(IInvoiceService invoiceService) : ControllerBase
	{
		[HttpGet("{id:int}", Name = "GetInvoiceById")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> GetById(int id)
		{
			Invoice? invoice = await invoiceService.GetByIdAsync(id);
			if (invoice is null)
				return StatusCode(404, ApiResponse<Address>.NotFound($"Invoice with id {id} not found"));
			return StatusCode(200, ApiResponse<Invoice>.Ok(invoice));
		}

		[HttpGet(Name = "GetAllInvoices")]
		[ProducesResponseType(200)]
		public async Task<IActionResult> GetAll()
		{
			IList<Invoice> invoices = await invoiceService.GetAllAsync();
			return StatusCode(200, ApiResponse<IList<Invoice>>.Ok(invoices));
		}

		[HttpPost(Name = "PostInvoice")]
		[ProducesResponseType(201)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> Post([FromBody] Invoice invoice)
		{
			if (invoice.Items is null || invoice.Items.Count == 0)
			{
				return StatusCode(400, ApiResponse<Invoice>.BadRequest("Invoice must have at least one item"));
			}

			Invoice? createdInvoice;
			try
			{
				createdInvoice = await invoiceService.CreateAsync(invoice);
			}
			catch (ArgumentException e)
			{
				return StatusCode(400, ApiResponse<Invoice>.BadRequest(e.Message));
			}
			return StatusCode(201, ApiResponse<Invoice>.Created(createdInvoice!));
		}

		[HttpPut("{id:int}", Name = "PutInvoice")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> Put(int id, [FromBody] Invoice invoice)
		{
			if (invoice.Items is null || invoice.Items.Count == 0)
			{
				return StatusCode(400, ApiResponse<Invoice>.BadRequest("Invoice must have at least one item"));
			}
			Invoice? existingInvoice = await invoiceService.GetByIdAsync(id);
			if (existingInvoice is null)
			{
				return StatusCode(404, ApiResponse<Invoice>.NotFound($"Invoice with id {id} not found"));
			}
			try
			{
				existingInvoice = await invoiceService.UpdateAsync(id, invoice);
			}
			catch (ArgumentException e)
			{
				return StatusCode(400, ApiResponse<Invoice>.BadRequest(e.Message));
			}
			return StatusCode(200, ApiResponse<Invoice>.Ok(existingInvoice));
		}

		[HttpDelete("{id:int}", Name = "DeleteInvoice")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> Delete(int id)
		{
			bool wasDeleted = await invoiceService.DeleteAsync(id);
			if (!wasDeleted)
				return StatusCode(404, ApiResponse<Invoice>.NotFound($"Invoice with id {id} not found"));
			return StatusCode(200, ApiResponse<bool>.Ok(true));
		}

		[HttpGet("{id:int}/export-pdf", Name = "ExportInvoicePdf")]
		public async Task<IActionResult> ExportPdf(int id)
		{
			try
			{
				IPdfGenerationResult invoiceResult = await invoiceService.ExportInvoicePdf(id);
				if (!invoiceResult.IsSuccess)
				{
					return StatusCode(invoiceResult.StatusCode, invoiceResult.ErrorMessage);
				}
				byte[] pdfFile = invoiceResult.Data!;
				//return StatusCode(200, ApiResponse<byte[]>.Ok(pdfFile));
				return File(pdfFile, "application/pdf", $"invoice_{id}.pdf");
			}
			catch (ArgumentException e)
			{
				return StatusCode(404, ApiResponse<byte[]>.NotFound(e.Message));
			}
			catch (InvalidOperationException e)
			{
				return StatusCode(400, ApiResponse<byte[]>.BadRequest(e.Message));
			}
			catch (Exception e)
			{
				return StatusCode(500, ApiResponse<byte[]>.InternalServerError(e.Message));
			}
		}
	}
}
