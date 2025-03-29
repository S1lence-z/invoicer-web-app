using Domain.Models;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Backend.Utils;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BankAccountController(IBankAccountService bankAccountService) : ControllerBase
	{
		[HttpGet("{id:int}", Name = "GetBankAccountById")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> GetById(int id)
		{
			BankAccount? bankAccount = await bankAccountService.GetByIdAsync(id);
			if (bankAccount is null)
			{
				return StatusCode(404, ApiResponse<BankAccount>.NotFound($"Bank account with id {id} not found"));
			}
			return StatusCode(200, ApiResponse<BankAccount>.Ok(bankAccount));
		}

		[HttpGet(Name = "GetAllBankAccounts")]
		[ProducesResponseType(200)]
		public async Task<IActionResult> GetAll()
		{
			IList<BankAccount> bankAccountList = await bankAccountService.GetAllAsync();
			return StatusCode(200, ApiResponse<IList<BankAccount>>.Ok(bankAccountList));
		}

		[HttpPost(Name = "PostBankAccount")]
		[ProducesResponseType(201)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> Post([FromBody] BankAccount bankAccount)
		{
			if (bankAccount is null)
			{
				return StatusCode(400, ApiResponse<BankAccount>.BadRequest("Bank account is null"));
			}
			// TODO: Refactor this like post in EntityController
			BankAccount? newBankAcc = await bankAccountService.CreateAsync(bankAccount);
			return StatusCode(201, ApiResponse<BankAccount>.Created(newBankAcc!));
		}

		[HttpPut("{id:int}", Name = "PutBankAccount")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> Put(int id, [FromBody] BankAccount bankAccount)
		{
			if (bankAccount is null)
			{
				return StatusCode(400, ApiResponse<BankAccount>.BadRequest("New bank account is null"));
			}
			BankAccount? existingBankAccount = await bankAccountService.GetByIdAsync(id);
			if (existingBankAccount is null)
			{
				return StatusCode(404, ApiResponse<BankAccount>.NotFound($"Bank account with id {id} not found"));
			}
			await bankAccountService.UpdateAsync(id, bankAccount);
			return StatusCode(200, ApiResponse<BankAccount>.Ok(existingBankAccount));
		}

		[HttpDelete("{id:int}", Name = "DeleteBankAccount")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> Delete(int id)
		{
			bool wasDeleted = await bankAccountService.DeleteAsync(id);
			if (!wasDeleted)
			{
				return StatusCode(404, ApiResponse<BankAccount>.NotFound($"Bank account with id {id} not found"));
			}
			return StatusCode(200, ApiResponse<bool>.Ok(true));
		}
	}
}
