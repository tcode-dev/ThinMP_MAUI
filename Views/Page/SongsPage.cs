using CommunityToolkit.Maui.Markup;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.ViewModels;
using ThinMPm.Views.Img;

namespace ThinMPm.Views.Page;

class SongsPage : ContentPage
{

    private readonly IPlayerService _playerService;
    public SongsPage(SongViewModel vm, IPlayerService playerService)
    {
        BindingContext = vm;
        _playerService = playerService;

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
                        new ArtworkImg()
                        {
                            WidthRequest = 44,
                            HeightRequest = 44
                        }
                            .Bind(ArtworkImg.IdProperty, "ImageId")
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

                            _playerService.StartAllSongs(index);
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