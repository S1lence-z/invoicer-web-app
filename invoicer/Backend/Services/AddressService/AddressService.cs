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

		public async Task<Address> CreateAsync(Address obj)
		{
			// TODO: Add db exception handling
			await context.Address.AddAsync(obj);
			await context.SaveChangesAsync();
			return obj;
		}

		public async Task<Address> UpdateAsync(int id, Address obj)
		{
			Address? existingAddress = await context.Address.FindAsync(id);
			if (existingAddress == null)
				throw new KeyNotFoundException($"Address with id {id} not found");
			existingAddress.Replace(obj, context);
			await context.SaveChangesAsync();
			return existingAddress;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			Address? address = await context.Address.FindAsync(id);
			if (address == null)
				return false;
			context.Address.Remove(address);
			await context.SaveChangesAsync();
			return true;
		}
	}
}
