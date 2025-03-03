using Backend.Database;
using Domain.Models;
using Domain.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
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

		public async Task<Entity> CreateAsync(Entity newEntity)
		{
			Address? address = await context.Address.FindAsync(newEntity.AddressId);
			BankAccount? bankAccount = await context.BankAccount.FindAsync(newEntity.BankAccountId);

			if (address is null)
				throw new ArgumentException($"Address with id {newEntity.AddressId} not found.");

			if (bankAccount is null)
				throw new ArgumentException($"Bank account with id {newEntity.BankAccountId} not found.");

			newEntity.Address = address;
			newEntity.BankAccount = bankAccount;

			context.Entity.Add(newEntity);
			await context.SaveChangesAsync();
			return newEntity;
		}

		public async Task<Entity> UpdateAsync(int id, Entity newEntityData)
		{
			Entity? existingEntity = await context.Entity.FindAsync(id);
			if (existingEntity is null)
				throw new ArgumentException($"Entity with id {id} not found.");
			
			Address? possibleNewAddres = await context.Address.FindAsync(newEntityData.AddressId);
			BankAccount? possibleNewBankAcc = await context.BankAccount.FindAsync(newEntityData.BankAccountId);
			
			if (possibleNewAddres is null)
				throw new ArgumentException($"Updated address with id {newEntityData.AddressId} not found.");
			if (possibleNewBankAcc is null)
				throw new ArgumentException($"Updated bank account with id {newEntityData.BankAccountId} not found.");

			existingEntity.AddressId = newEntityData.AddressId;
			existingEntity.BankAccountId = newEntityData.BankAccountId;
			existingEntity.Address = possibleNewAddres;
			existingEntity.BankAccount = possibleNewBankAcc;
			existingEntity.Email = newEntityData.Email;
			existingEntity.Ico = newEntityData.Ico;
			existingEntity.Name = newEntityData.Name;
			existingEntity.PhoneNumber = newEntityData.PhoneNumber;

			await context.SaveChangesAsync();
			return existingEntity;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			Entity? entity = await context.Entity.FindAsync(id);
			if (entity is null)
				return false;
			context.Entity.Remove(entity);
			await context.SaveChangesAsync();
			return true;
		}
	}
}
