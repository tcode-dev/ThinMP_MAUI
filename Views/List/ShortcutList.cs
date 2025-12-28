using ThinMPm.Constants;
using ThinMPm.Views.ListItem;

namespace ThinMPm.Views.List;

public class ShortcutList : CollectionView
{
    public ShortcutList()
    {
        Margin = new Thickness(LayoutConstants.SpacingLarge, 0, LayoutConstants.SpacingLarge, 0);
        ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical)
        {
            VerticalItemSpacing = LayoutConstants.SpacingLarge,
            HorizontalItemSpacing = LayoutConstants.SpacingLarge
        };
        ItemTemplate = new DataTemplate(() => new ShortcutGridItem());
    }
}
