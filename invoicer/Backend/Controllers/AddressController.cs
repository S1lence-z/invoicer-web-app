using Domain.Models;
using Application.ServiceInterfaces;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AddressController(IAddressService addressService) : ControllerBase
	{
		[HttpGet("{id:int}", Name = "GetAddressById")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				AddressDto? address = await addressService.GetByIdAsync(id);
				return Ok(address);
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

		[HttpGet(Name = "GetAllAddresses")]
		[ProducesResponseType(200)]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				IList<AddressDto> addressList = await addressService.GetAllAsync();
				return Ok(addressList);
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}

		[HttpPost(Name = "PostAddress")]
		[ProducesResponseType(201)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> Post([FromBody] AddressDto address)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				AddressDto? newAddres = await addressService.CreateAsync(address);
				return CreatedAtRoute("GetAddressById", new { id = newAddres!.Id }, newAddres);
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}

		[HttpPut("{id:int}", Name = "PutAddress")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> Put(int id, [FromBody] AddressDto address)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				AddressDto? existingAddress = await addressService.UpdateAsync(id, address);
				return Ok(existingAddress);
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

		[HttpDelete("{id:int}", Name = "DeleteAddress")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
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
				return StatusCode(500, e.Message);
			}
		}
	}
}
