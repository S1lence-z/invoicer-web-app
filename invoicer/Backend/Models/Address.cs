using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
	public sealed class Address : ModelBase<Address>
	{
		[Key]
		public int Id { get; private set; }

		[Required]
		public required string Street { get; set; }
		
		[Required]
		public required string City { get; set; }

		[Required]
		public required int ZipCode { get; set; }

		[Required]
		public required string Country { get; set; }
	}
}
