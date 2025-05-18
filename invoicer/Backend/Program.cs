using Backend.Services;
using Domain.Interfaces;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using Application.ServiceInterfaces;
using Application.Interfaces;
using Infrastructure.Persistance;
using Application.RepositoryInterfaces;
using Infrastructure.Repositories;
using Infrastructure.ExternalServices.AresApi;
using Application.ExternalServiceInterfaces;
using Infrastructure.ExternalServices.InvoicePdfGenerator;

namespace Backend
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add db context
			builder.Services.AddDbContext<ApplicationDbContext>(options => 
				options.UseSqlite(builder.Configuration.GetConnectionString("InvoicerDb"))
			);

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
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Enable Swagger UI
			app.UseSwagger();
			app.UseSwaggerUI();

			// app.UseHttpsRedirection();

			// Enable CORS
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