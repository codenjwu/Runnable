# Publish Package Script for Runnable NuGet Package
# This script publishes the built Runnable package to NuGet.org

param(
    [string]$PackagePath = $null,
    [string]$ApiKey = $null,
    [string]$Source = "https://api.nuget.org/v3/index.json",
    [switch]$DryRun = $false,
    [switch]$NoPrompt = $false
)

$ErrorActionPreference = "Stop"

# Colors for output
$Colors = @{
    Success = 'Green'
    Error   = 'Red'
    Warning = 'Yellow'
    Info    = 'Cyan'
}

function Write-ColorOutput {
    param([string]$Message, [string]$Color = 'White')
    Write-Host $Message -ForegroundColor $Color
}

Write-ColorOutput "========================================" $Colors.Info
Write-ColorOutput "Runnable NuGet Package Publisher" $Colors.Info
Write-ColorOutput "========================================" $Colors.Info
Write-ColorOutput ""

# If no package path provided, search for the latest .nupkg
if (-not $PackagePath) {
    $ProjectRoot = Split-Path -Parent $PSScriptRoot
    $SearchDir = Join-Path $ProjectRoot "src" "Runnable" "nupkg"
    
    if (Test-Path $SearchDir) {
        $NupkgFile = Get-ChildItem -Path $SearchDir -Filter "*.nupkg" -ErrorAction SilentlyContinue | 
            Sort-Object -Property LastWriteTime -Descending | 
            Select-Object -First 1
        
        if ($NupkgFile) {
            $PackagePath = $NupkgFile.FullName
            Write-ColorOutput "Found package: $($NupkgFile.Name)" $Colors.Info
        }
    }
}

# Validate package file exists
if (-not $PackagePath -or -not (Test-Path $PackagePath)) {
    Write-ColorOutput "ERROR: Package file not found" $Colors.Error
    Write-ColorOutput "Usage: .\Publish-Package.ps1 -PackagePath '<path-to-nupkg>'" $Colors.Warning
    exit 1
}

if (-not $PackagePath.EndsWith(".nupkg")) {
    Write-ColorOutput "ERROR: File is not a .nupkg package" $Colors.Error
    exit 1
}

$PackageFile = Get-Item $PackagePath
$SizeMB = [math]::Round($PackageFile.Length / 1MB, 2)

Write-ColorOutput "Package: $($PackageFile.Name)" $Colors.Info
Write-ColorOutput "Size: $SizeMB MB" $Colors.Info
Write-ColorOutput "Location: $PackagePath" $Colors.Info
Write-ColorOutput "Target: $Source" $Colors.Info
Write-ColorOutput ""

# Check if API key is provided
if (-not $ApiKey) {
    Write-ColorOutput "API Key not provided. To publish, you need:" $Colors.Warning
    Write-ColorOutput ""
    Write-ColorOutput "1. Get API key from: https://www.nuget.org/" $Colors.Info
    Write-ColorOutput "   - Sign in or create account" $Colors.Info
    Write-ColorOutput "   - Go to Account Settings > API Keys" $Colors.Info
    Write-ColorOutput "   - Create new key with 'Push new packages and package versions' scope" $Colors.Info
    Write-ColorOutput ""
    Write-ColorOutput "2. Then run one of these commands:" $Colors.Info
    Write-ColorOutput "   Option A - Provide API key as parameter:" $Colors.Info
    Write-ColorOutput "   .\Publish-Package.ps1 -ApiKey 'your-api-key'" $Colors.Info
    Write-ColorOutput ""
    Write-ColorOutput "   Option B - Store API key securely (recommended):" $Colors.Info
    Write-ColorOutput "   dotnet nuget update source nuget.org -u __USERNAME__ -p 'your-api-key'" $Colors.Info
    Write-ColorOutput "   Then run: .\Publish-Package.ps1" $Colors.Info
    Write-ColorOutput ""
    
    if ($NoPrompt) {
        exit 1
    }
    
    $ApiKeyInput = Read-Host "Enter your NuGet API key (or press Enter to skip)"
    if (-not [string]::IsNullOrWhiteSpace($ApiKeyInput)) {
        $ApiKey = $ApiKeyInput
    }
    else {
        Write-ColorOutput "Publish cancelled" $Colors.Warning
        exit 0
    }
}

Write-ColorOutput ""
Write-ColorOutput "========================================" $Colors.Info

if ($DryRun) {
    Write-ColorOutput "DRY RUN MODE - No changes will be made" $Colors.Warning
    Write-ColorOutput ""
    Write-ColorOutput "Command that would be executed:" $Colors.Info
    Write-ColorOutput "dotnet nuget push '$PackagePath' -k '***API-KEY***' -s '$Source'" $Colors.Info
    Write-ColorOutput ""
    exit 0
}

# Ask for confirmation
if (-not $NoPrompt) {
    Write-ColorOutput ""
    Write-ColorOutput "Publishing will:" $Colors.Warning
    Write-ColorOutput "  • Upload package to NuGet.org" $Colors.Warning
    Write-ColorOutput "  • Make it publicly available" $Colors.Warning
    Write-ColorOutput "  • Cannot be deleted (only unlisted)" $Colors.Warning
    Write-ColorOutput ""
    $Confirm = Read-Host "Continue with publishing? (yes/no)"
    if ($Confirm -ne "yes") {
        Write-ColorOutput "Publish cancelled" $Colors.Warning
        exit 0
    }
}

Write-ColorOutput ""
Write-ColorOutput "Publishing package..." $Colors.Info

# Run dotnet nuget push
$PushArgs = @(
    "nuget", "push"
    "$PackagePath"
    "-k", $ApiKey
    "-s", $Source
)

try {
    & dotnet @PushArgs
    if ($LASTEXITCODE -ne 0) {
        Write-ColorOutput "ERROR: dotnet nuget push failed with exit code $LASTEXITCODE" $Colors.Error
        exit 1
    }
}
catch {
    Write-ColorOutput "ERROR: Failed to run dotnet nuget push: $_" $Colors.Error
    exit 1
}

Write-ColorOutput ""
Write-ColorOutput "========================================" $Colors.Info
Write-ColorOutput "✓ Package published successfully!" $Colors.Success
Write-ColorOutput ""
Write-ColorOutput "Package will be available at:" $Colors.Info
Write-ColorOutput "https://www.nuget.org/packages/Runnable/" $Colors.Success
Write-ColorOutput ""
Write-ColorOutput "Installation command:" $Colors.Info
Write-ColorOutput "dotnet add package Runnable" $Colors.Success
Write-ColorOutput "========================================" $Colors.Info
