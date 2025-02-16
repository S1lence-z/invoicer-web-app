using System.ComponentModel.DataAnnotations;
using Backend.Models;
using Backend.Services.EntityService.Models;

namespace Backend.Services.InvoiceService.Models
{
	public class Invoice : ModelBase<Invoice>
	{
		[Key]
		public int Id { get; set; }

		// Invoice properties
		[Required]
		public required Entity Seller { get; set; }
		
		[Required]
		public required Entity Buyer { get; set; }

		[Required]
		public required string InvoiceNumber { get; set; }

		[Required]
		public DateTime IssueDate { get; set; } = DateTime.Now;

		[Required]
		public DateTime DueDate { get; set; }

		[Required]
		public Currency Currency { get; set; } = Currency.CZK;

		public string VariableSymbol { get; set; } = string.Empty;

		public string ConstantSymbol { get; set; } = string.Empty;

		public PaymentMethod? PaymentMethod { get; set; }

		public DeliveryMethod? DeliveryMethod { get; set; }

		[Required]
		public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
	}
}
