using Backend.Database;
using Domain.ServiceInterfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Backend.Utils.InvoicePdfGenerator;
using Domain.Interfaces;

namespace Backend.Services
{
	public class InvoiceService(ApplicationDbContext context) : IInvoiceService
	{
		public async Task<Invoice?> GetByIdAsync(int id)
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
				.FirstOrDefaultAsync(i => i.Id == id);
		}

		public async Task<IList<Invoice>> GetAllAsync()
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
				.ToListAsync();
		}

		public async Task<Invoice?> CreateAsync(Invoice newInvoice)
		{
			Entity? seller = await context.Entity
				.Include(e => e.BankAccount)
				.Include(e => e.Address)
				.FirstOrDefaultAsync(e => e.Id == newInvoice.SellerId);
			Entity? buyer = await context.Entity
				.Include(e => e.BankAccount)
				.Include(e => e.Address)
				.FirstOrDefaultAsync(e => e.Id == newInvoice.BuyerId);

			if (seller is null)
				throw new ArgumentException($"Seller entity with id {newInvoice.SellerId} not found");
			if (buyer is null)
				throw new ArgumentException($"Buyer entity with id {newInvoice.BuyerId} not found");

			newInvoice.Seller = seller;
			newInvoice.Buyer = buyer;

			context.Invoice.Add(newInvoice);
			await context.SaveChangesAsync();
			return newInvoice;
		}

		public async Task<Invoice> UpdateAsync(int id, Invoice updatedInvoice)
		{
			Invoice? existingInvoice = await context.Invoice.FindAsync(id);
			if (existingInvoice is null)
				throw new ArgumentException($"Invoice with id {id} not found");

			Entity? seller = await context.Entity
				.Include(e => e.BankAccount)
				.Include(e => e.Address)
				.FirstOrDefaultAsync(e => e.Id == updatedInvoice.SellerId);
			Entity? buyer = await context.Entity
				.Include(e => e.BankAccount)
				.Include(e => e.Address)
				.FirstOrDefaultAsync(e => e.Id == updatedInvoice.BuyerId);
			if (seller is null)
				throw new ArgumentException($"Seller entity with id {updatedInvoice.SellerId} not found");
			if (buyer is null)
				throw new ArgumentException($"Buyer entity with id {updatedInvoice.BuyerId} not found");

			existingInvoice.SellerId = updatedInvoice.SellerId;
			existingInvoice.BuyerId = updatedInvoice.BuyerId;
			existingInvoice.Seller = seller;
			existingInvoice.Buyer = buyer;
			existingInvoice.InvoiceNumber = updatedInvoice.InvoiceNumber;
			existingInvoice.IssueDate = updatedInvoice.IssueDate;
			existingInvoice.DueDate = updatedInvoice.DueDate;
			existingInvoice.Currency = updatedInvoice.Currency;
			existingInvoice.PaymentMethod = updatedInvoice.PaymentMethod;
			existingInvoice.DeliveryMethod = updatedInvoice.DeliveryMethod;
			existingInvoice.Items = updatedInvoice.Items;

			await context.SaveChangesAsync();
			return existingInvoice;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			Invoice? existingInvoice = await context.Invoice.FindAsync(id);
			if (existingInvoice is null)
				return false;
			context.Invoice.Remove(existingInvoice);
			await context.SaveChangesAsync();
			return true;
		}

		public async Task<IPdfGenerationResult> ExportInvoicePdf(int id)
		{
			Invoice? invoiceToExport = await GetByIdAsync(id);

			if (invoiceToExport is null)
				throw new ArgumentException($"Invoice with id {id} not found");

			IPdfGenerationResult pdfFile = await InvoicePdfGenerator.ExportInvoicePdf(invoiceToExport);

			return pdfFile;
		}
	}
}
