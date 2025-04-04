using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Domain.Enums;

namespace Backend.Database
{
	public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
	{
		public DbSet<Address> Address { get; set; }
		public DbSet<BankAccount> BankAccount { get; set; }
		public DbSet<Entity> Entity { get; set; }
		public DbSet<Invoice> Invoice { get; set; }
		public DbSet<InvoiceItem> InvoiceItem { get; set; }
		public DbSet<InvoiceNumberScheme> InvoiceNumberScheme { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			SetUpAddress(modelBuilder);
			SetUpBankAccount(modelBuilder);
			SetUpEntity(modelBuilder);
			SetUpInvoice(modelBuilder);
			SetUpInvoiceItem(modelBuilder);
			SetUpInvoiceNumberScheme(modelBuilder);
		}

		private static void SetUpAddress(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Address>(address =>
			{
				address.HasKey(a => a.Id);
				address.Property(a => a.Street).IsRequired();
				address.Property(a => a.City).IsRequired();
				address.Property(a => a.ZipCode).IsRequired();
				address.Property(a => a.Country).IsRequired();
			});
		}

		private static void SetUpBankAccount(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<BankAccount>(bankAccount =>
			{
				bankAccount.HasKey(ba => ba.Id);
				bankAccount.Property(ba => ba.AccountNumber).IsRequired();
				bankAccount.Property(ba => ba.BankCode).IsRequired();
				bankAccount.Property(ba => ba.BankName).IsRequired();
				bankAccount.Property(ba => ba.IBAN).HasDefaultValue(string.Empty);
			});
		}

		private static void SetUpEntity(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Entity>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Ico).IsRequired();
				entity.Property(e => e.Name).IsRequired();
				entity.Property(e => e.Email).HasDefaultValue(string.Empty);
				entity.Property(e => e.PhoneNumber).HasDefaultValue(string.Empty);
				entity.HasOne(e => e.BankAccount).WithMany().HasForeignKey(e => e.BankAccountId);
				entity.HasOne(e => e.Address).WithMany().HasForeignKey(e => e.AddressId);
			});
		}

		private static void SetUpInvoice(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Invoice>(invoice =>
			{
				invoice.HasKey(i => i.Id);
				invoice.Property(i => i.SellerId).IsRequired();
				invoice.HasOne(i => i.Seller).WithMany().HasForeignKey(i => i.SellerId);
				invoice.Property(i => i.BuyerId).IsRequired();
				invoice.HasOne(i => i.Buyer).WithMany().HasForeignKey(i => i.BuyerId);
				invoice.Property(i => i.InvoiceNumber).IsRequired();
				invoice.Property(i => i.IssueDate).IsRequired();
				invoice.Property(i => i.DueDate).IsRequired();
				invoice.Property(i => i.Currency).IsRequired().HasConversion<string>().HasDefaultValue(Currency.CZK);
				invoice.Property(i => i.PaymentMethod).HasConversion<string>().HasDefaultValue(PaymentMethod.BankTransfer);
				invoice.Property(i => i.DeliveryMethod).HasConversion<string>().HasDefaultValue(DeliveryMethod.PersonalPickUp);
				invoice.HasMany(i => i.Items).WithOne().HasForeignKey(i => i.InvoiceId);
			});
		}

		private static void SetUpInvoiceItem(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<InvoiceItem>(invoiceItem =>
			{
				invoiceItem.HasKey(ii => ii.Id);
				invoiceItem.Property(ii => ii.InvoiceId).IsRequired();
				invoiceItem.Property(ii => ii.Unit).IsRequired();
				invoiceItem.Property(ii => ii.Quantity).IsRequired();
				invoiceItem.Property(ii => ii.Description).IsRequired();
				invoiceItem.Property(ii => ii.UnitPrice).IsRequired();
				invoiceItem.Property(ii => ii.VatRate).IsRequired().HasDefaultValue(0.21m);
			});
		}

		private static void SetUpInvoiceNumberScheme(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<InvoiceNumberScheme>(invoiceNumberScheme =>
			{
				invoiceNumberScheme.HasKey(ins => ins.Id);
				invoiceNumberScheme.Property(ins => ins.EntityId).IsRequired();
				invoiceNumberScheme.HasOne(ins => ins.Entity).WithMany().HasForeignKey(ins => ins.EntityId);
				invoiceNumberScheme.Property(ins => ins.Prefix).HasConversion<string>().HasDefaultValue(string.Empty);
				invoiceNumberScheme.Property(ins => ins.UseSeperator).HasDefaultValue(true);
				invoiceNumberScheme.Property(ins => ins.Seperator).HasConversion<string>().HasDefaultValue("-");
				invoiceNumberScheme.Property(ins => ins.SequencePosition).HasConversion<string>().HasDefaultValue(InvoiceNumberSequencePosition.Start);
				invoiceNumberScheme.Property(ins => ins.SequencePadding).HasDefaultValue(3);
				invoiceNumberScheme.Property(ins => ins.InvoiceNumberYearFormat).HasConversion<string>().HasDefaultValue(InvoiceNumberYearFormat.FourDigit);
				invoiceNumberScheme.Property(ins => ins.IncludeMonth).HasDefaultValue(true);
				invoiceNumberScheme.Property(ins => ins.ResetFrequency).HasConversion<string>().HasDefaultValue(InvoiceNumberResetFrequency.Yearly);
				invoiceNumberScheme.Property(ins => ins.LastSequenceNumber).HasDefaultValue(0);
				invoiceNumberScheme.Property(ins => ins.LastGenerationYear).HasDefaultValue(0);
				invoiceNumberScheme.Property(ins => ins.LastGenerationMonth).HasDefaultValue(0);
			});
		}
	}
}
