using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.Services.BankAccountService.Models
{
	public sealed class BankAccount : ModelBase<BankAccount>
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int AccountNumber { get; set; }

		[Required]
		public int BankCode { get; set; }

		[Required]
		public required string BankName { get; set; }

		[Required]
		public string? IBAN { get; set; }
	}
}
