using Backend.Services.InvoiceGeneratorService.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class InvoiceController : ControllerBase
	{
		[HttpGet("{id:int}", Name = "GetInvoiceById")]
		public async Task<IActionResult> GetById(int id)
		{
			return Ok();
		}

		[HttpGet(Name = "GetAllInvoices")]
		public async Task<IActionResult> GetAll()
		{
			return Ok();
		}

		[HttpPost(Name = "PostInvoice")]
		public async Task<IActionResult> Post([FromBody] Invoice invoice)
		{
			return Created();
		}

		[HttpPut("{id:int}", Name = "PutInvoice")]
		public async Task<IActionResult> Put(int id, [FromBody] Invoice invoice)
		{
			return Ok();
		}

		[HttpDelete("{id:int}", Name = "DeleteInvoice")]
		public async Task<IActionResult> Delete(int id)
		{
			return Ok();
		}
	}
}
