using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using Frontend.Models.Base;

namespace Frontend.Models
{
	public class InvoiceItemFormModel : FormModelBase<InvoiceItemFormModel, InvoiceItemDto>
	{
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

		public override InvoiceItemDto ToDto()
		{
			return new InvoiceItemDto
			{
				Id = Id,
				InvoiceId = InvoiceId,
				Unit = Unit,
				Quantity = Quantity,
				Description = Description,
				UnitPrice = UnitPrice,
				VatRate = VatRate
			};
		}

		protected override void LoadFromDto(InvoiceItemDto dto)
		{
			Id = dto.Id;
			InvoiceId = dto.InvoiceId;
			Unit = dto.Unit;
			Quantity = dto.Quantity;
			Description = dto.Description;
			UnitPrice = dto.UnitPrice;
			VatRate = dto.VatRate;
		}

		protected override void ResetProperties()
		{
			Id = 0;
			InvoiceId = 0;
			Unit = string.Empty;
			Quantity = 0;
			Description = string.Empty;
			UnitPrice = 0;
			VatRate = 0.21m;
		}
	}
}
