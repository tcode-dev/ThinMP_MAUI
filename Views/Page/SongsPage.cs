using CommunityToolkit.Maui.Markup;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Contracts.Utils;
using ThinMPm.ViewModels;
using ThinMPm.Views.List;
using ThinMPm.Views.Header;
using Microsoft.Maui.Layouts;

namespace ThinMPm.Views.Page;

class SongsPage : ContentPage
{
    private readonly IPlayerService _playerService;
    private readonly IPlatformUtil _platformUtil;
    private readonly SongsHeader header;
    public SongsPage(SongViewModel vm, IPlayerService playerService, IPlatformUtil platformUtil)
    {
        Shell.SetNavBarIsVisible(this, false);

        BindingContext = vm;
        _playerService = playerService;
        _platformUtil = platformUtil;
        var layout = new AbsoluteLayout {
            SafeAreaEdges = SafeAreaEdges.None,
        };
        header = new SongsHeader();

        AbsoluteLayout.SetLayoutFlags(header, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(header, new Rect(0, 0, 1, _platformUtil.GetAppBarHeight()));
        var scrollView = new ScrollView
        {
            SafeAreaEdges = SafeAreaEdges.None,
            Content = new VerticalStackLayout
            {
                Children = {
                    new EmptyHeader(),
                    new SongList(OnSongTapped)
                    .Bind(ItemsView.ItemsSourceProperty, nameof(vm.Songs))
                }
            }
        };

        AbsoluteLayout.SetLayoutFlags(scrollView, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(scrollView, new Rect(0, 0, 1, 1));

        layout.Children.Add(scrollView);
        layout.Children.Add(header);

        Content = layout;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is SongViewModel vm)
        {
            vm.Load();
        }
    }

    private void OnSongTapped(object? sender, EventArgs e)
    {
        if (sender is BindableObject bindable && BindingContext is SongViewModel vm)
        {
            if (bindable.BindingContext is ISongModel item)
            {
                int index = vm.Songs.IndexOf(item);
                _playerService.StartAllSongs(index);
            }
        }
    }
}