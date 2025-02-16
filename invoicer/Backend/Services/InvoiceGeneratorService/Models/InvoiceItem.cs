using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.Services.InvoiceGeneratorService.Models
{
	public class InvoiceItem : ModelBase<InvoiceItem>
	{
		// Composite key
		[Key]
		public InvoiceItemKey Id { get; set; }

		// InvoiceItem properties
		[Required]
		public required string Unit { get; set; }

		[Required]
		public decimal Quantity { get; set; }

		[Required]
		public required string Description { get; set; }

		[Required]
		public decimal UnitPrice { get; set; }
	}
}