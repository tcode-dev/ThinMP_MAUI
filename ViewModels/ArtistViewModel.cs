using CommunityToolkit.Mvvm.ComponentModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public partial class ArtistViewModel(IArtistService artistService) : ObservableObject
{
    readonly IArtistService _artistService = artistService;

    [ObservableProperty]
    private IList<IArtistModel> _artists = [];

    public void Load()
    {
        Artists = _artistService.FindAll();
    }
}
