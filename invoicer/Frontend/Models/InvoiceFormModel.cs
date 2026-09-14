using System.ComponentModel.DataAnnotations;
using Frontend.Validators;
using Shared.Enums;
using Application.DTOs;
using Frontend.Localization;
using Frontend.Models.Base;

namespace Frontend.Models
{
	public class InvoiceFormModel : FormModelBase<InvoiceFormModel, InvoiceDto>
	{
		// Seller
		[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.SellerRequired))]
		[MinValue<int>(1, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.SellerRequired))]
		public int SellerId { get; set; }

		// Buyer
		[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.BuyerRequired))]
		[MinValue<int>(1, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.BuyerRequired))]
		public int BuyerId { get; set; }

		// Invoice attributes

		[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.InvoiceNumberRequired))]
		public string InvoiceNumber { get; set; } = string.Empty;

		[Required]
		public bool IsCustomInvoiceNumber { get; set; } = false;

		[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.IssueDateRequired))]
		[DataType(DataType.DateTime)]
		public DateTime IssueDate { get; set; } = DateTime.Now;

		[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.DueDateRequired))]
		[DataType(DataType.DateTime)]
		public DateTime DueDate { get; set; } = DateTime.Now.AddDays(14);

		[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.VatDateRequired))]
		[DataType(DataType.DateTime)]
		public DateTime VatDate { get; set; } = DateTime.Now;

		[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.StatusRequired))]
		[EnumDataType(typeof(InvoiceStatus), ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.StatusInvalid))]
		public InvoiceStatus Status { get; set; } = InvoiceStatus.Pending;

		[Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.CurrencyRequired))]
		[EnumDataType(typeof(Currency), ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.CurrencyInvalid))]
		public Currency Currency { get; set; } = Currency.CZK;

		// Optional: null means the invoice does not state a payment method
		public PaymentMethod? PaymentMethod { get; set; }

		// Optional: null means the invoice does not state a delivery method
		public DeliveryMethod? DeliveryMethod { get; set; }

		// Optional: name of the person who signs the invoice
		public string SignedBy { get; set; } = string.Empty;

		[MinItemsRequired(1, typeof(InvoiceItemFormModel), ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.AtLeastOneItemRequired))]
		public IList<InvoiceItemFormModel> Items { get; set; } = [];

		public override InvoiceDto ToDto()
		{
			return new InvoiceDto
			{
				Id = Id,
				SellerId = SellerId,
				BuyerId = BuyerId,
				InvoiceNumber = InvoiceNumber,
				IsCustomInvoiceNumber = IsCustomInvoiceNumber,
				IssueDate = IssueDate,
				DueDate = DueDate,
				VatDate = VatDate,
				Status = Status,
				Currency = Currency,
				PaymentMethod = PaymentMethod,
				DeliveryMethod = DeliveryMethod,
				SignedBy = SignedBy,
				Items = [.. Items.Select(item => item.ToDto())]
			};
		}

		protected override void LoadFromDto(InvoiceDto dto)
		{
			Id = dto.Id;
			SellerId = dto.SellerId;
			BuyerId = dto.BuyerId;
			InvoiceNumber = dto.InvoiceNumber;
			IsCustomInvoiceNumber = dto.IsCustomInvoiceNumber;
			IssueDate = dto.IssueDate;
			DueDate = dto.DueDate;
			VatDate = dto.VatDate;
			Status = dto.Status;
			Currency = dto.Currency;
			PaymentMethod = dto.PaymentMethod;
			DeliveryMethod = dto.DeliveryMethod;
			SignedBy = dto.SignedBy;
			Items = [.. dto.Items.Select(InvoiceItemFormModel.FromDto)];
		}

		protected override void ResetProperties()
		{
			Id = 0;
			SellerId = 0;
			BuyerId = 0;
			InvoiceNumber = string.Empty;
			IsCustomInvoiceNumber = false;
			IssueDate = DateTime.Now;
			DueDate = DateTime.Now.AddDays(14);
			VatDate = DateTime.Now;
			Status = InvoiceStatus.Pending;
			Currency = Currency.CZK;
			PaymentMethod = null;
			DeliveryMethod = null;
			SignedBy = string.Empty;
			Items.Clear();
		}

		public void AddItem(InvoiceItemFormModel item) => Items.Add(item);

		public void RemoveItem(InvoiceItemFormModel item) => Items?.Remove(item);
	}
}
