using ThinMPm.Constants;

namespace ThinMPm.Contracts.Models;

public interface IMainMenuEditItemModel
{
    MainMenuItem Id { get; }
    string Title { get; }
    bool IsVisible { get; set; }
}
