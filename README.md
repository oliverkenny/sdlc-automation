# Team Dashboard

A team dashboard application built with Blazor WebAssembly, showing team member cards, project progress indicators, and real-time status updates.

## Tech Stack

- **Framework:** .NET 8 Blazor WebAssembly (standalone)
- **Testing:** xUnit + bUnit
- **Deployment:** Cloudflare Pages
- **CI/CD:** GitHub Actions

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later

### Run Locally

```bash
dotnet run --project src/TeamDashboard
```

### Run Tests

```bash
dotnet test
```

### Build for Production

```bash
dotnet publish src/TeamDashboard/TeamDashboard.csproj -c Release -o publish
```

The static output in `publish/wwwroot` is deployed to Cloudflare Pages.

## Project Structure

```
├── src/TeamDashboard/       # Blazor WASM application
│   ├── Pages/               # Routable page components
│   ├── Components/          # Reusable UI components
│   ├── Services/            # Data and business logic services
│   └── wwwroot/             # Static assets
├── tests/TeamDashboard.Tests/  # bUnit + xUnit tests
├── infrastructure/          # Terraform (Cloudflare Pages)
└── .github/workflows/       # CI/CD pipelines
```