using Domain.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Shared.Extensions;
using Shared.Enums;
using Backend.Utils.InvoicePdfGenerator;

namespace Infrastructure.ExternalServices.InvoicePdfGenerator.Components
{
	public class InvoicePdfDocument(Invoice invoiceModel, string languageTag) : ComponentBase, IDocument
	{
		private readonly string languageTag = languageTag;

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
			string headerText = GetLocalizedText("Invoice", "Faktura", languageTag);
			container.Row(row =>
			{
				row.RelativeItem().Text(headerText).AlignCenter().FontSize(20).Bold().FontColor(Colors.Black);
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
							text.Span(GetLocalizedText("Issue Date: ", "Datum vystavení: ", languageTag));
							text.Span(invoiceModel.IssueDate.FormatByCurrencyLocale(invoiceCurrency)).Bold();
						});

						col.Item().Text(text =>
						{
							text.Span(GetLocalizedText("Due Date: ", "Datum splatnosti: ", languageTag));
							text.Span(invoiceModel.DueDate.FormatByCurrencyLocale(invoiceCurrency)).Bold();
						});

						col.Item().Text(text =>
						{
							text.Span(GetLocalizedText("VAT Date: ", "Datum DPH: ", languageTag));
							text.Span(invoiceModel.VatDate.FormatByCurrencyLocale(invoiceCurrency)).Bold();
						});
					});

					row.RelativeItem().AlignRight().Column(col =>
					{
						col.Spacing(5);
						col.Item().Text(text =>
						{
							text.Span(GetLocalizedText("Invoice Number: ", "Číslo faktury: ", languageTag));
							text.Span(invoiceModel.InvoiceNumber).Bold();
						});

						col.Item().Text(text =>
						{
							text.Span(GetLocalizedText("Variable Symbol: ", "Variabilní symbol: ", languageTag));
							text.Span(invoiceModel.InvoiceNumber).Bold();
						});

						col.Item().Text(text =>
						{
							text.Span(GetLocalizedText("Payment Method: ", "Způsob platby: ", languageTag));
							text.Span(invoiceModel.PaymentMethod.ToString().SeperateCamelCase()).Bold();
						});

						col.Item().Text(text =>
						{
							text.Span(GetLocalizedText("Delivery Method: ", "Způsob doručení: ", languageTag));
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
						innerCol.Item().Text(GetLocalizedText("Seller", "Prodávající", languageTag)).FontSize(14);
						innerCol.Item().Component(new EntityComponent(invoiceModel.Seller!, languageTag));
					});

					row.RelativeItem().Column(innerCol =>
					{
						innerCol.Spacing(5);
						innerCol.Item().Text(GetLocalizedText("Buyer", "Kupující", languageTag)).FontSize(14);
						innerCol.Item().Component(new EntityComponent(invoiceModel.Buyer!, languageTag));
					});
				});

				// Invoice Items Data
				col.Item().Component(new InvoiceItemsComponent(invoiceModel.Items, invoiceModel.Currency, languageTag));
			});
		}

		private void ComposeFooter(IContainer container)
		{
			container.AlignCenter().Text(x => x.CurrentPageNumber());
		}
	}
}
