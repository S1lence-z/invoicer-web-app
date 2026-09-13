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

		/// <summary>
		/// VAT rate as a whole percent (21 = 21 %). The DTO and storage keep the fraction (0.21).
		/// </summary>
		[Required(ErrorMessage = "VAT rate is required")]
		[Range(0, 100, ErrorMessage = "VAT rate must be between 0 and 100")]
		public int VatRatePercent { get; set; } = 21;

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
				VatRate = VatRatePercent / 100m
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
			VatRatePercent = (int)Math.Round(dto.VatRate * 100);
		}

		protected override void ResetProperties()
		{
			Id = 0;
			InvoiceId = 0;
			Unit = string.Empty;
			Quantity = 0;
			Description = string.Empty;
			UnitPrice = 0;
			VatRatePercent = 21;
		}
	}
}
