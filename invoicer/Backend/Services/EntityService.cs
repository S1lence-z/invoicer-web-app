using Application.DTOs;
using Application.Mappers;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces;
using Domain.Models;

namespace Backend.Services
{
	public class EntityService(IEntityRepository entityRepository, IAddressService addressService, IBankAccountService bankAccountService,  INumberingSchemeService numberingSchemeService) : IEntityService
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
			await addressService.GetByIdAsync(newEntityDto.AddressId);
			await bankAccountService.GetByIdAsync(newEntityDto.BankAccountId);

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

			existingEntity.Name = newEntityData.Name;
			existingEntity.Ico = newEntityData.Ico;
			existingEntity.Email = newEntityData.Email;
			existingEntity.PhoneNumber = newEntityData.PhoneNumber;
			existingEntity.BankAccountId = newEntityData.BankAccountId;
			existingEntity.AddressId = newEntityData.AddressId;
			existingEntity.CurrentNumberingSchemeId = newEntityData.CurrentNumberingSchemeId;
			existingEntity.IsClient = newEntityData.IsClient;

			entityRepository.Update(existingEntity);
			await entityRepository.SaveChangesAsync();

			Entity updatedEntity = await entityRepository.GetByIdAsync(id, true);
			return EntityMapper.MapToDto(updatedEntity);
		}

		public async Task<bool> DeleteAsync(int id)
		{
			Entity entityToDelete = await entityRepository.GetByIdAsync(id, false);
			bool hasExistingInvoices = await entityRepository.HasAnyInvoicesAsync(id);
			if (hasExistingInvoices)
				throw new InvalidOperationException($"{entityToDelete.Name} cannot be deleted because it has existing invoices.");

			int addressId = entityToDelete.AddressId;
			int bankAccountId = entityToDelete.BankAccountId;
			bool status = await entityRepository.DeleteAsync(id);
			bool addressStatus = await addressService.DeleteAsync(addressId);
			bool bankAccountStatus = await bankAccountService.DeleteAsync(bankAccountId);
			await entityRepository.SaveChangesAsync();
			return status && addressStatus && bankAccountStatus;
		}
	}
}
