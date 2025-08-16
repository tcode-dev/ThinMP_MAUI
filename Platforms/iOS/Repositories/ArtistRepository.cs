using MediaPlayer;
using ThinMPm.Platforms.iOS.Models;
using ThinMPm.Platforms.iOS.Models.Contracts;
using ThinMPm.Platforms.iOS.Repositories.Contracts;
using ThinMPm.Platforms.iOS.ValueObjects;

namespace ThinMPm.Platforms.iOS.Repositories;

public class ArtistRepository : IArtistRepository
{
  public IList<IArtistModel> FindAll()
  {
    var predicate = new MPMediaPropertyPredicate();
    var query = MPMediaQuery.ArtistsQuery;

    query.AddFilterPredicate(predicate);

    var collections = query.Collections;
    if (collections == null)
    {
      return new List<IArtistModel>();
    }

    return [.. collections.Select(media => new ArtistModel(media)).Cast<IArtistModel>()];
  }

  public IArtistModel? FindById(Id id)
  {
    var predicate = MPMediaPropertyPredicate.PredicateWithValue(
        id.AsNSNumber,
        MPMediaItem.ArtistPersistentIDProperty,
        MPMediaPredicateComparison.EqualsTo
    );
    var query = MPMediaQuery.ArtistsQuery;

    query.AddFilterPredicate(predicate);

    return query.Collections!.Select(media => new ArtistModel(media)).FirstOrDefault();
  }

  public IList<IArtistModel> FindByIds(IList<Id> ids)
  {
    return Array.Empty<IArtistModel>();
  }
}