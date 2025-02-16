using Backend.Models;

namespace Backend.Services.InvoiceGeneratorService.Models
{
	public readonly record struct InvoiceKey(string InvoiceNumber, Entity Seller);
}
