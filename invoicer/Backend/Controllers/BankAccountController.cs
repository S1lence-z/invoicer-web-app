using Application.ServiceInterfaces;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

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
			try
			{
				BankAccountDto? bankAccount = await bankAccountService.GetByIdAsync(id);
				return Ok(bankAccount);
			}
			catch (KeyNotFoundException e)
			{
				return NotFound(e.Message);
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}

		[HttpGet(Name = "GetAllBankAccounts")]
		[ProducesResponseType(200)]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				IList<BankAccountDto> bankAccountList = await bankAccountService.GetAllAsync();
				return Ok(bankAccountList);
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}

		[HttpPost(Name = "PostBankAccount")]
		[ProducesResponseType(201)]
		[ProducesResponseType(400)]
		public async Task<IActionResult> Post([FromBody] BankAccountDto bankAccount)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				BankAccountDto? newBankAccount = await bankAccountService.CreateAsync(bankAccount);
				return CreatedAtRoute("GetBankAccountById", new { id = newBankAccount!.Id }, newBankAccount);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpPut("{id:int}", Name = "PutBankAccount")]
		[ProducesResponseType(200)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> Put(int id, [FromBody] BankAccountDto bankAccount)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				BankAccountDto? updatedBankAccount = await bankAccountService.UpdateAsync(id, bankAccount);
				return Ok(updatedBankAccount);
			}
			catch (KeyNotFoundException e)
			{
				return NotFound(e.Message);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpDelete("{id:int}", Name = "DeleteBankAccount")]
		[ProducesResponseType(200)]
		[ProducesResponseType(404)]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				bool wasDeleted = await bankAccountService.DeleteAsync(id);
				if (!wasDeleted)
					return NotFound($"Bank account with id {id} not found");
				return Ok($"Bank account with id {id} deleted");
			}
			catch (Exception e)
			{
				return StatusCode(500, e.Message);
			}
		}
	}
}
