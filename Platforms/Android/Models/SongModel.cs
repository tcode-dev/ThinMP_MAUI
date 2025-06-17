using Android.Provider;
using Uri = Android.Net.Uri;
using ThinMPm.Platforms.Android.Constants;
using ThinMPm.Platforms.Android.Models.Contracts;

namespace ThinMPm.Platforms.Android.Models;

public class SongModel : ISongModel
{
    private readonly string trackNumberRaw;

    public string Id { get; }
    public string Name { get; }
    public string AlbumId { get; }
    public string AlbumName { get; }
    public string ArtistId { get; }
    public string ArtistName { get; }
    public int Duration { get; }

    public SongModel(
        string id,
        string name,
        string albumId,
        string albumName,
        string artistId,
        string artistName,
        int duration,
        string trackNumber)
    {
        trackNumberRaw = trackNumber;
        Id = id;
        Name = name;
        AlbumId = albumId;
        AlbumName = albumName;
        ArtistId = artistId;
        ArtistName = artistName;
        Duration = duration;
    }

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