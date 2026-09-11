using System.Net;
using System.Runtime.InteropServices;
using Photino.NET;

namespace Desktop;

public class Program
{
	private const string AppName = "Invoicer";

	// Fixed port so the browser origin (and therefore localStorage, e.g. the chosen culture)
	// stays the same between launches. Falls back to a random port if it is taken.
	private const int PreferredPort = 47831;

	private static readonly string DataDir = Path.Combine(
		Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppName);

	private static readonly string LogPath = Path.Combine(DataDir, "desktop.log");

	[STAThread]
	public static int Main(string[] args)
	{
		Directory.CreateDirectory(DataDir);
		Log($"Starting {AppName} desktop; data directory: {DataDir}");

		WebApplication app;
		try
		{
			app = StartServer(args);
		}
		catch (Exception ex)
		{
			Log($"FATAL: could not start the embedded server: {ex}");
			ShowFatalError(ex);
			return 1;
		}

		var url = app.Urls.First();
		Log($"Server listening on {url}");

		var window = CreateWindow();
		window.Load(url);
		window.WaitForClose();

		Log("Window closed, stopping server");
		app.StopAsync().GetAwaiter().GetResult();
		return 0;
	}

	/// <summary>
	/// Builds and starts the ASP.NET Core backend in-process, serving both the API and the
	/// Blazor WASM frontend on the loopback interface.
	/// </summary>
	private static WebApplication StartServer(string[] args)
	{
		try
		{
			return StartServer(args, $"http://127.0.0.1:{PreferredPort}");
		}
		catch (IOException ex)
		{
			// Most likely "address already in use" (another instance is running)
			Log($"Port {PreferredPort} unavailable ({ex.Message}); falling back to a random port");
			return StartServer(args, "http://127.0.0.1:0");
		}
	}

	private static WebApplication StartServer(string[] args, string url)
	{
		var builder = WebApplication.CreateBuilder(new WebApplicationOptions
		{
			Args = args,
			ContentRootPath = AppContext.BaseDirectory,
		});

		// During `dotnet run` the Frontend assets are served from the build manifest rather than
		// a physical wwwroot; this is a no-op in a published build.
		builder.WebHost.UseStaticWebAssets();
		builder.WebHost.UseUrls(url);

		// Desktop-only settings. Added last, so they win over any appsettings.json that was
		// copied from the Backend project, environment variables, or command-line arguments.
		var dbPath = Path.Combine(DataDir, "Invoicer.db");
		builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
		{
			["ServeFrontend"] = "true",
			["DatabaseProvider"] = "Sqlite",
			["ConnectionStrings:sqliteConnection"] = $"Data Source={dbPath}",
		});

		builder.Logging.AddProvider(new FileLoggerProvider(LogPath));

		Backend.Program.ConfigureServices(builder);

		var app = builder.Build();

		Backend.Program.ConfigurePipeline(app);

		try
		{
			app.StartAsync().GetAwaiter().GetResult();
		}
		catch
		{
			app.DisposeAsync().AsTask().GetAwaiter().GetResult();
			throw;
		}
		return app;
	}

	private static PhotinoWindow CreateWindow()
	{
#if DEBUG
		const bool devTools = true;
#else
		const bool devTools = false;
#endif
		var iconFile = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "icon.ico" : "icon.png";
		var iconPath = Path.Combine(AppContext.BaseDirectory, "Assets", iconFile);

		var window = new PhotinoWindow()
			.SetTitle(AppName)
			.SetUseOsDefaultSize(false)
			.SetSize(1400, 900)
			.SetMinSize(900, 600)
			.Center()
			.SetDevToolsEnabled(devTools)
			.SetContextMenuEnabled(devTools);

		if (File.Exists(iconPath))
			window.SetIconFile(iconPath);
		else
			Log($"Icon not found at {iconPath}");

		return window;
	}

	/// <summary>
	/// The Windows build has no console, so a startup failure is shown in a plain window
	/// instead of silently exiting.
	/// </summary>
	private static void ShowFatalError(Exception ex)
	{
		try
		{
			var html = $"""
				<html><body style="font-family:sans-serif;padding:2em">
				<h2>{AppName} could not start</h2>
				<p>{WebUtility.HtmlEncode(ex.Message)}</p>
				<p>Details were written to:<br><code>{WebUtility.HtmlEncode(LogPath)}</code></p>
				</body></html>
				""";

			new PhotinoWindow()
				.SetTitle($"{AppName} - startup error")
				.SetUseOsDefaultSize(false)
				.SetSize(640, 360)
				.Center()
				.LoadRawString(html)
				.WaitForClose();
		}
		catch (Exception windowEx)
		{
			// If even the webview cannot be created (e.g. WebView2 / WebKitGTK missing) there is
			// nothing left to show; the log file has both errors.
			Log($"Could not show the error window: {windowEx}");
		}
	}

	private static void Log(string message) => FileLoggerProvider.Write(LogPath, message);
}
