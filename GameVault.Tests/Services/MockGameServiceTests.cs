using GameVault.Shared.Models;
using GameVault.Shared.Services;

namespace GameVault.Tests.Services;

public class MockGameServiceTests
{
    private static MockGameService CreateService() => new();

    // ── GetGamesAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task GetGamesAsync_NoFilter_ReturnsAllGames()
    {
        var svc = CreateService();
        var games = await svc.GetGamesAsync();
        Assert.Equal(30, games.Count);
    }

    [Fact]
    public async Task GetGamesAsync_SearchText_FiltersByTitleOrDeveloper()
    {
        var svc = CreateService();
        var filter = new GameFilter { SearchText = "neon" };
        var games = await svc.GetGamesAsync(filter);
        Assert.All(games, g =>
            Assert.True(
                g.Title.Contains("neon", StringComparison.OrdinalIgnoreCase) ||
                g.Developer.Contains("neon", StringComparison.OrdinalIgnoreCase) ||
                g.Description.Contains("neon", StringComparison.OrdinalIgnoreCase)));
    }

    [Fact]
    public async Task GetGamesAsync_SearchText_NoMatch_ReturnsEmpty()
    {
        var svc = CreateService();
        var filter = new GameFilter { SearchText = "zzznomatchzzz" };
        var games = await svc.GetGamesAsync(filter);
        Assert.Empty(games);
    }

    [Fact]
    public async Task GetGamesAsync_GenreFilter_ReturnsOnlyMatchingGames()
    {
        var svc = CreateService();
        var filter = new GameFilter { Genres = ["Racing"] };
        var games = await svc.GetGamesAsync(filter);
        Assert.NotEmpty(games);
        Assert.All(games, g => Assert.Contains("Racing", g.Genres));
    }

    [Fact]
    public async Task GetGamesAsync_PlatformFilter_ReturnsOnlyMatchingGames()
    {
        var svc = CreateService();
        var filter = new GameFilter { Platforms = ["Nintendo Switch"] };
        var games = await svc.GetGamesAsync(filter);
        Assert.NotEmpty(games);
        Assert.All(games, g => Assert.Contains("Nintendo Switch", g.Platforms));
    }

    [Fact]
    public async Task GetGamesAsync_RatingRangeFilter_ReturnsMatchingGames()
    {
        var svc = CreateService();
        var filter = new GameFilter { MinRating = 9.0, MaxRating = 10.0 };
        var games = await svc.GetGamesAsync(filter);
        Assert.NotEmpty(games);
        Assert.All(games, g => Assert.InRange(g.Rating, 9.0, 10.0));
    }

    [Fact]
    public async Task GetGamesAsync_SortByRating_Ascending_IsSorted()
    {
        var svc = CreateService();
        var filter = new GameFilter { SortBy = "Rating", SortDescending = false };
        var games = await svc.GetGamesAsync(filter);
        for (int i = 1; i < games.Count; i++)
            Assert.True(games[i].Rating >= games[i - 1].Rating);
    }

    [Fact]
    public async Task GetGamesAsync_SortByTitle_Descending_IsSorted()
    {
        var svc = CreateService();
        var filter = new GameFilter { SortBy = "Title", SortDescending = true };
        var games = await svc.GetGamesAsync(filter);
        for (int i = 1; i < games.Count; i++)
            Assert.True(
                string.Compare(games[i].Title, games[i - 1].Title, StringComparison.OrdinalIgnoreCase) <= 0);
    }

    [Fact]
    public async Task GetGamesAsync_SortByReleaseYear_Ascending_IsSorted()
    {
        var svc = CreateService();
        var filter = new GameFilter { SortBy = "ReleaseYear", SortDescending = false };
        var games = await svc.GetGamesAsync(filter);
        for (int i = 1; i < games.Count; i++)
            Assert.True(games[i].ReleaseYear >= games[i - 1].ReleaseYear);
    }

    // ── GetByIdAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task GetByIdAsync_ValidId_ReturnsCorrectGame()
    {
        var svc = CreateService();
        var game = await svc.GetByIdAsync(1);
        Assert.NotNull(game);
        Assert.Equal(1, game.Id);
        Assert.Equal("Neon Blade: Origins", game.Title);
    }

    [Fact]
    public async Task GetByIdAsync_InvalidId_ReturnsNull()
    {
        var svc = CreateService();
        var game = await svc.GetByIdAsync(9999);
        Assert.Null(game);
    }

    // ── GetStatsAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task GetStatsAsync_ReturnsCorrectTotalGames()
    {
        var svc = CreateService();
        var stats = await svc.GetStatsAsync();
        Assert.Equal(30, stats.TotalGames);
    }

    [Fact]
    public async Task GetStatsAsync_AverageRatingIsReasonable()
    {
        var svc = CreateService();
        var stats = await svc.GetStatsAsync();
        Assert.InRange(stats.AverageRating, 1.0, 10.0);
    }

    [Fact]
    public async Task GetStatsAsync_GamesByStatusCoversAllStatuses()
    {
        var svc = CreateService();
        var stats = await svc.GetStatsAsync();
        var total = stats.GamesByStatus.Values.Sum();
        Assert.Equal(stats.TotalGames, total);
    }

    // ── UpdateStatusAsync ─────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateStatusAsync_ChangesGameStatus()
    {
        var svc = CreateService();
        await svc.UpdateStatusAsync(1, GameStatus.Backlog);
        var game = await svc.GetByIdAsync(1);
        Assert.Equal(GameStatus.Backlog, game!.Status);
    }

    [Fact]
    public async Task UpdateStatusAsync_InvalidId_DoesNotThrow()
    {
        var svc = CreateService();
        var ex = await Record.ExceptionAsync(() => svc.UpdateStatusAsync(9999, GameStatus.Playing));
        Assert.Null(ex);
    }

    // ── UpdateRatingAsync ─────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateRatingAsync_ChangesGameRating()
    {
        var svc = CreateService();
        await svc.UpdateRatingAsync(1, 6.5);
        var game = await svc.GetByIdAsync(1);
        Assert.Equal(6.5, game!.Rating);
    }

    // ── GetAllGenres / GetAllPlatforms ────────────────────────────────────────

    [Fact]
    public void GetAllGenres_ReturnsDistinctSortedList()
    {
        var svc = CreateService();
        var genres = svc.GetAllGenres();
        Assert.NotEmpty(genres);
        Assert.Equal(genres.Count, genres.Distinct().Count());
        Assert.Equal(genres, genres.OrderBy(g => g).ToList());
    }

    [Fact]
    public void GetAllPlatforms_ReturnsDistinctSortedList()
    {
        var svc = CreateService();
        var platforms = svc.GetAllPlatforms();
        Assert.NotEmpty(platforms);
        Assert.Equal(platforms.Count, platforms.Distinct().Count());
        Assert.Equal(platforms, platforms.OrderBy(p => p).ToList());
    }

    // ── AddGameAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task AddGameAsync_AddsGameAndReturnsWithNewId()
    {
        var svc = CreateService();
        var newGame = new Game { Title = "Test Game", Developer = "Dev", Publisher = "Pub", ReleaseYear = 2024 };
        var added = await svc.AddGameAsync(newGame);
        Assert.True(added.Id > 0);
        var all = await svc.GetGamesAsync();
        Assert.Contains(all, g => g.Id == added.Id && g.Title == "Test Game");
    }

    [Fact]
    public async Task AddGameAsync_AssignsUniqueId()
    {
        var svc = CreateService();
        var a = await svc.AddGameAsync(new Game { Title = "A" });
        var b = await svc.AddGameAsync(new Game { Title = "B" });
        Assert.NotEqual(a.Id, b.Id);
    }

    // ── UpdateGameAsync ───────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateGameAsync_UpdatesAllFields()
    {
        var svc = CreateService();
        var game = await svc.GetByIdAsync(1);
        Assert.NotNull(game);
        game.Title = "Updated Title";
        game.Developer = "New Dev";
        game.Rating = 5.5;
        game.Genres = ["Indie"];
        await svc.UpdateGameAsync(game);
        var updated = await svc.GetByIdAsync(1);
        Assert.Equal("Updated Title", updated!.Title);
        Assert.Equal("New Dev", updated.Developer);
        Assert.Equal(5.5, updated.Rating);
        Assert.Contains("Indie", updated.Genres);
    }

    // ── DeleteGameAsync ───────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteGameAsync_RemovesGameFromList()
    {
        var svc = CreateService();
        await svc.DeleteGameAsync(1);
        var all = await svc.GetGamesAsync();
        Assert.DoesNotContain(all, g => g.Id == 1);
    }

    [Fact]
    public async Task DeleteGameAsync_InvalidId_DoesNotThrow()
    {
        var svc = CreateService();
        var ex = await Record.ExceptionAsync(() => svc.DeleteGameAsync(9999));
        Assert.Null(ex);
    }
}
