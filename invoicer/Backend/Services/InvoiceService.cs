using Backend.Database;
using Microsoft.EntityFrameworkCore;
using Application.DTOs;
using Application.ServiceInterfaces;
using Application.Mappers;
using Domain.Models;
using Application.PdfGenerator; 

namespace Backend.Services
{
	public class InvoiceService(ApplicationDbContext context, IInvoiceNumberingService numberingService, IInvoicePdfGenerator invoicePdfGenerator) : IInvoiceService
	{
		public async Task<InvoiceDto?> GetByIdAsync(int id)
		{
			Invoice? invoice = await context.Invoice
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
				.FirstOrDefaultAsync(i => i.Id == id);
			if (invoice is null)
				throw new KeyNotFoundException($"Invoice with id {id} not found");
			return InvoiceMapper.MapToDto(invoice);
		}

		public async Task<IList<InvoiceDto>> GetAllAsync()
		{
			List<Invoice> allInvoices = await context.Invoice
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
			return allInvoices.Select(InvoiceMapper.MapToDto).ToList();
		}

		public async Task<InvoiceDto?> CreateAsync(InvoiceDto newInvoice)
		{
			Entity? seller = await context.Entity
				.Include(e => e.BankAccount)
				.Include(e => e.Address)
				.AsNoTracking()
				.FirstOrDefaultAsync(e => e.Id == newInvoice.SellerId);
			Entity? buyer = await context.Entity
				.Include(e => e.BankAccount)
				.Include(e => e.Address)
				.AsNoTracking()
				.FirstOrDefaultAsync(e => e.Id == newInvoice.BuyerId);

			if (seller is null)
				throw new ArgumentException($"Seller entity with id {newInvoice.SellerId} not found");

			if (buyer is null)
				throw new ArgumentException($"Buyer entity with id {newInvoice.BuyerId} not found");

			// Assign the seller numbering scheme, the one who is generating the invoice
			newInvoice.NumberingSchemeId = seller.NumberingSchemeId;

			// Generate invoice number
			string newInvoiceNumber = await numberingService.GetNextInvoiceNumberAsync(seller.Id, DateTime.Now);
			if (string.IsNullOrEmpty(newInvoiceNumber))
				throw new ArgumentException($"Failed to generate invoice number for seller with id {seller.Id}");
			newInvoice.InvoiceNumber = newInvoiceNumber;
			newInvoice.Seller = EntityMapper.MapToDto(seller);
			newInvoice.Buyer = EntityMapper.MapToDto(buyer);

			Invoice invoice = InvoiceMapper.MapToDomain(newInvoice);
			await context.Invoice.AddAsync(invoice);
			await context.SaveChangesAsync();

			Invoice? createdInvoice = await context.Invoice
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
				.FirstOrDefaultAsync(i => i.Id == invoice.Id);

			if (createdInvoice is null)
				throw new ArgumentException($"Failed to create invoice with id {invoice.Id}");

			return InvoiceMapper.MapToDto(createdInvoice);
		}

		public async Task<InvoiceDto?> UpdateAsync(int id, InvoiceDto updatedInvoice)
		{
			Invoice? existingInvoice = await context.Invoice
				.Include(i => i.Items)
				.FirstOrDefaultAsync(i => i.Id == id);
			if (existingInvoice is null)
				throw new KeyNotFoundException($"Invoice with id {id} not found");

			Entity? seller = await context.Entity
				.Include(e => e.BankAccount)
				.Include(e => e.Address)
				.AsNoTracking()
				.FirstOrDefaultAsync(e => e.Id == updatedInvoice.SellerId);
			Entity? buyer = await context.Entity
				.Include(e => e.BankAccount)
				.Include(e => e.Address)
				.AsNoTracking()
				.FirstOrDefaultAsync(e => e.Id == updatedInvoice.BuyerId);

			if (seller is null)
				throw new KeyNotFoundException($"Seller entity with id {updatedInvoice.SellerId} not found");

			if (buyer is null)
				throw new KeyNotFoundException($"Buyer entity with id {updatedInvoice.BuyerId} not found");

			existingInvoice.InvoiceNumber = updatedInvoice.InvoiceNumber;
			existingInvoice.IssueDate = updatedInvoice.IssueDate;
			existingInvoice.DueDate = updatedInvoice.DueDate;
			existingInvoice.Currency = updatedInvoice.Currency;
			existingInvoice.PaymentMethod = updatedInvoice.PaymentMethod;
			existingInvoice.VatDate = updatedInvoice.VatDate;
			existingInvoice.Status = updatedInvoice.Status;
			existingInvoice.DeliveryMethod = updatedInvoice.DeliveryMethod;
			await UpdateInvoiceItemsAsync(existingInvoice, updatedInvoice.Items);

			await context.SaveChangesAsync();

			Invoice? updatedInvoiceEntity = await context.Invoice
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
				.FirstOrDefaultAsync(i => i.Id == id);

			if (updatedInvoiceEntity is null)
				throw new ArgumentException($"Failed to update invoice with id {id}");

			return InvoiceMapper.MapToDto(updatedInvoiceEntity);
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
			Invoice? invoiceToExport = await context.Invoice
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
				.FirstOrDefaultAsync(i => i.Id == id);

			if (invoiceToExport is null)
				throw new ArgumentException($"Invoice with id {id} not found");

			IPdfGenerationResult pdfFile = invoicePdfGenerator.ExportInvoicePdf(invoiceToExport);

			return pdfFile;
		}

		private async Task UpdateInvoiceItemsAsync(Invoice existingInvoice, ICollection<InvoiceItemDto> updatedItemDtos)
		{
			List<InvoiceItem> updatedItems = updatedItemDtos.Select(InvoiceItemMapper.MapToDomain).ToList();
			Dictionary<int, InvoiceItem> updatedItemsDict = updatedItems.ToDictionary(i => i.Id);

			// Update, Add and Delete the items
			foreach (var existingItem in existingInvoice.Items)
			{
				if (updatedItemsDict.TryGetValue(existingItem.Id, out var updatedItem))
				{
					existingItem.Unit = updatedItem.Unit;
					existingItem.Quantity = updatedItem.Quantity;
					existingItem.Description = updatedItem.Description;
					existingItem.UnitPrice = updatedItem.UnitPrice;
					existingItem.VatRate = updatedItem.VatRate;
					updatedItemsDict.Remove(existingItem.Id);
				}
				else
				{
					context.InvoiceItem.Remove(existingItem);
				}
			}

			// Add new items
			foreach (var newItem in updatedItemsDict.Values)
			{
				newItem.InvoiceId = existingInvoice.Id;
				await context.InvoiceItem.AddAsync(newItem);
			}
		}
	}
}
