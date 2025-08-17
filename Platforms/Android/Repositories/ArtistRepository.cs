using Android.Provider;
using ThinMPm.Platforms.Android.Models;
using ThinMPm.Platforms.Android.Models.Contracts;
using ThinMPm.Platforms.Android.Repositories.Contracts;

namespace ThinMPm.Platforms.Android.Repositories;

public class ArtistRepository : MediaStoreRepository<IArtistModel>, IArtistRepository
{
    public ArtistRepository()
        : base(
            MediaStore.Audio.Artists.ExternalContentUri,
            new[]
            {
              MediaStore.Audio.Artists.InterfaceConsts.Id,
              MediaStore.Audio.Artists.InterfaceConsts.Artist,
              MediaStore.Audio.Artists.InterfaceConsts.NumberOfAlbums,
                MediaStore.Audio.Artists.InterfaceConsts.NumberOfTracks
            })
    {
    }

    public IList<IArtistModel> FindAll()
    {
        Selection = null;
        SelectionArgs = null;
        SortOrder = MediaStore.Audio.Media.InterfaceConsts.Artist + " ASC";
        Bundle = null;

        return GetList();
    }

    public IArtistModel? FindById(string id)
    {
        Selection = MediaStore.Audio.Artists.InterfaceConsts.Id + " = ?";
        SelectionArgs = new[] { id };
        SortOrder = null;
        Bundle = null;

        return Get();
    }

    public IList<IArtistModel> FindByIds(IList<string> ids)
    {
        var idList = new List<string>();
        foreach (var id in ids)
        {
            idList.Add(id);
        }

        Selection = MediaStore.Audio.Artists.InterfaceConsts.Id + " IN (" + MakePlaceholders(idList.Count) + ")";
        SelectionArgs = [.. idList];
        SortOrder = null;

        return GetList();
    }

    private string? GetArtistId() =>
        Cursor?.GetColumnIndex(MediaStore.Audio.Media.InterfaceConsts.ArtistId) is int idx && idx >= 0
            ? Cursor.GetString(idx)
            : string.Empty;

    private string? GetArtistName() =>
        Cursor?.GetColumnIndex(MediaStore.Audio.Media.InterfaceConsts.Artist) is int idx && idx >= 0
            ? Cursor.GetString(idx)
            : string.Empty;

    private IArtistModel GetArtist()
    {
        var id = GetArtistId();
        var name = GetArtistName();

        return new ArtistModel(id, name);
    }

    public override IArtistModel Fetch() => GetArtist();
}