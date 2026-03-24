using Bunit;
using GameVault.Shared.Components;
using GameVault.Shared.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace GameVault.Tests.Components;

public class CatalogFilterPanelTests : MudBlazorTestBase
{
    // Services, JSInterop, and MudPopoverProvider are set up in MudBlazorTestBase

    private static IReadOnlyList<string> Genres => ["Action", "RPG", "Puzzle", "Racing"];
    private static IReadOnlyList<string> Platforms => ["PC", "PlayStation", "Xbox"];

    [Fact]
    public void CatalogFilterPanel_RendersAllProvidedGenres()
    {
        var cut = Render<CatalogFilterPanel>(p => p
            .Add(c => c.AllGenres, Genres)
            .Add(c => c.AllPlatforms, Platforms));

        // MudSelect items are in the component tree even when the dropdown is closed
        var items = cut.FindComponents<MudSelectItem<string>>();
        var renderedValues = items.Select(i => i.Instance.Value).ToList();
        Assert.All(Genres, g => Assert.Contains(g, renderedValues));
    }

    [Fact]
    public void CatalogFilterPanel_RendersAllProvidedPlatforms()
    {
        var cut = Render<CatalogFilterPanel>(p => p
            .Add(c => c.AllGenres, Genres)
            .Add(c => c.AllPlatforms, Platforms));

        var items = cut.FindComponents<MudSelectItem<string>>();
        var renderedValues = items.Select(i => i.Instance.Value).ToList();
        Assert.All(Platforms, p => Assert.Contains(p, renderedValues));
    }

    [Fact]
    public async Task CatalogFilterPanel_ResetButton_InvokesOnReset()
    {
        bool resetCalled = false;
        var cut = Render<CatalogFilterPanel>(p => p
            .Add(c => c.AllGenres, Genres)
            .Add(c => c.AllPlatforms, Platforms)
            .Add(c => c.OnReset, EventCallback.Factory.Create(this, () => resetCalled = true)));

        var buttons = cut.FindAll("button");
        var resetButton = buttons.FirstOrDefault(b => b.TextContent.Trim() == "Reset");
        Assert.NotNull(resetButton);

        await cut.InvokeAsync(() => resetButton.Click());

        Assert.True(resetCalled);
    }
}
