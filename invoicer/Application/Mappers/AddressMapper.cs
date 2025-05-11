using Domain.Models;
using Shared.DTOs;

namespace Application.Mappers
{
	public class AddressMapper
	{
		public static AddressDto MapToDto(Address address)
		{
			return new AddressDto
			{
				Id = address.Id,
				Street = address.Street,
				City = address.City,
				Country = address.Country,
				ZipCode = address.ZipCode
			};
		}

		public static Address MapToDomain(AddressDto addressDto)
		{
			return new Address
			{
				Id = addressDto.Id,
				Street = addressDto.Street,
				City = addressDto.City,
				Country = addressDto.Country,
				ZipCode = addressDto.ZipCode
			};
		}
	}
}
