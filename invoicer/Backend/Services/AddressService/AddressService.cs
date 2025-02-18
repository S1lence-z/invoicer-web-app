using System.Collections;
using Backend.Database;
using Backend.Services.AddressService.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.AddressService
{
	public class AddressService(ApplicationDbContext context) : IAddressService
	{
		public async Task<Address?> GetByIdAsync(int id)
		{
			return await context.Address.FindAsync(id);
		}

		public async Task<IList<Address>> GetAllAsync()
		{
			return await context.Address.ToListAsync();
		}

		public async Task<Address> CreateAsync(Address newAddress)
		{
			// TODO: Add db exception handling
			await context.Address.AddAsync(newAddress);
			await context.SaveChangesAsync();
			return newAddress;
		}

		public async Task<Address> UpdateAsync(int id, Address updatedAddress)
		{
			Address? existingAddress = await context.Address.FindAsync(id);
			if (existingAddress is null)
				throw new KeyNotFoundException($"Address with id {id} not found");

			existingAddress.Street = updatedAddress.Street;
			existingAddress.City = updatedAddress.City;
			existingAddress.Country = updatedAddress.Country;
			existingAddress.ZipCode = updatedAddress.ZipCode;

			await context.SaveChangesAsync();
			return existingAddress;
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
