using Domain.Models;
using Shared.DTOs;

namespace Application.Mappers
{
	public class BankAccountMapper
	{
		public static BankAccountDto MapToDto(BankAccount bankAcc)
		{
			return new BankAccountDto
			{
				Id = bankAcc.Id,
				AccountNumber = bankAcc.AccountNumber,
				BankCode = bankAcc.BankCode,
				BankName = bankAcc.BankName,
				IBAN = bankAcc.IBAN
			};
		}

		public static BankAccount MapToDomain(BankAccountDto bankAccDto)
		{
			return new BankAccount
			{
				Id = bankAccDto.Id,
				AccountNumber = bankAccDto.AccountNumber,
				BankCode = bankAccDto.BankCode,
				BankName = bankAccDto.BankName,
				IBAN = bankAccDto.IBAN
			};
		}
	}
}
