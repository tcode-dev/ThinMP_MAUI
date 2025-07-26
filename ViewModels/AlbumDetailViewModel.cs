using System.Collections.ObjectModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public class AlbumDetailViewModel
{
    readonly ISongService _songService;
    public ObservableCollection<ISongModel> Songs { get; } = new();

    public AlbumDetailViewModel(ISongService songService)
    {
        _songService = songService;
    }

    public void Load()
    {
        var songs = _songService.FindAll();

        Songs.Clear();

        foreach (var song in songs)
        {
            Songs.Add(song);
        }
    }
}