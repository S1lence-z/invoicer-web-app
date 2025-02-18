using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.InvoiceService.Models
{
	public class InvoiceItem : ModelBase<InvoiceItem>, IModel
	{
		[Key]
		public int Id { get; set; }

		// Invoice properties
		[Required]
		public required int InvoiceId { get; set; }

		[ForeignKey(nameof(InvoiceId))]
		public Invoice? Invoice { get; set; }

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