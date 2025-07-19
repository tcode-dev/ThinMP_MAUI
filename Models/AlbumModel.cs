using ThinMPm.Contracts.Models;

namespace ThinMPm.Models;

public class AlbumModel : IAlbumModel
{
  public AlbumModel(string id, string name, string artistId, string artistName, string imageId)
  {
    Id = id;
    Name = name;
    ArtistId = artistId;
    ArtistName = artistName;
    ImageId = imageId;
  }

  public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ArtistId { get; set; } = string.Empty;
    public string ArtistName { get; set; } = string.Empty;
    public string ImageId { get; set; } = string.Empty;
}