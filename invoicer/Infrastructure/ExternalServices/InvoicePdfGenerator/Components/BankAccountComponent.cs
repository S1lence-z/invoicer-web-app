using Backend.Utils.InvoicePdfGenerator;
using Domain.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Infrastructure.ExternalServices.InvoicePdfGenerator.Components
{
	public class BankAccountComponent(BankAccount bankAccModel, string languageTag) : ComponentBase, IComponent
	{
		public void Compose(IContainer container)
		{
			container.Column(col =>
			{
				col.Item().Row(row =>
				{
					row.RelativeItem().Text(GetLocalizedText("Bank: ", "Banka: ", languageTag) + bankAccModel.BankName);
				});
				col.Item().Row(row =>
				{
					row.RelativeItem().Text(GetLocalizedText("Account/Bank Code: ", "Číslo účtu/Kód banky: ", languageTag) + $"{bankAccModel.AccountNumber}/{ bankAccModel.BankCode}");
				});
				col.Item().Row(row =>
				{
					row.RelativeItem().Text($"IBAN: {bankAccModel.IBAN}");
				});
			});
		}
	}
}
