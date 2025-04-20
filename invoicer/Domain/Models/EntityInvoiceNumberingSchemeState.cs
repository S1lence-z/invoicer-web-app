namespace Domain.Models
{
	public class EntityInvoiceNumberingSchemeState
	{
		public int EntityId { get; set; }
		public int NumberingSchemeId { get; set; }

		// Navigation properties
		public virtual Entity Entity { get; set; } = null!;
		public virtual NumberingScheme InvoiceNumberingScheme { get; set; } = null!;

		// Current state
		public int LastSequenceNumber { get; set; } = 0;
		public int LastGenerationYear { get; set; } = 0;
		public int LastGenerationMonth { get; set; } = 0;

		public EntityInvoiceNumberingSchemeState UpdateForNext()
		{
			LastSequenceNumber++;
			LastGenerationYear = DateTime.Now.Year;
			LastGenerationMonth = DateTime.Now.Month;
			return this;
		}
	}
}
