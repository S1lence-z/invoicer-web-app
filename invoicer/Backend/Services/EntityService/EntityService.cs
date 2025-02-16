using System.Runtime.InteropServices;
using Backend.Database;
using Backend.Services.EntityService.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.EntityService
{
	public class EntityService(ApplicationDbContext context) : IEntityService
	{
		public async Task<Entity?> GetByIdAsync(int id)
		{
			return await context.Entity
				.Include(e => e.BankAccount)
				.Include(e => e.Address)
				.FirstOrDefaultAsync(e => e.Id == id);
		}

		public async Task<IList<Entity>> GetAllAsync()
		{
			return await context.Entity
				.Include(e => e.BankAccount)
				.Include(e => e.Address)
				.ToListAsync();
		}

		public async Task<Entity> CreateAsync(Entity obj)
		{
			// Check if the bank account exists
			if (obj.BankAccount != null)
			{
				var existingBankAccount = await context.BankAccount.FindAsync(obj.BankAccount.Id);
				if (existingBankAccount == null)
					throw new ArgumentException($"Bank account with id {obj.BankAccount.Id} not found.");
				obj.BankAccount = existingBankAccount;
			}

			// Check if the address exists
			if (obj.Address != null)
			{
				var existingAddress = await context.Address.FindAsync(obj.Address.Id);
				if (existingAddress == null)
					throw new ArgumentException($"Address with id {obj.Address.Id} not found.");
				obj.Address = existingAddress;
			}

			await context.Entity.AddAsync(obj);
			await context.SaveChangesAsync();
			return obj;
		}

		public async Task<Entity> UpdateAsync(int id, Entity obj)
		{
			Entity? existingEntity = await context.Entity
				.Include(e => e.BankAccount)
				.Include(e => e.Address)
				.FirstOrDefaultAsync(e => e.Id == id);

			if (existingEntity == null)
				throw new ArgumentException($"Entity with id {id} not found.");
			existingEntity.Replace(obj, context);

			// Check if the bank account exists
			if (existingEntity.BankAccount != null)
			{
				var existingBankAccount = await context.BankAccount.FindAsync(existingEntity.BankAccount.Id);
				if (existingBankAccount == null)
					throw new ArgumentException($"Bank account with id {obj.BankAccount.Id} not found.");
				existingEntity.BankAccount = existingBankAccount;
			}

			// Check if the address exists
			if (obj.Address != null)
			{
				var existingAddress = await context.Address.FindAsync(obj.Address.Id);
				if (existingAddress == null)
					throw new ArgumentException($"Address with id {obj.Address.Id} not found.");
				existingEntity.Address = existingAddress;
			}

			await context.SaveChangesAsync();
			return existingEntity;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			Entity? entity = await context.Entity.FindAsync(id);
			if (entity == null)
				return false;
			context.Entity.Remove(entity);
			await context.SaveChangesAsync();
			return true;
		}
	}
}
