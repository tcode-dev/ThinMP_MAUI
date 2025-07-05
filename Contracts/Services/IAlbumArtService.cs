namespace ThinMPm.Contracts.Services;

public interface IAlbumArtService
{
    Task<string?> GetArtwork(string id);
}