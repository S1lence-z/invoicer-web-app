namespace Domain.Models
{
	public class InvoiceItem
    {
		public int Id { get; set; }
		public int InvoiceId { get; set; }
		public string Unit { get; set; } = string.Empty;
		public decimal Quantity { get; set; }
		public string Description { get; set; } = string.Empty;
		public decimal UnitPrice { get; set; }
		public decimal VatRate { get; set; } = 0.21m;
	}
}
