using Domain.Models;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BankAccountController(IBankAccountService bankAccountService) : ControllerBase
	{
		[HttpGet("{id:int}", Name = "GetBankAccountById")]
		public async Task<IActionResult> GetById(int id)
		{
			BankAccount? bankAccount = await bankAccountService.GetByIdAsync(id);
			if (bankAccount is null)
			{
				return NotFound($"Bank account with id {id} not found");
			}
			return Ok(bankAccount);
		}

		[HttpGet(Name = "GetAllBankAccounts")]
		public async Task<IActionResult> GetAll()
		{
			IList<BankAccount> bankAccountList = await bankAccountService.GetAllAsync();
			return Ok(bankAccountList);
		}

		[HttpPost(Name = "PostBankAccount")]
		public async Task<IActionResult> Post([FromBody] BankAccount bankAccount)
		{
			if (bankAccount is null)
			{
				return BadRequest("Bank account is null");
			}
			BankAccount newBankAcc = await bankAccountService.CreateAsync(bankAccount);
			return CreatedAtRoute("GetBankAccountById", new { id = newBankAcc.Id }, newBankAcc);
		}

		[HttpPut("{id:int}", Name = "PutBankAccount")]
		public async Task<IActionResult> Put(int id, [FromBody] BankAccount bankAccount)
		{
			if (bankAccount is null)
			{
				return BadRequest("New bank account is null");
			}
			BankAccount? existingBankAccount = await bankAccountService.GetByIdAsync(id);
			if (existingBankAccount is null)
			{
				return NotFound($"Bank account with id {id} not found");
			}
			await bankAccountService.UpdateAsync(id, bankAccount);
			return Ok(existingBankAccount);
		}

		[HttpDelete("{id:int}", Name = "DeleteBankAccount")]
		public async Task<IActionResult> Delete(int id)
		{
			bool wasDeleted = await bankAccountService.DeleteAsync(id);
			if (!wasDeleted)
			{
				return NotFound($"Bank account with id {id} not found");
			}
			return Ok($"Bank account with id {id} deleted");
		}
	}
}
