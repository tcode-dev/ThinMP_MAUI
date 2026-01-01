using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public partial class FavoriteArtistsEditViewModel(IFavoriteArtistService favoriteArtistService) : ObservableObject
{
    private readonly IFavoriteArtistService _favoriteArtistService = favoriteArtistService;
    private IList<IArtistModel> _originalArtists = [];

    [ObservableProperty]
    private ObservableCollection<IArtistModel> _artists = [];

    public async Task LoadAsync()
    {
        _originalArtists = await _favoriteArtistService.GetFavoriteArtistsAsync();
        Artists = new ObservableCollection<IArtistModel>(_originalArtists);
    }

    public void RemoveArtist(IArtistModel artist)
    {
        Artists.Remove(artist);
    }

    public async Task SaveAsync()
    {
        var currentIds = Artists.Select(a => a.Id).ToList();
        await _favoriteArtistService.UpdateAsync(currentIds);
    }

    public bool HasChanges()
    {
        if (_originalArtists.Count != Artists.Count) return true;

        for (int i = 0; i < _originalArtists.Count; i++)
        {
            if (_originalArtists[i].Id != Artists[i].Id) return true;
        }

        return false;
    }

    public void UpdateOrder(object itemsSource)
    {
        if (itemsSource is IEnumerable<IArtistModel> items)
        {
            Artists = new ObservableCollection<IArtistModel>(items);
        }
    }
}
