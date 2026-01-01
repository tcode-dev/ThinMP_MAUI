using ThinMPm.Contracts.Models;

namespace ThinMPm.Contracts.Services;

public interface IMainMenuService
{
    IList<IMainMenuItemModel> GetAll();
    IList<IMenuModel> GetVisibleMenus();
    IList<IMainMenuEditItemModel> GetAllForEdit();
    void Save(IList<IMainMenuEditItemModel> items);
}
