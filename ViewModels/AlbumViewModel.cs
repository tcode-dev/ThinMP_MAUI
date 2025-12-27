using CommunityToolkit.Mvvm.ComponentModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public partial class AlbumViewModel(IAlbumService albumService) : ObservableObject
{
    readonly IAlbumService _albumService = albumService;

    [ObservableProperty]
    private IList<IAlbumModel> _albums = [];

    public void Load()
    {
        Albums = _albumService.FindAll();
    }
}
