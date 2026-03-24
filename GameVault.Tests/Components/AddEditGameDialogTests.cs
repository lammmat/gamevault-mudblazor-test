using Bunit;
using GameVault.Shared.Components;
using GameVault.Shared.Models;
using GameVault.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor;

namespace GameVault.Tests.Components;

public class AddEditGameDialogTests : MudBlazorTestBase
{
    private readonly Mock<IGameService> _svcMock = new();

    private static Game MakeGame() => new()
    {
        Id = 7,
        Title = "Existing Game",
        Developer = "Dev Corp",
        Publisher = "Pub Inc",
        ReleaseYear = 2022,
        Rating = 8.0,
        Genres = ["Action", "RPG"],
        Platforms = ["PC"],
        Tags = ["Singleplayer"],
        Status = GameStatus.Playing,
        Description = "A great game."
    };

    public AddEditGameDialogTests()
    {
        _svcMock.Setup(s => s.GetAllGenres()).Returns(["Action", "RPG", "Strategy"]);
        _svcMock.Setup(s => s.GetAllPlatforms()).Returns(["PC", "PlayStation", "Xbox"]);
        Services.AddSingleton<IGameService>(_svcMock.Object);
    }

    private static DialogOptions DefaultOptions() =>
        new() { MaxWidth = MaxWidth.Medium, FullWidth = true };

    [Fact]
    public async Task AddEditGameDialog_AddMode_ShowsAddTitle()
    {
        var cut = Render<MudDialogProvider>();
        var svc = Services.GetRequiredService<IDialogService>();

        await svc.ShowAsync<AddEditGameDialog>("", new DialogParameters(), DefaultOptions());

        Assert.Contains("ADD GAME", cut.Markup);
    }

    [Fact]
    public async Task AddEditGameDialog_EditMode_ShowsEditTitle()
    {
        var cut = Render<MudDialogProvider>();
        var svc = Services.GetRequiredService<IDialogService>();

        await svc.ShowAsync<AddEditGameDialog>("",
            new DialogParameters { ["ExistingGame"] = MakeGame() }, DefaultOptions());

        Assert.Contains("EDIT GAME", cut.Markup);
    }

    [Fact]
    public async Task AddEditGameDialog_EditMode_PrePopulatesTitle()
    {
        var cut = Render<MudDialogProvider>();
        var svc = Services.GetRequiredService<IDialogService>();

        await svc.ShowAsync<AddEditGameDialog>("",
            new DialogParameters { ["ExistingGame"] = MakeGame() }, DefaultOptions());

        Assert.Contains("Existing Game", cut.Markup);
    }

    [Fact]
    public async Task AddEditGameDialog_Save_CallsAddGameAsync_InAddMode()
    {
        _svcMock.Setup(s => s.AddGameAsync(It.IsAny<Game>()))
                .ReturnsAsync((Game g) => { g.Id = 99; return g; });

        var cut = Render<MudDialogProvider>();
        var svc = Services.GetRequiredService<IDialogService>();
        await svc.ShowAsync<AddEditGameDialog>("", new DialogParameters(), DefaultOptions());

        var dialog = cut.FindComponent<AddEditGameDialog>();
        dialog.Find("input[type=text]").Change("Brand New Game");
        await cut.InvokeAsync(() => dialog.Instance.TriggerSubmitForTest());

        _svcMock.Verify(s => s.AddGameAsync(It.Is<Game>(g => g.Title == "Brand New Game")), Times.Once);
    }

    [Fact]
    public async Task AddEditGameDialog_Save_CallsUpdateGameAsync_InEditMode()
    {
        _svcMock.Setup(s => s.UpdateGameAsync(It.IsAny<Game>())).Returns(Task.CompletedTask);

        var cut = Render<MudDialogProvider>();
        var svc = Services.GetRequiredService<IDialogService>();
        var game = MakeGame();
        await svc.ShowAsync<AddEditGameDialog>("",
            new DialogParameters { ["ExistingGame"] = game }, DefaultOptions());

        var dialog = cut.FindComponent<AddEditGameDialog>();
        await cut.InvokeAsync(() => dialog.Instance.TriggerSubmitForTest());

        _svcMock.Verify(s => s.UpdateGameAsync(It.Is<Game>(g => g.Id == game.Id)), Times.Once);
    }
}
