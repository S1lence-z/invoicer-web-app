using System.Net.Http.Json;
using Frontend.Models;
using Frontend.Utils;
using Frontend.Api;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Domain.ServiceInterfaces;

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

			// Load config files
			var envConfig = await httpClient.GetFromJsonAsync<EnvironmentConfig>("./Data/env.json");
			var navMenuItemsList = await httpClient.GetFromJsonAsync<List<NavLinkItem>>("./Data/NavMenuContent.json");

			// Register config files
			builder.Services.AddScoped(sp => httpClient);
			if (envConfig is not null)
				builder.Services.AddSingleton(envConfig);
			if (navMenuItemsList is not null)
				builder.Services.AddSingleton(new NavMenuItemsProvider(navMenuItemsList));

			// Register api services
			
			builder.Services.AddScoped<IAddressService, AddressService>();

			// Build and run the app
			await builder.Build().RunAsync();
		}
	}
}
