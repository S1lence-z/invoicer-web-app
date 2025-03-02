using Domain.Interfaces;
using Domain.Models;

namespace Backend.Utils
{
	public class InvoicePdfGenerator : IInvoicePdfGenerator
	{
		public static Task<byte[]> ExportInvoicePdf(Invoice invoice)
		{
			throw new NotImplementedException();
		}
	}
}
