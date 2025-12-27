namespace ThinMPm.Contracts.Models;

public interface IPlaylistModel
{
    int Id { get; }
    string Name { get; }
    string? ImageId { get; }
}
