using System;
using Grimoire.Views;
using Grimoire.Components.UI;

namespace Grimoire;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		
		// Register routes for navigation
		Routing.RegisterRoute(nameof(ArchivePage), typeof(ArchivePage));
		Routing.RegisterRoute(nameof(ThemeSettingsPage), typeof(ThemeSettingsPage));

	}
}
