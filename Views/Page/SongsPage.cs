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

        // var collectionView = new CollectionView
        // {
        //     ItemTemplate = new DataTemplate(() =>
        //         new Label().Bind(Label.TextProperty, "Name")
        //     )
        // };
        // collectionView.SetBinding(ItemsView.ItemsSourceProperty, "Songs");
        var items = new ObservableCollection<string>
        {
            "aa", "bb", "cc"
        };

        Content = new CollectionView
        {
            ItemsSource = items,
            ItemTemplate = new DataTemplate(() =>
                new Label()
                    .Bind(Label.TextProperty, ".")
                    .FontSize(20)
                    .Padding(10)
            )
        }
        .Margin(20);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // _viewModel.Load();
    }
}