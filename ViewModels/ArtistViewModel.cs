using System.Collections.ObjectModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public class ArtistViewModel(IArtistService artistService)
{
    readonly IArtistService _artistService = artistService;
    public ObservableCollection<IArtistModel> Artists { get; } = [];

    public void Load()
    {
        var artists = _artistService.FindAll();

        Artists.Clear();

        foreach (var artist in artists)
        {
            Artists.Add(artist);
        }
    }
}