using Microsoft.AspNetCore.Mvc;
using Application.ServiceInterfaces;
using Application.DTOs;
using Application.Api;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class InvoiceNumberingController(IInvoiceNumberingService numberingService) : ControllerBase
	{
		[HttpGet("{id:int}", Name = "GetInvoiceNumberingSchemeById")]
		[ProducesResponseType(typeof(InvoiceNumberSchemeDto), 200)]
		[ProducesResponseType(typeof(ApiErrorResponse), 404)]
		[ProducesResponseType(typeof(ApiErrorResponse), 500)]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				InvoiceNumberSchemeDto? scheme = await numberingService.GetByIdAsync(id);
				return Ok(scheme);
			}
			catch (KeyNotFoundException e)
			{
				return NotFound(ApiErrorResponse.Create("Invoice numbering scheme not found", e.Message, 404));
			}
			catch (Exception e)
			{
				return StatusCode(500, ApiErrorResponse.Create("Internal server error", e.Message, 500));
			}
		}

		[HttpGet(Name = "GetAllInvoiceNumberingSchemes")]
		[ProducesResponseType(typeof(IList<InvoiceNumberSchemeDto>), 200)]
		[ProducesResponseType(typeof(ApiErrorResponse), 500)]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				IList<InvoiceNumberSchemeDto> schemes = await numberingService.GetAllAsync();
				return Ok(schemes);
			}
			catch (Exception e)
			{
				return StatusCode(500, ApiErrorResponse.Create("Internal server error", e.Message, 500));
			}
		}

		[HttpPost(Name = "PostInvoiceNumberingScheme")]
		[ProducesResponseType(typeof(InvoiceNumberSchemeDto), 201)]
		[ProducesResponseType(typeof(ApiErrorResponse), 400)]
		[ProducesResponseType(typeof(ApiErrorResponse), 500)]
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
				return StatusCode(500, ApiErrorResponse.Create("Internal server error", e.Message, 500));
			}
		}

		[HttpPut("{id:int}", Name = "UpdateInvoiceNumberingScheme")]
		[ProducesResponseType(typeof(InvoiceNumberSchemeDto), 200)]
		[ProducesResponseType(typeof(ApiErrorResponse), 400)]
		[ProducesResponseType(typeof(ApiErrorResponse), 404)]
		[ProducesResponseType(typeof(ApiErrorResponse), 500)]
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
				return NotFound(ApiErrorResponse.Create("Invoice numbering scheme not found", e.Message, 404));
			}
			catch (Exception e)
			{
				return StatusCode(500, ApiErrorResponse.Create("Internal server error", e.Message, 500));
			}
		}

		[HttpDelete("{id:int}", Name = "DeleteInvoiceNumberingScheme")]
		[ProducesResponseType(typeof(string), 200)]
		[ProducesResponseType(typeof(ApiErrorResponse), 404)]
		[ProducesResponseType(typeof(ApiErrorResponse), 500)]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				bool wasDeleted = await numberingService.DeleteAsync(id);
				if (!wasDeleted)
					return NotFound(
						ApiErrorResponse.Create("Invoice numbering scheme not found", $"No scheme found with id {id}", 404)
						);
				return Ok($"Invoice Numbering Scheme with id {id} deleted");
			}
			catch (Exception e)
			{
				return StatusCode(500, ApiErrorResponse.Create("Internal server error", e.Message, 500));
			}
		}
	}
}
