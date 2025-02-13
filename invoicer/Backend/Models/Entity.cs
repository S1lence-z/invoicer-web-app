using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
	public class Entity : ModelBase<Entity>
	{
		[Key]
		public int Id { get; private set; }

		[Required]
		public required int Ico { get; set; }

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
