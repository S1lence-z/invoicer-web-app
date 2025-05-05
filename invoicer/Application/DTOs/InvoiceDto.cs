using Domain.Enums;

namespace Application.DTOs
{
	public record class InvoiceDto
	{
		public int Id { get; set; }
		public int NumberingSchemeId { get; set; }
		public int SellerId { get; set; }
		public int BuyerId { get; set; }
		public EntityDto? Seller { get; set; }
		public EntityDto? Buyer { get; set; }
		public string InvoiceNumber { get; set; } = string.Empty;
		public DateTime IssueDate { get; set; } = DateTime.Now;
		public DateTime DueDate { get; set; }
		public DateTime VatDate { get; set; } = DateTime.Now;
		public Currency Currency { get; set; }
		public PaymentMethod PaymentMethod { get; set; }
		public DeliveryMethod DeliveryMethod { get; set; }
		public InvoiceStatus Status { get; set; } = InvoiceStatus.Pending;
		public ICollection<InvoiceItemDto> Items { get; set; } = [];
	}
}
