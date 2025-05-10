using Domain.Models;

namespace Backend.Utils
{
	public static class EntityInvoiceNumberingStateUpdater
	{
		public static void SetNewSequenceNumber(EntityInvoiceNumberingSchemeState state, int newSequenceNumber)
		{
			state.LastSequenceNumber = newSequenceNumber;
			int currentYear = DateTime.Now.Year;
			int currentMonth = DateTime.Now.Month;
			if (currentYear != state.LastGenerationYear) state.LastGenerationYear = currentYear;
			if (currentMonth != state.LastGenerationMonth) state.LastGenerationMonth = currentMonth;
		}
	}
}
