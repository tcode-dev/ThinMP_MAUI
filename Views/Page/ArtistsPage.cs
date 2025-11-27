using CommunityToolkit.Maui.Markup;
using ThinMPm.ViewModels;
using ThinMPm.Views.Row;
using ThinMPm.Views.Header;
using ThinMPm.Contracts.Models;

namespace ThinMPm.Views.Page;

class ArtistsPage : ContentPage
{
    public ArtistsPage(ArtistViewModel vm)
    {
        Shell.SetNavBarIsVisible(this, false);

        BindingContext = vm;

        var scrollView = new ScrollView
        {
            SafeAreaEdges = SafeAreaEdges.None,
            Content = new VerticalStackLayout
            {
                Children = {
                    new ArtistsHeader(),
                    new CollectionView
                    {
                        ItemTemplate = new DataTemplate(() => new ArtistListItem(OnTapped))
                    }
                    .Bind(ItemsView.ItemsSourceProperty, nameof(vm.Artists))
                }
            }
        };
        scrollView.Scrolled += OnScrolled;

        Content = scrollView;
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