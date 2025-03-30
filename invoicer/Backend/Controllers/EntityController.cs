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
		public async Task<IActionResult> GetAll()
		{
			IList<Entity> entityList = await entityService.GetAllAsync();
			return Ok(entityList);
		}

		[HttpPost(Name = "PostEntity")]
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
