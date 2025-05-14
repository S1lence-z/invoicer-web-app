using Application.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Api;
using Shared.DTOs;

namespace Backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BankAccountController(IBankAccountService bankAccountService) : ControllerBase
	{
		[HttpGet("{id:int}", Name = "GetBankAccountById")]
		[ProducesResponseType(typeof(BankAccountDto), 200)]
		[ProducesResponseType(typeof(ApiErrorResponse), 404)]
		[ProducesResponseType(typeof(ApiErrorResponse), 500)]
		public async Task<IActionResult> GetById(int id)
		{
			try
			{
				BankAccountDto? bankAccount = await bankAccountService.GetByIdAsync(id);
				return Ok(bankAccount);
			}
			catch (KeyNotFoundException e)
			{
				return NotFound(ApiErrorResponse.Create("Bank account not found", e.Message, 404));
			}
			catch (Exception e)
			{
				return StatusCode(500, ApiErrorResponse.Create("Internal server error", e.Message, 500));
			}
		}

		[HttpGet(Name = "GetAllBankAccounts")]
		[ProducesResponseType(typeof(IList<BankAccountDto>), 200)]
		[ProducesResponseType(typeof(ApiErrorResponse), 500)]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				IList<BankAccountDto> bankAccountList = await bankAccountService.GetAllAsync();
				return Ok(bankAccountList);
			}
			catch (Exception e)
			{
				return StatusCode(500, ApiErrorResponse.Create("Internal server error", e.Message, 500));
			}
		}

		[HttpPost(Name = "PostBankAccount")]
		[ProducesResponseType(typeof(BankAccountDto), 201)]
		[ProducesResponseType(typeof(ApiErrorResponse), 400)]
		[ProducesResponseType(typeof(ApiErrorResponse), 500)]
		public async Task<IActionResult> Post([FromBody] BankAccountDto bankAccount)
		{
			if (!ModelState.IsValid)
				return BadRequest(ApiErrorResponse.Create("Invalid model state", ModelState.ToString()!, 400));

			try
			{
				BankAccountDto? newBankAccount = await bankAccountService.CreateAsync(bankAccount);
				return CreatedAtRoute("GetBankAccountById", new { id = newBankAccount!.Id }, newBankAccount);
			}
			catch (Exception e)
			{
				return StatusCode(500, ApiErrorResponse.Create("Internal server error", e.Message, 500));
			}
		}

		[HttpPut("{id:int}", Name = "PutBankAccount")]
		[ProducesResponseType(typeof(BankAccountDto), 200)]
		[ProducesResponseType(typeof(ApiErrorResponse), 400)]
		[ProducesResponseType(typeof(ApiErrorResponse), 404)]
		[ProducesResponseType(typeof(ApiErrorResponse), 500)]
		public async Task<IActionResult> Put(int id, [FromBody] BankAccountDto bankAccount)
		{
			if (!ModelState.IsValid)
				return BadRequest(ApiErrorResponse.Create("Invalid model state", ModelState.ToString()!, 400));

			try
			{
				BankAccountDto? updatedBankAccount = await bankAccountService.UpdateAsync(id, bankAccount);
				return Ok(updatedBankAccount);
			}
			catch (KeyNotFoundException e)
			{
				return NotFound(ApiErrorResponse.Create("Bank account not found", e.Message, 404));
			}
			catch (Exception e)
			{
				return StatusCode(500, ApiErrorResponse.Create("Internal server error", e.Message, 500));
			}
		}

		[HttpDelete("{id:int}", Name = "DeleteBankAccount")]
		[ProducesResponseType(typeof(string), 200)]
		[ProducesResponseType(typeof(ApiErrorResponse), 404)]
		[ProducesResponseType(typeof(ApiErrorResponse), 500)]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				bool wasDeleted = await bankAccountService.DeleteAsync(id);
				if (!wasDeleted)
				{
					return NotFound(ApiErrorResponse.Create("Bank account not found", $"Bank account with id {id} not found", 404));
				}
				return Ok($"Bank account with id {id} deleted");
			}
			catch (Exception e)
			{
				return StatusCode(500, ApiErrorResponse.Create("Internal server error", e.Message, 500));
			}
		}
	}
}
