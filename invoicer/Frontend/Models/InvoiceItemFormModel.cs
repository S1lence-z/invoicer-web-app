using System.ComponentModel.DataAnnotations;
using Application.DTOs;
using Frontend.Localization;
using Frontend.Models.Base;

namespace Frontend.Models
{
	public class InvoiceItemFormModel : FormModelBase<InvoiceItemFormModel, InvoiceItemDto>
	{
		public int InvoiceId { get; set; }

		[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.UnitRequired))]
		public string Unit { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.QuantityRequired))]
		[Range(0.01, double.MaxValue, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.QuantityPositive))]
		public decimal Quantity { get; set; }

		[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.DescriptionRequired))]
		public string Description { get; set; } = string.Empty;

		[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.UnitPriceRequired))]
		[Range(0.01, double.MaxValue, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.UnitPricePositive))]
		public decimal UnitPrice { get; set; }

		/// <summary>
		/// VAT rate as a whole percent (21 = 21 %). The DTO and storage keep the fraction (0.21).
		/// </summary>
		[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.VatRateRequired))]
		[Range(0, 100, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.VatRateRange))]
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
