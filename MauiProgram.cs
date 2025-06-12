using Microsoft.Extensions.Logging;
using Grimoire.Domain.Services;
using Grimoire.Domain.Ports;
using Grimoire.Presentation.ViewModels;
using Grimoire.Presentation.Views;
using Grimoire.Infrastructure.Persistence;
using Grimoire.Domain.UseCases;
using Grimoire.Infrastructure.Abstractions;
using Grimoire.Infrastructure.Implementations;

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

		// TODO: Add markdown services
		//builder.Services.AddSingleton<IMarkdownService, MarkdownService>();
		// builder.Services.AddSingleton<IMarkdownRenderer, MarkdownRenderer>();
		// builder.Services.AddSingleton<IMarkdownEditor, MarkdownEditor>();	




		builder.Services.AddSingleton<ThemeService>();
		builder.Services.AddSingleton<IArchiveStore, LocalArchiveStore>();
		builder.Services.AddSingleton<IArchive, LocalArchive>();
		builder.Services.AddSingleton<ArchiveViewModel>();
		builder.Services.AddSingleton<ArchivePage>();
		builder.Services.AddSingleton<ThemeSettingsPage>();
		builder.Services.AddSingleton<StartPage>();
		builder.Services.AddSingleton<StartViewModel>();
		builder.Services.AddSingleton<ArchiveStoreViewModel>();
		builder.Services.AddSingleton<WorkspaceViewModel>();
		builder.Services.AddSingleton<IFileSys, FileSystemAdapter>();
		builder.Services.AddSingleton<IJsonSerializer, SystemTextJsonSerializer>();
		builder.Services.AddSingleton<IPathProvider, AppPathProvider>();

		builder.Services.AddSingleton<ILoadArchiveUseCase, LoadArchiveUseCase>();
		builder.Services.AddSingleton<ICreateNewTomeUseCase, CreateNewTomeUseCase>();
		builder.Services.AddSingleton<ISaveEntryUseCase, SaveEntryUseCase>();
		// TODO: Add reminder services
		// builder.Services.AddSingleton<IScheduleReminderUseCase, ScheduleReminderUseCase>();
		builder.Services.AddSingleton<ILoadAvailableThemesUseCase, LoadAvailableThemesUseCase>();
		builder.Services.AddSingleton<ISetThemeUseCase, SetThemeUseCase>();
		builder.Services.AddSingleton<ThemeSettingsViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif
		
		return builder.Build();

	}
}
