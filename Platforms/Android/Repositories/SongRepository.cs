using Android.Provider;

using ThinMPm.Platforms.Android.Models;
using ThinMPm.Platforms.Android.Models.Contracts;
using ThinMPm.Platforms.Android.Repositories.Contracts;

namespace ThinMPm.Platforms.Android.Repositories;

public class SongRepository : MediaStoreRepository<ISongModel>, ISongRepository
{

  public SongRepository()
      : base(
          MediaStore.Audio.Media.ExternalContentUri,
          new[]
          {
              MediaStore.Audio.Media.InterfaceConsts.Id,
              MediaStore.Audio.Media.InterfaceConsts.Title,
              MediaStore.Audio.Media.InterfaceConsts.ArtistId,
              MediaStore.Audio.Media.InterfaceConsts.Artist,
              MediaStore.Audio.Media.InterfaceConsts.AlbumId,
              MediaStore.Audio.Media.InterfaceConsts.Album,
              MediaStore.Audio.Media.InterfaceConsts.Duration,
              MediaStore.Audio.Media.InterfaceConsts.Track
          })
  {
  }

  private readonly string trackNumberSortOrder =
      $"CASE " +
      $"WHEN {MediaStore.Audio.Media.InterfaceConsts.Track} LIKE '%/%' THEN " +
      $"CAST(SUBSTR({MediaStore.Audio.Media.InterfaceConsts.Track}, 0, INSTR({MediaStore.Audio.Media.InterfaceConsts.Track}, '/')) AS INTEGER) " +
      $"ELSE " +
      $"CAST({MediaStore.Audio.Media.InterfaceConsts.Track} AS INTEGER) " +
      $"END ASC";

    public IList<ISongModel> FindAll()
    {
        Selection = MediaStore.Audio.Media.InterfaceConsts.IsMusic + " = 1";
        SelectionArgs = null;
        SortOrder = MediaStore.Audio.Media.InterfaceConsts.Title + " ASC";
        return GetList();
    }

    public ISongModel? FindById(string songId)
    {
        Selection = MediaStore.Audio.Media.InterfaceConsts.Id + " = ?";
        SelectionArgs = new[] { songId };
        SortOrder = null;
        return Get();
    }

    public IList<ISongModel> FindByIds(IList<string> songIds)
    {
        var ids = new List<string>();
        foreach (var id in songIds)
            ids.Add(id);

        Selection = MediaStore.Audio.Media.InterfaceConsts.Id + " IN (" + MakePlaceholders(ids.Count) + ") AND " +
                    MediaStore.Audio.Media.InterfaceConsts.IsMusic + " = 1";
        SelectionArgs = ids.ToArray();
        SortOrder = null;
        return GetList();
    }

    public IList<ISongModel> FindByAlbumId(string albumId)
    {
        Selection = MediaStore.Audio.Media.InterfaceConsts.AlbumId + " = ? AND " +
                    MediaStore.Audio.Media.InterfaceConsts.IsMusic + " = 1";
        SelectionArgs = new[] { albumId };
        SortOrder = trackNumberSortOrder;
        return GetList();
    }

    public IList<ISongModel> FindByArtistId(string artistId)
    {
        Selection = MediaStore.Audio.Media.InterfaceConsts.ArtistId + " = ? AND " +
                    MediaStore.Audio.Media.InterfaceConsts.IsMusic + " = 1";
        SelectionArgs = new[] { artistId };
        SortOrder = $"{MediaStore.Audio.Media.InterfaceConsts.Album} ASC, {trackNumberSortOrder}";
        return GetList();
    }

    private string? GetId() =>
        Cursor?.GetColumnIndex(MediaStore.Audio.Media.InterfaceConsts.Id) is int idx && idx >= 0
            ? Cursor.GetString(idx)
            : string.Empty;

    private string? GetTitle() =>
        Cursor?.GetColumnIndex(MediaStore.Audio.Media.InterfaceConsts.Title) is int idx && idx >= 0
            ? Cursor.GetString(idx)
            : string.Empty;

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

    private int GetDuration() =>
        Cursor?.GetColumnIndex(MediaStore.Audio.Media.InterfaceConsts.Duration) is int idx && idx >= 0
            ? Cursor.GetInt(idx)
            : 0;

    private string? GetTrackNumber() =>
        Cursor?.GetColumnIndex(MediaStore.Audio.Media.InterfaceConsts.Track) is int idx && idx >= 0
            ? Cursor.GetString(idx)
            : string.Empty;

    private ISongModel GetSong() =>
        new SongModel(
            id: GetId(),
            name: GetTitle(),
            albumId: GetAlbumId(),
            albumName: GetAlbumName(),
            artistId: GetArtistId(),
            artistName: GetArtistName(),
            duration: GetDuration(),
            trackNumber: GetTrackNumber()
        );

    public override ISongModel Fetch() => GetSong();
}