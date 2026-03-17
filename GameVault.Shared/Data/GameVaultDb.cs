namespace GameVault.Shared.Data;

public static class GameVaultDb
{
    public static string GetDbPath()
    {
        // Walk up from the executable location to find the solution root (contains .slnx)
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir != null && !dir.GetFiles("*.slnx").Any())
            dir = dir.Parent;
        var root = dir?.FullName ?? AppContext.BaseDirectory;
        return Path.Combine(root, "gamevault.db");
    }
}
