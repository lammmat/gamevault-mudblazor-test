# 🎮 GameVault

A gaming library & catalog app built to explore the **MudBlazor + Blazor MAUI Hybrid** tech stack — the same pattern used for building shared-component web and desktop clients.

## What This Is

This project serves as a hands-on playground for a React developer getting familiar with:

- **MudBlazor** (Material Design component library for Blazor — think MUI for .NET)
- **Blazor** (React-like component model, but C# instead of JavaScript)
- **MAUI Blazor Hybrid** (same Blazor components running natively on desktop via WebView)

## Architecture

```
GameVault.slnx
├── GameVault.Shared      ← ALL UI lives here (Razor Class Library)
│   ├── Models/           Data models (Game, GameFilter, GameStats, GameStatus)
│   ├── Services/         IGameService + MockGameService (30 games, no backend needed)
│   ├── Layout/           MainLayout.razor (AppBar, Drawer, dark/light mode)
│   └── Pages/            Dashboard, Catalog, GameDetail, NotFound
│
├── GameVault.Web         ← Blazor Web App host (thin wrapper for the browser)
└── GameVault.Maui        ← MAUI Blazor Hybrid host (thin wrapper for native desktop)
```

The key idea: **write once, run everywhere**. `GameVault.Shared` contains every page and component. Both `GameVault.Web` and `GameVault.Maui` just wire up dependency injection and host the Blazor WebView.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- MAUI workload: `dotnet workload install maui`
- VS Code with the [C# Dev Kit extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)

## Running the App

### Option A — VS Code (Recommended)

Press **F5** or go to the **Run & Debug** panel and choose:

- **🌐 GameVault Web** — opens in your browser at `http://localhost:5227`
- **🖥️ GameVault MAUI (Windows)** — launches a native Windows desktop window

### Option B — Terminal

```bash
# Web app
dotnet run --project GameVault.Web

# with watch for hot reloading
dotnet watch --project GameVault.Web

# MAUI desktop app (Windows)
dotnet run --project GameVault.Maui -f net10.0-windows10.0.19041.0
```

## Pages

| Route           | Description                                                                         |
| --------------- | ----------------------------------------------------------------------------------- |
| `/`             | **Dashboard** — stat cards, genre donut chart, platform bar chart, status breakdown |
| `/catalog`      | **Game Catalog** — search, filter by genre/platform/rating, toggle grid ↔ list view |
| `/catalog/{id}` | **Game Detail** — full info, genres/tags as chips, expandable sections              |

## MudBlazor Components Used

| Component                                  | Where                             |
| ------------------------------------------ | --------------------------------- |
| `MudAppBar`, `MudDrawer`, `MudNavMenu`     | Layout navigation                 |
| `MudThemeProvider`                         | Dark / light mode toggle          |
| `MudPaper`, `MudCard`                      | Stat cards, game cards            |
| `MudChart`                                 | Genre donut + platform bar chart  |
| `MudTable`                                 | List view in catalog              |
| `MudTextField`, `MudSelect`                | Search & filter inputs            |
| `MudSlider`                                | Rating range filter               |
| `MudChip`, `MudRating`                     | Genre/platform tags, star ratings |
| `MudExpansionPanel`                        | Collapsible game details          |
| `MudBreadcrumbs`                           | Navigation trail                  |
| `MudSkeleton`                              | Loading states                    |
| `MudProgressLinear`                        | Status breakdown bars             |
| `MudSnackbarProvider`, `MudDialogProvider` | Global providers in layout        |

## React → Blazor Cheatsheet

| React                        | Blazor                                                                   |
| ---------------------------- | ------------------------------------------------------------------------ |
| `function MyComp({ title })` | `@code { [Parameter] public string Title { get; set; } }`                |
| `useState(false)`            | `private bool _open = false;` + `StateHasChanged()`                      |
| `useEffect(() => {}, [])`    | `protected override async Task OnInitializedAsync()`                     |
| `useContext` / Provider      | `@inject IGameService GameService` (DI)                                  |
| `<Link to="/catalog">`       | `<MudNavLink Href="/catalog">`                                           |
| React Router `useParams`     | `[Parameter] public int Id { get; set; }` on `@page "/catalog/{Id:int}"` |
| `npm install mudblazor`      | `dotnet add package MudBlazor`                                           |
| `tailwind.config.js`         | `MudTheme` with `PaletteDark` / `PaletteLight`                           |

## Mock Data

The app ships with 30 fictional games in `MockGameService.cs` — no backend, no database. The `IGameService` interface makes it easy to swap in a real HTTP API later:

```csharp
// Swap mock for a real API client:
builder.Services.AddScoped<IGameService, GameApiClient>();
```
