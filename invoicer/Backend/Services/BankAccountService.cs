using Backend.Database;
using Domain.Models;
using Domain.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
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

		public async Task<BankAccount?> CreateAsync(BankAccount newBankAcc)
		{
			// TODO: Add db exception handling
			await context.BankAccount.AddAsync(newBankAcc);
			await context.SaveChangesAsync();
			return newBankAcc;
		}

		public async Task<BankAccount?> UpdateAsync(int id, BankAccount updatedBankAcc)
		{
			BankAccount? bankAcc = await context.BankAccount.FindAsync(id);
			if (bankAcc is null)
				throw new KeyNotFoundException($"Bank account with id {id} not found");

			bankAcc.BankName = updatedBankAcc.BankName;
			bankAcc.AccountNumber = updatedBankAcc.AccountNumber;
			bankAcc.BankCode = updatedBankAcc.BankCode;
			bankAcc.IBAN = updatedBankAcc.IBAN;

			await context.SaveChangesAsync();
			return bankAcc;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			BankAccount? bankAcc = await context.BankAccount.FindAsync(id);
			if (bankAcc is null)
				return false;
			context.BankAccount.Remove(bankAcc);
			await context.SaveChangesAsync();
			return true;
		}
	}
}
