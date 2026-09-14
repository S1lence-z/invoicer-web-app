; ============================================================================
;  Invoicer - Windows installer
;
;  Build:
;      dotnet publish Desktop/Desktop.csproj -c Release -r win-x64 --self-contained \
;             -p:Version=1.2.0 -p:DebugType=none -o publish-desktop
;      "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" /DMyAppVersion=1.2.0 installer\Invoicer.iss
;
;  Or just run ..\build-installer.ps1 -Version 1.2.0, which does both.
;
;  Requires Inno Setup 6.3+ (for ArchitecturesAllowed=x64compatible).
;  This file must stay UTF-8 *with BOM* - it contains non-ASCII characters.
; ============================================================================

#ifndef MyAppVersion
  #define MyAppVersion "0.0.0"
#endif

; VersionInfoVersion only accepts a numeric a.b.c.d, so drop any pre-release
; suffix: "1.2.0-beta.1" -> "1.2.0". ISPP: Pos(SubStr, S), Copy(S, Index, Count).
#if Pos("-", MyAppVersion) > 0
  #define MyAppVersionNumeric Copy(MyAppVersion, 1, Pos("-", MyAppVersion) - 1)
#else
  #define MyAppVersionNumeric MyAppVersion
#endif

#define MyAppName      "Invoicer"
#define MyAppPublisher "Jiří Zelenka"
#define MyAppURL       "https://github.com/S1lence-z/invoicer-web-app"
#define MyAppExeName   "Invoicer.exe"
#define MyPublishDir   "..\publish-desktop"

[Setup]
; NEVER change AppId. It is the identity Windows and Inno use to recognise an
; existing installation for upgrades and uninstall. A new GUID here means every
; user ends up with two "Invoicer" entries in Apps & features.
AppId={{02CCE63C-886E-4A13-9E9A-62C689A28991}

AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
VersionInfoVersion={#MyAppVersionNumeric}
VersionInfoProductVersion={#MyAppVersionNumeric}
VersionInfoProductTextVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}/issues
AppUpdatesURL={#MyAppURL}/releases
AppCopyright=Copyright (C) 2025 {#MyAppPublisher}

; Per-user install: no elevation, no UAC prompt, nothing written outside the
; user's own profile.
PrivilegesRequired=lowest
DefaultDirName={localappdata}\Programs\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
UsePreviousAppDir=yes
AllowNoIcons=yes

; 64-bit only. MinVersion is the .NET 8 + WebView2 Evergreen supported floor.
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
MinVersion=10.0.17763

OutputDir=..\artifacts
OutputBaseFilename=InvoicerSetup-{#MyAppVersion}
SetupIconFile=..\Desktop\Assets\icon.ico
UninstallDisplayIcon={app}\{#MyAppExeName}
UninstallDisplayName={#MyAppName} {#MyAppVersion}

Compression=lzma2/max
SolidCompression=yes
WizardStyle=modern
ShowLanguageDialog=auto
; Offer to close a running Invoicer instead of failing to overwrite Invoicer.exe.
CloseApplications=yes
RestartApplications=no

[Languages]
Name: "en"; MessagesFile: "compiler:Default.isl"
Name: "cs"; MessagesFile: "compiler:Languages\Czech.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

; Clear out leftovers from an older layout (loose assemblies, stale fingerprinted
; Blazor assets) before laying down the new build. {app} never holds user data -
; the database lives in {localappdata}\Invoicer - so this is always safe.
[InstallDelete]
Type: filesandordirs; Name: "{app}\wwwroot"
Type: filesandordirs; Name: "{app}\runtimes"
Type: files;          Name: "{app}\*.dll"
Type: files;          Name: "{app}\*.pdb"
Type: files;          Name: "{app}\*.deps.json"
Type: files;          Name: "{app}\*.runtimeconfig.json"

[Files]
Source: "{#MyPublishDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs; Excludes: "*.pdb"

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}";  Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
; Photino needs the Edge WebView2 runtime. Present on Windows 11 and updated
; Windows 10, but not guaranteed - the bootstrapper installs per-user when we are
; not elevated, so this still needs no UAC prompt.
Filename: "{tmp}\MicrosoftEdgeWebview2Setup.exe"; Parameters: "/silent /install"; \
    StatusMsg: "Installing Microsoft Edge WebView2 Runtime..."; \
    Flags: waituntilterminated; Check: NeedsWebView2
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; \
    Flags: nowait postinstall skipifsilent

[UninstallDelete]
; Self-extracted native libraries from the single-file bundle. Cosmetic - Windows
; would clear these with the temp folder eventually.
Type: filesandordirs; Name: "{localappdata}\Temp\.net\{#MyAppName}"

[Code]
var
  DownloadPage: TDownloadWizardPage;

function WebView2Version(RootKey: Integer; SubKey: String): String;
begin
  if not RegQueryStringValue(RootKey, SubKey, 'pv', Result) then
    Result := '';
  if Result = '0.0.0.0' then
    Result := '';
end;

function NeedsWebView2: Boolean;
begin
  Result :=
    (WebView2Version(HKLM, 'SOFTWARE\WOW6432Node\Microsoft\EdgeUpdate\Clients\{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}') = '') and
    (WebView2Version(HKLM, 'SOFTWARE\Microsoft\EdgeUpdate\Clients\{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}') = '') and
    (WebView2Version(HKCU, 'SOFTWARE\Microsoft\EdgeUpdate\Clients\{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}') = '');
end;

procedure InitializeWizard;
begin
  DownloadPage := CreateDownloadPage(SetupMessage(msgWizardPreparing), SetupMessage(msgPreparingDesc), nil);
end;

function NextButtonClick(CurPageID: Integer): Boolean;
begin
  Result := True;
  if (CurPageID = wpReady) and NeedsWebView2 then
  begin
    DownloadPage.Clear;
    DownloadPage.Add('https://go.microsoft.com/fwlink/p/?LinkId=2124703', 'MicrosoftEdgeWebview2Setup.exe', '');
    DownloadPage.Show;
    try
      try
        DownloadPage.Download;
      except
        if not SuppressibleMsgBox(
             'The Microsoft Edge WebView2 Runtime could not be downloaded.' + #13#10#13#10 +
             'Invoicer needs it to display its window. You can continue and install it ' +
             'later from https://developer.microsoft.com/microsoft-edge/webview2/.' + #13#10#13#10 +
             'Continue anyway?',
             mbError, MB_YESNO or MB_DEFBUTTON2, IDYES) = IDYES then
          Result := False;
      end;
    finally
      DownloadPage.Hide;
    end;
  end;
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
var
  DataDir: String;
begin
  if CurUninstallStep = usPostUninstall then
  begin
    DataDir := ExpandConstant('{localappdata}\{#MyAppName}');
    if DirExists(DataDir) then
    begin
      // The invoice database lives here. Deleting it silently would be data loss,
      // so we ask, default to No, and keep the data on a silent uninstall.
      if SuppressibleMsgBox(
           'Also delete your Invoicer data?' + #13#10#13#10 +
           DataDir + #13#10#13#10 +
           'This contains your invoice database (Invoicer.db) and the diagnostic log. ' +
           'Choose No to keep it for a future reinstall.',
           mbConfirmation, MB_YESNO or MB_DEFBUTTON2, IDNO) = IDYES then
        DelTree(DataDir, True, True, True);
    end;
  end;
end;
