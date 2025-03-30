using Domain.ServiceInterfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class InvoiceController(IInvoiceService invoiceService) : ControllerBase
	{
		[HttpGet("{id:int}", Name = "GetInvoiceById")]
		public async Task<IActionResult> GetById(int id)
		{
			Invoice? invoice = await invoiceService.GetByIdAsync(id);
			if (invoice is null)
				return NotFound($"Entity with id {id} not found");
			return Ok(invoice);
		}

		[HttpGet(Name = "GetAllInvoices")]
		public async Task<IActionResult> GetAll()
		{
			IList<Invoice> invoices = await invoiceService.GetAllAsync();
			return Ok(invoices);
		}

		[HttpPost(Name = "PostInvoice")]
		public async Task<IActionResult> Post([FromBody] Invoice invoice)
		{
			if (invoice.Items is null || invoice.Items.Count == 0)
			{
				return BadRequest("Invoice must have at least one item");
			}

			Invoice? createdInvoice;
			try
			{
				createdInvoice = await invoiceService.CreateAsync(invoice);
			}
			catch (ArgumentException e)
			{
				return BadRequest(e.Message);
			}
			return CreatedAtRoute("GetInvoiceById", new { id = createdInvoice!.Id }, createdInvoice);
		}

		[HttpPut("{id:int}", Name = "PutInvoice")]
		public async Task<IActionResult> Put(int id, [FromBody] Invoice invoice)
		{
			if (invoice.Items is null || invoice.Items.Count == 0)
			{
				return BadRequest("Invoice must have at least one item");
			}

			Invoice? updatedInvoice;
			try
			{
				updatedInvoice = await invoiceService.UpdateAsync(id, invoice);
			}
			catch (ArgumentException e)
			{
				return BadRequest(e.Message);
			}
			return Ok(updatedInvoice);
		}

		[HttpDelete("{id:int}", Name = "DeleteInvoice")]
		public async Task<IActionResult> Delete(int id)
		{
			bool wasDeleted = await invoiceService.DeleteAsync(id);
			if (!wasDeleted)
				return NotFound($"Invoice with id {id} not found");
			return Ok($"Invoice with id {id} deleted");
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
				return File(pdfFile, "application/pdf", $"invoice_{id}.pdf");
			}
			catch (ArgumentException e)
			{
				return NotFound(e.Message);
			}
			catch (InvalidOperationException e)
			{
				return BadRequest(e.Message);
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}
	}
}
