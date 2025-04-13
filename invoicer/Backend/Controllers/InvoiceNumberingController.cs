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

		[HttpDelete("{id:int}", Name = "DeleteInvoiceNumberingScheme")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> Delete(int id)
		{
			InvoiceNumberScheme? scheme = await numberingService.GetByIdAsync(id);
			if (scheme is null)
			{
				return NotFound($"Invoice Numbering Scheme with id {id} not found");
			}
			bool wasDeleted = await numberingService.DeleteAsync(id);
			if (!wasDeleted)
				return BadRequest($"Invoice Numbering Scheme with id {id} could not be deleted");
			return Ok($"Invoice Numbering Scheme with id {id} deleted");
		}

		[HttpPost(Name = "PostInvoiceNumberingScheme")]
		[ProducesResponseType(201)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> Post([FromBody] InvoiceNumberScheme scheme)
		{
			if (scheme.EntityId <= 0)
			{
				return BadRequest("Entity ID must be greater than 0");
			}
			InvoiceNumberScheme? createdScheme;
			try
			{
				createdScheme = await numberingService.CreateAsync(scheme);
			}
			catch (ArgumentException e)
			{
				return BadRequest(e.Message);
			}
			return CreatedAtRoute("GetInvoiceNumberingSchemeById", new { id = createdScheme!.Id }, createdScheme);
		}

		[HttpPut("{id:int}", Name = "UpdateInvoiceNumberingScheme")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> Put(int id, [FromBody] InvoiceNumberScheme scheme)
		{
			if (scheme.EntityId <= 0)
				return BadRequest("Entity ID must be greater than 0");

			InvoiceNumberScheme? existingScheme = await numberingService.GetByIdAsync(id);
			if (existingScheme is null)
				return NotFound($"Invoice Numbering Scheme with id {id} not found");

			try
			{
				existingScheme = await numberingService.UpdateAsync(id, existingScheme);
			}
			catch (ArgumentException e)
			{
				return BadRequest(e.Message);
			}
			return Ok(existingScheme);
		}
	}
}
