namespace GameVault.Shared.Models;

public class GameFilter
{
    public string? SearchText { get; set; }
    public List<string> Genres { get; set; } = [];
    public List<string> Platforms { get; set; } = [];
    public double MinRating { get; set; } = 0;
    public double MaxRating { get; set; } = 10;
    public string SortBy { get; set; } = "Title";
    public bool SortDescending { get; set; }
}
