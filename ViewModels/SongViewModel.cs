using CommunityToolkit.Mvvm.ComponentModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public partial class SongViewModel(ISongService songService) : ObservableObject
{
    readonly ISongService _songService = songService;

    [ObservableProperty]
    private IList<ISongModel> _songs = [];

    public void Load()
    {
        Songs = _songService.FindAll();
    }
}
