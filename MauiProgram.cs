using Microsoft.Extensions.Logging;
using Grimoire.Services;
using Grimoire.Services.Interfaces;
using Grimoire.ViewModels;
using Grimoire.Views;
using Grimoire.Components.UI;



namespace Grimoire;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});


		builder.Services.AddSingleton<IArchive, LocalArchive>();
		builder.Services.AddSingleton<TomeService>();
		builder.Services.AddSingleton<ThemeService>();
		builder.Services.AddSingleton<NotificationService>();
		builder.Services.AddSingleton<ArchiveViewModel>();
		builder.Services.AddSingleton<ArchivePage>();
		builder.Services.AddSingleton<ThemeSettingsPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif
		
		return builder.Build();

	}
}
