using Backend.Services.InvoiceService;
using Backend.Services.InvoiceService.Models;
using Microsoft.AspNetCore.Mvc;

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
			Invoice createdInvoice;
			try
			{
				createdInvoice = await invoiceService.CreateAsync(invoice);
			}
			catch (ArgumentException e)
			{
				return BadRequest(e.Message);
			}
			return CreatedAtRoute("GetInvoiceById", new { id = createdInvoice.Id }, createdInvoice);
		}

		[HttpPut("{id:int}", Name = "PutInvoice")]
		public async Task<IActionResult> Put(int id, [FromBody] Invoice invoice)
		{
			Invoice updatedInvoice;
			try
			{
				updatedInvoice = await invoiceService.UpdateAsync(id, invoice);
			}
			catch (ArgumentException e)
			{
				return NotFound(e.Message);
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
	}
}
