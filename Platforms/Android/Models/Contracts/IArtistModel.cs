using Uri = Android.Net.Uri;

namespace ThinMPm.Platforms.Android.Models.Contracts;

public interface IArtistModel
{
    string Id { get; }
    string Name { get; }
}