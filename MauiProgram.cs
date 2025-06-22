using Microsoft.Extensions.Logging;
using ThinMPm.ViewModels;
using ThinMPm.Contracts.Services;
using ThinMPm.Views.Page;

#if ANDROID
using ThinMPm.Platforms.Android.Repositories.Contracts;
using ThinMPm.Platforms.Android.Repositories;
using ThinMPm.Platforms.Android.Services;
#elif IOS
using ThinMPm.Platforms.iOS.Repositories.Contracts;
using ThinMPm.Platforms.iOS.Repositories;
using ThinMPm.Platforms.iOS.Services;
#endif

namespace ThinMPm;

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

		builder.Services.AddTransient<ISongRepository, SongRepository>();
		builder.Services.AddTransient<ISongService, SongService>();
		builder.Services.AddSingleton<SongViewModel>();
		builder.Services.AddSingleton<SongsPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
