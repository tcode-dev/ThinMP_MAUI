using MediaPlayer;
using ThinMPm.Platforms.iOS.Models;
using ThinMPm.Platforms.iOS.Models.Contracts;
using ThinMPm.Platforms.iOS.Repositories.Contracts;

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

  public IAlbumModel? FindById(string id)
  {
    var predicate = new MPMediaPropertyPredicate();
    var query = MPMediaQuery.AlbumsQuery;

    query.AddFilterPredicate(predicate);

    return query.Collections!.Select(media => new AlbumModel(media)).FirstOrDefault();
  }

  public IList<IAlbumModel> FindByArtistId(string artistId)
  {
    throw new NotImplementedException();
  }

  public IList<IAlbumModel> FindByRecent(int count)
  {
    throw new NotImplementedException();
  }
}