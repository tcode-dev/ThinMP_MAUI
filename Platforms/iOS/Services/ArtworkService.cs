using CoreGraphics;
using ThinMPm.Contracts.Services;
using ThinMPm.Platforms.iOS.Repositories.Contracts;
using ThinMPm.Platforms.iOS.ValueObjects;

namespace ThinMPm.Platforms.iOS.Services;

public class ArtworkService : IArtworkService
{
    public async Task<byte[]?> GetArtwork(string id)
    {
        var repository = Application.Current?.Handler?.MauiContext?.Services.GetRequiredService<ISongRepository>();
        var song = repository?.FindById(new Id(id));
        var image = song?.Artwork?.ImageWithSize(new CGSize(100, 100));
        var data = image?.AsPNG();

        return data?.ToArray();
    }
}