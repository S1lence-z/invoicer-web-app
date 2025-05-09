using System.Net.Http.Json;
using Frontend.Utils;
using Frontend.Api;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Application.ServiceInterfaces;
using Frontend.Services;
using Microsoft.JSInterop;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace Frontend
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");
			builder.RootComponents.Add<HeadOutlet>("head::after");

			// Create a new http client
			var httpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };

			// Add localization services
			builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

			// Load config files
			var envConfig = await httpClient.GetFromJsonAsync<EnvironmentConfig>("./Data/env.json");
			var navMenuItemsList = await httpClient.GetFromJsonAsync<IEnumerable<NavLinkItem>>("./Data/NavMenuContent.json");

			// Register config files
			builder.Services.AddScoped(sp => httpClient);
			if (envConfig is not null)
				builder.Services.AddSingleton(envConfig);
			if (navMenuItemsList is not null)
				builder.Services.AddSingleton(new NavMenuItemsProvider(navMenuItemsList));

			// Register api services
			builder.Services.AddScoped<IAresApiService, AresApiService>();
			builder.Services.AddScoped<IEntityService, EntityService>();
			builder.Services.AddScoped<IAddressService, AddressService>();
			builder.Services.AddScoped<IBankAccountService, BankAccountService>();
			builder.Services.AddScoped<IInvoiceService, InvoiceService>();
			builder.Services.AddScoped<INumberingSchemeService, NumberingSchemeService>();

			// Register other services
			builder.Services.AddScoped<LoadingService>();
			builder.Services.AddScoped<ErrorService>();

			// Localization
			var tempServices = builder.Services.BuildServiceProvider();
			var jsRuntime = tempServices.GetRequiredService<IJSRuntime>();

			var persistedCultureName = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "blazorCulture");

			CultureInfo initialCulture;
			if (!string.IsNullOrEmpty(persistedCultureName))
			{
				try
				{
					initialCulture = new CultureInfo(persistedCultureName);
				}
				catch (CultureNotFoundException)
				{
					// Fallback if the persisted culture string is invalid
					initialCulture = new CultureInfo("en-US");
				}
			}
			else
				initialCulture = new CultureInfo("en-US");

			CultureInfo.DefaultThreadCurrentCulture = initialCulture;
			CultureInfo.DefaultThreadCurrentUICulture = initialCulture;

			// Build and run the app
			var host = builder.Build();
			await host.RunAsync();
		}
	}
}
