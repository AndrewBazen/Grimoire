using Grimoire.Domain.Models;
using Microsoft.Maui.Controls;
using System.Linq;
using Microsoft.Maui.Storage;
using System.Collections.Generic;
using System;

namespace Grimoire.Domain.Services;

    public sealed class ThemeService
    {
        // ───────── constants ─────────
        private const string PreferenceKey   = "CurrentThemeId";
        private const string DefaultThemeName  = "Grimoire";

        // ───────── fields ─────────    
        private readonly List<Theme> _themes = new()
        {
            new Theme(
                DefaultThemeName,
                true,
                Color.FromArgb("#24b6b3"),
                Color.FromArgb("#1f9c9a"),
                Color.FromArgb("#2e3039"),
                Color.FromArgb("#1b2d33"),
                Color.FromArgb("#38484b"),
                Color.FromArgb("#2e3039"),
                Color.FromArgb("#1f9c9a"),
                Color.FromArgb("#2e3039"),
                Color.FromArgb("#1b2d33"),
                Color.FromArgb("#38484b"),
                Color.FromArgb("#634368"),
                Color.FromArgb("#33866f"),
                Color.FromArgb("#635a43"),
                Color.FromArgb("#3b90aa")
            )
        };

        private Theme _current;

        // ───────── ctor ─────────
        public ThemeService()
        {
            // 1. start with the default
            _current = _themes.First(t => t.Name == DefaultThemeName);

            // 2. see whether the user changed it before
            var savedId = Preferences.Get(PreferenceKey, DefaultThemeName);
            _ = SetTheme(savedId); // fire and forget
        }

        // ───────── public surface ─────────
        public IReadOnlyList<Theme> Available => _themes.AsReadOnly();
        public Theme Current              => _current;

        public async Task SetTheme(string name)
        {
            var theme = _themes.FirstOrDefault(t => t.Name == name) ?? _themes.First(t => t.IsSelected);

            if (theme == _current) return;

            _current = theme;
            ApplyToResources(theme);
            Preferences.Set(PreferenceKey, theme.Name);
            ThemeChanged?.Invoke(this, theme);
            await Task.CompletedTask;
        }

        public event EventHandler<Theme>? ThemeChanged;

        // ───────── helpers ─────────
        private static void ApplyToResources(Theme t)
        {
            var app = Application.Current;
            if (app is null) return; // too early in app lifecycle
            var res = app.Resources;
            res["Primary"]      = t.Primary;
            res["Secondary"]    = t.Secondary;
            res["Background"]   = t.Background;
            res["Surface"]      = t.Surface;
            res["OnPrimary"]    = t.OnPrimary;
            res["OnBackground"] = t.OnBackground;
            res["Error"]        = t.Error;
        }

}
