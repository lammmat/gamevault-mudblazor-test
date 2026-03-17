using GameVault.Shared.Models;
using GameVault.Shared.Services;
using GameVault.Web.Components;
using GameVault.Web.Data;
using GameVault.Web.Services;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();
builder.Services.AddDbContext<GameDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=gamevault.db"));
builder.Services.AddScoped<IGameService, EfGameService>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Ensure DB is created and seed if empty
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GameDbContext>();
    db.Database.EnsureCreated();
    if (!db.Games.Any())
    {
        var seed = new MockGameService();
        var games = await seed.GetGamesAsync();
        db.Games.AddRange(games);
        await db.SaveChangesAsync();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found");
app.UseHttpsRedirection();
app.UseAntiforgery();

// Minimal API endpoints
var api = app.MapGroup("/api/games");

api.MapGet("", async (IGameService svc,
    string? search, string? genres, string? platforms,
    double minRating = 0, double maxRating = 10,
    string sortBy = "Title", bool sortDescending = false) =>
{
    var filter = new GameFilter
    {
        SearchText = search,
        Genres = genres?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? [],
        Platforms = platforms?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? [],
        MinRating = minRating,
        MaxRating = maxRating,
        SortBy = sortBy,
        SortDescending = sortDescending
    };
    return await svc.GetGamesAsync(filter);
});

api.MapGet("{id:int}", async (int id, IGameService svc) =>
    await svc.GetByIdAsync(id) is { } game ? Results.Ok(game) : Results.NotFound());

api.MapGet("stats", async (IGameService svc) =>
    Results.Ok(await svc.GetStatsAsync()));

api.MapGet("genres", (IGameService svc) =>
    Results.Ok(svc.GetAllGenres()));

api.MapGet("platforms", (IGameService svc) =>
    Results.Ok(svc.GetAllPlatforms()));

api.MapPatch("{id:int}/status", async (int id, StatusUpdateRequest req, IGameService svc) =>
{
    await svc.UpdateStatusAsync(id, req.Status);
    return Results.NoContent();
});

api.MapPatch("{id:int}/rating", async (int id, RatingUpdateRequest req, IGameService svc) =>
{
    await svc.UpdateRatingAsync(id, req.Rating);
    return Results.NoContent();
});

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(GameVault.Shared.Layout.MainLayout).Assembly);

app.Run();

record StatusUpdateRequest(GameStatus Status);
record RatingUpdateRequest(double Rating);
