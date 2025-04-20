namespace Application.DTOs
{
	public record class EntityInvoiceNumberingSchemeStateDto
	{
		public int EntityId { get; set; }
		public int NumberingSchemeId { get; set; }
		public int LastSequenceNumber { get; set; } = 0;
		public int LastGenerationYear { get; set; } = 0;
		public int LastGenerationMonth { get; set; } = 0;
	}
}
