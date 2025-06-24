using CommunityToolkit.Maui.Markup;
using ThinMPm.Contracts.Models;
using ThinMPm.ViewModels;

namespace ThinMPm.Views.Page;

class SongsPage : ContentPage
{
    public SongsPage(SongViewModel vm)
    {
        BindingContext = vm;

        Content = new CollectionView
        {
            ItemTemplate = new DataTemplate(() =>
            {
                var grid = new Grid
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
                };

                var tapGesture = new TapGestureRecognizer();

                tapGesture.Tapped += (s, e) =>
                {
                    if (s is BindableObject bindable && BindingContext is SongViewModel vm)
                    {
                        if (bindable.BindingContext is ISongModel item)
                        {
                            int index = vm.Songs.IndexOf(item);
                            Console.WriteLine($"Tapped index: {index}, Name: {item.Name}");
                        }
                    }
                };
                grid.GestureRecognizers.Add(tapGesture);

                return grid;
            })
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