using System.Text.Json;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Models;
using ThinMPm.Resources.Strings;
using ThinMPm.Views.Page;

namespace ThinMPm.Services;

public class MainMenuService : IMainMenuService
{
    private const string MainMenuKey = "main_menu";

    private static readonly IList<IMainMenuItemModel> DefaultMenuItems =
    [
        new MainMenuItemModel(MainMenuItem.Artists, true),
        new MainMenuItemModel(MainMenuItem.Albums, true),
        new MainMenuItemModel(MainMenuItem.Songs, true),
        new MainMenuItemModel(MainMenuItem.FavoriteArtists, true),
        new MainMenuItemModel(MainMenuItem.FavoriteSongs, true),
        new MainMenuItemModel(MainMenuItem.Playlists, true),
    ];

    private static readonly Dictionary<MainMenuItem, (string Title, string Page)> MenuMapping = new()
    {
        { MainMenuItem.Artists, (AppResources.Artists, nameof(ArtistsPage)) },
        { MainMenuItem.Albums, (AppResources.Albums, nameof(AlbumsPage)) },
        { MainMenuItem.Songs, (AppResources.Songs, nameof(SongsPage)) },
        { MainMenuItem.FavoriteArtists, (AppResources.FavoriteArtists, nameof(FavoriteArtistsPage)) },
        { MainMenuItem.FavoriteSongs, (AppResources.FavoriteSongs, nameof(FavoriteSongsPage)) },
        { MainMenuItem.Playlists, (AppResources.Playlists, nameof(PlaylistsPage)) },
    };

    public IList<IMainMenuItemModel> GetAll()
    {
        var json = Preferences.Get(MainMenuKey, string.Empty);

        if (string.IsNullOrEmpty(json))
        {
            return DefaultMenuItems.ToList();
        }

        try
        {
            var items = JsonSerializer.Deserialize<List<MainMenuItemModel>>(json);

            if (items == null || items.Count == 0)
            {
                return DefaultMenuItems.ToList();
            }

            return items.Cast<IMainMenuItemModel>().ToList();
        }
        catch
        {
            return DefaultMenuItems.ToList();
        }
    }

    public IList<IMenuModel> GetVisibleMenus()
    {
        return GetAll()
            .Where(item => item.IsVisible)
            .Select(item => MenuMapping.TryGetValue(item.Id, out var mapping)
                ? new MenuModel(mapping.Title, mapping.Page)
                : null)
            .Where(menu => menu != null)
            .Cast<IMenuModel>()
            .ToList();
    }

    public IList<IMainMenuEditItemModel> GetAllForEdit()
    {
        return GetAll()
            .Select(item => MenuMapping.TryGetValue(item.Id, out var mapping)
                ? new MainMenuEditItemModel(item.Id, mapping.Title, item.IsVisible)
                : null)
            .Where(item => item != null)
            .Cast<IMainMenuEditItemModel>()
            .ToList();
    }

    public void Save(IList<IMainMenuEditItemModel> items)
    {
        var menuItems = items
            .Select(item => new MainMenuItemModel(item.Id, item.IsVisible))
            .ToList();

        var json = JsonSerializer.Serialize(menuItems);
        Preferences.Set(MainMenuKey, json);
    }
}
