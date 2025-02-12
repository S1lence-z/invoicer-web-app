using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
	public class Address
	{
		[Key]
		public int Id { get; set; }

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
