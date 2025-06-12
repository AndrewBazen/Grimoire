using Grimoire.Presentation.Views;

namespace Grimoire;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		
		// Register routes for navigation
		Routing.RegisterRoute("startPage", typeof(StartPage));
		Routing.RegisterRoute("themeSettingsPage", typeof(ThemeSettingsPage));
		Routing.RegisterRoute("archive", typeof(ArchivePage));
	}
}
