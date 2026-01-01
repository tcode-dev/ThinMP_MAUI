using ThinMPm.Constants;
using ThinMPm.Contracts.Models;

namespace ThinMPm.Models;

public class MainMenuItemModel(MainMenuItem id, bool isVisible) : IMainMenuItemModel
{
    public MainMenuItem Id { get; set; } = id;
    public bool IsVisible { get; set; } = isVisible;
}
