using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Markup;
using ThinMPm.ViewModels;

namespace ThinMPm.Views.Page;

class SongsPage : ContentPage
{
    // private readonly SongViewModel _vm;
    public SongsPage(SongViewModel vm)
    {
        BindingContext = vm;

        Content = new CollectionView
        {
            ItemTemplate = new DataTemplate(() =>
                new Label().Bind(Label.TextProperty, "Name")
            )
        }.Bind(ItemsView.ItemsSourceProperty, nameof(vm.Songs));
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is SongViewModel vm)
        {
            vm.Load();
        }
    }
}