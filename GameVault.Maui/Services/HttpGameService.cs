using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using GameVault.Shared.Models;
using GameVault.Shared.Services;

namespace GameVault.Maui.Services;

public class HttpGameService(HttpClient http) : IGameService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<List<Game>> GetGamesAsync(GameFilter? filter = null)
    {
        var url = BuildGamesUrl(filter);
        return await http.GetFromJsonAsync<List<Game>>(url, JsonOptions) ?? [];
    }

    public async Task<Game?> GetByIdAsync(int id) =>
        await http.GetFromJsonAsync<Game>($"api/games/{id}", JsonOptions);

    public async Task<GameStats> GetStatsAsync() =>
        await http.GetFromJsonAsync<GameStats>("api/games/stats", JsonOptions) ?? new();

    public IReadOnlyList<string> GetAllGenres() =>
        http.GetFromJsonAsync<List<string>>("api/games/genres", JsonOptions).GetAwaiter().GetResult() ?? [];

    public IReadOnlyList<string> GetAllPlatforms() =>
        http.GetFromJsonAsync<List<string>>("api/games/platforms", JsonOptions).GetAwaiter().GetResult() ?? [];

    public async Task UpdateStatusAsync(int id, GameStatus status)
    {
        var body = JsonSerializer.Serialize(new { Status = status.ToString() }, JsonOptions);
        await http.PatchAsync($"api/games/{id}/status", new StringContent(body, Encoding.UTF8, "application/json"));
    }

    public async Task UpdateRatingAsync(int id, double rating)
    {
        var body = JsonSerializer.Serialize(new { Rating = rating }, JsonOptions);
        await http.PatchAsync($"api/games/{id}/rating", new StringContent(body, Encoding.UTF8, "application/json"));
    }

    private static string BuildGamesUrl(GameFilter? filter)
    {
        if (filter is null) return "api/games";

        var qs = new StringBuilder("api/games?");
        if (!string.IsNullOrWhiteSpace(filter.SearchText))
            qs.Append($"search={Uri.EscapeDataString(filter.SearchText)}&");
        if (filter.Genres.Count > 0)
            qs.Append($"genres={Uri.EscapeDataString(string.Join(',', filter.Genres))}&");
        if (filter.Platforms.Count > 0)
            qs.Append($"platforms={Uri.EscapeDataString(string.Join(',', filter.Platforms))}&");
        if (filter.MinRating > 0)
            qs.Append($"minRating={filter.MinRating}&");
        if (filter.MaxRating < 10)
            qs.Append($"maxRating={filter.MaxRating}&");
        qs.Append($"sortBy={Uri.EscapeDataString(filter.SortBy)}&sortDescending={filter.SortDescending}");
        return qs.ToString().TrimEnd('&');
    }
}
