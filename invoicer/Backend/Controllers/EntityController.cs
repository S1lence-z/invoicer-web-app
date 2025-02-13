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
			Entity? entity = await context.Entity.Include(e => e.BankAccount).Include(e => e.Address).FirstOrDefaultAsync(e => e.Id == id);
			if (entity == null)
			{
				return NotFound($"Entity with id {id} not found");
			}
			return Ok(entity);
		}

		[HttpGet(Name = "GetAllEntities")]
		public async Task<IActionResult> GetAll()
		{
			IList<Entity> entityList = await context.Entity.Include(e => e.BankAccount).Include(e => e.Address).ToListAsync();
			return Ok(entityList);
		}

		[HttpPost(Name = "PostEntity")]
		public async Task<IActionResult> Post([FromBody] Entity entity)
		{
			if (entity == null)
			{
				return BadRequest("Entity is null");
			}

			if (entity.BankAccount != null)
			{
				var existingBankAccount = await context.BankAccount.FindAsync(entity.BankAccount.Id);
				if (existingBankAccount == null)
				{
					return BadRequest($"Bank account with id {entity.BankAccount.Id} not found.");
				}
				entity.BankAccount = existingBankAccount;
			}

			if (entity.Address != null)
			{
				var existingAddress = await context.Address.FindAsync(entity.Address.Id);
				if (existingAddress == null)
				{
					return BadRequest($"Address with id {entity.Address.Id} not found.");
				}
				entity.Address = existingAddress;
			}

			await context.Entity.AddAsync(entity);
			await context.SaveChangesAsync();
			return CreatedAtRoute("GetEntityById", new { id = entity.Id }, entity);
		}

		[HttpPut("{id:int}", Name = "PutEntity")]
		public async Task<IActionResult> Put(int id, [FromBody] Entity entity)
		{
			if (entity == null)
			{
				return BadRequest("New entity is null");
			}

			Entity? existingEntity = await context.Entity.Include(e => e.BankAccount).Include(e => e.Address).FirstOrDefaultAsync(e => e.Id == id);	

			if (existingEntity == null)
			{
				return NotFound($"Entity with id {id} not found");
			}
			existingEntity.Replace(entity, context);

			// Handle other related entities
			if (existingEntity.BankAccount != null)
			{
				var existingBankAccount = await context.BankAccount.FindAsync(existingEntity.BankAccount.Id);
				if (existingBankAccount == null)
				{
					return BadRequest($"Bank account with id {existingEntity.BankAccount.Id} not found.");
				}
				existingEntity.BankAccount = existingBankAccount;
			}

			if (existingEntity.Address != null)
			{
				var existingAddress = await context.Address.FindAsync(existingEntity.Address.Id);
				if (existingAddress == null)
				{
					return BadRequest($"Address with id {existingEntity.Address.Id} not found.");
				}
				existingEntity.Address = existingAddress;
			}

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
			return Ok($"Entity with id {id} deleted");
		}
	}
}
