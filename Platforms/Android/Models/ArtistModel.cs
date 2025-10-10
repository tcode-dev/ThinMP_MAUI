using ThinMPm.Platforms.Android.Models.Contracts;

namespace ThinMPm.Platforms.Android.Models;

public class ArtistModel(
    string id,
    string name) : IArtistModel
{
  public string Id { get; } = id;
  public string Name { get; } = name;
}