#Requires -Version 5.1
<#
.SYNOPSIS
    Publishes the Invoicer desktop app for Windows and builds the Inno Setup installer.
.DESCRIPTION
    Produces artifacts\InvoicerSetup-<Version>.exe - a per-user installer that needs
    no administrator rights. Inno Setup 6 must be installed:
        winget install -e --id JRSoftware.InnoSetup
.EXAMPLE
    .\build-installer.ps1 -Version 1.2.0
#>
[CmdletBinding()]
param(
    [string] $Version = '0.0.0-dev',
    [ValidateSet('win-x64')]
    [string] $Rid = 'win-x64'
)

$ErrorActionPreference = 'Stop'
Set-Location -LiteralPath $PSScriptRoot

$publishDir = Join-Path $PSScriptRoot 'publish-desktop'
if (Test-Path $publishDir) { Remove-Item $publishDir -Recurse -Force }

Write-Host "Publishing Invoicer $Version for $Rid..." -ForegroundColor Cyan
# DebugType is passed here rather than set in Desktop.csproj so that it reaches the
# referenced projects too - otherwise their .pdb files land loose next to the exe.
dotnet publish Desktop/Desktop.csproj `
    -c Release `
    -r $Rid `
    --self-contained `
    -p:Version=$Version `
    -p:DebugType=none `
    -o $publishDir `
    --nologo
if ($LASTEXITCODE -ne 0) { throw 'dotnet publish failed' }

$iscc = @(
    "${env:ProgramFiles(x86)}\Inno Setup 6\ISCC.exe",
    "$env:ProgramFiles\Inno Setup 6\ISCC.exe",
    "$env:LOCALAPPDATA\Programs\Inno Setup 6\ISCC.exe"
) | Where-Object { Test-Path $_ } | Select-Object -First 1

if (-not $iscc) {
    throw "Inno Setup 6 was not found. Install it with:  winget install -e --id JRSoftware.InnoSetup"
}

Write-Host "Compiling installer with $iscc..." -ForegroundColor Cyan
& $iscc "/DMyAppVersion=$Version" (Join-Path $PSScriptRoot 'installer\Invoicer.iss')
if ($LASTEXITCODE -ne 0) { throw "ISCC failed with exit code $LASTEXITCODE" }

Get-ChildItem (Join-Path $PSScriptRoot 'artifacts') -Filter 'InvoicerSetup-*.exe' |
    ForEach-Object { Write-Host "Installer: $($_.FullName)" -ForegroundColor Green }
