using Application.RepositoryInterfaces;
using Domain.Models;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public class EntityRepository(ApplicationDbContext context) : IEntityRepository
	{
		public async Task<Entity> CreateAsync(Entity entity)
		{
			await context.Entity.AddAsync(entity);
			return entity;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			Entity entity = await GetByIdAsync(id, false);
			context.Entity.Remove(entity);
			return true;
		}

		public async Task<IEnumerable<Entity>> GetAllAsync()
		{
			return await context.Entity
				.Include(e => e.BankAccount)
				.Include(e => e.Address)
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<Entity> GetByIdAsync(int id, bool isReadonly)
		{
			if (isReadonly)
				return await context.Entity
					.Include(e => e.BankAccount)
					.Include(e => e.Address)
					.AsNoTracking()
					.FirstOrDefaultAsync(e => e.Id == id) ?? throw new KeyNotFoundException($"Entity with id {id} not found.");
			else
				return await context.Entity
					.Include(e => e.BankAccount)
					.Include(e => e.Address)
					.FirstOrDefaultAsync(e => e.Id == id) ?? throw new KeyNotFoundException($"Entity with id {id} not found.");
		}

		public async Task<bool> HasAnyInvoicesAsync(int entityId)
		{
			return await context.Invoice
				.AsNoTracking()
				.AnyAsync(i => i.SellerId == entityId || i.BuyerId == entityId);
		}

		public async Task SaveChangesAsync()
		{
			await context.SaveChangesAsync();
		}

		public Entity Update(Entity entity)
		{
			context.Entity.Update(entity);
			return entity;
		}
	}
}
