using ThinMPm.Contracts.Models;

namespace ThinMPm.Contracts.Services;

public interface IMainMenuService
{
    IList<IMainMenuItemModel> GetAll();
    IList<IMenuModel> GetVisibleMenus();
}
