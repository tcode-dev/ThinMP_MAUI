using CommunityToolkit.Mvvm.ComponentModel;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Models;
using ThinMPm.Resources.Strings;

namespace ThinMPm.ViewModels;

[QueryProperty(nameof(ArtistId), "ArtistId")]
public partial class ArtistDetailViewModel(IArtistService artistService, IAlbumService albumService, ISongService songService) : ObservableObject
{
    public string ArtistId { get; set; } = string.Empty;
    readonly IArtistService _artistService = artistService;
    readonly IAlbumService _albumService = albumService;
    readonly ISongService _songService = songService;

    [ObservableProperty]
    private IArtistModel? _artist;

    [ObservableProperty]
    private string? _imageId;

    [ObservableProperty]
    private IList<ISongModel> _songs = [];

    [ObservableProperty]
    private IList<object> _items = [];

    [ObservableProperty]
    private string? _secondaryText;

    public void Load()
    {
        Artist = _artistService.FindById(ArtistId);

        var albums = _albumService.FindByArtistId(ArtistId);
        ImageId = albums.FirstOrDefault()?.ImageId;
        var albumStacks = ConvertToAlbumStacks(albums);

        var songs = _songService.FindByArtistId(ArtistId);
        Songs = songs;

        Items = BuildItems(albumStacks, songs);

        SecondaryText = $"{albums?.Count ?? 0} Albums, {songs?.Count ?? 0} Songs";
    }

    private static IList<object> BuildItems(IList<IAlbumStackModel> albumStacks, IList<ISongModel> songs)
    {
        var items = new List<object>();

        if (albumStacks.Count > 0)
        {
            items.Add(new SectionTitleModel(AppResources.Albums));
            items.AddRange(albumStacks);
        }

        if (songs.Count > 0)
        {
            items.Add(new SectionTitleModel(AppResources.Songs));
            items.AddRange(songs);
        }

        return items;
    }

    private static IList<IAlbumStackModel> ConvertToAlbumStacks(IList<IAlbumModel> albums, int columnCount = 2)
    {
        var stacks = new List<IAlbumStackModel>();

        for (int i = 0; i < albums.Count; i += columnCount)
        {
            var stackAlbums = albums.Skip(i).Take(columnCount).ToList();
            stacks.Add(new AlbumStackModel(stackAlbums, columnCount));
        }

        return stacks;
    }
}