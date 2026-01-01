using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public partial class MainMenuEditViewModel(IMainMenuService mainMenuService) : ObservableObject
{
    private readonly IMainMenuService _mainMenuService = mainMenuService;

    [ObservableProperty]
    private ObservableCollection<IMainMenuEditItemModel> _menuItems = [];

    public void Load()
    {
        MenuItems = new ObservableCollection<IMainMenuEditItemModel>(_mainMenuService.GetAllForEdit());
    }

    public void Save()
    {
        _mainMenuService.Save(MenuItems.ToList());
    }
}
