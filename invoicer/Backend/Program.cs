using Backend.Services;
using Domain.Interfaces;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpOverrides;
using Application.ServiceInterfaces;
using Application.Interfaces;
using Infrastructure.Persistance;
using Application.RepositoryInterfaces;
using Infrastructure.Repositories;
using Infrastructure.ExternalServices.AresApi;
using Application.ExternalServiceInterfaces;
using Infrastructure.ExternalServices.InvoicePdfGenerator;
using Shared.Enums;

namespace Backend
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add db context with provider-specific subclass for migrations
			DatabaseType databaseType = builder.Configuration.GetValue("DatabaseProvider", DatabaseType.Sqlite);
			switch (databaseType)
			{
				case DatabaseType.PostgreSql:
					builder.Services.AddDbContext<ApplicationDbContext, PgsqlDbContext>(options =>
						options.UseNpgsql(builder.Configuration.GetConnectionString("pgsqlConnection")));
					break;
				case DatabaseType.Sqlite:
					builder.Services.AddDbContext<ApplicationDbContext, SqliteDbContext>(options =>
						options.UseSqlite(builder.Configuration.GetConnectionString("sqliteConnection")));
					break;
				default:
					throw new InvalidOperationException("Invalid database provider specified in configuration.");
			}

			// Add repositories
			builder.Services.AddScoped<IAddressRepository, AddressRepository>();
			builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
			builder.Services.AddScoped<IEntityRepository, EntityRepository>();
			builder.Services.AddScoped<INumberingSchemeRepository, NumberingSchemeRepository>();
			builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
			builder.Services.AddScoped<IInvoiceItemRepository, InvoiceItemRepository>();
			builder.Services.AddScoped<IEntityInvoiceNumberingStateRepository, EntityInvoiceNumberingStateRepository>();

			// Add services
			builder.Services.AddScoped<IAddressService, AddressService>();
			builder.Services.AddScoped<IBankAccountService, BankAccountService>();
			builder.Services.AddScoped<IEntityService, EntityService>();
			builder.Services.AddScoped<IInvoiceService, InvoiceService>();
			builder.Services.AddScoped<INumberingSchemeService, NumberingSchemeService>();
			builder.Services.AddScoped<IEntityInvoiceNumberingStateService, EntityInvoiceNumberingStateService>();

			// Add external services
			builder.Services.AddScoped<IAresApiService, AresApiService>();
			builder.Services.AddScoped<IInvoicePdfGenerator, QuestInvoicePdfGenerator>();
			builder.Services.AddScoped<IInvoiceNumberParser, InvoiceNumberParser>();
			builder.Services.AddScoped<IInvoiceNumberGenerator, InvoiceNumberGenerator>();

			// Add controllers after all the services
			builder.Services.AddControllers();

			// Swagger only in Development
			if (builder.Environment.IsDevelopment())
			{
				builder.Services.AddEndpointsApiExplorer();
				builder.Services.AddSwaggerGen();
			}

			var app = builder.Build();

			// Create the db migrations and apply them
			using (var scope = app.Services.CreateScope())
			{
				try
				{
					var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
					dbContext.Database.Migrate();
				}
				catch (Exception ex)
				{
					Console.WriteLine($"An error occurred while migrating the database: {ex.Message}");
				}
			}

			// Swagger UI only in Development
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			// Support forwarded headers from reverse proxy
			app.UseForwardedHeaders(new ForwardedHeadersOptions
			{
				ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
			});

			// Read path base from X-Forwarded-Prefix header (set by nginx)
			app.Use((context, next) =>
			{
				if (context.Request.Headers.TryGetValue("X-Forwarded-Prefix", out var prefix))
				{
					var pathBase = prefix.ToString().TrimEnd('/');
					if (!string.IsNullOrEmpty(pathBase))
					{
						context.Request.PathBase = pathBase;
					}
				}
				return next();
			});

			// Enable CORS (frontend and backend are on different origins)
			EnableCors(app);

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}

		private static void EnableCors(IApplicationBuilder app)
		{
			app.UseCors(builder =>
			{
				builder.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader()
					.WithExposedHeaders("Content-Disposition");
			});
		}
	}
}
