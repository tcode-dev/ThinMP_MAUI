using ThinMPm.Contracts.Models;

namespace ThinMPm.Models;

public class ArtistModel(string id, string name) : IArtistModel
{
  public string Id { get; set; } = id;
  public string Name { get; set; } = name;
}