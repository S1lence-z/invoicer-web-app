namespace Backend.Services.InvoiceGeneratorService.Models
{
	public readonly record struct InvoiceItemKey(int ItemId, string InvoiceNumber, int SellerId);
}
