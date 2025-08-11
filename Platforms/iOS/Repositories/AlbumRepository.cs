using MediaPlayer;
using ThinMPm.Platforms.iOS.Models;
using ThinMPm.Platforms.iOS.Models.Contracts;
using ThinMPm.Platforms.iOS.Repositories.Contracts;
using ThinMPm.Platforms.iOS.ValueObjects;

namespace ThinMPm.Platforms.iOS.Repositories;

public class AlbumRepository : IAlbumRepository
{
  public IList<IAlbumModel> FindAll()
  {
    var predicate = new MPMediaPropertyPredicate();
    var query = MPMediaQuery.AlbumsQuery;

    query.AddFilterPredicate(predicate);

    var collections = query.Collections;
    if (collections == null)
    {
      return new List<IAlbumModel>();
    }

    return collections.Select(media => new AlbumModel(media)).Cast<IAlbumModel>().ToList();
  }

  public IAlbumModel? FindById(Id id)
  {
    var predicate = MPMediaPropertyPredicate.PredicateWithValue(
        id.AsNSNumber,
        MPMediaItem.AlbumPersistentIDProperty,
        MPMediaPredicateComparison.EqualsTo
    );
    var query = MPMediaQuery.AlbumsQuery;

    query.AddFilterPredicate(predicate);

    return query.Collections!.Select(media => new AlbumModel(media)).FirstOrDefault();
  }

  public IList<IAlbumModel> FindByArtistId(Id artistId)
  {
    return Array.Empty<IAlbumModel>();
  }

  public IList<IAlbumModel> FindByRecent(int count)
  {
    return Array.Empty<IAlbumModel>();
  }
}