using CommunityToolkit.Mvvm.ComponentModel;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public partial class MainViewModel(IAlbumService albumService, IShortcutService shortcutService, IMainMenuService mainMenuService) : ObservableObject
{
    readonly IAlbumService _albumService = albumService;
    readonly IShortcutService _shortcutService = shortcutService;
    readonly IMainMenuService _mainMenuService = mainMenuService;

    [ObservableProperty]
    private IList<IMenuModel> _menuItems = [];

    [ObservableProperty]
    private IList<IShortcutModel> _shortcuts = [];

    [ObservableProperty]
    private IList<IAlbumModel> _albums = [];

    public async void Load()
    {
        MenuItems = _mainMenuService.GetVisibleMenus();
        Shortcuts = await _shortcutService.GetAllAsync();
        Albums = _albumService.FindByRecent(AppConstants.RecentAlbumsLimit);
    }
}