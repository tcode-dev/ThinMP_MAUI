using CommunityToolkit.Maui.Markup;
using ThinMPm.ViewModels;
using ThinMPm.Views.Row;
using ThinMPm.Views.Header;

namespace ThinMPm.Views.Page;

class ArtistsPage : ContentPage
{
    public ArtistsPage(ArtistViewModel vm)
    {
        NavigationPage.SetHasNavigationBar(this, false);

        BindingContext = vm;

        var scrollView = new ScrollView
        {
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
        scrollView.Scrolled += (sender, e) =>
        {
            double x = e.ScrollX;
            double y = e.ScrollY;
            Console.WriteLine($"Scrolled to position: ({x}, {y})");
        };

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

    private void OnTapped(object? sender, EventArgs e)
    {

    }
}