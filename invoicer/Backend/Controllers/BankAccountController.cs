using Backend.Database;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BankAccountController(ApplicationDbContext context) : ControllerBase
	{
		[HttpGet("{id:int}", Name = "GetBankAccountById")]
		public IActionResult GetById(int id)
		{
			BankAccount? bankAccount = context.BankAccount.Find(id);
			if (bankAccount == null)
			{
				return NotFound($"Bank account with id {id} not found");
			}
			return Ok(bankAccount);
		}

		[HttpGet(Name = "GetAllBankAccounts")]
		public IActionResult GetAll()
		{
			IList<BankAccount> bankAccountList = context.BankAccount.ToList();
			return Ok(bankAccountList);
		}

		[HttpPost(Name = "PostBankAccount")]
		public IActionResult Post([FromBody] BankAccount bankAccount)
		{
			if (bankAccount == null)
			{
				return BadRequest("Bank account is null");
			}
			context.BankAccount.Add(bankAccount);
			context.SaveChanges();
			return Ok(bankAccount);
		}

		[HttpPut("{id:int}", Name = "PutBankAccount")]
		public IActionResult Put(int id, [FromBody] BankAccount bankAccount)
		{
			if (bankAccount == null)
			{
				return BadRequest("New bank account is null");
			}
			BankAccount? existingBankAccount = context.BankAccount.Find(id);
			if (existingBankAccount == null)
			{
				return NotFound($"Bank account with id {id} not found");
			}
			existingBankAccount.Replace(bankAccount, context);
			context.SaveChanges();
			return Ok(existingBankAccount);
		}

		[HttpDelete("{id:int}", Name = "DeleteBankAccount")]
		public IActionResult Delete(int id)
		{
			BankAccount? bankAccount = context.BankAccount.Find(id);
			if (bankAccount == null)
			{
				return NotFound($"Bank account with id {id} not found");
			}
			context.BankAccount.Remove(bankAccount);
			context.SaveChanges();
			return Ok($"Bank account with id {id} deleted");
		}
	}
}
