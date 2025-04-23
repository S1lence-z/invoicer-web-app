using Domain.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Application.InvoicePdfGenerator;
using Backend.Utils.InvoicePdfGenerator.Components;
using Application.PdfGenerator;

namespace Backend.Utils.InvoicePdfGenerator
{
	public class InvoicePdfGenerator : IInvoicePdfGenerator
	{
		public IPdfGenerationResult ExportInvoicePdf(Invoice invoice)
		{
			try
			{
				QuestPDF.Settings.License = LicenseType.Community;
				InvoicePdfDocument newDoc = new(invoice);
				byte[] pdfFile = newDoc.GeneratePdf();
				return PdfGenerationResult.Success(pdfFile);
			}
			catch (Exception e)
			{
				return PdfGenerationResult.Failure(e.Message);
			}
		}
	}
}
