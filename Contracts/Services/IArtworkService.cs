namespace ThinMPm.Contracts.Services;

public interface IArtworkService
{
    Task<byte[]?> GetArtwork(string id);
}