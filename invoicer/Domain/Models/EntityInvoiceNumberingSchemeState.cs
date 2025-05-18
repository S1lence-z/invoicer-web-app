namespace Domain.Models
{
	public class EntityInvoiceNumberingSchemeState
	{
		// Primary key
		public int EntityId { get; set; }

		// Navigation properties
		public virtual Entity Entity { get; set; } = null!;

		// Current state
		public int LastSequenceNumber { get; set; } = 0;
		public int LastGenerationYear { get; set; } = 0;
		public int LastGenerationMonth { get; set; } = 0;

		public void SetNewSequenceNumber(int newSequenceNumber)
		{
			LastSequenceNumber = newSequenceNumber;
			SetNewGenerationYearAndMonth(DateTime.Now.Year, DateTime.Now.Month);
		}

		public void SetNewGenerationYearAndMonth(int newYear, int newMonth)
		{
			int currentYear = newYear;
			int currentMonth = newMonth;
			if (currentYear != LastGenerationYear) LastGenerationYear = currentYear;
			if (currentMonth != LastGenerationMonth) LastGenerationMonth = currentMonth;
		}
	}
}
