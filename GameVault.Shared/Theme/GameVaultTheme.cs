using MudBlazor;

namespace GameVault.Shared.Theme;

/// <summary>
/// Central design system — Pillar 1.
/// Edit colors, fonts, and layout properties here to restyle the entire app.
/// These values are automatically applied to all MudBlazor components via MudThemeProvider.
/// The companion CSS file (gamevault.css) contains the mirrored CSS variables and effects.
/// </summary>
public static class GameVaultTheme
{
    // ─── Palette tokens ───────────────────────────────────────────────────────
    // Edit these to change the entire color scheme.
    public const string PrimaryColor    = "#00E5CC"; // Electric teal  — vault signature
    public const string SecondaryColor  = "#FF4D5A"; // Vivid coral    — ratings, action
    public const string SuccessColor    = "#00E599"; // Neon green     — completed
    public const string WarningColor    = "#FFB347"; // Amber          — backlog, caution
    public const string ErrorColor      = "#FF3B5C"; // Hot red        — dropped, errors
    public const string InfoColor       = "#4D9FFF"; // Electric blue  — playing, info

    // Dark mode backgrounds
    public const string BgVoid          = "#08080F"; // Page background
    public const string BgSurface       = "#0F0F1A"; // Cards, panels
    public const string BgDrawer        = "#06060F"; // Side drawer
    public const string BgAppBar        = "rgba(8,8,15,0.97)"; // AppBar (slightly transparent)

    // ─── Theme ────────────────────────────────────────────────────────────────
    public static MudTheme Theme { get; } = new()
    {
        PaletteDark = new PaletteDark
        {
            Primary                 = PrimaryColor,
            PrimaryContrastText     = "#08080F",
            Secondary               = SecondaryColor,
            SecondaryContrastText   = "#FFFFFF",
            Success                 = SuccessColor,
            Warning                 = WarningColor,
            Error                   = ErrorColor,
            Info                    = InfoColor,

            Background              = BgVoid,
            BackgroundGray          = "#0C0C16",
            Surface                 = BgSurface,
            DrawerBackground        = BgDrawer,
            AppbarBackground        = BgAppBar,
            AppbarText              = "rgba(255,255,255,0.92)",

            TextPrimary             = "rgba(255,255,255,0.92)",
            TextSecondary           = "rgba(255,255,255,0.50)",
            TextDisabled            = "rgba(255,255,255,0.25)",

            ActionDefault           = "rgba(255,255,255,0.55)",
            ActionDisabled          = "rgba(255,255,255,0.25)",
            ActionDisabledBackground= "rgba(255,255,255,0.08)",

            Divider                 = "rgba(255,255,255,0.07)",
            LinesDefault            = "rgba(255,255,255,0.05)",
            LinesInputs             = "rgba(255,255,255,0.15)",
            TableLines              = "rgba(255,255,255,0.06)",
            TableHover              = "rgba(0,229,204,0.06)",
            TableStriped            = "rgba(255,255,255,0.02)",

            OverlayLight            = "rgba(0,0,0,0.6)",
            OverlayDark             = "rgba(0,0,0,0.8)",
        },

        PaletteLight = new PaletteLight
        {
            Primary                 = "#007A6E",
            PrimaryContrastText     = "#FFFFFF",
            Secondary               = "#C4202A",
            SecondaryContrastText   = "#FFFFFF",
            Success                 = "#00875A",
            Warning                 = "#D97706",
            Error                   = "#DC2626",
            Info                    = "#1D6FB8",

            Background              = "#F0F0F7",
            Surface                 = "#FFFFFF",
            DrawerBackground        = "#FAFAFA",
            AppbarBackground        = "#08080F",
            AppbarText              = "rgba(255,255,255,0.92)",

            TextPrimary             = "rgba(0,0,0,0.87)",
            TextSecondary           = "rgba(0,0,0,0.54)",
            Divider                 = "rgba(0,0,0,0.10)",
        },

        Typography = new Typography
        {
            Default   = new DefaultTypography  { FontFamily = ["Nunito",  "sans-serif"], FontSize = "0.95rem", LineHeight = "1.6" },
            H1        = new H1Typography        { FontFamily = ["Orbitron","sans-serif"], FontWeight = "900", FontSize = "2.6rem",  LetterSpacing = "0.05em" },
            H2        = new H2Typography        { FontFamily = ["Orbitron","sans-serif"], FontWeight = "700", FontSize = "2rem",    LetterSpacing = "0.04em" },
            H3        = new H3Typography        { FontFamily = ["Orbitron","sans-serif"], FontWeight = "700", FontSize = "1.75rem", LetterSpacing = "0.03em" },
            H4        = new H4Typography        { FontFamily = ["Orbitron","sans-serif"], FontWeight = "700", FontSize = "1.4rem",  LetterSpacing = "0.03em" },
            H5        = new H5Typography        { FontFamily = ["Orbitron","sans-serif"], FontWeight = "600", FontSize = "1.15rem", LetterSpacing = "0.02em" },
            H6        = new H6Typography        { FontFamily = ["Orbitron","sans-serif"], FontWeight = "600", FontSize = "1rem",    LetterSpacing = "0.02em" },
            Subtitle1 = new Subtitle1Typography { FontFamily = ["Nunito",  "sans-serif"], FontWeight = "700", LetterSpacing = "0.01em" },
            Subtitle2 = new Subtitle2Typography { FontFamily = ["Nunito",  "sans-serif"], FontWeight = "600" },
            // Body1, Body2, Button, Caption, Overline font overrides are handled via CSS
            // in gamevault.css (.mud-typography-body1, .mud-button-label, etc.)
        },

        LayoutProperties = new LayoutProperties
        {
            DefaultBorderRadius = "8px",
            DrawerWidthLeft     = "260px",
            AppbarHeight        = "64px",
        },
    };
}
