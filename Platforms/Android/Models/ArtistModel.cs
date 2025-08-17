using ThinMPm.Platforms.Android.Models.Contracts;

namespace ThinMPm.Platforms.Android.Models;

public class ArtistModel : IArtistModel
{
    public string Id { get; }
    public string Name { get; }

    public ArtistModel(
        string id,
        string name)
    {
        Id = id;
        Name = name;
    }
}