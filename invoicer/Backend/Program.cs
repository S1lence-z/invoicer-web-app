using Application.ServiceInterfaces;
using Backend.Database;
using Backend.Services;
using Backend.Services.AresApiService;
using Domain.Interfaces;
using Domain.Services;
using Backend.Utils.InvoicePdfGenerator;
using Microsoft.EntityFrameworkCore;
using Application.PdfGenerator;

namespace Backend
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddDbContext<ApplicationDbContext>(options => 
				options.UseSqlite(builder.Configuration.GetConnectionString("InvoicerDb"))
			);

			// Add custom services
			builder.Services.AddScoped<IAddressService, AddressService>();
			builder.Services.AddScoped<IBankAccountService, BankAccountService>();
			builder.Services.AddScoped<IEntityService, EntityService>();
			builder.Services.AddScoped<IInvoiceService, InvoiceService>();
			builder.Services.AddScoped<IAresApiService, AresApiService>();
			builder.Services.AddScoped<IInvoiceNumberingService, InvoiceNumberingService>();
			builder.Services.AddSingleton<IInvoiceNumberGenerator, InvoiceNumberGenerator>();
			builder.Services.AddScoped<IInvoicePdfGenerator, InvoicePdfGenerator>();
			builder.Services.AddScoped<IEntityInvoiceNumberingSchemeState, EntityInvoiceNumberingSchemeStateService>();

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
				builder.AllowAnyOrigin();
				builder.AllowAnyMethod();
				builder.AllowAnyHeader();
			});
		}
	}
}