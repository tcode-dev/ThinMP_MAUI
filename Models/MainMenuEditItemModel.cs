using CommunityToolkit.Mvvm.ComponentModel;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;

namespace ThinMPm.Models;

public partial class MainMenuEditItemModel(MainMenuItem id, string title, bool isVisible) : ObservableObject, IMainMenuEditItemModel
{
    public MainMenuItem Id { get; } = id;
    public string Title { get; } = title;

    [ObservableProperty]
    private bool _isVisible = isVisible;
}
