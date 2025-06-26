namespace ThinMPm.Constants;

public enum RepeatMode
{
    Off = 0,
    One = 1,
    All = 2
}

public static class RepeatModeExtensions
{
    public static RepeatMode? OfRaw(int raw)
    {
        return Enum.GetValues(typeof(RepeatMode))
            .Cast<RepeatMode>()
            .FirstOrDefault(mode => (int)mode == raw);
    }
}

public enum ShuffleMode
{
    Off = 0,
    On = 1
}

public static class ShuffleModeExtensions
{
    public static ShuffleMode? OfRaw(int raw)
    {
        return Enum.GetValues(typeof(ShuffleMode))
            .Cast<ShuffleMode>()
            .FirstOrDefault(mode => (int)mode == raw);
    }
}
