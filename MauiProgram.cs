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

#if ANDROID || IOS
    builder.Services.AddSingleton<ISongRepository, SongRepository>();
    builder.Services.AddSingleton<IAlbumRepository, AlbumRepository>();
    builder.Services.AddSingleton<IArtistRepository, ArtistRepository>();

    builder.Services.AddSingleton<ISongService, SongService>();
		builder.Services.AddSingleton<IAlbumService, AlbumService>();
		builder.Services.AddSingleton<IArtistService, ArtistService>();
    builder.Services.AddSingleton<IArtworkService, ArtworkService>();
    builder.Services.AddSingleton<IPlayerService, PlayerService>();
#endif

		builder.Services.AddTransient<ArtistViewModel>();
		builder.Services.AddTransient<AlbumViewModel>();
		builder.Services.AddTransient<SongViewModel>();
		builder.Services.AddTransient<ArtistDetailViewModel>();
		builder.Services.AddTransient<AlbumDetailViewModel>();

		builder.Services.AddTransient<ArtistsPage>();
		builder.Services.AddTransient<AlbumsPage>();
		builder.Services.AddTransient<SongsPage>();
		builder.Services.AddTransient<ArtistDetailPage>();
		builder.Services.AddTransient<AlbumDetailPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
