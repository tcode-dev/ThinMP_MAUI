using ThinMPm.Contracts.Services;

namespace ThinMPm.Views.Page;

class AlbumsPage : ContentPage
{

    private readonly IPlayerService _playerService;
    public AlbumsPage()
    {
        NavigationPage.SetHasNavigationBar(this, false);

        Content = new Label()
        {
            Text = "Albums"
        };
    }
}