using Foundation;

namespace ThinMPm.Platforms.iOS.ValueObjects;

// Swift: MPMediaEntityPersistentID
// C#: ulong
public class Id(string id)
{
  public NSNumber Raw { get; } = NSNumber.FromUInt64(ulong.Parse(id));
}