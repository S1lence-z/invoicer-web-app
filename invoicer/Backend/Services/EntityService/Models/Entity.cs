using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models;
using Backend.Services.AddressService.Models;
using Backend.Services.BankAccountService.Models;

namespace Backend.Services.EntityService.Models
{
	public class Entity : ModelBase<Entity>, IModel
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int Ico { get; set; }

		[Required]
		public required string Name { get; set; }

		[Required]
		public string? Email { get; set; }

		[Required]
		public string? PhoneNumber { get; set; }

		[Required]
		public int BankAccountId { get; set; }

		[ForeignKey(nameof(BankAccountId))]
		public BankAccount? BankAccount { get; set; }

		[Required]
		public int AddressId { get; set; }

		[ForeignKey(nameof(AddressId))]
		public Address? Address { get; set; }
	}
}
