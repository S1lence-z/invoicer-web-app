using Shared.Enums;

namespace Domain.Models
{
	public record class Invoice
	{
		public int Id { get; set; }
		public int SellerId { get; set; }
		public int BuyerId { get; set; }
		public string InvoiceNumber { get; set; } = string.Empty;
		public DateTime IssueDate { get; set; } = DateTime.Now;
		public DateTime DueDate { get; set; }
		public DateTime VatDate { get; set; } = DateTime.Now;
		public Currency Currency { get; set; }
		public PaymentMethod PaymentMethod { get; set; }
		public DeliveryMethod DeliveryMethod { get; set; }
		public InvoiceStatus Status { get; set; } = InvoiceStatus.Pending;
		public ICollection<InvoiceItem> Items { get; set; } = [];
		public int NumberingSchemeId { get; set; }

		// Navigation properties
		public virtual Entity Seller { get; set; } = null!;
		public virtual Entity Buyer { get; set; } = null!;
		public virtual NumberingScheme InvoiceNumberingScheme { get; set; } = null!;
	}
}
