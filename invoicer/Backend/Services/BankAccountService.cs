using Application.Mappers;
using Application.RepositoryInterfaces;
using Application.ServiceInterfaces;
using Domain.Models;
using Shared.DTOs;

namespace Backend.Services
{
	public class BankAccountService(IBankAccountRepository bankAccountRepository) : IBankAccountService
	{
		public async Task<BankAccountDto> GetByIdAsync(int id)
		{
			BankAccount foundBankAcc = await bankAccountRepository.GetByIdAsync(id, true);
			return BankAccountMapper.MapToDto(foundBankAcc);
		}

		public async Task<IList<BankAccountDto>> GetAllAsync()
		{
			IEnumerable<BankAccount> allBankAccs = await bankAccountRepository.GetAllAsync();
			return allBankAccs.Select(BankAccountMapper.MapToDto).ToList();
		}

		public async Task<BankAccountDto> CreateAsync(BankAccountDto newBankAcc)
		{
			BankAccount bankAcc = BankAccountMapper.MapToDomain(newBankAcc);
			await bankAccountRepository.CreateAsync(bankAcc);
			await bankAccountRepository.SaveChangesAsync();
			return BankAccountMapper.MapToDto(bankAcc);
		}

		public async Task<BankAccountDto> UpdateAsync(int id, BankAccountDto updatedBankAcc)
		{
			BankAccount existingBankAcc = await bankAccountRepository.GetByIdAsync(id, false);
			existingBankAcc.BankName = updatedBankAcc.BankName;
			existingBankAcc.AccountNumber = updatedBankAcc.AccountNumber;
			existingBankAcc.BankCode = updatedBankAcc.BankCode;
			existingBankAcc.IBAN = updatedBankAcc.IBAN;

			bankAccountRepository.Update(existingBankAcc);
			await bankAccountRepository.SaveChangesAsync();
			return BankAccountMapper.MapToDto(existingBankAcc);
		}

		public async Task<bool> DeleteAsync(int id)
		{
			await bankAccountRepository.DeleteAsync(id);
			await bankAccountRepository.SaveChangesAsync();
			return true;
		}
	}
}
