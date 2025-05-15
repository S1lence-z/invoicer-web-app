using Application.DTOs;
using Application.Mappers;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces;
using Domain.Models;

namespace Backend.Services
{
	public class EntityService(IEntityRepository entityRepository, IAddressRepository addressRepository, IBankAccountRepository bankAccountRepository,  INumberingSchemeService numberingSchemeService) : IEntityService
	{
		public async Task<EntityDto> GetByIdAsync(int id)
		{
			Entity? foundEntity = await entityRepository.GetByIdAsync(id, true);
			return EntityMapper.MapToDto(foundEntity);
		}

		public async Task<IList<EntityDto>> GetAllAsync()
		{
			IEnumerable<Entity> allEntities = await entityRepository.GetAllAsync();
			return allEntities.Select(EntityMapper.MapToDto).ToList();
		}

		public async Task<EntityDto> CreateAsync(EntityDto newEntityDto)
		{
			await addressRepository.GetByIdAsync(newEntityDto.AddressId, true);
			await bankAccountRepository.GetByIdAsync(newEntityDto.BankAccountId, true);

			NumberingSchemeDto defaultScheme = await numberingSchemeService.GetDefaultNumberingSchemeAsync();
			newEntityDto.CurrentNumberingSchemeId = defaultScheme.Id;
			Entity entity = EntityMapper.MapToDomain(newEntityDto);

			await entityRepository.CreateAsync(entity);
			await entityRepository.SaveChangesAsync();

			Entity createdEntity = await entityRepository
				.GetByIdAsync(entity.Id, true);
			return EntityMapper.MapToDto(createdEntity);
		}

		public async Task<EntityDto> UpdateAsync(int id, EntityDto newEntityData)
		{
			Entity existingEntity = await entityRepository.GetByIdAsync(id, false);
			Address possibleNewAddress = await addressRepository.GetByIdAsync(newEntityData.AddressId, false);
			BankAccount possibleNewBankAcc = await bankAccountRepository.GetByIdAsync(newEntityData.BankAccountId, false);

			existingEntity.Address = possibleNewAddress;
			existingEntity.BankAccount = possibleNewBankAcc;
			existingEntity.Email = newEntityData.Email;
			existingEntity.Ico = newEntityData.Ico;
			existingEntity.Name = newEntityData.Name;
			existingEntity.PhoneNumber = newEntityData.PhoneNumber;
			existingEntity.CurrentNumberingSchemeId = newEntityData.CurrentNumberingSchemeId;
			entityRepository.Update(existingEntity);
			await entityRepository.SaveChangesAsync();

			Entity updatedEntity = await entityRepository.GetByIdAsync(id, true);
			return EntityMapper.MapToDto(updatedEntity);
		}

		public async Task<bool> DeleteAsync(int id)
		{
			Entity entityToDelete = await entityRepository.GetWithSoldInvoicesByIdAsync(id, true);
			if (entityToDelete.SoldInvoices.Count > 0)
				throw new InvalidOperationException($"{entityToDelete.Name} cannot be deleted because it has existing invoices.");

			int addressId = entityToDelete.AddressId;
			int bankAccountId = entityToDelete.BankAccountId;
			bool status = await entityRepository.DeleteAsync(id);
			bool addressStatus = await addressRepository.DeleteAsync(addressId);
			bool bankAccountStatus = await bankAccountRepository.DeleteAsync(bankAccountId);
			await entityRepository.SaveChangesAsync();
			return status && addressStatus && bankAccountStatus;
		}
	}
}
