using Application.ServiceInterfaces;
using Application.DTOs;
using Application.Mappers;
using Backend.Database;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services
{
	public class BankAccountService(ApplicationDbContext context) : IBankAccountService
	{
		public async Task<BankAccountDto?> GetByIdAsync(int id)
		{
			BankAccount? foundBankAcc = await context.BankAccount.FindAsync(id);
			if (foundBankAcc is null)
				throw new KeyNotFoundException($"Bank account with id {id} not found");
			return BankAccountMapper.MapToDto(foundBankAcc);
		}

		public async Task<IList<BankAccountDto>> GetAllAsync()
		{
			List<BankAccount> allBankAccs = await context.BankAccount.ToListAsync();
			return allBankAccs.Select(BankAccountMapper.MapToDto).ToList();
		}

		public async Task<BankAccountDto?> CreateAsync(BankAccountDto newBankAcc)
		{
			BankAccount bankAcc = BankAccountMapper.MapToDomain(newBankAcc);
			await context.BankAccount.AddAsync(bankAcc);
			await context.SaveChangesAsync();
			return BankAccountMapper.MapToDto(bankAcc);
		}

		public async Task<BankAccountDto?> UpdateAsync(int id, BankAccountDto updatedBankAcc)
		{
			BankAccount? bankAcc = await context.BankAccount.FindAsync(id);
			if (bankAcc is null)
				throw new KeyNotFoundException($"Bank account with id {id} not found");

			bankAcc.BankName = updatedBankAcc.BankName;
			bankAcc.AccountNumber = updatedBankAcc.AccountNumber;
			bankAcc.BankCode = updatedBankAcc.BankCode;
			bankAcc.IBAN = updatedBankAcc.IBAN;

			await context.SaveChangesAsync();

			return BankAccountMapper.MapToDto(bankAcc);
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
