using Android.Provider;
using Uri = Android.Net.Uri;
using ThinMPm.Platforms.Android.Constants;
using ThinMPm.Platforms.Android.Models.Contracts;

namespace ThinMPm.Platforms.Android.Models;

public class SongModel(
    string id,
    string name,
    string albumId,
    string albumName,
    string artistId,
    string artistName,
    int duration,
    string trackNumber) : ISongModel
{
    private readonly string trackNumberRaw = trackNumber;

  public string Id { get; } = id;
  public string Name { get; } = name;
  public string AlbumId { get; } = albumId;
  public string AlbumName { get; } = albumName;
  public string ArtistId { get; } = artistId;
  public string ArtistName { get; } = artistName;
  public int Duration { get; } = duration;

  public string ImageId => AlbumId;

    public Uri ImageUri => Uri.Parse($"{MediaConstant.ALBUM_ART}/{AlbumId}");

    public Uri MediaUri => Uri.Parse($"{MediaStore.Audio.Media.ExternalContentUri}/{Id}");

    public double TrackNumber
    {
        get
        {
            // "15"、"15/30" → 15
            var match = System.Text.RegularExpressions.Regex.Match(trackNumberRaw ?? "", @"\d+");
            return match.Success ? double.Parse(match.Value) : 0.0;
        }
    }
}