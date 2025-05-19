namespace Domain.Models
{
	public record class Entity
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
		public BankAccount? BankAccount { get; set; }
		public Address? Address { get; set; }
		public virtual NumberingScheme? CurrentNumberingScheme { get; set; }
		public virtual EntityInvoiceNumberingSchemeState? NumberingSchemeState { get; set; }
		public virtual ICollection<Invoice> SoldInvoices { get; set; } = [];
		public virtual ICollection<Invoice> PurchasedInvoices { get; set; } = [];
	}
}
