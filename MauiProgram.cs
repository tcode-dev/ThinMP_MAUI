using Microsoft.Extensions.Logging;
using FFImageLoading.Maui;
using ThinMPm.ViewModels;
using ThinMPm.Contracts.Services;
using ThinMPm.Views.Page;
using ThinMPm.Views.Popup;
using ThinMPm.Contracts.Utils;
using ThinMPm.Views.Img;
using ThinMPm.Views.Background;
using ThinMPm.Services;
using ThinMPm.Database.Repositories;

#if ANDROID
using ThinMPm.Platforms.Android.Repositories.Contracts;
using ThinMPm.Platforms.Android.Repositories;
using ThinMPm.Platforms.Android.Services;
using ThinMPm.Platforms.Android.Utils;
using ThinMPm.Platforms.Android.Handlers;
#elif IOS
using ThinMPm.Platforms.iOS.Repositories.Contracts;
using ThinMPm.Platforms.iOS.Repositories;
using ThinMPm.Platforms.iOS.Services;
using ThinMPm.Platforms.iOS.Utils;
using ThinMPm.Platforms.iOS.Handlers;
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
            })
            .ConfigureMauiHandlers(handlers =>
            {
#if ANDROID || IOS
                handlers.AddHandler<BlurredImageView, BlurredImageViewHandler>();
                handlers.AddHandler<BlurBackgroundView, BlurBackgroundViewHandler>();
#endif
            })
            .UseFFImageLoading();

#if ANDROID || IOS
        builder.Services.AddSingleton<ISongRepository, SongRepository>();
        builder.Services.AddSingleton<IAlbumRepository, AlbumRepository>();
        builder.Services.AddSingleton<IArtistRepository, ArtistRepository>();

        builder.Services.AddSingleton<ISongService, SongService>();
        builder.Services.AddSingleton<IAlbumService, AlbumService>();
        builder.Services.AddSingleton<IArtistService, ArtistService>();
        builder.Services.AddSingleton<IArtworkService, ArtworkService>();
        builder.Services.AddSingleton<IPlayerService, PlayerService>();
        builder.Services.AddSingleton<IPermissionService, PermissionService>();

        builder.Services.AddSingleton<IPlatformUtil, PlatformUtili>();
#endif

        builder.Services.AddSingleton<DisplayInfoService>();

        builder.Services.AddSingleton<IFavoriteSongService, FavoriteSongService>();
        builder.Services.AddSingleton<IFavoriteArtistService, FavoriteArtistService>();
        builder.Services.AddSingleton<IPreferenceService, PreferenceService>();
        builder.Services.AddSingleton<IMainMenuService, MainMenuService>();
        builder.Services.AddSingleton<IPlaylistService, PlaylistService>();
        builder.Services.AddSingleton<IShortcutService, ShortcutService>();

        builder.Services.AddSingleton<PlaylistRepository>();
        builder.Services.AddSingleton<PlaylistSongRepository>();
        builder.Services.AddSingleton<ShortcutRepository>();

        builder.Services.AddSingleton<MiniPlayerViewModel>();

        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<ArtistViewModel>();
        builder.Services.AddTransient<AlbumViewModel>();
        builder.Services.AddTransient<SongViewModel>();
        builder.Services.AddTransient<ArtistDetailViewModel>();
        builder.Services.AddTransient<AlbumDetailViewModel>();
        builder.Services.AddTransient<PlayerViewModel>();
        builder.Services.AddTransient<FavoriteSongsViewModel>();
        builder.Services.AddTransient<FavoriteSongsEditViewModel>();
        builder.Services.AddTransient<FavoriteArtistsViewModel>();
        builder.Services.AddTransient<FavoriteArtistsEditViewModel>();
        builder.Services.AddTransient<MainMenuEditViewModel>();
        builder.Services.AddTransient<PlaylistsEditViewModel>();
        builder.Services.AddTransient<PlaylistPopupViewModel>();
        builder.Services.AddTransient<PlaylistsViewModel>();
        builder.Services.AddTransient<PlaylistDetailViewModel>();
        builder.Services.AddTransient<PlaylistDetailEditViewModel>();

        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<ArtistsPage>();
        builder.Services.AddTransient<AlbumsPage>();
        builder.Services.AddTransient<SongsPage>();
        builder.Services.AddTransient<ArtistDetailPage>();
        builder.Services.AddTransient<AlbumDetailPage>();
        builder.Services.AddTransient<PlayerPage>();
        builder.Services.AddTransient<FavoriteSongsPage>();
        builder.Services.AddTransient<FavoriteSongsEditPage>();
        builder.Services.AddTransient<FavoriteArtistsPage>();
        builder.Services.AddTransient<FavoriteArtistsEditPage>();
        builder.Services.AddTransient<MainEditPage>();
        builder.Services.AddTransient<PlaylistsPage>();
        builder.Services.AddTransient<PlaylistsEditPage>();
        builder.Services.AddTransient<PlaylistDetailPage>();
        builder.Services.AddTransient<PlaylistDetailEditPage>();

        builder.Services.AddTransient<PlaylistPopup>();
        builder.Services.AddSingleton<Func<PlaylistPopup>>(sp => () => sp.GetRequiredService<PlaylistPopup>());

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
