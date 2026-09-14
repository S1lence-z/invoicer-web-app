using Backend.Utils.InvoicePdfGenerator;
using QuestPDF.Elements;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Infrastructure.ExternalServices.InvoicePdfGenerator.Components
{
	/// <summary>
	/// Blank line for a handwritten signature with the signer's name under it.
	/// Rendered as a dynamic component so it is re-evaluated on every page and only drawn on the last one;
	/// a plain ShowIf would be cached from the first rendering pass, where the page count is not known yet.
	/// The block always reserves the same height so the footer size does not change between pages or passes.
	/// </summary>
	public class SignatureBlockComponent(string signedBy, string languageTag) : ComponentBase, IDynamicComponent
	{
		public const float Height = 60;
		private const float SignatureLineHeight = 40;

		public DynamicComponentComposeResult Compose(DynamicContext context)
		{
			bool isLastPage = context.PageNumber == context.TotalPages;

			var content = context.CreateElement(container =>
			{
				container.MinHeight(Height).ShowIf(isLastPage).Column(col =>
				{
					col.Item().Height(SignatureLineHeight).BorderBottom(1);
					col.Item().PaddingTop(4).Text(text =>
					{
						text.Span(GetLocalizedText("Signed by: ", "Podepsal: ", languageTag)).FontSize(9);
						text.Span(signedBy).FontSize(9).Bold();
					});
				});
			});

			return new DynamicComponentComposeResult { Content = content, HasMoreContent = false };
		}
	}
}
