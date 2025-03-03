using Backend.Utils.InvoicePdfGenerator.Models;
using Domain.Interfaces;
using Domain.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Backend.Utils.InvoicePdfGenerator
{
	public class InvoicePdfGenerator : IInvoicePdfGenerator
	{
		public static async Task<byte[]?> ExportInvoicePdf(Invoice invoice)
		{
			return await Task.Run(() =>
			{
				try
				{
					QuestPDF.Settings.License = LicenseType.Community;
					InvoicePdfDocument newDoc = new(invoice);
					byte[] pdfFile = newDoc.GeneratePdf();
					return pdfFile;
				} 
				catch (Exception e)
				{
					return null;
				}
			});
		}
	}
}
