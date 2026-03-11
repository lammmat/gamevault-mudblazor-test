using GameVault.Shared.Models;

namespace GameVault.Shared.Services;

public class MockGameService : IGameService
{
    private static readonly List<Game> _games =
    [
        new() { Id = 1, Title = "Neon Blade: Origins", Developer = "Synth Studios", Publisher = "Apex Digital", ReleaseYear = 2023, Rating = 8.5, Genres = ["Action", "RPG"], Platforms = ["PC", "PlayStation"], Tags = ["Singleplayer", "Cyberpunk", "Open World"], Status = GameStatus.Completed, Description = "A gritty cyberpunk action RPG set in a neon-drenched megacity where you play as a rogue street samurai uncovering a corporate conspiracy." },
        new() { Id = 2, Title = "Kingdom's Fall", Developer = "Irongate Games", Publisher = "Irongate Games", ReleaseYear = 2022, Rating = 9.2, Genres = ["Strategy"], Platforms = ["PC"], Tags = ["4X", "Turn-Based", "Multiplayer"], Status = GameStatus.Playing, Description = "A sweeping grand strategy game where you lead a medieval empire from humble beginnings to continental dominance across centuries." },
        new() { Id = 3, Title = "Iron Horizon", Developer = "Warfront Dev", Publisher = "Titan Publishing", ReleaseYear = 2021, Rating = 7.8, Genres = ["FPS"], Platforms = ["PC", "Xbox"], Tags = ["Multiplayer", "Military", "Competitive"], Status = GameStatus.Completed, Description = "A tactical military shooter featuring large-scale battles, destructible environments and squad-based gameplay." },
        new() { Id = 4, Title = "Crystal Maze Runner", Developer = "Puzzle Box Co.", Publisher = "Puzzle Box Co.", ReleaseYear = 2023, Rating = 8.0, Genres = ["Puzzle"], Platforms = ["Nintendo Switch", "Mobile"], Tags = ["Casual", "Singleplayer", "Relaxing"], Status = GameStatus.Backlog, Description = "Guide a luminous crystal through hundreds of intricate mazes, using light refraction and gravity puzzles to reach the exit." },
        new() { Id = 5, Title = "Galactic Arena", Developer = "Orbit Systems", Publisher = "Star League Inc.", ReleaseYear = 2020, Rating = 7.5, Genres = ["Sports", "Action"], Platforms = ["PC", "PlayStation", "Xbox"], Tags = ["Multiplayer", "Competitive", "Futuristic"], Status = GameStatus.Dropped, Description = "Zero-gravity team sports played aboard orbiting stations. Think basketball meets rocket physics in the far future." },
        new() { Id = 6, Title = "Shadow Protocol", Developer = "Ghost Frame", Publisher = "Covert Media", ReleaseYear = 2022, Rating = 8.8, Genres = ["Action", "Stealth"], Platforms = ["PC", "PlayStation"], Tags = ["Singleplayer", "Espionage", "Story Rich"], Status = GameStatus.Completed, Description = "A cinematic stealth-action thriller where an elite spy must dismantle a global assassination network from the shadows." },
        new() { Id = 7, Title = "Pixel Dungeon Deluxe", Developer = "RetroForge", Publisher = "RetroForge", ReleaseYear = 2019, Rating = 9.0, Genres = ["RPG", "Roguelike"], Platforms = ["PC", "Nintendo Switch"], Tags = ["Singleplayer", "Pixel Art", "Hardcore"], Status = GameStatus.Completed, Description = "A lovingly crafted pixel-art dungeon crawler with procedurally generated levels, deep loot systems, and brutal permadeath." },
        new() { Id = 8, Title = "Turbo Rally Championship", Developer = "Velocity Labs", Publisher = "Speed King Media", ReleaseYear = 2021, Rating = 7.2, Genres = ["Racing"], Platforms = ["PC", "Xbox"], Tags = ["Multiplayer", "Competitive", "Cars"], Status = GameStatus.Backlog, Description = "High-octane rally racing across 50 tracks spanning deserts, tundras, and mountain passes. Upgrade and customize your vehicle." },
        new() { Id = 9, Title = "Ancient Empires", Developer = "Historica Games", Publisher = "Chronicle Interactive", ReleaseYear = 2020, Rating = 8.3, Genres = ["Strategy", "Simulation"], Platforms = ["PC"], Tags = ["Turn-Based", "Historical", "Multiplayer"], Status = GameStatus.Playing, Description = "Build civilizations from stone-age tribes to sprawling empires. Manage diplomacy, warfare, science and culture across millennia." },
        new() { Id = 10, Title = "Forest Spirit", Developer = "Wisp Entertainment", Publisher = "Nature Games", ReleaseYear = 2023, Rating = 9.1, Genres = ["Adventure", "Platformer"], Platforms = ["Nintendo Switch"], Tags = ["Singleplayer", "Wholesome", "Exploration"], Status = GameStatus.Completed, Description = "Play as an ancient forest spirit restoring balance to a dying woodland. A breathtaking hand-painted adventure with wordless storytelling." },
        new() { Id = 11, Title = "Zombie Outbreak Z", Developer = "Dead Zone Studios", Publisher = "Horror Vault", ReleaseYear = 2020, Rating = 7.6, Genres = ["Survival", "Action"], Platforms = ["PC", "PlayStation", "Xbox"], Tags = ["Multiplayer", "Co-op", "Horror"], Status = GameStatus.Dropped, Description = "Survive the zombie apocalypse with up to 4 friends. Scavenge, fortify your base, and make agonizing moral choices." },
        new() { Id = 12, Title = "Starfield Chronicles", Developer = "Nebula Software", Publisher = "Cosmos Publishing", ReleaseYear = 2022, Rating = 8.4, Genres = ["RPG", "Adventure"], Platforms = ["PC", "Xbox"], Tags = ["Open World", "Singleplayer", "Space"], Status = GameStatus.Playing, Description = "Explore a procedurally generated galaxy with thousands of star systems. Mine, trade, fight and forge your own destiny among the stars." },
        new() { Id = 13, Title = "Puzzle Realm", Developer = "Mind Forge", Publisher = "Mind Forge", ReleaseYear = 2021, Rating = 7.9, Genres = ["Puzzle"], Platforms = ["Mobile", "Nintendo Switch", "PC"], Tags = ["Casual", "Brain Teaser", "Relaxing"], Status = GameStatus.Completed, Description = "A charming puzzle game with hundreds of hand-crafted levels across magical realms. Perfect for unwinding after a long day." },
        new() { Id = 14, Title = "Combat League", Developer = "Fist Forward", Publisher = "Knockout Media", ReleaseYear = 2022, Rating = 8.1, Genres = ["Fighting"], Platforms = ["PlayStation", "Xbox", "PC"], Tags = ["Competitive", "Multiplayer", "Esports"], Status = GameStatus.Playing, Description = "A technically deep fighting game with a diverse roster of 40 fighters, each with unique mechanics and combo systems." },
        new() { Id = 15, Title = "Digital Frontier", Developer = "CyberCore Dev", Publisher = "NetPulse Games", ReleaseYear = 2023, Rating = 8.6, Genres = ["FPS", "Sci-Fi"], Platforms = ["PC", "PlayStation"], Tags = ["Multiplayer", "Competitive", "Futuristic"], Status = GameStatus.Backlog, Description = "Fast-paced team shooter set in a virtual reality battlefield. Use digital abilities and hacking tools to outmaneuver opponents." },
        new() { Id = 16, Title = "Harvest Moon Rising", Developer = "Farmland Interactive", Publisher = "Cozy Games Co.", ReleaseYear = 2021, Rating = 9.3, Genres = ["Simulation"], Platforms = ["Nintendo Switch", "PC"], Tags = ["Relaxing", "Farming", "Singleplayer"], Status = GameStatus.Completed, Description = "Restore your grandfather's forgotten farm, befriend the townspeople, and discover the magical secrets hidden in the valley." },
        new() { Id = 17, Title = "Rogue Academy", Developer = "Deck Runner", Publisher = "Card Dungeon Inc.", ReleaseYear = 2022, Rating = 8.7, Genres = ["Roguelike", "Strategy"], Platforms = ["PC", "Nintendo Switch"], Tags = ["Deckbuilding", "Singleplayer", "Hardcore"], Status = GameStatus.Playing, Description = "Build a deck of powerful cards and battle through a procedurally generated magical school full of monsters and rival students." },
        new() { Id = 18, Title = "Battle Arena Champions", Developer = "Rift Entertainment", Publisher = "Rift Entertainment", ReleaseYear = 2019, Rating = 8.2, Genres = ["MOBA", "Strategy"], Platforms = ["PC"], Tags = ["Multiplayer", "Competitive", "Esports"], Status = GameStatus.Dropped, Description = "A feature-rich MOBA with 120 champions across three roles. Deep team strategy and individual mechanical skill combine in 5v5 battles." },
        new() { Id = 19, Title = "Deep Sea Explorer", Developer = "Aqua Labs", Publisher = "Ocean Digital", ReleaseYear = 2020, Rating = 7.7, Genres = ["Adventure", "Simulation"], Platforms = ["PC"], Tags = ["Singleplayer", "Exploration", "Underwater"], Status = GameStatus.Backlog, Description = "Pilot your submarine through uncharted ocean depths, discover ancient ruins, study alien-like sea life, and manage your oxygen supply." },
        new() { Id = 20, Title = "Thunderstrike Racing", Developer = "Circuit Breaker", Publisher = "Apex Speed Co.", ReleaseYear = 2023, Rating = 8.0, Genres = ["Racing"], Platforms = ["PlayStation", "Xbox", "PC"], Tags = ["Multiplayer", "Arcade", "Competitive"], Status = GameStatus.Backlog, Description = "Over-the-top arcade racing with insane tracks, power-ups, and fully destructible cars. 24-player online races." },
        new() { Id = 21, Title = "The Last Sanctuary", Developer = "Dread Works", Publisher = "Shadow Box Media", ReleaseYear = 2021, Rating = 8.9, Genres = ["Survival Horror", "Adventure"], Platforms = ["PC", "PlayStation"], Tags = ["Singleplayer", "Horror", "Story Rich"], Status = GameStatus.Completed, Description = "A atmospheric survival horror game set in a crumbling monastery during a supernatural siege. Manage resources, solve mysteries, and stay sane." },
        new() { Id = 22, Title = "Mech Warriors Unite", Developer = "Steel Frame Studios", Publisher = "Iron Giant Media", ReleaseYear = 2022, Rating = 7.4, Genres = ["Action", "Simulation"], Platforms = ["PC", "Xbox"], Tags = ["Mech", "Multiplayer", "Sci-Fi"], Status = GameStatus.Dropped, Description = "Pilot customizable giant mechs in large-scale PvP and PvE battles. Collect parts, upgrade systems, and dominate the battlefield." },
        new() { Id = 23, Title = "Chromatic Odyssey", Developer = "Prism Games", Publisher = "Rainbow Interactive", ReleaseYear = 2023, Rating = 9.4, Genres = ["Platformer", "Puzzle"], Platforms = ["Nintendo Switch", "PC"], Tags = ["Singleplayer", "Colorful", "Indie"], Status = GameStatus.Playing, Description = "A mind-bending platformer where you manipulate color wavelengths to phase through or solidify different colored surfaces." },
        new() { Id = 24, Title = "Dragon Siege", Developer = "Scale & Fire", Publisher = "Dragon Keep Games", ReleaseYear = 2020, Rating = 8.5, Genres = ["Strategy", "Action"], Platforms = ["PC", "Mobile"], Tags = ["Tower Defense", "Fantasy", "Multiplayer"], Status = GameStatus.Completed, Description = "Command dragon armies to defend your castle against waves of invaders. Build defenses, breed dragons, and siege enemy fortresses." },
        new() { Id = 25, Title = "Velocity Prime", Developer = "Speed Demon Dev", Publisher = "FastLane Publishing", ReleaseYear = 2021, Rating = 7.8, Genres = ["FPS", "Action"], Platforms = ["PC", "PlayStation", "Xbox"], Tags = ["Fast-Paced", "Competitive", "Multiplayer"], Status = GameStatus.Backlog, Description = "A movement-focused arena shooter where speed is your greatest weapon. Wall-run, slide-jump and rocket-jump across vertical arenas." },
        new() { Id = 26, Title = "Echo Chambers", Developer = "Sound Wave Games", Publisher = "Resonance Media", ReleaseYear = 2022, Rating = 8.3, Genres = ["Puzzle", "Adventure"], Platforms = ["PC", "PlayStation"], Tags = ["Singleplayer", "Atmospheric", "Story Rich"], Status = GameStatus.Completed, Description = "Solve puzzles by manipulating sound waves in a mysterious underground facility. The environment changes with every note you play." },
        new() { Id = 27, Title = "Viking Age", Developer = "Norse Craft", Publisher = "Valhalla Games", ReleaseYear = 2021, Rating = 8.7, Genres = ["Action RPG", "Adventure"], Platforms = ["PC", "PlayStation"], Tags = ["Open World", "Norse Mythology", "Singleplayer"], Status = GameStatus.Completed, Description = "Lead a Viking clan from a small coastal village to legendary status. Raid, trade, explore, and forge alliances across a vast Norse world." },
        new() { Id = 28, Title = "Nano Quest", Developer = "Micro Worlds", Publisher = "Tiny Planet Games", ReleaseYear = 2023, Rating = 8.6, Genres = ["RPG", "Adventure"], Platforms = ["Nintendo Switch", "PC"], Tags = ["Singleplayer", "Cute", "Turn-Based"], Status = GameStatus.Playing, Description = "Shrink down to microscopic size and explore a vast ecosystem living inside a raindrop. Fight germs, befriend protozoa, save the cell." },
        new() { Id = 29, Title = "Summit Rush", Developer = "Peak Performance Dev", Publisher = "Altitude Sports Co.", ReleaseYear = 2022, Rating = 7.3, Genres = ["Sports", "Racing"], Platforms = ["PC", "Xbox"], Tags = ["Multiplayer", "Snowboard", "Competitive"], Status = GameStatus.Backlog, Description = "Compete in extreme winter sports races down treacherous mountain courses. Includes snowboarding, skiing, and base jumping events." },
        new() { Id = 30, Title = "Codex Chronicles", Developer = "Lore Keeper Studios", Publisher = "Epic Tome Games", ReleaseYear = 2023, Rating = 9.0, Genres = ["RPG", "Strategy"], Platforms = ["PC", "PlayStation", "Xbox"], Tags = ["Turn-Based", "Story Rich", "Singleplayer"], Status = GameStatus.Playing, Description = "A sprawling tactical RPG with branching storylines, 200+ hours of content, and a deep lore system where your choices rewrite history." }
    ];

