using System.Threading.Tasks;
using Application.RepositoryInterfaces;
using Domain.Models;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public class BankAccountRepository(ApplicationDbContext context) : IBankAccountRepository
	{
		public async Task<BankAccount> CreateAsync(BankAccount newBankAccount)
		{
			await context.BankAccount.AddAsync(newBankAccount);
			return newBankAccount;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			BankAccount bankAccount = await GetByIdAsync(id, false);
			context.BankAccount.Remove(bankAccount);
			return true;
		}

		public async Task<IEnumerable<BankAccount>> GetAllAsync()
		{
			return await context.BankAccount.AsNoTracking().ToListAsync();
		}

		public async Task<BankAccount> GetByIdAsync(int id, bool isReadonly)
		{
			if (isReadonly)
				return await context.BankAccount.AsNoTracking().FirstOrDefaultAsync(ba => ba.Id == id) ?? throw new KeyNotFoundException($"Bank account with id {id} not found");
			else
				return await context.BankAccount.FirstOrDefaultAsync(ba => ba.Id == id) ?? throw new KeyNotFoundException($"Bank account with id {id} not found");
		}

		public async Task SaveChangesAsync()
		{
			await context.SaveChangesAsync();
		}

		public BankAccount Update(BankAccount updatedBankAccount)
		{
			context.BankAccount.Update(updatedBankAccount);
			return updatedBankAccount;
		}
	}
}
