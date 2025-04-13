namespace Domain.Models
{
	public class Entity
	{
		public int Id { get; set; }
		public string Ico { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string PhoneNumber { get; set; } = string.Empty;
		public int BankAccountId { get; set; }
		public int AddressId { get; set; }
		public int InvoiceNumberSchemeId { get; set; }

		// Navigation properties
		public BankAccount? BankAccount { get; set; }
		public Address? Address { get; set; }
		public virtual InvoiceNumberScheme? InvoiceNumberScheme { get; set; }
		public virtual ICollection<EntityInvoiceNumberSchemeState> EntityInvoiceNumberSchemeStates { get; set; } = [];
		public virtual ICollection<Invoice> SoldInvoices { get; set; } = [];
		public virtual ICollection<Invoice> PurchasedInvoices { get; set; } = [];
	}
}
