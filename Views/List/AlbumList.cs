using ThinMPm.Constants;
using ThinMPm.Utils;
using ThinMPm.Views.ListItem;

namespace ThinMPm.Views.List;

public class AlbumList : CollectionView
{
    public AlbumList()
    {
        Margin = new Thickness(LayoutConstants.SpacingLarge, 0, LayoutConstants.SpacingLarge, 0);
        ItemsLayout = new GridItemsLayout(LayoutHelper.GetGridCount(), ItemsLayoutOrientation.Vertical)
        {
            VerticalItemSpacing = LayoutConstants.SpacingLarge,
            HorizontalItemSpacing = LayoutConstants.SpacingLarge
        };
        ItemTemplate = new DataTemplate(() => new AlbumGridItem());
    }
}