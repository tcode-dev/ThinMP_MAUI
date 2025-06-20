using System.Collections.ObjectModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public class SongViewModel
{
    readonly ISongService _songService;
    public ObservableCollection<ISongModel> Songs { get; } = new();

    public SongViewModel(ISongService songService)
    {
        _songService = songService;
    }

    public void Load()
    {
        Songs.Clear();
        foreach (var song in _songService.FindAll())
        {
            Songs.Add(song);
        }
    }
}