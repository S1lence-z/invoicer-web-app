using Domain.Models;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AddressController(IAddressService addressService) : ControllerBase
	{
		[HttpGet("{id:int}", Name = "GetAddressById")]
		public async Task<IActionResult> GetById(int id)
		{
			Address? address = await addressService.GetByIdAsync(id);
			if (address is null)
			{
				return NotFound($"Address with id {id} not found");
			}
			return Ok(address);
		}

		[HttpGet(Name = "GetAllAddresses")]
		public async Task<IActionResult> GetAll()
		{
			IList<Address> addressList = await addressService.GetAllAsync();
			return Ok(addressList);
		}

		[HttpPost(Name = "PostAddress")]
		public async Task<IActionResult> Post([FromBody] Address address)
		{
			if (address is null) {
				return BadRequest("Address is null");
			}
			Address newAddres = await addressService.CreateAsync(address);
			return CreatedAtRoute("GetAddressById", new { id = newAddres.Id }, newAddres);
		}

		[HttpPut("{id:int}", Name = "PutAddress")]
		public async Task<IActionResult> Put(int id, [FromBody] Address address)
		{
			if (address is null)
			{
				return BadRequest("New address is null");
			}
			Address? existingAddress = await addressService.GetByIdAsync(id);
			if (existingAddress is null)
			{
				return NotFound($"Address with id {id} not found");
			}
			await addressService.UpdateAsync(id, address);
			return Ok(existingAddress);
		}

		[HttpDelete("{id:int}", Name = "DeleteAddress")]
		public async Task<IActionResult> Delete(int id)
		{
			bool wasDeleted = await addressService.DeleteAsync(id);
			if (!wasDeleted)
			{
				return NotFound($"Address with id {id} not found");
			}
			return Ok($"Address with id {id} was deleted");
		}
	}
}
