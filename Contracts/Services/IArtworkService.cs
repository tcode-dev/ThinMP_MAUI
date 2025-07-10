namespace ThinMPm.Contracts.Services;

public interface IArtworkService
{
    Task<string?> GetArtwork(string id);
}