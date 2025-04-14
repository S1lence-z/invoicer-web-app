namespace Application.DTOs
{
	public class InvoiceDto
	{
		public int Id { get; set; }
		public int SellerId { get; set; }
		public int BuyerId { get; set; }
		public string InvoiceNumber { get; set; } = string.Empty;
		public DateTime IssueDate { get; set; } = DateTime.Now;
		public DateTime DueDate { get; set; }
		public DateTime VatDate { get; set; } = DateTime.Now;
		public string Currency { get; set; } = string.Empty;
		public string PaymentMethod { get; set; } = string.Empty;
		public string DeliveryMethod { get; set; } = string.Empty;
		public string Status { get; set; } = string.Empty;
		public ICollection<InvoiceItemDto> Items { get; set; } = [];

		// Navigation properties
		public EntityDto? Seller { get; set; }
		public EntityDto? Buyer { get; set; }
	}
}
