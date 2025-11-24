using ThinMPm.Contracts.Models;
using ThinMPm.Views.GridItem;
using ThinMPm.Views.Page;

namespace ThinMPm.Views.List;

public class AlbumList : CollectionView
{
    public AlbumList()
    {
        Margin = new Thickness(20, 0, 20, 0);
        ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical)
        {
            VerticalItemSpacing = 20,
            HorizontalItemSpacing = 20
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