using Backend.Database;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class EntityController(ApplicationDbContext context) : ControllerBase
	{
		[HttpGet("{id:int}", Name = "GetEntityById")]
		public async Task<IActionResult> GetById(int id)
		{
			Entity? entity = await context.Entity.FindAsync(id);
			if (entity == null)
			{
				return NotFound($"Entity with id {id} not found");
			}
			return Ok(entity);
		}

		[HttpGet(Name = "GetAllEntities")]
		public async Task<IActionResult> GetAll()
		{
			IList<Entity> entityList = await context.Entity.ToListAsync();
			return Ok(entityList);
		}

		[HttpPost(Name = "PostEntity")]
		public async Task<IActionResult> Post([FromBody] Entity entity)
		{
			if (entity == null)
			{
				return BadRequest("Entity is null");
			}
			await context.Entity.AddAsync(entity);
			await context.SaveChangesAsync();
			return Ok(entity);
		}

		[HttpPut("{id:int}", Name = "PutEntity")]
		public async Task<IActionResult> Put(int id, [FromBody] Entity entity)
		{
			if (entity == null)
			{
				return BadRequest("New entity is null");
			}
			Entity? existingEntity = await context.Entity.FindAsync(id);
			if (existingEntity == null)
			{
				return NotFound($"Entity with id {id} not found");
			}
			existingEntity.Replace(entity, context);
			await context.SaveChangesAsync();
			return Ok(existingEntity);
		}

		[HttpDelete("{id:int}", Name = "DeleteEntity")]
		public async Task<IActionResult> Delete(int id)
		{
			Entity? entity = await context.Entity.FindAsync(id);
			if (entity == null)
			{
				return NotFound($"Entity with id {id} not found");
			}
			context.Entity.Remove(entity);
			await context.SaveChangesAsync();
			return Ok();
		}
	}
}
