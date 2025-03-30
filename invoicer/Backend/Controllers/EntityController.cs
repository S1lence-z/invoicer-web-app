using Domain.Models;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class EntityController(IEntityService entityService) : ControllerBase
	{
		[HttpGet("{id:int}", Name = "GetEntityById")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> GetById(int id)
		{
			Entity? entity = await entityService.GetByIdAsync(id);
			if (entity is null)
			{
				return NotFound($"Entity with id {id} not found");
			}
			return Ok(entity);
		}

		[HttpGet(Name = "GetAllEntities")]
		[ProducesResponseType(200)]
		public async Task<IActionResult> GetAll()
		{
			IList<Entity> entityList = await entityService.GetAllAsync();
			return Ok(entityList);
		}

		[HttpPost(Name = "PostEntity")]
		[ProducesResponseType(201)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> Post([FromBody] Entity entity)
		{
			if (entity is null)
			{
				return BadRequest("Entity is null");
			}
			Entity? newEntity;
			try
			{
				newEntity = await entityService.CreateAsync(entity);
			}
			catch (ArgumentException e)
			{
				return BadRequest(e.Message);
			}
			return CreatedAtRoute("GetEntityById", new { id = newEntity!.Id }, newEntity);
		}

		[HttpPut("{id:int}", Name = "PutEntity")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> Put(int id, [FromBody] Entity entity)
		{
			if (entity is null)
			{
				return BadRequest("New entity is null");
			}
			Entity? existingEntity = await entityService.GetByIdAsync(id);	
			if (existingEntity is null)
			{
				return NotFound($"Entity with id {id} not found");
			}
			try
			{
				await entityService.UpdateAsync(id, entity);
			}
			catch (ArgumentException e)
			{
				return BadRequest(e.Message);
			}
			return Ok(existingEntity);
		}

		[HttpDelete("{id:int}", Name = "DeleteEntity")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> Delete(int id)
		{
			bool wasDeleted = await entityService.DeleteAsync(id);
			if (!wasDeleted)
			{
				return NotFound($"Entity with id {id} not found");
			}
			return Ok($"Entity with id {id} deleted");
		}
	}
}
