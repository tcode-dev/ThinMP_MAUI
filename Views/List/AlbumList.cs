using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Views.ListItem;
using ThinMPm.Views.Page;

namespace ThinMPm.Views.List;

public class AlbumList : CollectionView
{
    public AlbumList()
    {
        Margin = new Thickness(LayoutConstants.SpacingLarge, 0, LayoutConstants.SpacingLarge, 0);
        ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical)
        {
            VerticalItemSpacing = LayoutConstants.SpacingLarge,
            HorizontalItemSpacing = LayoutConstants.SpacingLarge
        };
        ItemTemplate = new DataTemplate(() => new AlbumGridItem(OnAlbumTapped));
    }

    private async void OnAlbumTapped(object? sender, EventArgs e)
    {
        if (sender is BindableObject bindable)
        {
            if (bindable.BindingContext is IAlbumModel item)
            {
                await Shell.Current.GoToAsync($"{nameof(AlbumDetailPage)}?AlbumId={item.Id}");
            }
        }
    }
}