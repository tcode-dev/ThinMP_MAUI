using ThinMPm.Contracts.Models;

namespace ThinMPm.Models;

public class PlaylistModel(int id, string name, string? imageId) : IPlaylistModel
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string? ImageId { get; set; } = imageId;
}
