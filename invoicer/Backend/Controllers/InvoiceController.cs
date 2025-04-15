using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;
using Application.ServiceInterfaces;
using Application.DTOs;

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
			try
			{
				InvoiceDto? invoice = await invoiceService.GetByIdAsync(id);
				return Ok(invoice);
			}
			catch (KeyNotFoundException e)
			{
				return NotFound(e.Message);
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}

		[HttpGet(Name = "GetAllInvoices")]
		[ProducesResponseType(200)]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				IList<InvoiceDto> invoiceList = await invoiceService.GetAllAsync();
				return Ok(invoiceList);
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}

		[HttpPost(Name = "PostInvoice")]
		[ProducesResponseType(201)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> Post([FromBody] InvoiceDto invoice)
		{
			if (invoice.Items is null || invoice.Items.Count == 0)
			{
				return BadRequest("Invoice must have at least one item");
			}

			try
			{
				InvoiceDto? createdInvoice = await invoiceService.CreateAsync(invoice);
				return CreatedAtRoute("GetInvoiceById", new { id = createdInvoice!.Id }, createdInvoice);
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}

		[HttpPut("{id:int}", Name = "PutInvoice")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> Put(int id, [FromBody] InvoiceDto invoice)
		{
			if (invoice.Items is null || invoice.Items.Count == 0)
				return BadRequest("Invoice must have at least one item");

			try
			{
				InvoiceDto? updatedInvoice = await invoiceService.UpdateAsync(id, invoice);
				return Ok(updatedInvoice);
			}
			catch (KeyNotFoundException e)
			{
				return NotFound(e.Message);
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}

		[HttpDelete("{id:int}", Name = "DeleteInvoice")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				bool wasDeleted = await invoiceService.DeleteAsync(id);
				if (!wasDeleted)
					return NotFound($"Invoice with id {id} not found");
				return Ok($"Invoice with id {id} deleted");
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}

		[HttpGet("{id:int}/export-pdf", Name = "ExportInvoicePdf")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		[ProducesResponseType(500)]
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
