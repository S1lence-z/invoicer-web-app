using Application.ServiceInterfaces;
using Application.DTOs;
using Application.Mappers;
using Backend.Database;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
	public class EntityService(ApplicationDbContext context, IInvoiceNumberingService numberingService) : IEntityService
	{
		public async Task<EntityDto?> GetByIdAsync(int id)
		{
			Entity? foundEntity = await context.Entity
				.Include(e => e.BankAccount)
				.Include(e => e.Address)
				.FirstOrDefaultAsync(e => e.Id == id);
			if (foundEntity is null)
				throw new KeyNotFoundException($"Entity with id {id} not found");
			return EntityMapper.MapToDto(foundEntity);
		}

		public async Task<IList<EntityDto>> GetAllAsync()
		{
			List<Entity> allEntities = await context.Entity
				.Include(e => e.BankAccount)
				.Include(e => e.Address)
				.ToListAsync();
			return allEntities.Select(EntityMapper.MapToDto).ToList();
		}

		public async Task<EntityDto?> CreateAsync(EntityDto newEntity)
		{
			Address? address = await context.Address.AsNoTracking().FirstOrDefaultAsync(a => a.Id == newEntity.AddressId);
			BankAccount? bankAccount = await context.BankAccount.AsNoTracking().FirstOrDefaultAsync(ba => ba.Id == newEntity.BankAccountId);

			if (address is null)
				throw new ArgumentException($"Address with id {newEntity.AddressId} not found.");

			if (bankAccount is null)
				throw new ArgumentException($"Bank account with id {newEntity.BankAccountId} not found.");

			NumberingSchemeDto? defaultScheme = await numberingService.GetDefaultNumberScheme();
			newEntity.NumberingSchemeId = defaultScheme.Id;
			Entity entity = EntityMapper.MapToDomain(newEntity);

			await context.Entity.AddAsync(entity);
			await context.SaveChangesAsync();

			Entity? createdEntity = await context.Entity
				.Include(e => e.BankAccount)
				.Include(e => e.Address)
				.AsNoTracking()
				.FirstOrDefaultAsync(e => e.Id == entity.Id);

			if (createdEntity is null)
				throw new ArgumentException($"Failed to create entity with id {entity.Id}");

			return EntityMapper.MapToDto(createdEntity);
		}

		public async Task<EntityDto?> UpdateAsync(int id, EntityDto newEntityData)
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
			existingEntity.NumberingSchemeId = newEntityData.NumberingSchemeId;

			await context.SaveChangesAsync();

			Entity? updatedEntity = await context.Entity
				.Include(e => e.BankAccount)
				.Include(e => e.Address)
				.AsNoTracking()
				.FirstOrDefaultAsync(e => e.Id == id);

			return EntityMapper.MapToDto(updatedEntity!);
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
