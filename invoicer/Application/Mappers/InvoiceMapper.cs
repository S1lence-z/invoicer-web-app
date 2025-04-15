using Domain.Models;
using Application.DTOs;

namespace Application.Mappers
{
	public class InvoiceMapper
	{
		public static InvoiceDto MapToDto(Invoice invoice)
		{
			return new InvoiceDto
			{
				Id = invoice.Id,
				SellerId = invoice.SellerId,
				BuyerId = invoice.BuyerId,
				Seller = EntityMapper.MapToDto(invoice.Seller!),
				Buyer = EntityMapper.MapToDto(invoice.Buyer!),
				InvoiceNumber = invoice.InvoiceNumber,
				IssueDate = invoice.IssueDate,
				DueDate = invoice.DueDate,
				VatDate = invoice.VatDate,
				Currency = invoice.Currency,
				PaymentMethod = invoice.PaymentMethod,
				DeliveryMethod = invoice.DeliveryMethod,
				InvoiceNumberSchemeId = invoice.InvoiceNumberSchemeId,
				Status = invoice.Status,
				Items = invoice.Items.Select(InvoiceItemMapper.MapToDto).ToList()
			};
		}

		public static Invoice MapToDomain(InvoiceDto invoiceDto)
		{
			return new Invoice
			{
				Id = invoiceDto.Id,
				SellerId = invoiceDto.SellerId,
				BuyerId = invoiceDto.BuyerId,
				InvoiceNumber = invoiceDto.InvoiceNumber,
				IssueDate = invoiceDto.IssueDate,
				DueDate = invoiceDto.DueDate,
				VatDate = invoiceDto.VatDate,
				Currency = invoiceDto.Currency,
				PaymentMethod = invoiceDto.PaymentMethod,
				DeliveryMethod = invoiceDto.DeliveryMethod,
				InvoiceNumberSchemeId = invoiceDto.InvoiceNumberSchemeId,
				Status = invoiceDto.Status,
				Items = invoiceDto.Items.Select(InvoiceItemMapper.MapToDomain).ToList()
			};
		}
	}
}
