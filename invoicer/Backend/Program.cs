using Backend.Database;
using Backend.Services.AddressService;
using Backend.Services.AresApiService;
using Backend.Services.BankAccountService;
using Backend.Services.EntityService;
using Backend.Services.InvoiceGeneratorService;
using Microsoft.EntityFrameworkCore;

namespace Backend
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(
				builder.Configuration.GetConnectionString("InvoicerDb"))
			);

			// Add custom services
			builder.Services.AddScoped<IAddressService, AddressService>();
			builder.Services.AddScoped<IBankAccountService, BankAccountService>();
			builder.Services.AddScoped<IEntityService, EntityService>();
			builder.Services.AddScoped<AresApiService>();
			builder.Services.AddScoped<InvoiceGeneratorService>();

			// Add controllers after all the services
			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
