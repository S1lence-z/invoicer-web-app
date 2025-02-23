using System.Net.Http.Json;
using Frontend.Models;
using Frontend.Utils;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

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

			// Register services
			builder.Services.AddScoped(sp => httpClient);
			if (envConfig is not null)
				builder.Services.AddSingleton(envConfig);
			if (navMenuItemsList is not null)
				builder.Services.AddSingleton(new NavMenuItemsProvider(navMenuItemsList));

			// Build and run the app
			await builder.Build().RunAsync();
		}
	}
}
