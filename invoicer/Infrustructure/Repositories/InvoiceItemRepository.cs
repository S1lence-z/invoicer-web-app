using Application.RepositoryInterfaces;
using Domain.Models;
using Infrustructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrustructure.Repositories
{
	public class InvoiceItemRepository(ApplicationDbContext context) : IInvoiceItemRepository
	{
		public async Task AddAsync(InvoiceItem item)
		{
			await context.InvoiceItem.AddAsync(item);
		}

		public async Task<IEnumerable<InvoiceItem>> GetByInvoiceIdAsync(int invoiceId, bool isReadonly)
		{
			if (isReadonly)
				return await context.InvoiceItem
					.AsNoTracking()
					.Where(i => i.InvoiceId == invoiceId)
					.ToListAsync();
			else
				return await context.InvoiceItem
					.Where(i => i.InvoiceId == invoiceId)
					.ToListAsync();
		}

		public void Remove(InvoiceItem item)
		{
			context.InvoiceItem.Remove(item);
		}

		public async Task SaveChangesAsync()
		{
			await context.SaveChangesAsync();
		}

		public void Update(InvoiceItem item)
		{
			context.InvoiceItem.Update(item);
		}
	}
}
