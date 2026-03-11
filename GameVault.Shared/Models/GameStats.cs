namespace GameVault.Shared.Models;

public class GameStats
{
    public int TotalGames { get; set; }
    public double AverageRating { get; set; }
    public Dictionary<string, int> GamesByGenre { get; set; } = [];
    public Dictionary<string, int> GamesByPlatform { get; set; } = [];
    public Dictionary<string, int> GamesByStatus { get; set; } = [];
}
