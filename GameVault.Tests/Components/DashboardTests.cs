using Bunit;
using GameVault.Shared.Models;
using GameVault.Shared.Pages;
using GameVault.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace GameVault.Tests.Components;

public class DashboardTests : MudBlazorTestBase
{
    private readonly Mock<IGameService> _svcMock = new();

    private static GameStats MakeStats() => new()
    {
        TotalGames = 15,
        AverageRating = 8.3,
        GamesByStatus = new Dictionary<string, int>
        {
            ["Playing"]   = 4,
            ["Completed"] = 7,
            ["Backlog"]   = 3,
            ["Dropped"]   = 1
        },
        GamesByGenre = new Dictionary<string, int> { ["Action"] = 6 },
        GamesByPlatform = new Dictionary<string, int> { ["PC"] = 10 }
    };

    public DashboardTests()
    {
        // Services and JSInterop are set up in MudBlazorTestBase
        _svcMock.Setup(s => s.GetStatsAsync()).ReturnsAsync(MakeStats());
        Services.AddSingleton<IGameService>(_svcMock.Object);
    }

    [Fact]
    public async Task Dashboard_ShowsTotalGamesCount()
    {
        var cut = Render<Dashboard>();
        await cut.WaitForStateAsync(() => cut.Markup.Contains("15"));
        Assert.Contains("15", cut.Markup);
    }

    [Fact]
    public async Task Dashboard_ShowsAverageRating()
    {
        var expectedRating = MakeStats().AverageRating.ToString("F1");
        var cut = Render<Dashboard>();
        await cut.WaitForStateAsync(() => cut.Markup.Contains(expectedRating));
        Assert.Contains(expectedRating, cut.Markup);
    }

    [Fact]
    public async Task Dashboard_ShowsCurrentlyPlayingCount()
    {
        var cut = Render<Dashboard>();
        await cut.WaitForStateAsync(() => cut.Markup.Contains("4"));
        Assert.Contains("4", cut.Markup);
    }

    [Fact]
    public async Task Dashboard_ShowsCompletedCount()
    {
        var cut = Render<Dashboard>();
        await cut.WaitForStateAsync(() => cut.Markup.Contains("7"));
        Assert.Contains("7", cut.Markup);
    }
}
