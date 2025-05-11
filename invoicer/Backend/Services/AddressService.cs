using Backend.Database;
using Application.Mappers;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs;
using Shared.ServiceInterfaces;

namespace Backend.Services
{
	public class AddressService(ApplicationDbContext context) : IAddressService
	{
		public async Task<AddressDto> GetByIdAsync(int id)
		{
			Address? foundAddress = await context.Address.FindAsync(id);
			if (foundAddress is null)
				throw new KeyNotFoundException($"Address with id {id} not found");

			return AddressMapper.MapToDto(foundAddress);
		}
		 
		public async Task<IList<AddressDto>> GetAllAsync()
		{
			List<Address> allAdresses = await context.Address.ToListAsync();
			return allAdresses.Select(AddressMapper.MapToDto).ToList();
		}

		public async Task<AddressDto> CreateAsync(AddressDto newAddress)
		{
			Address address = AddressMapper.MapToDomain(newAddress);
			await context.Address.AddAsync(address);
			await context.SaveChangesAsync();
			return AddressMapper.MapToDto(address);
		}

		public async Task<AddressDto> UpdateAsync(int id, AddressDto updatedAddress)
		{
			Address? existingAddress = await context.Address.FindAsync(id);
			if (existingAddress is null)
				throw new KeyNotFoundException($"Address with id {id} not found");

			existingAddress.Street = updatedAddress.Street;
			existingAddress.City = updatedAddress.City;
			existingAddress.Country = updatedAddress.Country;
			existingAddress.ZipCode = updatedAddress.ZipCode;

			await context.SaveChangesAsync();

			return AddressMapper.MapToDto(existingAddress);
		}

		public async Task<bool> DeleteAsync(int id)
		{
			Address? address = await context.Address.FindAsync(id);
			if (address is null)
				return false;
			context.Address.Remove(address);
			await context.SaveChangesAsync();
			return true;
		}
	}
}
