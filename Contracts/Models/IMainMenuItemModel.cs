using ThinMPm.Constants;

namespace ThinMPm.Contracts.Models;

public interface IMainMenuItemModel
{
    MainMenuItem Id { get; }
    bool IsVisible { get; }
}
