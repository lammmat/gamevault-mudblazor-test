using GameVault.Shared.Models;

namespace GameVault.Shared.Services;

public interface IGameService
{
    Task<List<Game>> GetGamesAsync(GameFilter? filter = null);
    Task<Game?> GetByIdAsync(int id);
    Task<GameStats> GetStatsAsync();
    IReadOnlyList<string> GetAllGenres();
    IReadOnlyList<string> GetAllPlatforms();
    Task UpdateStatusAsync(int id, GameStatus status);
    Task UpdateRatingAsync(int id, double rating);
}
