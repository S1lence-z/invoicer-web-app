using Application.RepositoryInterfaces;
using Domain.Models;
using Infrustructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrustructure.Repositories
{
	public class AddressRepository(ApplicationDbContext context) : IAddressRepository
	{
		public async Task<Address> CreateAsync(Address newAddress)
		{
			await context.Address.AddAsync(newAddress);
			return newAddress;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			Address address = await GetByIdAsync(id, false);
			context.Address.Remove(address);
			return true;
		}

		public async Task<IEnumerable<Address>> GetAllAsync()
		{
			return await context.Address.AsNoTracking().ToListAsync();
		}

		public async Task<Address> GetByIdAsync(int id, bool isReadonly)
		{
			if (isReadonly)
				return await context.Address.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id) ?? throw new KeyNotFoundException($"Address with id {id} not found");
			else
				return await context.Address.FirstOrDefaultAsync(a => a.Id == id) ?? throw new KeyNotFoundException($"Address with id {id} not found");
		}

		public Address Update(Address updatedAddress)
		{
			context.Address.Update(updatedAddress);
			return updatedAddress;
		}

		public async Task SaveChangesAsync()
		{
			await context.SaveChangesAsync();
		}
	}
}
