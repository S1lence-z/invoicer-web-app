using Domain.Models;
using QuestPDF.Infrastructure;
using QuestPDF.Fluent;

namespace Backend.Utils.InvoicePdfGenerator.Models
{
	public class AddressComponent(Address addressModel) : IComponent
	{
		public void Compose(IContainer container)
		{
			container.Column(col =>
			{
				col.Item().Row(row =>
				{
					row.RelativeItem().Text($"{addressModel.Street}");
				});
				col.Item().Row(row =>
				{
					row.RelativeItem().Text($"{addressModel.ZipCode} {addressModel.City}, {addressModel.Country}");
				});
			});
		}
	}
}
