using ThinMPm.Contracts.Models;
using ThinMPm.Views.Row;

namespace ThinMPm.Views.List;

public class MenuList : CollectionView
{
    public MenuList()
    {
        ItemTemplate = new DataTemplate(() => new MenuListItem(OnTapped));
    }

    private async void OnTapped(object? sender, EventArgs e)
    {
        if (sender is BindableObject bindable)
        {
            if (bindable.BindingContext is IMenuModel item)
            {
                await Shell.Current.GoToAsync(item.Page);
            }
        }
    }
}