namespace Application.DTOs
{
	public record class EntityInvoiceNumberSchemeStateDto
	{
		public int EntityId { get; set; }
		public int InvoiceNumberSchemeId { get; set; }
		public int LastSequenceNumber { get; set; } = 0;
		public int LastGenerationYear { get; set; } = 0;
		public int LastGenerationMonth { get; set; } = 0;
	}
}
