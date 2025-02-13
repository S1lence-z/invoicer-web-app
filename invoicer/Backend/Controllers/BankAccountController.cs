using Backend.Database;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BankAccountController(ApplicationDbContext context) : ControllerBase
	{
		[HttpGet("{id:int}", Name = "GetBankAccountById")]
		public async Task<IActionResult> GetById(int id)
		{
			BankAccount? bankAccount = await context.BankAccount.FindAsync(id);
			if (bankAccount == null)
			{
				return NotFound($"Bank account with id {id} not found");
			}
			return Ok(bankAccount);
		}

		[HttpGet(Name = "GetAllBankAccounts")]
		public async Task<IActionResult> GetAll()
		{
			IList<BankAccount> bankAccountList = await context.BankAccount.ToListAsync();
			return Ok(bankAccountList);
		}

		[HttpPost(Name = "PostBankAccount")]
		public async Task<IActionResult> Post([FromBody] BankAccount bankAccount)
		{
			if (bankAccount == null)
			{
				return BadRequest("Bank account is null");
			}
			await context.BankAccount.AddAsync(bankAccount);
			await context.SaveChangesAsync();
			return CreatedAtRoute("GetBankAccountById", new { id = bankAccount.Id }, bankAccount);
		}

		[HttpPut("{id:int}", Name = "PutBankAccount")]
		public async Task<IActionResult> Put(int id, [FromBody] BankAccount bankAccount)
		{
			if (bankAccount == null)
			{
				return BadRequest("New bank account is null");
			}
			BankAccount? existingBankAccount = await context.BankAccount.FindAsync(id);
			if (existingBankAccount == null)
			{
				return NotFound($"Bank account with id {id} not found");
			}
			existingBankAccount.Replace(bankAccount, context);
			await context.SaveChangesAsync();
			return Ok(existingBankAccount);
		}

		[HttpDelete("{id:int}", Name = "DeleteBankAccount")]
		public async Task<IActionResult> Delete(int id)
		{
			BankAccount? bankAccount = await context.BankAccount.FindAsync(id);
			if (bankAccount == null)
			{
				return NotFound($"Bank account with id {id} not found");
			}
			context.BankAccount.Remove(bankAccount);
			await context.SaveChangesAsync();
			return Ok($"Bank account with id {id} deleted");
		}
	}
}
