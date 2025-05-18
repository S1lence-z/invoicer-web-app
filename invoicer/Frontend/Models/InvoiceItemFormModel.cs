using System.ComponentModel.DataAnnotations;
using Application.DTOs;

namespace Frontend.Models
{
	public record class InvoiceItemFormModel
	{
		public int Id { get; set; }

		public int InvoiceId { get; set; }

		[Required(ErrorMessage = "Unit is required")]
		public string Unit { get; set; } = string.Empty;

		[Required(ErrorMessage = "Quantity is required")]
		[Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
		public decimal Quantity { get; set; }

		[Required(ErrorMessage = "Description is required")]
		public string Description { get; set; } = string.Empty;

		[Required(ErrorMessage = "Unit price is required")]
		[Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0")]
		public decimal UnitPrice { get; set; }

		[Required(ErrorMessage = "VAT rate is required")]
		[Range(0.0, 1.0, ErrorMessage = "VAT rate must be between 0 and 1")]
		public decimal VatRate { get; set; } = 0.21m;

		public static InvoiceItemFormModel FromInvoiceItem(InvoiceItemDto item)
		{
			return new InvoiceItemFormModel
			{
				Id = item.Id,
				InvoiceId = item.InvoiceId,
				Unit = item.Unit,
				Quantity = item.Quantity,
				Description = item.Description,
				UnitPrice = item.UnitPrice,
				VatRate = item.VatRate
			};
		}

		public static InvoiceItemDto ToInvoiceItem(InvoiceItemFormModel invFormModel)
		{
			return new InvoiceItemDto
			{
				Id = invFormModel.Id,
				InvoiceId = invFormModel.InvoiceId,
				Unit = invFormModel.Unit,
				Quantity = invFormModel.Quantity,
				Description = invFormModel.Description,
				UnitPrice = invFormModel.UnitPrice,
				VatRate = invFormModel.VatRate
			};
		}
	}
}
