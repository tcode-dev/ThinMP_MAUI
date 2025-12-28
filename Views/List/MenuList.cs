using ThinMPm.Views.ListItem;

namespace ThinMPm.Views.List;

public class MenuList : CollectionView
{
    public MenuList()
    {
        ItemTemplate = new DataTemplate(() => new MenuListItem());
    }
}