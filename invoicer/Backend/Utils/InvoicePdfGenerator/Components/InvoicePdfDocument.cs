using Domain.Enums;
using Application.Extensions;
using Domain.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Backend.Utils.InvoicePdfGenerator.Components
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
				row.RelativeItem().Text($"Invoice").AlignCenter().FontSize(20).Bold().FontColor(Colors.Black);
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
						col.Item().Text(text =>
						{
							text.Span("Issue Date: ");
							text.Span(invoiceModel.IssueDate.FormatByCurrencyLocale(invoiceCurrency)).Bold();
						});

						col.Item().Text(text =>
						{
							text.Span("Due Date: ");
							text.Span(invoiceModel.DueDate.FormatByCurrencyLocale(invoiceCurrency)).Bold();
						});

						col.Item().Text(text =>
						{
							text.Span("VAT Date: ");
							text.Span(invoiceModel.VatDate.FormatByCurrencyLocale(invoiceCurrency)).Bold();
						});

					});

					row.RelativeItem().AlignRight().Column(col =>
					{
						col.Spacing(5);
						col.Item().Text(text =>
						{
							text.Span("Invoice Number: ");
							text.Span(invoiceModel.InvoiceNumber).Bold();
						});

						col.Item().Text(text =>
						{
							text.Span("Variable Symbol: ");
							text.Span(invoiceModel.InvoiceNumber).Bold();
						});

						col.Item().Text(text =>
						{
							text.Span("Payment Method: ");
							text.Span(invoiceModel.PaymentMethod.ToString().SeperateCamelCase()).Bold();
						});

						col.Item().Text(text =>
						{
							text.Span("Delivery Method: ");
							text.Span(invoiceModel.DeliveryMethod.ToString().SeperateCamelCase()).Bold();
						});

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
