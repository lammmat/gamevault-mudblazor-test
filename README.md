# 🎮 GameVault

A gaming library & catalog app built to explore the **MudBlazor + Blazor MAUI Hybrid** tech stack — the same pattern used for building shared-component web and desktop clients.

## What This Is

This project serves as a hands-on playground for a React developer getting familiar with:

- **MudBlazor** (Material Design component library for Blazor — think MUI for .NET)
- **Blazor** (React-like component model, but C# instead of JavaScript)
- **MAUI Blazor Hybrid** (same Blazor components running natively on desktop via WebView)
- **EF Core + SQLite** (persistent local storage — no external server needed)

## Architecture

```
GameVault.slnx
├── GameVault.Shared      ← ALL UI lives here (Razor Class Library)
│   ├── Models/           Data models (Game, GameFilter, GameStats, GameStatus)
│   ├── Services/         IGameService · EfGameService (SQLite) · MockGameService (fallback)
│   ├── Data/             GameDbContext (EF Core) · GameVaultDb (shared DB path resolver)
│   ├── Theme/            GameVaultTheme.cs — central MudTheme (Design System Pillar 1)
│   ├── wwwroot/css/      gamevault.css — CSS variables + effects (Design System Pillar 2)
│   ├── Layout/           MainLayout.razor (AppBar, Drawer, dark/light mode toggle)
│   └── Pages/            Dashboard, Catalog, GameDetail, NotFound
│
├── GameVault.Web         ← Blazor Web App host + Minimal API (7 REST endpoints)
└── GameVault.Maui        ← MAUI Blazor Hybrid host (standalone, no web server needed)
```

The key idea: **write once, run everywhere**. `GameVault.Shared` contains every page and component. Both `GameVault.Web` and `GameVault.Maui` wire up their own SQLite database and host the Blazor WebView — they share the same `gamevault.db` file at the repo root, so edits in one app are visible in the other.

## Data & Backend

Both apps use **EF Core 10 + SQLite** via `EfGameService`. There is no external server.

| | Web (`GameVault.Web`) | MAUI (`GameVault.Maui`) |
|---|---|---|
| Transport | In-process EF Core | In-process EF Core |
| DB path | `gamevault.db` at repo root | same `gamevault.db` at repo root |
| API | 10 Minimal API endpoints (`/api/games/…`) | not needed — direct service call |
| First run | Seeds from `MockGameService` (30 games) | same seed logic |

The database file (`gamevault.db`) is git-ignored. Delete it to re-seed from scratch.

### Minimal API endpoints (Web only)

| Method | Route | Description |
|--------|-------|-------------|
| `GET` | `/api/games` | List games (search, filter, sort) |
| `GET` | `/api/games/{id}` | Get single game |
| `GET` | `/api/games/genres` | All distinct genres |
| `GET` | `/api/games/platforms` | All distinct platforms |
| `GET` | `/api/games/stats` | Dashboard statistics |
| `POST` | `/api/games` | Add a new game |
| `PUT` | `/api/games/{id}` | Update all fields of a game |
| `DELETE` | `/api/games/{id}` | Remove a game |
| `PATCH` | `/api/games/{id}/status` | Update play status |
| `PATCH` | `/api/games/{id}/rating` | Update rating |

## Design System — "VAULT OS"

The UI uses a custom **VAULT OS** aesthetic: deep near-black backgrounds, electric teal glows, Orbitron headings. The design system has two central files — edit either to restyle the entire app:

### Pillar 1 — `GameVault.Shared/Theme/GameVaultTheme.cs`
Central C# class exposing `MudTheme Theme`. Change a value here → all MudBlazor components update instantly.

```csharp
// Key tokens (also mirrored as CSS variables):
public const string PrimaryColor   = "#00E5CC"; // electric teal
public const string SecondaryColor = "#FF4D5A"; // vivid coral
public const string BgVoid         = "#08080F"; // page background
```

### Pillar 2 — `GameVault.Shared/wwwroot/css/gamevault.css`
CSS custom properties that mirror the theme tokens, plus utility classes and effects.

```css
/* Key utility classes: */
.gv-card-hover     /* lift + teal glow on hover */
.gv-stat-card      /* gradient accent strip at top */
.gv-section-title  /* primary-color left-border accent */
.gv-cover-art      /* game cover placeholder with radial glow */
```

Dark / light mode is driven by a `gv-dark` class toggled on `<html>` via JS interop — `:root` holds dark defaults and `html:not(.gv-dark)` overrides for light mode. MudBlazor's `PaletteDark` / `PaletteLight` stay in sync automatically.

**Fonts:** [Orbitron](https://fonts.google.com/specimen/Orbitron) (headings, buttons) + [Nunito](https://fonts.google.com/specimen/Nunito) (body)

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

| Route           | Description                                                                              |
| --------------- | ---------------------------------------------------------------------------------------- |
| `/`             | **Dashboard** — stat cards, genre/platform bars, status breakdown, quick action buttons  |
| `/catalog`      | **Game Catalog** — search, filter by genre/platform/rating, toggle grid ↔ list view; **Add Game** FAB |
| `/catalog/{id}` | **Game Detail** — full info, editable status & star rating; **Edit** and **Delete** buttons |

## MudBlazor Components Used

| Component                                  | Where                             |
| ------------------------------------------ | --------------------------------- |
| `MudAppBar`, `MudDrawer`, `MudNavMenu`     | Layout navigation                 |
| `MudThemeProvider`                         | Dark / light mode toggle          |
| `MudPaper`, `MudCard`                      | Stat cards, game cards            |
| `MudTable`                                 | List view in catalog              |
| `MudTextField`, `MudSelect`                | Search & filter inputs            |
| `MudSlider`                                | Rating range filter               |
| `MudChip`, `MudRating`                     | Genre/platform tags, star ratings |
| `MudExpansionPanel`                        | Collapsible game details          |
| `MudBreadcrumbs`                           | Navigation trail                  |
| `MudSkeleton`                              | Loading states                    |
| `MudProgressLinear`                        | Status breakdown bars             |
| `MudSnackbar`                              | Save confirmation toasts          |
| `MudFab`                                   | "Add Game" floating action button |
| `MudDialog`, `MudDialogProvider`           | Add/Edit game dialog              |
| `MudSnackbarProvider`, `MudDialogProvider` | Global providers in layout        |

## Testing

The project uses **xUnit** for unit tests and **bUnit** for Blazor component tests — bUnit is the de-facto standard for testing Blazor components, roughly analogous to React Testing Library.

### Running tests

```bash
dotnet test GameVault.Tests
```

### Test structure

```
GameVault.Tests/
├── MudBlazorTestBase.cs             ← shared bUnit base class (see "Learnings" below)
├── Services/
│   └── MockGameServiceTests.cs      ← pure C# unit tests (no Blazor)
└── Components/
    ├── GameCardTests.cs
    ├── GameGridTests.cs
    ├── DashboardTests.cs
    ├── CatalogFilterPanelTests.cs
    └── AddEditGameDialogTests.cs
```

| File | What it tests |
|------|--------------|
| `MockGameServiceTests` | Filtering, sorting, stats, full CRUD (add/update/delete) |
| `GameCardTests` | Renders title/rating, genre/platform limits, click callback |
| `GameGridTests` | Card count, empty list, click event propagation |
| `DashboardTests` | Stat values displayed after async load, with a Moq mock |
| `CatalogFilterPanelTests` | Select items rendered, reset callback fires |
| `AddEditGameDialogTests` | Add/Edit mode titles, field pre-population, service invocation |

### bUnit vs React Testing Library — Cheatsheet

| React Testing Library               | bUnit (Blazor)                                                             |
| ----------------------------------- | -------------------------------------------------------------------------- |
| `render(<MyComp prop={val} />)`     | `Render<MyComp>(p => p.Add(c => c.Prop, val))`                             |
| `screen.getByText("hello")`         | `cut.Find("css-selector")` / `Assert.Contains("hello", cut.Markup)`       |
| `fireEvent.click(el)`               | `cut.Find("button").Click()` (wrap in `InvokeAsync` for async events)      |
| `jest.fn()` / `vi.fn()`             | `EventCallback.Factory.Create<T>(this, handler)` as parameter              |
| `jest.mock('./service')`            | `new Mock<IService>()` (Moq) + `Services.AddSingleton<IService>(mock.Object)` |
| `waitFor(() => expect(…))`          | `await cut.WaitForStateAsync(() => cut.Markup.Contains(…))`               |
| `screen.getAllByRole("option")`     | `cut.FindComponents<MudSelectItem<string>>()`                              |

### Learnings & gotchas — bUnit with MudBlazor 9.x

These tripped us up so you don't have to rediscover them:

#### 1. `MudPopoverProvider` must be in the render tree

MudBlazor components that open floating elements (`MudSelect`, `MudAutocomplete`, `MudMenu`, …) call into `PopoverService` on init. Without `MudPopoverProvider` rendered first, every such test throws:

```
InvalidOperationException: Missing <MudPopoverProvider />
```

**Fix:** render it once before the component under test. In `MudBlazorTestBase` this is done inside `IAsyncLifetime.InitializeAsync()` (see point 3 for why not in the constructor).

#### 2. MudBlazor 9.x services are `IAsyncDisposable`-only

`KeyInterceptorService` and `PointerEventsNoneService` (registered by `AddMudServices()`) only implement `IAsyncDisposable`, not `IDisposable`. When bUnit/xUnit sync-disposes the test class at the end of a test, the DI container throws:

```
InvalidOperationException: 'MudBlazor.KeyInterceptorService' type only implements IAsyncDisposable. Use DisposeAsync to dispose the container.
```

**Fix:** implement `IAsyncLifetime` on test classes, call `await DisposeAsync()` in `DisposeAsync()`, and override `Dispose(bool)` to be a no-op so the sync path is suppressed. This is encapsulated in `MudBlazorTestBase`.

#### 3. Register mock services *before* any `Render<>()` call

`BunitContext` lazily builds the DI container on the first `Render<>()`. Anything added to `Services` *after* that call is invisible to rendered components.

**Fix:** in `MudBlazorTestBase`, `AddMudServices()` runs in the constructor (before subclasses add their own mocks), and `Render<MudPopoverProvider>()` runs in `InitializeAsync()` — which xUnit calls *after* the subclass constructor finishes.

#### 4. MudSelect items live in a popover, not in `cut.Markup`

`MudSelect` renders its `MudSelectItem` children inside a `MudPopover` that is collapsed by default. Checking `cut.Markup` for option text will fail — the text isn't there.

**Fix:** query the component tree instead:

```csharp
var items = cut.FindComponents<MudSelectItem<string>>();
Assert.Contains("Action", items.Select(i => i.Instance.Value));
```

#### 5. Culture-safe number assertions

`ToString("F1")` is culture-sensitive

**Fix:** use the same format call in the assertion as the component does:

```csharp
Assert.Contains(game.Rating.ToString("F1"), cut.Markup); // matches whatever culture renders
```

#### 6. `MudDialog` sections only render inside `MudDialogProvider`

`TitleContent`, `DialogContent`, and `DialogActions` render slots do **not** produce markup when a `MudDialog` is rendered directly via `Render<MyDialog>()`. The content is invisible, `@ref` fields (like `MudForm`) remain null, and `FindComponent` calls fail.

**Fix:** use the full dialog infrastructure:

```csharp
var provider = Render<MudDialogProvider>();
var dialogService = Services.GetRequiredService<IDialogService>();
IDialogReference? dialogRef = null;
await provider.InvokeAsync(async () =>
    dialogRef = await dialogService.ShowAsync<AddEditGameDialog>("Title", parameters));
var dialog = provider.FindComponent<AddEditGameDialog>();
```

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
| `tailwind.config.js`         | `GameVaultTheme.cs` (C# tokens) + `gamevault.css` (CSS variables)        |
| `localStorage` / `IndexedDB` | EF Core + SQLite via `IDbContextFactory<GameDbContext>`                   |
