using MediaPlayer;
using ThinMPm.Platforms.iOS.Models;
using ThinMPm.Platforms.iOS.Models.Contracts;
using ThinMPm.Platforms.iOS.Repositories.Contracts;
using ThinMPm.Platforms.iOS.ValueObjects;

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

    public ISongModel? FindById(Id id)
    {
        var property = new MPMediaPropertyPredicate();
        var query = new MPMediaQuery();
        query.AddFilterPredicate(property);

        var collections = query.Collections;
        var song = collections?.FirstOrDefault(c =>
            c.RepresentativeItem?.PersistentID == id.Value);

        return song != null ? new SongModel(song) : null;
    }

    public IList<ISongModel> FindByIds(IList<Id> ids)
    {
        var property = new MPMediaPropertyPredicate();
        var query = new MPMediaQuery();
        query.AddFilterPredicate(property);

        var idSet = ids.Select(id => id.Value).ToHashSet();
        var collections = query.Collections ?? new MPMediaItemCollection[0];
        var filtered = collections
            .Where(c => c.RepresentativeItem != null && idSet.Contains(c.RepresentativeItem.PersistentID))
            .ToList();

        // 保持する順序をidsに合わせる
        var result = ids
            .Where(id => filtered.Any(f => f.RepresentativeItem?.PersistentID == id.Value
        ))
            .Select(id => filtered.First(f => f.RepresentativeItem?.PersistentID == id.Value))
            .Select(c => new SongModel(c) as ISongModel)
            .ToList();

        return result;
    }

    public IList<ISongModel> FindByAlbumId(Id albumId)
    {
        var predicate = MPMediaPropertyPredicate.PredicateWithValue(
            albumId.AsNSNumber,
            MPMediaItem.AlbumPersistentIDProperty,
            MPMediaPredicateComparison.EqualsTo
        );
        var query = new MPMediaQuery();
        query.AddFilterPredicate(predicate);
        var collections = query.Collections ?? new MPMediaItemCollection[0];
        return collections.Select(c => new SongModel(c)).Cast<ISongModel>().ToList();
    }

    public IList<ISongModel> FindByAlbumIds(IList<Id> albumIds)
    {
        var result = new List<ISongModel>();
        foreach (var albumId in albumIds)
        {
            result.AddRange(FindByAlbumId(albumId));
        }
        return result;
    }
}