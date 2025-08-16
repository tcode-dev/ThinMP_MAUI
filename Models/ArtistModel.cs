using ThinMPm.Contracts.Models;

namespace ThinMPm.Models;

public class ArtistModel : IArtistModel
{
  public ArtistModel(string id, string name, string imageId)
  {
    Id = id;
    Name = name;
    ImageId = imageId;
  }

  public string Id { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;
  public string ImageId { get; set; } = string.Empty;
}