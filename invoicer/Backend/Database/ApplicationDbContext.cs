using Microsoft.EntityFrameworkCore;
using Backend.Models;
using Backend.Services.InvoiceGeneratorService.Models;

namespace Backend.Database
{
	public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
	{
		public DbSet<Address> Address { get; set; }
		public DbSet<BankAccount> BankAccount { get; set; }
		public DbSet<Entity> Entity { get; set; }
		public DbSet<Invoice> Invoice { get; set; }
		public DbSet<InvoiceItem> InvoiceItem { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Composite key for Invoice
			modelBuilder.Entity<Invoice>()
				.HasKey(invoice => new { invoice.Id.InvoiceNumber, invoice.Id.Seller });

			// Composite key for InvoiceItem
			modelBuilder.Entity<InvoiceItem>()
				.HasKey(item => new { item.Id.ItemId, item.Id.InvoiceNumber, item.Id.SellerId});
		}
	}
}
