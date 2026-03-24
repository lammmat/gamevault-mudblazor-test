using Bunit;
using MudBlazor;
using MudBlazor.Services;

namespace GameVault.Tests;

/// <summary>
/// Base class for bUnit component tests that use MudBlazor.
/// Handles MudBlazor-specific setup (services, popover provider) and ensures
/// async-only MudBlazor services (e.g. KeyInterceptorService) are disposed
/// via DisposeAsync rather than Dispose to avoid InvalidOperationException.
/// </summary>
public abstract class MudBlazorTestBase : BunitContext, IAsyncLifetime
{
    protected MudBlazorTestBase()
    {
        Services.AddMudServices();
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    Task IAsyncLifetime.InitializeAsync()
    {
        // Render after all subclass service registrations are done.
        // MudSelect, MudAutocomplete, MudMenu etc. require MudPopoverProvider in the render tree.
        Render<MudPopoverProvider>();
        return Task.CompletedTask;
    }

    async Task IAsyncLifetime.DisposeAsync() => await DisposeAsync();

    // Prevent the sync Dispose(bool) path from touching services — DisposeAsync handles it.
    protected override void Dispose(bool disposing) { }
}
