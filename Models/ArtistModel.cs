using ThinMPm.Contracts.Models;

namespace ThinMPm.Models;

public class ArtistModel : IArtistModel
{
  public ArtistModel(string id, string name)
  {
    Id = id;
    Name = name;
  }

  public string Id { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;
}