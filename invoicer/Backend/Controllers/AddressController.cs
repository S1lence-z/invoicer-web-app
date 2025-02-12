using System.Text.Json;
using Backend.Database;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AddressController(ApplicationDbContext context) : ControllerBase
	{
		[HttpGet("{id:int}", Name = "GetAddressById")]
		public IActionResult GetById(int id)
		{
			Address? address = context.Address.Find(id);
			if (address == null)
			{
				return NotFound($"Addres with id {id} not found");
			}
			return Ok(address);
		}

		[HttpGet(Name = "GetAllAddresses")]
		public IActionResult GetAll()
		{
			IList<Address> addressList = context.Address.ToList();
			return Ok(addressList);
		}

		[HttpPost(Name = "PostAddress")]
		public IActionResult Post([FromBody] Address address)
		{
			if (address == null) {
				return BadRequest("Address is null");
			}
			context.Address.Add(address);
			context.SaveChanges();
			return Ok(address);
		}

		[HttpPut("{id:int}", Name = "PutAddress")]
		public IActionResult Put(int id, [FromBody] Address address)
		{
			if (address == null)
			{
				return BadRequest("New address is null");
			}
			Address? existingAddress = context.Address.Find(id);
			if (existingAddress == null)
			{
				return NotFound($"Addres with id {id} not found");
			}
			existingAddress.Replace(address, context);
			context.SaveChanges();
			return Ok(existingAddress);
		}

		[HttpDelete("{id:int}", Name = "DeleteAddress")]
		public IActionResult Delete(int id)
		{
			Address? address = context.Address.Find(id);
			if (address == null)
			{
				return NotFound($"Addres with id {id} not found");
			}
			context.Address.Remove(address);
			context.SaveChanges();
			return Ok($"Address with id {id} deleted");
		}
	}
}
