using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Services.AddressService.Models;
using Backend.Services.BankAccountService.Models;

namespace Backend.Models
{
	public class Entity : ModelBase<Entity>
	{
		[Key]
		public int Id { get; private set; }

		[Required]
		public int Ico { get; set; }

		[Required]
		public required string Name { get; set; }

		[Required]
		public string? Email { get; set; }

		[Required]
		public string? PhoneNumber { get; set; }

		[Required]
		public required BankAccount BankAccount { get; set; }

		[Required]
		public required Address Address { get; set; }
	}
}
