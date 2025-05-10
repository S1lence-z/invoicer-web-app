using Domain.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Backend.Utils.InvoicePdfGenerator.Components
{
	public class EntityComponent(Entity entityModel, string languageTag) : ComponentBase, IComponent
	{
		public void Compose(IContainer container)
		{
			container.Column(col =>
			{
				col.Item().Text($"{entityModel.Name}").Bold();
				col.Item().Component(new AddressComponent(entityModel.Address!));
				col.Item().Text(Environment.NewLine);
				if (entityModel.BankAccount is not null)
				{
					col.Item().Component(new BankAccountComponent(entityModel.BankAccount, languageTag));
					col.Item().Text(Environment.NewLine);
				}

				col.Item().Text($"{GetLocalizedText("ICO:", "IČO:", languageTag)} {entityModel.Ico}");
				col.Item().Text($"{GetLocalizedText("Email:", "Email:", languageTag)} {entityModel.Email}");
				col.Item().Text($"{GetLocalizedText("Phone:", "Telefon:", languageTag)}  {entityModel.PhoneNumber}");
			});
		}
	}
}
