using CoreGraphics;
using ThinMPm.Contracts.Services;
using ThinMPm.Platforms.iOS.Repositories.Contracts;
using ThinMPm.Platforms.iOS.ValueObjects;

namespace ThinMPm.Platforms.iOS.Services;

public class ArtworkService(ISongRepository songRepository) : IArtworkService
{
    private readonly ISongRepository _songRepository = songRepository;

  public async Task<byte[]?> GetArtwork(string id)
    {
        return await Task.Run(() =>
        {
            var song = _songRepository?.FindById(new Id(id));
            var image = song?.Artwork?.ImageWithSize(new CGSize(100, 100));
            var data = image?.AsPNG();

            return data?.ToArray();
        });
    }
}