using Android.Provider;
using ThinMPm.Platforms.Android.Models;
using ThinMPm.Platforms.Android.Models.Contracts;
using ThinMPm.Platforms.Android.Repositories.Contracts;

namespace ThinMPm.Platforms.Android.Repositories;

public class AlbumRepository : MediaStoreRepository<IAlbumModel>, IAlbumRepository
{
    public AlbumRepository()
        : base(
            MediaStore.Audio.Albums.ExternalContentUri,
            new[]
            {
              MediaStore.Audio.Albums.InterfaceConsts.AlbumId,
              MediaStore.Audio.Albums.InterfaceConsts.Album,
              MediaStore.Audio.Albums.InterfaceConsts.ArtistId,
              MediaStore.Audio.Albums.InterfaceConsts.Artist,
            })
    {
    }

    public IList<IAlbumModel> FindAll()
    {
        Selection = null;
        SelectionArgs = null;
        SortOrder = MediaStore.Audio.Media.InterfaceConsts.Album + " ASC";
        Bundle = null;

        return GetList();
    }

    public IList<IAlbumModel> FindByArtistId(string artistId)
    {
        throw new NotImplementedException();
    }

    public IAlbumModel? FindById(string albumId)
    {
        throw new NotImplementedException();
    }

    private string? GetArtistId() =>
        Cursor?.GetColumnIndex(MediaStore.Audio.Media.InterfaceConsts.ArtistId) is int idx && idx >= 0
            ? Cursor.GetString(idx)
            : string.Empty;

    private string? GetArtistName() =>
        Cursor?.GetColumnIndex(MediaStore.Audio.Media.InterfaceConsts.Artist) is int idx && idx >= 0
            ? Cursor.GetString(idx)
            : string.Empty;

    private string? GetAlbumId() =>
        Cursor?.GetColumnIndex(MediaStore.Audio.Media.InterfaceConsts.AlbumId) is int idx && idx >= 0
            ? Cursor.GetString(idx)
            : string.Empty;

    private string? GetAlbumName() =>
        Cursor?.GetColumnIndex(MediaStore.Audio.Media.InterfaceConsts.Album) is int idx && idx >= 0
            ? Cursor.GetString(idx)
            : string.Empty;

    private IAlbumModel GetAlbum()
    {
        var id = GetAlbumId();
        var name = GetAlbumName();
        var artistId = GetArtistId();
        var artistName = GetArtistName();

        return new AlbumModel(id, name, artistId, artistName);
    }

    public override IAlbumModel Fetch() => GetAlbum();
}