using System.Net;
using System.Net.Sockets;
using Photino.NET;

namespace Desktop;

public class Program
{
	[STAThread]
	public static void Main(string[] args)
	{
		// Resolve the actual exe directory
		var exeDir = Path.GetDirectoryName(Environment.ProcessPath) ?? AppContext.BaseDirectory;

		// Log startup info for debugging
		var logPath = Path.Combine(exeDir, "desktop.log");
		Log(logPath, $"Starting Invoicer Desktop");
		Log(logPath, $"Exe directory: {exeDir}");
		Log(logPath, $"AppContext.BaseDirectory: {AppContext.BaseDirectory}");

		// Verify wwwroot exists
		var wwwrootDir = Path.Combine(exeDir, "wwwroot");
		if (!Directory.Exists(wwwrootDir))
		{
			Log(logPath, $"ERROR: wwwroot not found at {wwwrootDir}");
			return;
		}
		Log(logPath, $"wwwroot found: {wwwrootDir}");

		// Find a free port for the embedded server
		var port = GetAvailablePort();
		var serverUrl = $"http://localhost:{port}";
		Log(logPath, $"Server URL: {serverUrl}");

		// Configure the SQLite database path next to the executable
		var dataDir = Path.Combine(exeDir, "data");
		Directory.CreateDirectory(dataDir);
		var dbPath = Path.Combine(dataDir, "Invoicer.db");
		Log(logPath, $"Database path: {dbPath}");

		// Track server errors
		Exception? serverError = null;
		var serverReady = new ManualResetEventSlim(false);

		// Start the ASP.NET Core backend in a background thread
		var serverThread = new Thread(() =>
		{
			try
			{
				var builder = WebApplication.CreateBuilder(new string[]
				{
					$"--urls={serverUrl}",
					$"--ConnectionStrings:sqliteConnection=Data Source={dbPath}",
					"--DatabaseProvider=Sqlite",
					$"--contentRoot={exeDir}"
				});

				builder.Environment.WebRootPath = wwwrootDir;

				Backend.Program.ConfigureServices(builder);

				var app = builder.Build();

				Backend.Program.ConfigurePipeline(app);

				app.Lifetime.ApplicationStarted.Register(() =>
				{
					Log(logPath, "Kestrel started, signaling ready");
					serverReady.Set();
				});

				Log(logPath, "Starting Kestrel...");
				app.Run();
			}
			catch (Exception ex)
			{
				Log(logPath, $"Server error: {ex}");
				serverError = ex;
				serverReady.Set();
			}
		});
		serverThread.IsBackground = true;
		serverThread.Start();

		// Wait for Kestrel to start
		Log(logPath, "Waiting for server to start...");
		if (!serverReady.Wait(TimeSpan.FromSeconds(60)))
		{
			Log(logPath, "ERROR: Server failed to start within 60 seconds");
			return;
		}

		if (serverError != null)
		{
			Log(logPath, $"ERROR: Server failed: {serverError.Message}");
			return;
		}

		// Verify the server is actually responding via HTTP
		Log(logPath, "Verifying server with HTTP health check...");
		if (!WaitForServerReady(serverUrl, TimeSpan.FromSeconds(15)))
		{
			Log(logPath, "ERROR: Server not responding to HTTP requests");
			return;
		}
		Log(logPath, "Server is ready, opening window");

		// Create the Photino window
		var iconPath = Path.Combine(wwwrootDir, "icon-512.png");
		var window = new PhotinoWindow()
			.SetTitle("Invoicer")
			.SetUseOsDefaultSize(false)
			.SetSize(1400, 900)
			.Center();

		if (File.Exists(iconPath))
			window.SetIconFile(iconPath);

		window.Load(serverUrl);
		window.WaitForClose();
	}

	private static bool WaitForServerReady(string url, TimeSpan timeout)
	{
		using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(3) };
		var deadline = DateTime.UtcNow + timeout;

		while (DateTime.UtcNow < deadline)
		{
			try
			{
				var response = client.GetAsync(url).Result;
				if ((int)response.StatusCode < 500)
					return true;
			}
			catch
			{
				// Not ready yet
			}
			Thread.Sleep(300);
		}
		return false;
	}

	private static int GetAvailablePort()
	{
		var listener = new TcpListener(IPAddress.Loopback, 0);
		listener.Start();
		var port = ((IPEndPoint)listener.LocalEndpoint).Port;
		listener.Stop();
		return port;
	}

	private static void Log(string logPath, string message)
	{
		var line = $"[{DateTime.Now:HH:mm:ss.fff}] {message}";
		Console.WriteLine(line);
		try { File.AppendAllText(logPath, line + Environment.NewLine); } catch { }
	}
}
