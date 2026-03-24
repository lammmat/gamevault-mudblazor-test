using Bunit;
using GameVault.Shared.Components;
using GameVault.Shared.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace GameVault.Tests.Components;

public class GameCardTests : MudBlazorTestBase
{
    // Services, JSInterop, and MudPopoverProvider are set up in MudBlazorTestBase

    private static Game MakeGame(int id = 1) => new()
    {
        Id = id,
        Title = "Test Game",
        Developer = "Test Dev",
        ReleaseYear = 2024,
        Rating = 8.5,
        Genres = ["Action", "RPG", "Puzzle"],
        Platforms = ["PC", "PlayStation", "Xbox", "Nintendo Switch"],
        Status = GameStatus.Playing
    };

    [Fact]
    public void GameCard_RendersTitle()
    {
        var cut = Render<GameCard>(p => p.Add(c => c.Game, MakeGame()));
        Assert.Contains("Test Game", cut.Markup);
    }

    [Fact]
    public void GameCard_RendersRating()
    {
        var game = MakeGame();
        var cut = Render<GameCard>(p => p.Add(c => c.Game, game));
        Assert.Contains(game.Rating.ToString("F1"), cut.Markup);
    }

    [Fact]
    public void GameCard_RendersUpToTwoGenres()
    {
        var cut = Render<GameCard>(p => p.Add(c => c.Game, MakeGame()));
        // Game has 3 genres but component uses .Take(2) — only first two should appear
        Assert.Contains("Action", cut.Markup);
        Assert.Contains("RPG", cut.Markup);
        Assert.DoesNotContain("Puzzle", cut.Markup);
    }

    [Fact]
    public void GameCard_RendersUpToThreePlatforms()
    {
        var cut = Render<GameCard>(p => p.Add(c => c.Game, MakeGame()));
        // Game has 4 platforms but component uses .Take(3)
        Assert.Contains("PC", cut.Markup);
        Assert.Contains("PlayStation", cut.Markup);
        Assert.Contains("Xbox", cut.Markup);
        Assert.DoesNotContain("Nintendo Switch", cut.Markup);
    }

    [Fact]
    public async Task GameCard_Click_InvokesCallbackWithCorrectId()
    {
        int? clickedId = null;
        var cut = Render<GameCard>(p => p
            .Add(c => c.Game, MakeGame(id: 42))
            .Add(c => c.OnClick, EventCallback.Factory.Create<int>(this, id => clickedId = id)));

        await cut.InvokeAsync(() => cut.Find(".mud-card").Click());

        Assert.Equal(42, clickedId);
    }
}
