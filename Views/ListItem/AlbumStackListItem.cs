using ThinMPm.Constants;
using ThinMPm.Contracts.Models;

namespace ThinMPm.Views.ListItem;

public class AlbumStackListItem : Grid
{
    public AlbumStackListItem()
    {
        Margin = new Thickness(LayoutConstants.SpacingLarge, 0, LayoutConstants.SpacingLarge, LayoutConstants.SpacingLarge);
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (BindingContext is not IAlbumStackModel stack)
            return;

        Children.Clear();
        ColumnDefinitions.Clear();

        for (int i = 0; i < stack.ColumnCount; i++)
        {
            if (i > 0)
            {
                ColumnDefinitions.Add(new ColumnDefinition { Width = LayoutConstants.SpacingLarge });
            }
            ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

            var gridItem = new AlbumGridItem();
            if (i < stack.Albums.Count)
            {
                gridItem.BindingContext = stack.Albums[i];
            }

            Children.Add(gridItem);
            Grid.SetColumn(gridItem, i * 2);
        }
    }
}
