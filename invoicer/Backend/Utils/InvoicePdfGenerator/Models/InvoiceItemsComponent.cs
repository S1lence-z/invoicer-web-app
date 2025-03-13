using Domain.Enums;
using Domain.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Backend.Utils.InvoicePdfGenerator.Models
{
	public class InvoiceItemsComponent(ICollection<InvoiceItem> items, Currency invoiceCurrency) : IComponent
	{
		private decimal TotalPriceWithoutVat { get; set; }

		private void ComposeInvoiceItemsTable(IContainer container)
		{
			container.Table(table =>
			{
				table.ColumnsDefinition(columns =>
				{
					columns.RelativeColumn(100); // Description
					columns.ConstantColumn(50); // Quantity
					columns.ConstantColumn(50); // Unit
					columns.ConstantColumn(100); // Unit Price
					columns.ConstantColumn(100); // Total Price
					columns.ConstantColumn(50); // Vat Rate
				});
				table.Header(header =>
				{
					header.Cell().Text("Description").Bold();
					header.Cell().Text("Qty").Bold();
					header.Cell().Text("Unit").Bold();
					header.Cell().Text("Unit Price").Bold();
					header.Cell().Text("Total").Bold();
					header.Cell().Text("VAT Rate").Bold();
				});
				foreach (var item in items)
				{
					table.Cell().Text(item.Description);
					table.Cell().Text(item.Quantity.ToString());
					table.Cell().Text(item.Unit);
					table.Cell().Text($"{item.UnitPrice} {invoiceCurrency}");
					table.Cell().Text($"{(item.Quantity * item.UnitPrice)} {invoiceCurrency}");
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
					header.Cell().Text("Total Price without VAT").Bold().AlignCenter();
					header.Cell().Text("Total VAT").Bold().AlignCenter();
					header.Cell().Text("Total Price with VAT").Bold().AlignCenter();
				});
				// Row with prices
				table.Cell().Text($"{totalPrice} {invoiceCurrency}").AlignCenter();
				table.Cell().Text($"{vatPrice:F1} {invoiceCurrency}").AlignCenter();
				table.Cell().Text($"{totalPriceWithVat:F1} {invoiceCurrency}").AlignCenter();
				// The final row
				table.Cell().ColumnSpan(3)
					.PaddingTop(20)
					.AlignRight()
					.Padding(10)
					.Text(text =>
					{
						text.Span("Final Price: ").Bold().FontSize(14);
						text.Span($"{totalPriceWithVat:F1} {invoiceCurrency}").FontSize(14);
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
