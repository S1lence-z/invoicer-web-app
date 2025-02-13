using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
	public sealed class Address : ModelBase<Address>
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public required string Street { get; set; }
		
		[Required]
		public required string City { get; set; }

		[Required]
		public int ZipCode { get; set; }

		[Required]
		public required string Country { get; set; }
	}
}
