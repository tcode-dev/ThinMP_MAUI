using Android.Provider;
using Uri = Android.Net.Uri;
using ThinMPm.Platforms.Android.Model.Contract;
using ThinMPm.Platforms.Android.Model.ValueObjects;
using ThinMPm.Platforms.Android.Constant;

namespace ThinMPm.Platforms.Android.Model;

public class SongModel : ISongModel
{
    private readonly string trackNumberRaw;

    public SongId Id { get; }
    public string Name { get; }
    public AlbumId AlbumId { get; }
    public string AlbumName { get; }
    public ArtistId ArtistId { get; }
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
        Id = new SongId(id);
        Name = name;
        AlbumId = new AlbumId(albumId);
        AlbumName = albumName;
        ArtistId = new ArtistId(artistId);
        ArtistName = artistName;
        Duration = duration;
    }

    public string ImageId => AlbumId.Raw;

    public Uri ImageUri => Uri.Parse($"{MediaConstant.ALBUM_ART}/{AlbumId.Raw}");

    public Uri MediaUri => Uri.Parse($"{MediaStore.Audio.Media.ExternalContentUri}/{Id.Raw}");

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