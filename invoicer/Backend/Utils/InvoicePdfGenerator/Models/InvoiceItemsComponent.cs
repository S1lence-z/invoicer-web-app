using Domain.Enums;
using Domain.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Backend.Utils.InvoicePdfGenerator.Models
{
	public class InvoiceItemsComponent(ICollection<InvoiceItem> items, Currency invoiceCurrency) : IComponent
	{
		private void ComposeTable(IContainer container)
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
				});
				table.Header(header =>
				{
					header.Cell().Text("Description").Bold();
					header.Cell().Text("Qty").Bold();
					header.Cell().Text("Unit").Bold();
					header.Cell().Text("Unit Price").Bold();
					header.Cell().Text("Total").Bold();
				});
				foreach (var item in items)
				{
					table.Cell().Text(item.Description);
					table.Cell().Text(item.Quantity.ToString());
					table.Cell().Text(item.Unit);
					table.Cell().Text($"{item.UnitPrice} {invoiceCurrency}");
					table.Cell().Text($"{(item.Quantity * item.UnitPrice)} {invoiceCurrency}");
				}
			});
		}

		public void Compose(IContainer container)
		{
			container.Column(col =>
			{
				col.Item().PaddingBottom(20).Element(ComposeTable);
				var totalPrice = items.Sum(item => item.Quantity * item.UnitPrice);
				col.Item().Text($"Total Price: {totalPrice} {invoiceCurrency}").AlignRight().Bold().FontSize(18);
			});
		}
	}
}
