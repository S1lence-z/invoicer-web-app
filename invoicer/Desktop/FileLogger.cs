using Microsoft.Extensions.Logging;

namespace Desktop;

/// <summary>
/// Minimal ILogger that appends warnings and errors to the desktop log file.
/// The desktop executable has no console on Windows, so this is the only place
/// ASP.NET Core / EF Core diagnostics end up.
/// </summary>
public sealed class FileLoggerProvider(string logPath) : ILoggerProvider
{
	private static readonly object _lock = new();

	public ILogger CreateLogger(string categoryName) => new FileLogger(logPath, categoryName);

	public void Dispose() { }

	public static void Write(string file, string line)
	{
		var stamped = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {line}";
		lock (_lock)
		{
			try { File.AppendAllText(file, stamped + Environment.NewLine); } catch { /* never let logging crash the app */ }
		}
	}

	private sealed class FileLogger(string filePath, string category) : ILogger
	{
		public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

		public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Warning;

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
		{
			if (!IsEnabled(logLevel))
				return;

			var message = $"{logLevel} {category}: {formatter(state, exception)}";
			if (exception is not null)
				message += Environment.NewLine + exception;
			Write(filePath, message);
		}
	}
}
