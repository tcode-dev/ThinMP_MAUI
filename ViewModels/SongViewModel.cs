using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public class SongViewModel
{
    readonly ISongService _songService;
    public IList<ISongModel> Songs { get; private set; } = new List<ISongModel>();

    public SongViewModel(ISongService songService)
    {
        _songService = songService;
    }

    public void Load()
    {
        this.Songs = _songService.FindAll();
    }
}