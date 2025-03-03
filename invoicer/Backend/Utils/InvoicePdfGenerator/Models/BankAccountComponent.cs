using Domain.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Backend.Utils.InvoicePdfGenerator.Models
{
	public class BankAccountComponent(BankAccount bankAccModel) : IComponent
	{
		public void Compose(IContainer container)
		{
			container.Column(col =>
			{
				col.Item().Row(row =>
				{
					row.RelativeItem().Text($"Bank: {bankAccModel.BankName}");
				});
				col.Item().Row(row =>
				{
					row.RelativeItem().Text($"Account/Bank Code: {bankAccModel.AccountNumber}/{bankAccModel.BankCode}");

				});
				col.Item().Row(row =>
				{
					row.RelativeItem().Text($"IBAN: {bankAccModel.IBAN}");
				});
			});
		}
	}
}
