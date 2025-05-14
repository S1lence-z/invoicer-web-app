using Application.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;
using Shared.DTOs;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AddressController(IAddressService addressService) : ControllerBase
	{
		[HttpGet("{id:int}", Name = "GetAddressById")]
		[ProducesResponseType(typeof(AddressDto), 200)]
		[ProducesResponseType(typeof(ApiErrorResponse), 404)]
		[ProducesResponseType(typeof(ApiErrorResponse), 500)]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				AddressDto? address = await addressService.GetByIdAsync(id);
				return Ok(address);
			}
			catch (KeyNotFoundException e)
			{
				return NotFound(ApiErrorResponse.Create("Address not found", e.Message, 404));
			}
			catch (Exception e)
			{
				return StatusCode(500, ApiErrorResponse.Create("Internal server error", e.Message, 500));
			}
		}

		[HttpGet(Name = "GetAllAddresses")]
		[ProducesResponseType(typeof(IList<AddressDto>), 200)]
		[ProducesResponseType(typeof(ApiErrorResponse), 500)]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				IList<AddressDto> addressList = await addressService.GetAllAsync();
				return Ok(addressList);
			}
			catch (Exception e)
			{
				return StatusCode(500, ApiErrorResponse.Create("Internal server error", e.Message, 500));
			}
		}

		[HttpPost(Name = "PostAddress")]
		[ProducesResponseType(typeof(AddressDto), 201)]
		[ProducesResponseType(typeof(ApiErrorResponse), 400)]
		[ProducesResponseType(typeof(ApiErrorResponse), 500)]
		public async Task<IActionResult> Post([FromBody] AddressDto address)
		{
			if (!ModelState.IsValid)
				return BadRequest(ApiErrorResponse.Create("Invalid model state", ModelState.ToString()!, 400));

			try
			{
				AddressDto? newAddres = await addressService.CreateAsync(address);
				return CreatedAtRoute("GetAddressById", new { id = newAddres!.Id }, newAddres);
			}
			catch (Exception e)
			{
				return StatusCode(500, ApiErrorResponse.Create("Internal server error", e.Message, 500));
			}
		}

		[HttpPut("{id:int}", Name = "PutAddress")]
		[ProducesResponseType(typeof(AddressDto), 200)]
		[ProducesResponseType(typeof(ApiErrorResponse), 400)]
		[ProducesResponseType(typeof(ApiErrorResponse), 404)]
		[ProducesResponseType(typeof(ApiErrorResponse), 500)]
		public async Task<IActionResult> Put(int id, [FromBody] AddressDto address)
		{
			if (!ModelState.IsValid)
				return BadRequest(ApiErrorResponse.Create("Invalid model state", ModelState.ToString()!, 400));

			try
			{
				AddressDto? existingAddress = await addressService.UpdateAsync(id, address);
				return Ok(existingAddress);
			}
			catch (KeyNotFoundException e)
			{
				return NotFound(ApiErrorResponse.Create("Address not found", e.Message, 404));
			}
			catch (Exception e)
			{
				return StatusCode(500, ApiErrorResponse.Create("Internal server error", e.Message, 500));
			}
		}

		[HttpDelete("{id:int}", Name = "DeleteAddress")]
		[ProducesResponseType(typeof(string), 200)]
		[ProducesResponseType(typeof(ApiErrorResponse), 404)]
		[ProducesResponseType(typeof(ApiErrorResponse), 500)]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				bool wasDeleted = await addressService.DeleteAsync(id);
				if (!wasDeleted)
				{
					return NotFound($"Address with id {id} not found");
				}
				return Ok($"Address with id {id} was deleted");
			}
			catch (Exception e)
			{
				return StatusCode(500, ApiErrorResponse.Create("Internal server error", e.Message, 500));
			}
		}
	}
}
