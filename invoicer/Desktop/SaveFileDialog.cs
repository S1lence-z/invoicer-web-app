using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Photino.NET;

namespace Desktop;

/// <summary>
/// Native "Save file" dialog that starts in the user's Downloads folder with the suggested file name filled in.
/// Photino's <see cref="PhotinoWindow.ShowSaveFile"/> only accepts a start folder (tryphotino/photino.NET#140),
/// so on Windows the classic comdlg32 dialog is called directly; other platforms fall back to Photino's dialog.
/// </summary>
internal static class SaveFileDialog
{
	/// <summary>Returns the chosen path, or null when the user cancelled.</summary>
	public static string? Show(PhotinoWindow window, string title, string suggestedFileName)
	{
		string startFolder = GetDownloadsFolder();

		if (OperatingSystem.IsWindows())
		{
			var (path, cancelled) = ShowWin32(window, title, startFolder, suggestedFileName);
			if (cancelled || path is not null)
				return path;
			// Win32 call failed: fall through to Photino's dialog so the user still gets a way to save
		}

		return window.ShowSaveFile(title, startFolder, [("PDF", ["*.pdf"])]);
	}

	private static string GetDownloadsFolder()
	{
		string? downloads = null;

		if (OperatingSystem.IsWindows())
			downloads = GetWindowsKnownFolder(DownloadsFolderId);

		if (downloads is null)
		{
			string profile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			downloads = Path.Combine(profile, "Downloads");
		}

		return Directory.Exists(downloads)
			? downloads
			: Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
	}

	// ---- Windows: comdlg32 GetSaveFileName -------------------------------------------------------

	private static readonly Guid DownloadsFolderId = new("374DE290-123F-4565-9164-39C4925E467B");

	private const int OFN_OVERWRITEPROMPT = 0x00000002;
	private const int OFN_NOCHANGEDIR = 0x00000008;
	private const int OFN_PATHMUSTEXIST = 0x00000800;
	private const int OFN_EXPLORER = 0x00080000;
	private const int MaxPath = 32767;

	[SupportedOSPlatform("windows")]
	private static string? GetWindowsKnownFolder(Guid folderId)
	{
		try
		{
			int hr = SHGetKnownFolderPath(folderId, 0, IntPtr.Zero, out string path);
			return hr == 0 ? path : null;
		}
		catch
		{
			return null;
		}
	}

	/// <summary>Returns (path, cancelled). Path null with cancelled false means the dialog failed to open.</summary>
	[SupportedOSPlatform("windows")]
	private static (string? Path, bool Cancelled) ShowWin32(PhotinoWindow window, string title, string startFolder, string suggestedFileName)
	{
		IntPtr fileBuffer = IntPtr.Zero;
		IntPtr filterBuffer = IntPtr.Zero;
		try
		{
			// Buffer receives the chosen path; it is pre-filled with the suggested name
			fileBuffer = Marshal.AllocHGlobal(MaxPath * 2);
			var name = SanitizeFileName(suggestedFileName);
			var nameBytes = new byte[(name.Length + 1) * 2];
			System.Text.Encoding.Unicode.GetBytes(name, 0, name.Length, nameBytes, 0);
			Marshal.Copy(new byte[MaxPath * 2], 0, fileBuffer, MaxPath * 2);
			Marshal.Copy(nameBytes, 0, fileBuffer, nameBytes.Length);

			// Filter pairs are separated by NUL and the list ends with a double NUL
			filterBuffer = Marshal.StringToHGlobalUni("PDF (*.pdf)\0*.pdf\0\0");

			var ofn = new OpenFileName
			{
				lStructSize = Marshal.SizeOf<OpenFileName>(),
				hwndOwner = TryGetHandle(window),
				lpstrFilter = filterBuffer,
				nFilterIndex = 1,
				lpstrFile = fileBuffer,
				nMaxFile = MaxPath,
				lpstrInitialDir = startFolder,
				lpstrTitle = title,
				Flags = OFN_OVERWRITEPROMPT | OFN_NOCHANGEDIR | OFN_PATHMUSTEXIST | OFN_EXPLORER,
				lpstrDefExt = "pdf",
			};

			if (GetSaveFileNameW(ref ofn))
				return (Marshal.PtrToStringUni(fileBuffer), false);

			int error = CommDlgExtendedError();
			if (error == 0)
				return (null, true); // user cancelled

			Program.Log($"GetSaveFileName failed with CommDlgExtendedError 0x{error:X}");
			return (null, false);
		}
		catch (Exception ex)
		{
			Program.Log($"Win32 save dialog failed: {ex}");
			return (null, false);
		}
		finally
		{
			if (fileBuffer != IntPtr.Zero) Marshal.FreeHGlobal(fileBuffer);
			if (filterBuffer != IntPtr.Zero) Marshal.FreeHGlobal(filterBuffer);
		}
	}

	[SupportedOSPlatform("windows")]
	private static IntPtr TryGetHandle(PhotinoWindow window)
	{
		try { return window.WindowHandle; }
		catch { return IntPtr.Zero; }
	}

	private static string SanitizeFileName(string fileName)
	{
		var invalid = Path.GetInvalidFileNameChars();
		var cleaned = new string(fileName.Select(c => invalid.Contains(c) ? '_' : c).ToArray());
		return string.IsNullOrWhiteSpace(cleaned) ? "invoice.pdf" : cleaned;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	private struct OpenFileName
	{
		public int lStructSize;
		public IntPtr hwndOwner;
		public IntPtr hInstance;
		public IntPtr lpstrFilter;
		public IntPtr lpstrCustomFilter;
		public int nMaxCustFilter;
		public int nFilterIndex;
		public IntPtr lpstrFile;
		public int nMaxFile;
		public IntPtr lpstrFileTitle;
		public int nMaxFileTitle;
		[MarshalAs(UnmanagedType.LPWStr)] public string? lpstrInitialDir;
		[MarshalAs(UnmanagedType.LPWStr)] public string? lpstrTitle;
		public int Flags;
		public short nFileOffset;
		public short nFileExtension;
		[MarshalAs(UnmanagedType.LPWStr)] public string? lpstrDefExt;
		public IntPtr lCustData;
		public IntPtr lpfnHook;
		public IntPtr lpTemplateName;
		public IntPtr pvReserved;
		public int dwReserved;
		public int FlagsEx;
	}

	[DllImport("comdlg32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	private static extern bool GetSaveFileNameW(ref OpenFileName ofn);

	[DllImport("comdlg32.dll")]
	private static extern int CommDlgExtendedError();

	[DllImport("shell32.dll", CharSet = CharSet.Unicode)]
	private static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, [MarshalAs(UnmanagedType.LPWStr)] out string pszPath);
}
