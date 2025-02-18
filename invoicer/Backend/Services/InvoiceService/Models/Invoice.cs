using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Models;
using Backend.Services.EntityService.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.InvoiceService.Models
{
	public class Invoice : ModelBase<Invoice>, IModel
	{
		[Key]
		public int Id { get; set; }

		// Invoice properties
		[Required]
		public required int SellerId { get; set; }

		[ForeignKey(nameof(SellerId))]
		public Entity? Seller { get; set; }

		[Required]
		public required int BuyerId { get; set; }

		[ForeignKey(nameof(BuyerId))]
		public Entity? Buyer { get; set; }

		// Invoice properties
		[Required]
		public required string InvoiceNumber { get; set; }

		[Required]
		public DateTime IssueDate { get; set; } = DateTime.Now;

		[Required]
		public DateTime DueDate { get; set; }

		[Required]
		[EnumDataType(typeof(Currency))]
		public Currency Currency { get; set; }

		[EnumDataType(typeof(PaymentMethod))]
		public PaymentMethod? PaymentMethod { get; set; }

		[EnumDataType(typeof(DeliveryMethod))]
		public DeliveryMethod? DeliveryMethod { get; set; }

		[Required]
		public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();

		protected override void SetUp(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Invoice>()
				.Property(i => i.DeliveryMethod)
				.HasConversion<string>();

			modelBuilder.Entity<Invoice>()
				.Property(i => i.PaymentMethod)
				.HasConversion<string>();

			modelBuilder.Entity<Invoice>()
				.Property(i => i.Currency)
				.HasConversion<string>();
		}
	}
}
