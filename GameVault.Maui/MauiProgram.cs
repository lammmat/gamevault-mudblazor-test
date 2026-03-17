using GameVault.Shared.Data;
using GameVault.Shared.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;

namespace GameVault.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddMudServices();

        var dbPath = GameVaultDb.GetDbPath();
        builder.Services.AddDbContextFactory<GameDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));
        builder.Services.AddSingleton<IGameService, EfGameService>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        // Ensure DB is created and seed if empty
        var factory = app.Services.GetRequiredService<IDbContextFactory<GameDbContext>>();
        using var db = factory.CreateDbContext();
        db.Database.EnsureCreated();
        if (!db.Games.Any())
        {
            var seed = new MockGameService();
            var games = seed.GetGamesAsync().GetAwaiter().GetResult();
            db.Games.AddRange(games);
            db.SaveChanges();
        }

        return app;
    }
}
