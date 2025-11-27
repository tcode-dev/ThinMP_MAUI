using ThinMPm.Views.Page;

namespace ThinMPm;

public partial class AppShell : Shell
{
    internal AppShell()
    {
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(ArtistsPage), typeof(ArtistsPage));
        Routing.RegisterRoute(nameof(AlbumsPage), typeof(AlbumsPage));
        Routing.RegisterRoute(nameof(SongsPage), typeof(SongsPage));
        Routing.RegisterRoute(nameof(AlbumDetailPage), typeof(AlbumDetailPage));
        Routing.RegisterRoute(nameof(ArtistDetailPage), typeof(ArtistDetailPage));

        Items.Add(new ShellContent
        {
            Title = "Home",
            ContentTemplate = new DataTemplate(() =>
            {
                return Handler?.MauiContext?.Services?.GetRequiredService<MainPage>();
            }),
            Route = "MainPage"
        });
    }
}