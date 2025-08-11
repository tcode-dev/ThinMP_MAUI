using Foundation;

namespace ThinMPm.Platforms.iOS.ValueObjects;

public class Id
{
  // MPMediaEntityPersistentID
  public ulong Value { get; }
  public NSNumber AsNSNumber { get; }

  public Id(string id)
  {
    Value = ulong.Parse(id);
    AsNSNumber = NSNumber.FromUInt64(Value);
  }
}