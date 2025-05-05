using Application.DTOs;
using Application.PdfGenerator;
using Domain.Interfaces;

namespace Application.ServiceInterfaces
{
	public interface IInvoiceService : IService<int, InvoiceDto> 
	{
		public Task<IPdfGenerationResult> ExportInvoicePdf(int id);
	}
}