    public Task<List<Game>> GetGamesAsync(GameFilter? filter = null)
    {
        var query = _games.AsEnumerable();

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

        return Task.FromResult(query.ToList());
    }

    public Task<Game?> GetByIdAsync(int id) =>
        Task.FromResult(_games.FirstOrDefault(g => g.Id == id));

    public Task<GameStats> GetStatsAsync()
    {
        var stats = new GameStats
        {
            TotalGames = _games.Count,
            AverageRating = Math.Round(_games.Average(g => g.Rating), 1),
            GamesByGenre = _games
                .SelectMany(g => g.Genres)
                .GroupBy(g => g)
                .OrderByDescending(g => g.Count())
                .Take(8)
                .ToDictionary(g => g.Key, g => g.Count()),
            GamesByPlatform = _games
                .SelectMany(g => g.Platforms)
                .GroupBy(p => p)
                .OrderByDescending(p => p.Count())
                .ToDictionary(p => p.Key, p => p.Count()),
            GamesByStatus = _games
                .GroupBy(g => g.Status.ToString())
                .ToDictionary(g => g.Key, g => g.Count())
        };
        return Task.FromResult(stats);
    }

    public IReadOnlyList<string> GetAllGenres() =>
        _games.SelectMany(g => g.Genres).Distinct().OrderBy(g => g).ToList();

    public IReadOnlyList<string> GetAllPlatforms() =>
        _games.SelectMany(g => g.Platforms).Distinct().OrderBy(p => p).ToList();
}
