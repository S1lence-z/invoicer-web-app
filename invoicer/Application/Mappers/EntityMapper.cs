using Application.DTOs;
using Domain.Models;

namespace Application.Mappers
{
	public class EntityMapper
	{
		public static Entity MapToDomain(EntityDto entityDto)
		{
			return new Entity
			{
				Ico = entityDto.Ico,
				Name = entityDto.Name,
				Email = entityDto.Email,
				PhoneNumber = entityDto.PhoneNumber,
				BankAccountId = entityDto.BankAccountId,
				AddressId = entityDto.AddressId,
				CurrentNumberingSchemeId = entityDto.CurrentNumberingSchemeId,
				IsClient = entityDto.IsClient
			};
		}

		public static EntityDto MapToDto(Entity entity)
		{
			return new EntityDto
			{
				Id = entity.Id,
				Ico = entity.Ico,
				Name = entity.Name,
				Email = entity.Email,
				PhoneNumber = entity.PhoneNumber,
				BankAccountId = entity.BankAccountId,
				AddressId = entity.AddressId,
				BankAccount = BankAccountMapper.MapToDto(entity.BankAccount!),
				Address = AddressMapper.MapToDto(entity.Address!),
				CurrentNumberingSchemeId = entity.CurrentNumberingSchemeId,
				IsClient = entity.IsClient
			};
		}
	}
}
