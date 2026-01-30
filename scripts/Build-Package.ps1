# Build Package Script for Runnable NuGet Package
# This script builds and packages the Runnable project for NuGet distribution

param(
    [string]$Configuration = "Release",
    [string]$OutputDir = "./nupkg",
    [switch]$Clean = $false,
    [switch]$Verbose = $false
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

# Get project root directory
$ProjectRoot = Split-Path -Parent $PSScriptRoot
$SrcDir = Join-Path $ProjectRoot "src" "Runnable"

Write-ColorOutput "========================================" $Colors.Info
Write-ColorOutput "Runnable NuGet Package Builder" $Colors.Info
Write-ColorOutput "========================================" $Colors.Info
Write-ColorOutput ""

# Validate project file exists
$ProjectFile = Join-Path $SrcDir "Runnable.csproj"
if (-not (Test-Path $ProjectFile)) {
    Write-ColorOutput "ERROR: Project file not found at $ProjectFile" $Colors.Error
    exit 1
}

Write-ColorOutput "Project file: $ProjectFile" $Colors.Info
Write-ColorOutput "Configuration: $Configuration" $Colors.Info
Write-ColorOutput "Output directory: $OutputDir" $Colors.Info
Write-ColorOutput ""

# Clean previous builds if requested
if ($Clean) {
    Write-ColorOutput "Cleaning previous builds..." $Colors.Warning
    $CleanPaths = @(
        (Join-Path $SrcDir "bin"),
        (Join-Path $SrcDir "obj"),
        (Join-Path $SrcDir $OutputDir)
    )
    
    foreach ($Path in $CleanPaths) {
        if (Test-Path $Path) {
            Remove-Item -Path $Path -Recurse -Force | Out-Null
            Write-ColorOutput "  ✓ Removed $Path" $Colors.Success
        }
    }
    Write-ColorOutput ""
}

# Create output directory if it doesn't exist
$FullOutputDir = Join-Path $SrcDir $OutputDir
if (-not (Test-Path $FullOutputDir)) {
    New-Item -ItemType Directory -Path $FullOutputDir -Force | Out-Null
}

# Run dotnet pack
Write-ColorOutput "Building NuGet package..." $Colors.Info
$PackArgs = @(
    "pack"
    "$ProjectFile"
    "-c", $Configuration
    "-o", $FullOutputDir
)

if ($Verbose) {
    $PackArgs += "-v", "detailed"
}

try {
    & dotnet @PackArgs
    if ($LASTEXITCODE -ne 0) {
        Write-ColorOutput "ERROR: dotnet pack failed with exit code $LASTEXITCODE" $Colors.Error
        exit 1
    }
}
catch {
    Write-ColorOutput "ERROR: Failed to run dotnet pack: $_" $Colors.Error
    exit 1
}

Write-ColorOutput ""
Write-ColorOutput "========================================" $Colors.Info

# Find and display the generated package
$NupkgFile = Get-ChildItem -Path $FullOutputDir -Filter "*.nupkg" -ErrorAction SilentlyContinue | 
    Select-Object -First 1

if ($NupkgFile) {
    $SizeMB = [math]::Round($NupkgFile.Length / 1MB, 2)
    Write-ColorOutput "✓ Package created successfully!" $Colors.Success
    Write-ColorOutput ""
    Write-ColorOutput "Package Details:" $Colors.Info
    Write-ColorOutput "  Name: $($NupkgFile.Name)" $Colors.Success
    Write-ColorOutput "  Size: $SizeMB MB" $Colors.Success
    Write-ColorOutput "  Location: $($NupkgFile.FullName)" $Colors.Success
    Write-ColorOutput ""
    Write-ColorOutput "Next step: Run Publish-Package.ps1 to upload to NuGet.org" $Colors.Info
    Write-ColorOutput "========================================" $Colors.Info
    exit 0
}
else {
    Write-ColorOutput "ERROR: No .nupkg file found in output directory" $Colors.Error
    exit 1
}
