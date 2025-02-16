using System.ComponentModel.DataAnnotations;
using Backend.Models;

namespace Backend.Services.InvoiceService.Models
{
	public class InvoiceItem : ModelBase<InvoiceItem>
	{
		[Key]
		public int Id { get; set; }

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