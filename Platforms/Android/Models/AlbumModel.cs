using Uri = Android.Net.Uri;
using ThinMPm.Platforms.Android.Constants;
using ThinMPm.Platforms.Android.Models.Contracts;

namespace ThinMPm.Platforms.Android.Models;

public class AlbumModel : IAlbumModel
{
    public string Id { get; }
    public string Name { get; }
    public string ArtistId { get; }
    public string ArtistName { get; }

    public AlbumModel(
        string id,
        string name,
        string artistId,
        string artistName)
    {
        Id = id;
        Name = name;
        ArtistId = artistId;
        ArtistName = artistName;
    }

    public string ImageId => Id;

    public Uri ImageUri => Uri.Parse($"{MediaConstant.ALBUM_ART}/{Id}");
}