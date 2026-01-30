# NuGet Package Build and Publish Scripts

This directory contains PowerShell scripts for building and publishing the Runnable NuGet package.

## Prerequisites

- .NET 6.0 SDK or later
- PowerShell 5.1 or later
- NuGet account (for publishing): https://www.nuget.org/

## Scripts

### 1. Build-Package.ps1

Builds and packages the Runnable library into a .nupkg file.

#### Usage

```powershell
# Basic usage (default Release configuration)
.\Build-Package.ps1

# With specific configuration
.\Build-Package.ps1 -Configuration Release

# Clean previous builds before building
.\Build-Package.ps1 -Clean

# Verbose output
.\Build-Package.ps1 -Verbose

# Custom output directory
.\Build-Package.ps1 -OutputDir "./my-nupkg"
```

#### Parameters

- **Configuration**: Build configuration (default: "Release")
  - `Release` - Optimized for distribution
  - `Debug` - For development

- **OutputDir**: Directory to output the .nupkg file (default: "./nupkg")

- **Clean**: Remove previous build and object directories before building

- **Verbose**: Show detailed build output

#### Output

```
========================================
Runnable NuGet Package Builder
========================================

Project file: D:\Project\codenjwu\Runnable\src\Runnable\Runnable.csproj
Configuration: Release
Output directory: ./nupkg

Building NuGet package...

========================================
✓ Package created successfully!

Package Details:
  Name: Runnable.1.0.0.nupkg
  Size: 1.33 MB
  Location: D:\Project\codenjwu\Runnable\src\Runnable\nupkg\Runnable.1.0.0.nupkg

Next step: Run Publish-Package.ps1 to upload to NuGet.org
========================================
```

---

### 2. Publish-Package.ps1

Publishes the built package to NuGet.org.

#### Usage

```powershell
# Interactive mode (searches for latest .nupkg)
.\Publish-Package.ps1

# With specific package file
.\Publish-Package.ps1 -PackagePath "./nupkg/Runnable.1.0.0.nupkg"

# With API key
.\Publish-Package.ps1 -ApiKey "your-api-key"

# Dry run (show what would happen without publishing)
.\Publish-Package.ps1 -DryRun

# Non-interactive mode (no prompts)
.\Publish-Package.ps1 -NoPrompt -ApiKey "your-api-key"
```

#### Parameters

- **PackagePath**: Path to the .nupkg file to publish
  - If not provided, automatically finds the latest in `src/Runnable/nupkg/`

- **ApiKey**: NuGet API key for authentication
  - If not provided, you'll be prompted to enter it
  - Get from: https://www.nuget.org/ > Account Settings > API Keys

- **Source**: NuGet feed URL (default: "https://api.nuget.org/v3/index.json")

- **DryRun**: Show what would be published without actually publishing

- **NoPrompt**: Skip confirmation prompts

#### Getting Your API Key

1. Sign in to https://www.nuget.org/
2. Go to **Account Settings** > **API Keys**
3. Click **Create** new API key
4. Configure scope: **Push new packages and package versions**
5. Set expiration date as needed
6. Copy the key and save it securely

#### Output Example

```
========================================
Runnable NuGet Package Publisher
========================================

Found package: Runnable.1.0.0.nupkg
Package: Runnable.1.0.0.nupkg
Size: 1.33 MB
Location: D:\Project\codenjwu\Runnable\src\Runnable\nupkg\Runnable.1.0.0.nupkg
Target: https://api.nuget.org/v3/index.json

Publishing package...

========================================
✓ Package published successfully!

Package will be available at:
https://www.nuget.org/packages/Runnable/

Installation command:
dotnet add package Runnable
========================================
```

---

## Complete Workflow

### Step 1: Build the Package

```powershell
cd D:\Project\codenjwu\Runnable\scripts
.\Build-Package.ps1 -Clean
```

This will:
- Clean previous builds
- Compile the project for all supported frameworks (net6.0, net8.0, net9.0, net10.0)
- Create the .nupkg file in `src/Runnable/nupkg/`

### Step 2: Test Locally (Optional)

```powershell
# Test installation in a temporary project
dotnet new console -n TestRunnable
cd TestRunnable
dotnet add package Runnable -s "D:\Project\codenjwu\Runnable\src\Runnable\nupkg"
```

### Step 3: Publish to NuGet

```powershell
cd D:\Project\codenjwu\Runnable\scripts
.\Publish-Package.ps1 -ApiKey "your-api-key"
```

Or interactively:
```powershell
.\Publish-Package.ps1
# Follow the prompts
```

---

## Troubleshooting

### "dotnet: The term 'dotnet' is not recognized"

Install .NET SDK from https://dotnet.microsoft.com/download

### "Cannot be loaded because running scripts is disabled"

If you get a script execution policy error, run:

```powershell
# For current user only
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser

# Or bypass for single execution
powershell -ExecutionPolicy Bypass -File .\Build-Package.ps1
```

### "Package already exists"

NuGet.org doesn't allow uploading the same version twice. You must:
- Increment the version in `Runnable.csproj`
- Rebuild the package
- Publish again

Update version in `src/Runnable/Runnable.csproj`:
```xml
<PropertyGroup>
    <Version>1.0.1</Version>
    ...
</PropertyGroup>
```

### "Authentication failed"

- Verify your API key is correct
- Ensure the key has "Push" permissions
- Keys expire after the date you set

---

## Version Management

Edit `src/Runnable/Runnable.csproj` to update version:

```xml
<PropertyGroup>
    <Version>1.0.0</Version>
    ...
</PropertyGroup>
```

Also update `CHANGELOG.md` with release notes.

---

## References

- NuGet Documentation: https://docs.microsoft.com/nuget/
- Publish Packages: https://docs.microsoft.com/nuget/nuget-org/publish-a-package
- API Keys: https://docs.microsoft.com/nuget/nuget-org/scoped-api-keys
