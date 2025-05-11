using System.Net.Http.Json;
using Frontend.Utils;
using Frontend.Api;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Application.ServiceInterfaces;
using Frontend.Services;
using Microsoft.JSInterop;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Frontend
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");
			builder.RootComponents.Add<HeadOutlet>("head::after");

			// Load the environment-specific configuration
			builder.Services.AddOptions<EnvironmentConfig>();
			builder.Services.Configure<EnvironmentConfig>(options =>
			{
				builder.Configuration.GetSection("AppSpecificSettings").Bind(options);
			});

			// Create a new http client
			builder.Services.AddScoped(sp =>
			{
				var envConfigOptions = sp.GetRequiredService<IOptions<EnvironmentConfig>>();
				var envConfig = envConfigOptions.Value;

				if (string.IsNullOrEmpty(envConfig.ApiBaseUrl))
					throw new InvalidOperationException("ApiBaseUrl is not configured in appsettings.json or its environment-specific override.");

				return new HttpClient { BaseAddress = new Uri(envConfig.ApiBaseUrl) };
			});

			// Add localization services
			builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

			// Get the nav menu
			using var localContentClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
			var navMenuItemsList = await localContentClient.GetFromJsonAsync<IEnumerable<NavLinkItem>>("./Data/NavMenuContent.json");
			if (navMenuItemsList is not null)
				builder.Services.AddSingleton(new NavMenuItemsProvider(navMenuItemsList));
			else
				throw new InvalidOperationException("NavMenuContent.json file not found or invalid.");

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
