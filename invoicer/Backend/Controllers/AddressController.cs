using Domain.Models;
using Backend.Utils;
using Domain.ServiceInterfaces;
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
			Address? address = await addressService.GetByIdAsync(id);
			if (address is null)
			{
				return StatusCode(404, ApiResponse<Address>.NotFound($"Address with id {id} not found"));
			}
			return StatusCode(200, ApiResponse<Address>.Ok(address));
		}

		[HttpGet(Name = "GetAllAddresses")]
		[ProducesResponseType(200)]
		public async Task<IActionResult> GetAll()
		{
			IList<Address> addressList = await addressService.GetAllAsync();
			return StatusCode(200, ApiResponse<IList<Address>>.Ok(addressList));
		}

		[HttpPost(Name = "PostAddress")]
		[ProducesResponseType(201)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> Post([FromBody] Address address)
		{
			if (address is null) {
				return StatusCode(400, ApiResponse<Address>.BadRequest("Address is null"));
			}
			// TODO: Refactor this like post in EntityController
			Address? newAddres = await addressService.CreateAsync(address);
			return StatusCode(201, ApiResponse<Address>.Created(newAddres!));
		}

		[HttpPut("{id:int}", Name = "PutAddress")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> Put(int id, [FromBody] Address address)
		{
			if (address is null)
			{
				return StatusCode(400, ApiResponse<Address>.BadRequest("New address is null"));
			}
			Address? existingAddress = await addressService.GetByIdAsync(id);
			if (existingAddress is null)
			{
				return StatusCode(404, ApiResponse<Address>.NotFound($"Address with id {id} not found"));
			}
			await addressService.UpdateAsync(id, address);
			return StatusCode(200, ApiResponse<Address>.Ok(existingAddress));
		}

		[HttpDelete("{id:int}", Name = "DeleteAddress")]
		public async Task<IActionResult> Delete(int id)
		{
			bool wasDeleted = await addressService.DeleteAsync(id);
			if (!wasDeleted)
			{
				return StatusCode(404, ApiResponse<Address>.NotFound($"Address with id {id} not found"));
			}
			return StatusCode(200, ApiResponse<bool>.Ok(true));
		}
	}
}
