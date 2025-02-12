using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
	public class Address
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

		public Address Replace(Address newAddress)
		{
			Street = newAddress.Street;
			City = newAddress.City;
			ZipCode = newAddress.ZipCode;
			Country = newAddress.Country;
			return this;
		}
	}
}
