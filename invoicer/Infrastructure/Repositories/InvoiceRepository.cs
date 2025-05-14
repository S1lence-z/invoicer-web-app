using Application.RepositoryInterfaces;
using Domain.Models;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public class InvoiceRepository(ApplicationDbContext context) : IInvoiceRepository
	{
		public async Task<Invoice> CreateAsync(Invoice invoice)
		{
			await context.Invoice.AddAsync(invoice);
			return invoice;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			Invoice invoice = await GetByIdAsync(id, false);
			context.Invoice.Remove(invoice);
			return true;
		}

		public async Task<IEnumerable<Invoice>> GetAllAsync()
		{
			return await context.Invoice
				.Include(i => i.Seller)
					.ThenInclude(s => s!.Address)
				.Include(i => i.Seller)
					.ThenInclude(s => s!.BankAccount)
				.Include(i => i.Buyer)
					.ThenInclude(b => b!.Address)
				.Include(i => i.Buyer)
					.ThenInclude(b => b!.BankAccount)
				.Include(i => i.Items)
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<IEnumerable<Invoice>> GetAllInvoicesByEntityId(int entityId, bool isReadonly)
		{
			if (isReadonly)
				return await context.Invoice
					.Include(i => i.Seller)
						.ThenInclude(s => s!.Address)
					.Include(i => i.Seller)
						.ThenInclude(s => s!.BankAccount)
					.Include(i => i.Buyer)
						.ThenInclude(b => b!.Address)
					.Include(i => i.Buyer)
						.ThenInclude(b => b!.BankAccount)
					.Include(i => i.Items)
					.AsNoTracking()
					.Where(i => i.SellerId == entityId)
					.ToListAsync() ?? throw new KeyNotFoundException($"No invoice found for entity with id {entityId}");
			else
				return await context.Invoice
					.Where(i => i.SellerId == entityId)
					.Include(i => i.Seller)
						.ThenInclude(s => s!.Address)
					.Include(i => i.Seller)
						.ThenInclude(s => s!.BankAccount)
					.Include(i => i.Buyer)
						.ThenInclude(b => b!.Address)
					.Include(i => i.Buyer)
						.ThenInclude(b => b!.BankAccount)
					.Include(i => i.Items)
					.ToListAsync() ?? throw new KeyNotFoundException($"No invoice found for entity with id {entityId}");
		}

		public async Task<Invoice> GetByIdAsync(int id, bool isReadonly)
		{
			if (isReadonly)
				return await context.Invoice
					.Include(i => i.Seller)
						.ThenInclude(s => s!.Address)
					.Include(i => i.Seller)
						.ThenInclude(s => s!.BankAccount)
					.Include(i => i.Buyer)
						.ThenInclude(b => b!.Address)
					.Include(i => i.Buyer)
						.ThenInclude(b => b!.BankAccount)
					.Include(i => i.Items)
					.AsNoTracking()
					.FirstOrDefaultAsync(i => i.Id == id) ?? throw new KeyNotFoundException($"Invoice with id {id} not found.");
			else
				return await context.Invoice
					.Include(i => i.Seller)
						.ThenInclude(s => s!.Address)
					.Include(i => i.Seller)
						.ThenInclude(s => s!.BankAccount)
					.Include(i => i.Buyer)
						.ThenInclude(b => b!.Address)
					.Include(i => i.Buyer)
						.ThenInclude(b => b!.BankAccount)
					.Include(i => i.Items)
					.FirstOrDefaultAsync(i => i.Id == id) ?? throw new KeyNotFoundException($"Invoice with id {id} not found.");
		}

		public async Task<int> GetInvoiceCountByEntityId(int entityId)
		{
			return await context.Invoice.AsNoTracking().CountAsync(i => i.SellerId == entityId);
		}

		public async Task<Invoice?> GetLastInvoiceAsync(int entityId, bool isReadonly, params int[] invoiceIdsToExclude)
		{
			if (isReadonly)
				return await context.Invoice
					.AsNoTracking()
					.Where(i => i.SellerId == entityId && !invoiceIdsToExclude.Contains(i.Id))
					.OrderByDescending(i => i.IssueDate)
					.OrderByDescending(i => i.Id)
					.FirstOrDefaultAsync() ?? null;
			else
				return await context.Invoice
					.Where(i => i.SellerId == entityId && !invoiceIdsToExclude.Contains(i.Id))
					.OrderByDescending(i => i.IssueDate)
					.FirstOrDefaultAsync() ?? null;
		}

		public async Task SaveChangesAsync()
		{
			await context.SaveChangesAsync();
		}

		public Invoice Update(Invoice invoice)
		{
			context.Invoice.Update(invoice);
			return invoice;
		}
	}
}
