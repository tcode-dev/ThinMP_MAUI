using CoreGraphics;
using ThinMPm.Contracts.Services;
using ThinMPm.Platforms.iOS.Repositories.Contracts;

namespace ThinMPm.Platforms.iOS.Services;

public class AlbumArtService : IAlbumArtService
{
    public async Task<string?> GetArtwork(string id)
    {
        var repository = Application.Current?.Handler?.MauiContext?.Services.GetRequiredService<ISongRepository>();
        var song = repository.FindBySongId(id);
        var image = song?.Artwork?.ImageWithSize(new CGSize(100, 100));
        if (image == null)
        {
            return null;
        }

        using var data = image.AsPNG();

        if (data == null)
        {
            return null;
        }

        return Convert.ToBase64String(data.ToArray());
    }
}