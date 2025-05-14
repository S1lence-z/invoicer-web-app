using Backend.Utils.InvoicePdfGenerator;
using Domain.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Shared.Enums;
using Shared.Extensions;

namespace Infrastructure.ExternalServices.InvoicePdfGenerator.Components
{
	public class InvoiceItemsComponent(ICollection<InvoiceItem> items, Currency invoiceCurrency, string languageTag) : ComponentBase, IComponent
	{
		private void ComposeInvoiceItemsTable(IContainer container)
		{
			container.Table(table =>
			{
				table.ColumnsDefinition(columns =>
				{
					columns.RelativeColumn(90); // Description
					columns.ConstantColumn(60); // Quantity
					columns.ConstantColumn(50); // Unit
					columns.ConstantColumn(100); // Unit Price
					columns.ConstantColumn(100); // Total Price
					columns.ConstantColumn(50); // Vat Rate
				});
				table.Header(header =>
				{
					header.Cell().Text(GetLocalizedText("Description", "Popis", languageTag)).Bold();
					header.Cell().Text(GetLocalizedText("Qty", "Množství", languageTag)).Bold();
					header.Cell().Text(GetLocalizedText("Unit", "MJ", languageTag)).Bold();
					header.Cell().Text(GetLocalizedText("Unit Price", "Cena MJ", languageTag)).Bold();
					header.Cell().Text(GetLocalizedText("Total", "Celkem", languageTag)).Bold();
					header.Cell().Text(GetLocalizedText("VAT Rate", "Sazba DPH", languageTag)).Bold();
				});
				foreach (var item in items)
				{
					table.Cell().Text(item.Description);
					table.Cell().Text(item.Quantity.ToString());
					table.Cell().Text(item.Unit);
					table.Cell().Text(invoiceCurrency.FormatAmount(item.UnitPrice));
					table.Cell().Text(invoiceCurrency.FormatAmount(item.Quantity * item.UnitPrice));
					table.Cell().Text($"{(int)(item.VatRate * 100)}%");
				}
			});
		}

		private void ComposeInvoicePriceTable(IContainer container)
		{
			var totalPrice = items.Sum(item => item.Quantity * item.UnitPrice);
			var vatPrice = items.Sum(item => item.Quantity * item.UnitPrice * item.VatRate);
			var totalPriceWithVat = totalPrice + vatPrice;

			container.Table(table =>
			{
				table.ColumnsDefinition(columns =>
				{
					columns.RelativeColumn();
					columns.RelativeColumn();
					columns.RelativeColumn();
				});

				table.Header(header =>
				{
					header.Cell().Text(GetLocalizedText("Total Price without VAT", "Celková cena bez DPH", languageTag)).Bold().AlignCenter();
					header.Cell().Text(GetLocalizedText("Total VAT", "Celkové DPH", languageTag)).Bold().AlignCenter();
					header.Cell().Text(GetLocalizedText("Total Price with VAT", "Celková cena s DPH", languageTag)).Bold().AlignCenter();
				});
				// Row with prices
				table.Cell().Text(invoiceCurrency.FormatAmount(totalPrice)).AlignCenter();
				table.Cell().Text(invoiceCurrency.FormatAmount(vatPrice)).AlignCenter();
				table.Cell().Text(invoiceCurrency.FormatAmount(totalPriceWithVat)).AlignCenter();
				// The final row
				table.Cell().ColumnSpan(3)
					.PaddingTop(20)
					.AlignRight()
					.Padding(10)
					.Text(text =>
					{
						text.Span(GetLocalizedText("Final Price: ", "Konečná cena: ", languageTag)).Bold().FontSize(14);
						text.Span(invoiceCurrency.FormatAmount(totalPriceWithVat)).FontSize(14);
					});
			});
		}

		public void Compose(IContainer container)
		{
			container.Column(col =>
			{
				col.Item().PaddingBottom(100).Element(ComposeInvoiceItemsTable);
				col.Item().Element(ComposeInvoicePriceTable);
			});
		}
	}
}
