using Application.DTOs;

namespace Application.Extensions
{
	public static class InvoiceDtoExtensions
	{
		public static decimal GetTotal(this InvoiceDto invoice)
		{
			return Math.Round(invoice.Items.Sum(item => item.UnitPrice * item.Quantity), 2, MidpointRounding.AwayFromZero);
		}

		public static decimal GetTotalWithTax(this InvoiceDto invoice)
		{
			return Math.Round(invoice.Items.Sum(item => item.UnitPrice * item.Quantity * (1 + item.VatRate)), 2, MidpointRounding.AwayFromZero);
		}
	}
}
