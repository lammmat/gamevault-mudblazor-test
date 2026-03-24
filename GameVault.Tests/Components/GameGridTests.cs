using Bunit;
using GameVault.Shared.Components;
using GameVault.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace GameVault.Tests.Components;

public class GameGridTests : MudBlazorTestBase
{
    // Services, JSInterop, and MudPopoverProvider are set up in MudBlazorTestBase

    private static List<Game> MakeGames(int count) =>
        Enumerable.Range(1, count).Select(i => new Game
        {
            Id = i,
            Title = $"Game {i}",
            Developer = "Dev",
            ReleaseYear = 2020,
            Rating = 8.0,
            Genres = ["Action"],
            Platforms = ["PC"],
            Status = GameStatus.Backlog
        }).ToList();

    [Fact]
    public void GameGrid_RendersCorrectNumberOfCards()
    {
        var games = MakeGames(4);
        var cut = Render<GameGrid>(p => p.Add(c => c.Games, games));
        var cards = cut.FindComponents<GameCard>();
        Assert.Equal(4, cards.Count);
    }

    [Fact]
    public void GameGrid_EmptyList_RendersNoCards()
    {
        var cut = Render<GameGrid>(p => p.Add(c => c.Games, []));
        var cards = cut.FindComponents<GameCard>();
        Assert.Empty(cards);
    }

    [Fact]
    public async Task GameGrid_CardClick_BubblesGameId()
    {
        int? receivedId = null;
        var games = MakeGames(2);

        var cut = Render<GameGrid>(p => p
            .Add(c => c.Games, games)
            .Add(c => c.OnGameClick, EventCallback.Factory.Create<int>(this, id => receivedId = id)));

        // click the first card's mud-card element
        await cut.InvokeAsync(() => cut.FindAll(".mud-card")[0].Click());

        Assert.Equal(1, receivedId);
    }
}
