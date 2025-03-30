using Domain.Enums;
using Domain.Extensions;
using Domain.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Backend.Utils.InvoicePdfGenerator.Models
{
	public class InvoicePdfDocument(Invoice invoiceModel) : IDocument
	{
		public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
		public DocumentSettings GetSettings() => DocumentSettings.Default;

		public void Compose(IDocumentContainer container)
		{
			container.Page(page =>
			{
				page.Margin(50);
				page.Header().Element(ComposeHeader);
				page.Content().Element(ComposeContent);
				page.Footer().Element(ComposeFooter);
			});
		}

		private void ComposeHeader(IContainer container)
		{
			container.Row(row =>
			{
				row.RelativeItem().Text($"Invoice").AlignCenter().FontSize(20).SemiBold().FontColor(Colors.Black);
			});
		}

		private void ComposeContent(IContainer container)
		{
			Currency invoiceCurrency = invoiceModel.Currency;
			container.Column(col =>
			{
				// Invoice Data
				col.Item().PaddingBottom(20).Row(row =>
				{
					row.RelativeItem().Column(col =>
					{
						col.Spacing(5);
						col.Item().Text($"Issue Date: {invoiceModel.IssueDate.FormatByCurrencyLocale(invoiceCurrency)}").SemiBold();
						col.Item().Text($"Due Date: {invoiceModel.DueDate.FormatByCurrencyLocale(invoiceCurrency)}").SemiBold();
						col.Item().Text($"VAT Date: {invoiceModel.VatDate.FormatByCurrencyLocale(invoiceCurrency)}").SemiBold();
					});

					row.RelativeItem().AlignRight().Column(col =>
					{
						col.Spacing(5);
						col.Item().Text($"Invoice Number: {invoiceModel.InvoiceNumber}").SemiBold();
						col.Item().Text($"Variable Symbol: {invoiceModel.InvoiceNumber}").SemiBold();
						col.Item().Text($"Payment Method: {invoiceModel.PaymentMethod}").SemiBold();
						col.Item().Text($"Delivery Method: {invoiceModel.DeliveryMethod}").SemiBold();
					});
				});
				// Entities Data
				col.Item().PaddingBottom(20).Border(1).Padding(10).Row(row =>
				{
					row.RelativeItem().Column(innerCol =>
					{
						innerCol.Spacing(5);
						innerCol.Item().Text("Seller").FontSize(14);
						innerCol.Item().Component(new EntityComponent(invoiceModel.Seller!));
					});

					row.RelativeItem().Column(innerCol =>
					{
						innerCol.Spacing(5);
						innerCol.Item().Text("Buyer").FontSize(14);
						innerCol.Item().Component(new EntityComponent(invoiceModel.Buyer!));
					});
				});
				// Invoice Items Data
				col.Item().Component(new InvoiceItemsComponent(invoiceModel.Items, invoiceModel.Currency));
			});
		}

		private void ComposeFooter(IContainer container)
		{
			container.AlignCenter().Text(x => x.CurrentPageNumber());
		}
	}
}
