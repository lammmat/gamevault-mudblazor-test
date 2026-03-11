namespace GameVault.Shared.Models;

public class Game
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Developer { get; set; } = "";
    public string Publisher { get; set; } = "";
    public int ReleaseYear { get; set; }
    public double Rating { get; set; }
    public string[] Genres { get; set; } = [];
    public string[] Platforms { get; set; } = [];
    public string[] Tags { get; set; } = [];
    public GameStatus Status { get; set; }
}
