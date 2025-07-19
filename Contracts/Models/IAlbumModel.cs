namespace ThinMPm.Contracts.Models;

public interface IAlbumModel
{
    string Id { get; }
    string Name { get; }
    string ArtistId { get; }
    string ArtistName { get; }
    string ImageId { get; }
}