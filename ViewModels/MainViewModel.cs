using ThinMPm.Contracts.Models;
using ThinMPm.Models;

namespace ThinMPm.ViewModels;

public class MainViewModel
{
    public List<IMenuModel> MenuItems { get; }

    public MainViewModel()
    {
        MenuItems = new List<IMenuModel>
        {
            new MenuModel("Artists", nameof(Views.Page.ArtistsPage)),
            new MenuModel("Albums", nameof(Views.Page.AlbumsPage)),
            new MenuModel("Songs", nameof(Views.Page.SongsPage)),
        };
    }
}