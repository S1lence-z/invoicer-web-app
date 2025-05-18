using Application.Interfaces;
using Domain.Models;

namespace Application.RepositoryInterfaces
{
	public interface IBankAccountRepository : IRepository
	{
		Task<BankAccount> GetByIdAsync(int id, bool isReadonly);
		Task<IEnumerable<BankAccount>> GetAllAsync();
		Task<BankAccount> CreateAsync(BankAccount newBankAccount);
		BankAccount Update(BankAccount updatedBankAccount);
		Task<bool> DeleteAsync(int id);
	}
}
