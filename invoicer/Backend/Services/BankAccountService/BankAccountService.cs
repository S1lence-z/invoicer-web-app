using Backend.Database;
using Backend.Services.BankAccountService.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.BankAccountService
{
	public class BankAccountService(ApplicationDbContext context) : IBankAccountService
	{
		public async Task<BankAccount?> GetByIdAsync(int id)
		{
			return await context.BankAccount.FindAsync(id);
		}

		public async Task<IList<BankAccount>> GetAllAsync()
		{
			return await context.BankAccount.ToListAsync();
		}

		public async Task<BankAccount> CreateAsync(BankAccount obj)
		{
			// TODO: Add db exception handling
			await context.BankAccount.AddAsync(obj);
			await context.SaveChangesAsync();
			return obj;
		}

		public async Task<BankAccount> UpdateAsync(int id, BankAccount obj)
		{
			BankAccount? bankAcc = await context.BankAccount.FindAsync(id);
			if (bankAcc == null)
				throw new KeyNotFoundException($"Bank account with id {id} not found");
			bankAcc.Replace(obj, context);
			await context.SaveChangesAsync();
			return bankAcc;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			BankAccount? bankAcc = await context.BankAccount.FindAsync(id);
			if (bankAcc == null)
				return false;
			context.BankAccount.Remove(bankAcc);
			await context.SaveChangesAsync();
			return true;
		}
	}
}
