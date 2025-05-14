using Domain.Models;
using Application.DTOs;

namespace Application.Mappers
{
	public class InvoiceItemMapper
	{
		public static InvoiceItemDto MapToDto(InvoiceItem invoiceItem)
		{
			return new InvoiceItemDto
			{
				Id = invoiceItem.Id,
				InvoiceId = invoiceItem.InvoiceId,
				Unit = invoiceItem.Unit,
				Quantity = invoiceItem.Quantity,
				Description = invoiceItem.Description,
				UnitPrice = invoiceItem.UnitPrice,
				VatRate = invoiceItem.VatRate
			};
		}

		public static InvoiceItem MapToDomain(InvoiceItemDto invoiceItemDto)
		{
			return new InvoiceItem
			{
				Id = invoiceItemDto.Id,
				InvoiceId = invoiceItemDto.InvoiceId,
				Unit = invoiceItemDto.Unit,
				Quantity = invoiceItemDto.Quantity,
				Description = invoiceItemDto.Description,
				UnitPrice = invoiceItemDto.UnitPrice,
				VatRate = invoiceItemDto.VatRate
			};
		}
	}
}
