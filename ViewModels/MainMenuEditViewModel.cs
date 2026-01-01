using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public partial class MainMenuEditViewModel(IMainMenuService mainMenuService, IShortcutService shortcutService) : ObservableObject
{
    private readonly IMainMenuService _mainMenuService = mainMenuService;
    private readonly IShortcutService _shortcutService = shortcutService;

    [ObservableProperty]
    private ObservableCollection<IMainMenuEditItemModel> _menuItems = [];

    [ObservableProperty]
    private ObservableCollection<IShortcutModel> _shortcuts = [];

    public async Task LoadAsync()
    {
        MenuItems = new ObservableCollection<IMainMenuEditItemModel>(_mainMenuService.GetAllForEdit());
        var shortcuts = await _shortcutService.GetAllAsync();
        Shortcuts = new ObservableCollection<IShortcutModel>(shortcuts);
    }

    public void RemoveShortcut(IShortcutModel shortcut)
    {
        Shortcuts.Remove(shortcut);
    }

    public async Task SaveAsync()
    {
        _mainMenuService.Save(MenuItems.ToList());
        await _shortcutService.UpdateAsync(Shortcuts.ToList());
    }
}
