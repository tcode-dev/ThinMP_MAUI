using MediaPlayer;
using ThinMPm.Platforms.iOS.Models;
using ThinMPm.Platforms.iOS.Models.Contracts;
using ThinMPm.Platforms.iOS.Repositories.Contracts;

namespace ThinMPm.Platforms.iOS.Repositories;

public class SongRepository : ISongRepository
{
    public IList<ISongModel> FindAll()
    {
        var property = new MPMediaPropertyPredicate();
        var query = new MPMediaQuery();
        query.AddFilterPredicate(property);

        var collections = query.Collections ?? new MPMediaItemCollection[0];
        return collections.Select(c => new SongModel(c)).Cast<ISongModel>().ToList();
    }

    public ISongModel? FindBySongId(string songId)
    {
        var property = new MPMediaPropertyPredicate();
        var query = new MPMediaQuery();
        query.AddFilterPredicate(property);

        var collections = query.Collections;
        var song = collections?.FirstOrDefault(c =>
            c.RepresentativeItem?.PersistentID.ToString() == songId);

        return song != null ? new SongModel(song) : null;
    }

    public IList<ISongModel> FindBySongIds(IList<string> songIds)
    {
        var property = new MPMediaPropertyPredicate();
        var query = new MPMediaQuery();
        query.AddFilterPredicate(property);

        var ids = songIds.ToHashSet();
        var collections = query.Collections ?? new MPMediaItemCollection[0];
        var filtered = collections
            .Where(c => c.RepresentativeItem != null && ids.Contains(c.RepresentativeItem.PersistentID.ToString()))
            .ToList();

        // 保持する順序をsongIdsに合わせる
        var result = songIds
            .Where(id => filtered.Any(f => f.RepresentativeItem?.PersistentID.ToString() == id))
            .Select(id => filtered.First(f => f.RepresentativeItem?.PersistentID.ToString() == id))
            .Select(c => new SongModel(c) as ISongModel)
            .ToList();

        return result;
    }

    public IList<ISongModel> FindByAlbumId(string albumId)
    {
        var property = new MPMediaPropertyPredicate();
        var query = new MPMediaQuery();
        query.AddFilterPredicate(property);

        var collections = query.Collections ?? new MPMediaItemCollection[0];
        return collections.Select(c => new SongModel(c)).Cast<ISongModel>().ToList();
    }

    public IList<ISongModel> FindByAlbumIds(IList<string> albumIds)
    {
        var result = new List<ISongModel>();
        foreach (var albumId in albumIds)
        {
            result.AddRange(FindByAlbumId(albumId));
        }
        return result;
    }
}