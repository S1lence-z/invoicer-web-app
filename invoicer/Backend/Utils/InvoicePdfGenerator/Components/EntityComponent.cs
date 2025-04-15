using Domain.Models;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Backend.Utils.InvoicePdfGenerator.Components
{
	public class EntityComponent(Entity entityModel) : IComponent
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
					col.Item().Component(new BankAccountComponent(entityModel.BankAccount));
					col.Item().Text(Environment.NewLine);
				}
				col.Item().Text($"IČO: {entityModel.Ico}");
				col.Item().Text($"Email: {entityModel.Email}");
				col.Item().Text($"Phone: {entityModel.PhoneNumber}");
			});
		}
	}
}
