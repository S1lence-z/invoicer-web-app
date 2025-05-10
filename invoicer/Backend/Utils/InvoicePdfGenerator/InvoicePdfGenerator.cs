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
		public IPdfGenerationResult ExportInvoicePdf(Invoice invoice, string lang)
		{
			try
			{
				QuestPDF.Settings.License = LicenseType.Community;
				string languageTag = ExtractLanguageTag(lang);
				InvoicePdfDocument newDoc = new(invoice, languageTag);
				byte[] pdfFile = newDoc.GeneratePdf();
				string fileName = CreateInvoiceName(invoice, languageTag);
				return PdfGenerationResult.Success(pdfFile, fileName);
			}
			catch (Exception e)
			{
				return PdfGenerationResult.Failure(e.Message);
			}
		}

		private static string CreateInvoiceName(Invoice invoice, string languageTag)
		{
			return $"{invoice.Seller!.Name}_{invoice.InvoiceNumber}_{languageTag}";
		}

		private static string ExtractLanguageTag(string lang)
		{
			if (string.IsNullOrEmpty(lang))
				return "en-US";
			string[] parts = lang.Split('-');
			if (parts.Length == 0)
				return "en-US";
			string language = parts[0];
			return language;
		}
	}
}
