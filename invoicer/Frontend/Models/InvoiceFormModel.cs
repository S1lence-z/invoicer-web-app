using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Frontend.Validators;
using Application.DTOs;

namespace Frontend.Models
{
	public record class InvoiceFormModel
	{
		public int Id { get; set; }

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

		public static InvoiceFormModel FromInvoice(InvoiceDto invoice)
		{
			return new InvoiceFormModel
			{
				Id = invoice.Id,
				SellerId = invoice.SellerId,
				BuyerId = invoice.BuyerId,
				InvoiceNumber = invoice.InvoiceNumber,
				IssueDate = invoice.IssueDate,
				DueDate = invoice.DueDate,
				VatDate = invoice.VatDate,
				Status = invoice.Status,
				Currency = invoice.Currency,
				PaymentMethod = invoice.PaymentMethod,
				DeliveryMethod = invoice.DeliveryMethod,
				Items = [.. invoice.Items.Select(InvoiceItemFormModel.FromInvoiceItem)]
			};
		}
	}
}
