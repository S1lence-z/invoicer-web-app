using Domain.Enums;

namespace Domain.Models
{
	public class EntityInvoiceNumberSchemeState
	{
		public int EntityId { get; set; }
		public virtual Entity Entity { get; set; } = null!;

		public int InvoiceNumberSchemeId { get; set; }
		public virtual InvoiceNumberScheme InvoiceNumberScheme { get; set; } = null!;

		// Current state
		public int LastSequenceNumber { get; set; } = 0;
		public int LastGenerationYear { get; set; } = 0;
		public int LastGenerationMonth { get; set; } = 0;

		public EntityInvoiceNumberSchemeState UpdateForNext()
		{
			LastSequenceNumber++;
			LastGenerationYear = DateTime.Now.Year;
			LastGenerationMonth = DateTime.Now.Month;
			return this;
		}
	}
}
