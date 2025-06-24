using System.ComponentModel.DataAnnotations;
using Frontend.Validators;
using Shared.Enums;
using Application.DTOs;
using Frontend.Models.Base;

namespace Frontend.Models
{
	public class InvoiceFormModel : FormModelBase<InvoiceFormModel, InvoiceDto>
	{
		// Seller
		[Required(ErrorMessage = "Seller is required")]
		[MinValue<int>(1, "Seller is required")]
		public int SellerId { get; set; }

		// Buyer
		[Required(ErrorMessage = "Buyer is required")]
		[MinValue<int>(1, "Buyer is required")]
		public int BuyerId { get; set; }

		// Invoice attributes

		[Required(ErrorMessage = "Invoice number is required")]
		public string InvoiceNumber { get; set; } = string.Empty;

		[Required]
		public bool IsCustomInvoiceNumber { get; set; } = false;

		[Required(ErrorMessage = "Issue date is required")]
		[DataType(DataType.DateTime)]
		public DateTime IssueDate { get; set; } = DateTime.Now;

		[Required(ErrorMessage = "Due date is required")]
		[DataType(DataType.DateTime)]
		public DateTime DueDate { get; set; } = DateTime.Now.AddDays(14);

		[Required(ErrorMessage = "Vat Date is required")]
		[DataType(DataType.DateTime)]
		public DateTime VatDate { get; set; } = DateTime.Now;

		[Required(ErrorMessage = "Status is required")]
		[EnumDataType(typeof(InvoiceStatus), ErrorMessage = "Invalid status")]
		public InvoiceStatus Status { get; set; } = InvoiceStatus.Pending;

		[Required(ErrorMessage = "Currency is required")]
		[EnumDataType(typeof(Currency), ErrorMessage = "Invalid currency")]
		public Currency Currency { get; set; } = Currency.CZK;

		[Required(ErrorMessage = "Payment method is required")]
		[EnumDataType(typeof(PaymentMethod), ErrorMessage = "Invalid payment method")]
		public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.BankTransfer;

		[Required(ErrorMessage = "Delivery method is required")]
		[EnumDataType(typeof(DeliveryMethod), ErrorMessage = "Invalid delivery method")]
		public DeliveryMethod DeliveryMethod { get; set; } = DeliveryMethod.Courier;

		[MinItemsRequired(1, typeof(InvoiceItemFormModel), ErrorMessage = "At least one item is required")]
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
			PaymentMethod = PaymentMethod.BankTransfer;
			DeliveryMethod = DeliveryMethod.Courier;
			Items.Clear();
		}

		public void AddItem(InvoiceItemFormModel item) => Items.Add(item);

		public void RemoveItem(InvoiceItemFormModel item) => Items?.Remove(item);
	}
}
