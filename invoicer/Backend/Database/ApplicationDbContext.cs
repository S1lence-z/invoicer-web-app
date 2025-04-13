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
		public DbSet<EntityInvoiceNumberSchemeState> EntityInvoiceNumberSchemeStates { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			SetUpAddress(modelBuilder);
			SetUpBankAccount(modelBuilder);
			SetUpEntity(modelBuilder);
			SetUpInvoice(modelBuilder);
			SetUpInvoiceItem(modelBuilder);
			SetUpInvoiceNumberScheme(modelBuilder);
			SetupEntityInvoiceNumberSchemeState(modelBuilder);

			modelBuilder.Entity<InvoiceNumberScheme>().HasData(
				Domain.Models.InvoiceNumberScheme.CreateDefault()
			);
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
				// Invoice Number Scheme
				entity.HasOne(e => e.InvoiceNumberScheme)
					.WithMany(ins => ins.EntitiesUsingScheme)
					.HasForeignKey(e => e.InvoiceNumberSchemeId)
					.OnDelete(DeleteBehavior.Restrict)
					.IsRequired(false);

				// Entity Invoice Number Scheme State
				entity.HasMany(e => e.EntityInvoiceNumberSchemeStates)
					.WithOne(eins => eins.Entity)
					.HasForeignKey(e => e.EntityId)
					.OnDelete(DeleteBehavior.Cascade);
			});
		}

		private static void SetUpInvoice(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Invoice>(invoice =>
			{
				invoice.HasKey(i => i.Id);
				invoice.Property(i => i.SellerId).IsRequired();
				invoice.HasOne(i => i.Seller)
					.WithMany(e => e.SoldInvoices)
					.HasForeignKey(i => i.SellerId)
					.OnDelete(DeleteBehavior.Restrict);
				invoice.Property(i => i.BuyerId).IsRequired();
				invoice.HasOne(i => i.Buyer)
					.WithMany(e => e.PurchasedInvoices)
					.HasForeignKey(i => i.BuyerId)
					.OnDelete(DeleteBehavior.Restrict);
				invoice.Property(i => i.InvoiceNumber).IsRequired();
				invoice.Property(i => i.IssueDate).IsRequired();
				invoice.Property(i => i.DueDate).IsRequired();
				invoice.Property(i => i.VatDate).IsRequired();
				invoice.Property(i => i.Status).HasConversion<string>().HasDefaultValue(InvoiceStatus.Pending);
				invoice.Property(i => i.Currency).IsRequired().HasConversion<string>().HasDefaultValue(Currency.CZK);
				invoice.Property(i => i.PaymentMethod).HasConversion<string>().HasDefaultValue(PaymentMethod.BankTransfer);
				invoice.Property(i => i.DeliveryMethod).HasConversion<string>().HasDefaultValue(DeliveryMethod.PersonalPickUp);
				invoice.HasMany(i => i.Items).WithOne().HasForeignKey(i => i.InvoiceId).OnDelete(DeleteBehavior.Cascade);

				// Invoice Number Scheme
				invoice.Property(i => i.InvoiceNumberSchemeId).IsRequired();
				invoice.HasOne(i => i.InvoiceNumberScheme)
					.WithMany(ins => ins.InvoicesGeneratedWithScheme)
					.HasForeignKey(i => i.InvoiceNumberSchemeId)
					.OnDelete(DeleteBehavior.Restrict);
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
				invoiceNumberScheme.Property(ins => ins.Prefix).HasConversion<string>().HasDefaultValue(string.Empty);
				invoiceNumberScheme.Property(ins => ins.UseSeperator).HasDefaultValue(true);
				invoiceNumberScheme.Property(ins => ins.Seperator).HasConversion<string>().HasDefaultValue("-");
				invoiceNumberScheme.Property(ins => ins.SequencePosition).HasConversion<string>().HasDefaultValue(InvoiceNumberSequencePosition.Start);
				invoiceNumberScheme.Property(ins => ins.SequencePadding).HasDefaultValue(3);
				invoiceNumberScheme.Property(ins => ins.InvoiceNumberYearFormat).HasConversion<string>().HasDefaultValue(InvoiceNumberYearFormat.FourDigit);
				invoiceNumberScheme.Property(ins => ins.IncludeMonth).HasDefaultValue(true);
				invoiceNumberScheme.Property(ins => ins.ResetFrequency).HasConversion<string>().HasDefaultValue(InvoiceNumberResetFrequency.Yearly);
				invoiceNumberScheme.Property(ins => ins.IsDefault).HasDefaultValue(false);

				// Invoices generated with this scheme
				invoiceNumberScheme.HasMany(ins => ins.InvoicesGeneratedWithScheme)
					.WithOne(e => e.InvoiceNumberScheme)
					.HasForeignKey(i => i.InvoiceNumberSchemeId)
					.OnDelete(DeleteBehavior.Restrict);
			});
		}

		private static void SetupEntityInvoiceNumberSchemeState(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<EntityInvoiceNumberSchemeState>(entityInvoiceNumberSchemeState =>
			{
				entityInvoiceNumberSchemeState.HasKey(eins => new { eins.EntityId, eins.InvoiceNumberSchemeId });
				entityInvoiceNumberSchemeState.Property(eins => eins.EntityId).IsRequired();
				entityInvoiceNumberSchemeState.Property(eins => eins.InvoiceNumberSchemeId).IsRequired();
				entityInvoiceNumberSchemeState.Property(eins => eins.LastSequenceNumber);
				entityInvoiceNumberSchemeState.Property(eins => eins.LastGenerationYear);
				entityInvoiceNumberSchemeState.Property(eins => eins.LastGenerationMonth);

				// Entity
				entityInvoiceNumberSchemeState.HasOne(eins => eins.Entity)
					.WithMany(e => e.EntityInvoiceNumberSchemeStates)
					.HasForeignKey(eins => eins.EntityId)
					.OnDelete(DeleteBehavior.Cascade);

				// Invoice Number Scheme
				entityInvoiceNumberSchemeState.HasOne(eins => eins.InvoiceNumberScheme)
					.WithMany(ins => ins.EntityInvoiceNumberSchemeStates)
					.HasForeignKey(eins => eins.InvoiceNumberSchemeId)
					.OnDelete(DeleteBehavior.Restrict);
			});
		}
	}
}
