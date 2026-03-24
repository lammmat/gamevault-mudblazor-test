# Copilot Instructions – GameVault MudBlazor

## Architecture

Three-project solution with a **shared Razor Class Library (RCL)** pattern:

```
GameVault.Shared   ← ALL UI code lives here (pages, components, services, models)
GameVault.Web      ← Thin host: ASP.NET Core Blazor Web App (net10.0)
GameVault.Maui     ← Thin host: MAUI Blazor Hybrid desktop app (net10.0-windows)
```

`GameVault.Shared` contains every Razor page and component. The two host projects only differ in:
- DI registration (`Scoped` in Web, `Singleton` in MAUI)
- Platform-specific hosting (ASP.NET Core vs. MAUI `BlazorWebView`)

When adding a new page or component, **always add it to `GameVault.Shared`**, not to either host.

## Build & Run

```bash
# Web app
dotnet run --project GameVault.Web

# Web with hot reload
dotnet watch --project GameVault.Web

# MAUI desktop (Windows)
dotnet run --project GameVault.Maui -f net10.0-windows10.0.19041.0
```

VS Code launch configs (`F5`): `🌐 GameVault Web` (port 5227) and `🖥️ GameVault MAUI (Windows)`.

Tests live in `GameVault.Tests` — run with `dotnet test GameVault.Tests`. There are no lint configurations.

## Service Layer

All data access goes through `IGameService` (defined in `GameVault.Shared/Services/`):

```csharp
Task<List<Game>> GetGamesAsync(GameFilter? filter = null);
Task<Game?> GetByIdAsync(int id);
Task<GameStats> GetStatsAsync();
IReadOnlyList<string> GetAllGenres();
IReadOnlyList<string> GetAllPlatforms();
Task<Game> AddGameAsync(Game game);
Task UpdateGameAsync(Game game);
Task DeleteGameAsync(int id);
```

### ⚠️ THREE implementations — update ALL of them whenever the interface changes

| Implementation | Location | Used by |
|---|---|---|
| `MockGameService` | `GameVault.Shared/Services/MockGameService.cs` | Tests + fallback |
| `EfGameService` | `GameVault.Web/Services/EfGameService.cs` | Web app (SQLite via EF Core) |
| `HttpGameService` | `GameVault.Maui/Services/HttpGameService.cs` | MAUI app (calls Web REST API) |

When you add a method to `IGameService`, you **must** implement it in all three classes or the MAUI build will break with `CS0535`.

Inject the service in Razor components with `@inject IGameService GameService`.

## Component Conventions

**File locations:**
- `Pages/` — routable components (have `@page` directive)
- `Components/` — reusable UI components (no route)
- `Layout/` — layout components (`MainLayout.razor`)

**Standard component structure:**
```razor
@inject IGameService GameService
@inject NavigationManager Nav

<!-- markup -->

@code {
    [Parameter, EditorRequired] public Game Game { get; set; } = null!;
    [Parameter] public EventCallback<int> OnClick { get; set; }

    private List<Game> _games = [];
    private bool _loading;

    protected override async Task OnInitializedAsync()
    {
        _loading = true;
        _games = await GameService.GetGamesAsync();
        _loading = false;
    }
}
```

- Private fields use `_camelCase`.
- `OnInitializedAsync()` for one-time page load; `OnParametersSetAsync()` when route parameters can change (e.g., `GameDetail`).
- Required component parameters use `[Parameter, EditorRequired]`.
- Child→parent communication uses `EventCallback<T>` (never direct method calls).
- Programmatic navigation: `Nav.NavigateTo($"/catalog/{id}")`.

## Routing

| Route | Component |
|---|---|
| `/` | `Pages/Dashboard.razor` |
| `/catalog` | `Pages/Catalog.razor` |
| `/catalog/{Id:int}` | `Pages/GameDetail.razor` |

404s are caught by `app.UseStatusCodePagesWithReExecute("/not-found")` in `GameVault.Web/Program.cs`.

## MudBlazor Patterns

**All four global providers must appear in `MainLayout.razor`:**
```razor
<MudThemeProvider @ref="_themeProvider" Theme="_theme" @bind-IsDarkMode="_isDarkMode" />
<MudPopoverProvider />
<MudDialogProvider />
<MudSnackbarProvider />
```

**Responsive grid** — standard breakpoints used throughout:
```razor
<MudGrid>
    <MudItem xs="12" sm="6" lg="4">...</MudItem>
</MudGrid>
```

**Loading states** use `MudSkeleton` (not spinners).

**Color mapping** uses switch expressions:
```csharp
private Color GetStatusColor(string status) => status switch
{
    "Playing"   => Color.Info,
    "Completed" => Color.Success,
    "Dropped"   => Color.Error,
    _           => Color.Warning
};
```

**Theme** is defined in `MainLayout.razor` as a `MudTheme` with a custom dark palette (primary `#7e6fff`, secondary `#ff6b9d`). Always extend the existing theme rather than creating a new one.

## Key Packages

| Package | Version |
|---|---|
| MudBlazor | 9.1.0 |
| Microsoft.AspNetCore.Components.Web | 10.0.4 |
| Microsoft.Maui.Controls | (MauiVersion) |
| Microsoft.AspNetCore.Components.WebView.Maui | (MauiVersion) |

## Nullable & Language Features

All projects have `<Nullable>enable</Nullable>` and `<ImplicitUsings>enable</ImplicitUsings>`. Use:
- Collection expressions: `[]` and `[.. source]`
- Switch expressions for color/icon mapping
- `= null!` for non-nullable fields initialized in lifecycle methods
