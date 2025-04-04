using Domain.ServiceInterfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class InvoiceNumberingController(IInvoiceNumberingService numberingService) : ControllerBase
	{
		[HttpGet("{id:int}", Name = "GetInvoiceNumberingSchemeById")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> GetById(int id)
		{
			InvoiceNumberScheme? scheme = await numberingService.GetByIdAsync(id);
			if (scheme is null)
			{
				return NotFound($"Invoice Numbering Scheme with id {id} not found");
			}
			return Ok(scheme);
		}

		[HttpGet(Name = "PostInvoiceNumberingScheme")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> GetAll()
		{
			IList<InvoiceNumberScheme> schemes = await numberingService.GetAllAsync();
			return Ok(schemes);
		}
	}
}
