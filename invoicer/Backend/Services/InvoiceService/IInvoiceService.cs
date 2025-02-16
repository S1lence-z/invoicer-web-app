using Backend.Models;
using Backend.Services.InvoiceService.Models;

namespace Backend.Services.InvoiceService
{
	public interface IInvoiceService : IService<int, Invoice> { }
}
