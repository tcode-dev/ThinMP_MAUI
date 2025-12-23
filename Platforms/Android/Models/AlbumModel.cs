using Uri = Android.Net.Uri;
using ThinMPm.Platforms.Android.Constants;
using ThinMPm.Platforms.Android.Models.Contracts;

namespace ThinMPm.Platforms.Android.Models;

public class AlbumModel(
    string id,
    string name,
    string artistId,
    string artistName) : IAlbumModel
{
  public string Id { get; } = id;
  public string Name { get; } = name;
  public string ArtistId { get; } = artistId;
  public string ArtistName { get; } = artistName;

  public string ImageId => Id;

    public Uri ImageUri => Uri.Parse($"{MediaConstants.ALBUM_ART}/{Id}");
}