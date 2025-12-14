using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Utils;
using ThinMPm.ViewModels;
using ThinMPm.Views.Header;
using ThinMPm.Views.ListItem;

namespace ThinMPm.Views.Page;

class ArtistsPage : ContentPage
{
    private readonly IPlatformUtil _platformUtil;
    private readonly ArtistsHeader header;
    public ArtistsPage(ArtistViewModel vm, IPlatformUtil platformUtil)
    {
        Shell.SetNavBarIsVisible(this, false);

        BindingContext = vm;
        _platformUtil = platformUtil;

        var layout = new AbsoluteLayout
        {
            SafeAreaEdges = SafeAreaEdges.None,
        };
        header = new ArtistsHeader();
        AbsoluteLayout.SetLayoutFlags(header, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(header, new Rect(0, 0, 1, _platformUtil.GetAppBarHeight()));

        var scrollView = new ScrollView
        {
            SafeAreaEdges = SafeAreaEdges.None,
            Content = new VerticalStackLayout
            {
                Children = {
                    new EmptyHeader(),
                    new CollectionView
                    {
                        ItemTemplate = new DataTemplate(() => new ArtistListItem(OnTapped))
                    }
                    .Bind(ItemsView.ItemsSourceProperty, nameof(vm.Artists))
                }
            }
        };
        scrollView.Scrolled += OnScrolled;

        AbsoluteLayout.SetLayoutFlags(scrollView, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(scrollView, new Rect(0, 0, 1, 1));

        layout.Children.Add(scrollView);
        layout.Children.Add(header);

        Content = layout;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ArtistViewModel vm)
        {
            vm.Load();
        }
    }

    private async void OnTapped(object? sender, EventArgs e)
    {
        if (sender is BindableObject bindable)
        {
            if (bindable.BindingContext is IArtistModel item)
            {
                await Shell.Current.GoToAsync($"{nameof(ArtistDetailPage)}?ArtistId={item.Id}");
            }
        }
    }

    private void OnScrolled(object? sender, ScrolledEventArgs e)
    {
        double x = e.ScrollX;
        double y = e.ScrollY;
        Console.WriteLine($"Scrolled to position: ({x}, {y})");
    }
}