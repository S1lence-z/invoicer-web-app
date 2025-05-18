using Application.Interfaces;
using Domain.Models;

namespace Application.RepositoryInterfaces
{
	public interface IAddressRepository : IRepository
	{
		Task<Address> GetByIdAsync(int id, bool isReadonly);
		Task<IEnumerable<Address>> GetAllAsync();
		Task<Address> CreateAsync(Address newAddress);
		Address Update(Address updatedAddress);
		Task<bool> DeleteAsync(int id);
	}
}
