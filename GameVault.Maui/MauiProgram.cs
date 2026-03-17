using GameVault.Maui.Services;
using GameVault.Shared.Services;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;

namespace GameVault.Maui;

public static class MauiProgram
{
    // Change this to match where GameVault.Web is running (http for local dev to avoid cert issues)
    private const string ApiBaseUrl = "http://localhost:5000/";

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

        builder.Services.AddSingleton(sp =>
            new HttpClient { BaseAddress = new Uri(ApiBaseUrl) });
        builder.Services.AddSingleton<IGameService, HttpGameService>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
