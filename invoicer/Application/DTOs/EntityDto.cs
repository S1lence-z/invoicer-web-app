namespace Application.DTOs
{
	public record class EntityDto
	{
		public int Id { get; set; }
		public string Ico { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string PhoneNumber { get; set; } = string.Empty;
		public int BankAccountId { get; set; }
		public int AddressId { get; set; }
		public int CurrentNumberingSchemeId { get; set; }
		public bool IsClient { get; set; }

		// Navigation properties
		public BankAccountDto? BankAccount { get; set; }
		public AddressDto? Address { get; set; }
	}
}
