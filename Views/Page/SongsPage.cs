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
                new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = 40 },
                        new ColumnDefinition { Width = GridLength.Star }
                    },
                    RowDefinitions =
                    {
                        new RowDefinition { Height = GridLength.Auto },
                        new RowDefinition { Height = GridLength.Auto }
                    },
                    Children =
                    {
                        new Image()
                            .Bind(Image.SourceProperty, "ImageId")
                            .Row(0).RowSpan(2).Column(0),

                        new Label()
                            .Bind(Label.TextProperty, "Name")
                            .Row(0).Column(1),

                        new Label()
                            .Bind(Label.TextProperty, "ArtistName")
                            .Row(1).Column(1)
                    }
                }
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