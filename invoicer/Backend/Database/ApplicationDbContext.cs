using Microsoft.EntityFrameworkCore;
using Backend.Services.InvoiceService.Models;
using Backend.Services.AddressService.Models;
using Backend.Services.BankAccountService.Models;
using Backend.Services.EntityService.Models;
using Backend.Models;

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
			ModelBase<Invoice>.ConfigureEntity(modelBuilder);
		}
	}
}
