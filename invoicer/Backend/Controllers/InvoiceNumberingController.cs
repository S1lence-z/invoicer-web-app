using Microsoft.AspNetCore.Mvc;
using Application.ServiceInterfaces;
using Application.DTOs;

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
			try
			{
				InvoiceNumberSchemeDto? scheme = await numberingService.GetByIdAsync(id);
				if (scheme is null)
					return NotFound($"Invoice Numbering Scheme with id {id} not found");
				return Ok(scheme);
			}
			catch (KeyNotFoundException e)
			{
				return NotFound(e.Message);
			}
			catch (Exception e)
			{
				return StatusCode(500, $"Internal server error: {e.Message}");
			}
		}

		[HttpGet(Name = "PostInvoiceNumberingScheme")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				IList<InvoiceNumberSchemeDto> schemes = await numberingService.GetAllAsync();
				return Ok(schemes);
			}
			catch (Exception e)
			{
				return StatusCode(500, $"Internal server error: {e.Message}");
			}
		}

		[HttpPost(Name = "PostInvoiceNumberingScheme")]
		[ProducesResponseType(201)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> Post([FromBody] InvoiceNumberSchemeDto scheme)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				InvoiceNumberSchemeDto? createdScheme = await numberingService.CreateAsync(scheme);
				return CreatedAtRoute("GetInvoiceNumberingSchemeById", new { id = createdScheme!.Id }, createdScheme);
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}

		[HttpPut("{id:int}", Name = "UpdateInvoiceNumberingScheme")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> Put(int id, [FromBody] InvoiceNumberSchemeDto scheme)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				InvoiceNumberSchemeDto? existingScheme = await numberingService.UpdateAsync(id, scheme);
				return Ok(existingScheme);
			}
			catch (KeyNotFoundException e)
			{
				return NotFound(e.Message);
			}
			catch (Exception e)
			{
				return StatusCode(500, $"Internal server error: {e.Message}");
			}
		}

		[HttpDelete("{id:int}", Name = "DeleteInvoiceNumberingScheme")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				bool wasDeleted = await numberingService.DeleteAsync(id);
				if (!wasDeleted)
					return NotFound($"Invoice Numbering Scheme with id {id} not found");
				return Ok($"Invoice Numbering Scheme with id {id} deleted");
			}
			catch (Exception e)
			{
				return StatusCode(500, $"Internal server error: {e.Message}");
			}
		}
	}
}
