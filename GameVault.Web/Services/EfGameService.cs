using GameVault.Shared.Models;
using GameVault.Shared.Services;
using GameVault.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace GameVault.Web.Services;

public class EfGameService(GameDbContext db) : IGameService
{
    public async Task<List<Game>> GetGamesAsync(GameFilter? filter = null)
    {
        var games = await db.Games.ToListAsync();
        var query = games.AsEnumerable();

        if (filter is not null)
        {
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                var term = filter.SearchText.ToLower();
                query = query.Where(g =>
                    g.Title.ToLower().Contains(term) ||
                    g.Developer.ToLower().Contains(term) ||
                    g.Description.ToLower().Contains(term));
            }

            if (filter.Genres.Count > 0)
                query = query.Where(g => g.Genres.Any(genre => filter.Genres.Contains(genre)));

            if (filter.Platforms.Count > 0)
                query = query.Where(g => g.Platforms.Any(p => filter.Platforms.Contains(p)));

            query = query.Where(g => g.Rating >= filter.MinRating && g.Rating <= filter.MaxRating);

            query = filter.SortBy switch
            {
                "Rating" => filter.SortDescending ? query.OrderByDescending(g => g.Rating) : query.OrderBy(g => g.Rating),
                "ReleaseYear" => filter.SortDescending ? query.OrderByDescending(g => g.ReleaseYear) : query.OrderBy(g => g.ReleaseYear),
                _ => filter.SortDescending ? query.OrderByDescending(g => g.Title) : query.OrderBy(g => g.Title),
            };
        }

        return query.ToList();
    }

    public async Task<Game?> GetByIdAsync(int id) =>
        await db.Games.FindAsync(id);

    public async Task<GameStats> GetStatsAsync()
    {
        var games = await db.Games.ToListAsync();
        return new GameStats
        {
            TotalGames = games.Count,
            AverageRating = games.Count > 0 ? Math.Round(games.Average(g => g.Rating), 1) : 0,
            GamesByGenre = games
                .SelectMany(g => g.Genres)
                .GroupBy(g => g)
                .OrderByDescending(g => g.Count())
                .Take(8)
                .ToDictionary(g => g.Key, g => g.Count()),
            GamesByPlatform = games
                .SelectMany(g => g.Platforms)
                .GroupBy(p => p)
                .OrderByDescending(p => p.Count())
                .ToDictionary(p => p.Key, p => p.Count()),
            GamesByStatus = games
                .GroupBy(g => g.Status.ToString())
                .ToDictionary(g => g.Key, g => g.Count())
        };
    }

    public IReadOnlyList<string> GetAllGenres() =>
        db.Games.AsEnumerable()
            .SelectMany(g => g.Genres)
            .Distinct()
            .OrderBy(g => g)
            .ToList();

    public IReadOnlyList<string> GetAllPlatforms() =>
        db.Games.AsEnumerable()
            .SelectMany(g => g.Platforms)
            .Distinct()
            .OrderBy(p => p)
            .ToList();

    public async Task UpdateStatusAsync(int id, GameStatus status)
    {
        var game = await db.Games.FindAsync(id);
        if (game is null) return;
        game.Status = status;
        await db.SaveChangesAsync();
    }

    public async Task UpdateRatingAsync(int id, double rating)
    {
        var game = await db.Games.FindAsync(id);
        if (game is null) return;
        game.Rating = rating;
        await db.SaveChangesAsync();
    }
}
