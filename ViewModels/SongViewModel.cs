using System.Collections.ObjectModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public class SongViewModel(ISongService songService)
{
    readonly ISongService _songService = songService;
    public ObservableCollection<ISongModel> Songs { get; } = new();

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