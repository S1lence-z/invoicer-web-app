namespace Application.DTOs
{
	public record class AddressDto
	{
		public int Id { get; set; }
		public string Street { get; set; } = string.Empty;
		public string City { get; set; } = string.Empty;
		public int ZipCode { get; set; }
		public string Country { get; set; } = string.Empty;
	}
}
