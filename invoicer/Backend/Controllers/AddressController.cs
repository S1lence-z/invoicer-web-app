using Backend.Database;
using Backend.Services.AddressService;
using Backend.Services.AddressService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
			if (address == null)
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
			if (address == null) {
				return BadRequest("Address is null");
			}
			await addressService.CreateAsync(address);
			return CreatedAtRoute("GetAddressById", new { id = address.Id }, address);
		}

		[HttpPut("{id:int}", Name = "PutAddress")]
		public async Task<IActionResult> Put(int id, [FromBody] Address address)
		{
			if (address == null)
			{
				return BadRequest("New address is null");
			}
			Address? existingAddress = await addressService.GetByIdAsync(id);
			if (existingAddress == null)
			{
				return NotFound($"Address with id {id} not found");
			}
			await addressService.UpdateAsync(id, address);
			return Ok(existingAddress);
		}

		[HttpDelete("{id:int}", Name = "DeleteAddress")]
		public async Task<IActionResult> Delete(int id)
		{
			Address? address = await addressService.GetByIdAsync(id);
			if (address == null)
			{
				return NotFound($"Address with id {id} not found");
			}
			await addressService.DeleteAsync(id);
			return Ok($"Address with id {id} deleted");
		}
	}
}
