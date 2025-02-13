using Backend.Database;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AddressController(ApplicationDbContext context) : ControllerBase
	{
		[HttpGet("{id:int}", Name = "GetAddressById")]
		public async Task<IActionResult> GetById(int id)
		{
			Address? address = await context.Address.FindAsync(id);
			if (address == null)
			{
				return NotFound($"Address with id {id} not found");
			}
			return Ok(address);
		}

		[HttpGet(Name = "GetAllAddresses")]
		public async Task<IActionResult> GetAll()
		{
			IList<Address> addressList = await context.Address.ToListAsync();
			return Ok(addressList);
		}

		[HttpPost(Name = "PostAddress")]
		public async Task<IActionResult> Post([FromBody] Address address)
		{
			if (address == null) {
				return BadRequest("Address is null");
			}
			await context.Address.AddAsync(address);
			await context.SaveChangesAsync();
			return CreatedAtRoute("GetAddressById", new { id = address.Id }, address);
		}

		[HttpPut("{id:int}", Name = "PutAddress")]
		public async Task<IActionResult> Put(int id, [FromBody] Address address)
		{
			if (address == null)
			{
				return BadRequest("New address is null");
			}
			Address? existingAddress = await context.Address.FindAsync(id);
			if (existingAddress == null)
			{
				return NotFound($"Address with id {id} not found");
			}
			existingAddress.Replace(address, context);
			await context.SaveChangesAsync();
			return Ok(existingAddress);
		}

		[HttpDelete("{id:int}", Name = "DeleteAddress")]
		public async Task<IActionResult> Delete(int id)
		{
			Address? address = await context.Address.FindAsync(id);
			if (address == null)
			{
				return NotFound($"Address with id {id} not found");
			}
			context.Address.Remove(address);
			await context.SaveChangesAsync();
			return Ok($"Address with id {id} deleted");
		}
	}
}
