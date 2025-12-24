using ThinMPm.Constants;
using ThinMPm.Contracts.Services;

namespace ThinMPm.Services;

public class PreferenceService : IPreferenceService
{
    private const string ShuffleModeKey = "shuffle_mode";
    private const string RepeatModeKey = "repeat_mode";

    public ShuffleMode GetShuffleMode()
    {
        var raw = Preferences.Get(ShuffleModeKey, (int)ShuffleMode.Off);
        return ShuffleModeExtensions.OfRaw(raw) ?? ShuffleMode.Off;
    }

    public void SetShuffleMode(ShuffleMode shuffleMode)
    {
        Preferences.Set(ShuffleModeKey, (int)shuffleMode);
    }

    public RepeatMode GetRepeatMode()
    {
        var raw = Preferences.Get(RepeatModeKey, (int)RepeatMode.Off);
        return RepeatModeExtensions.OfRaw(raw) ?? RepeatMode.Off;
    }

    public void SetRepeatMode(RepeatMode repeatMode)
    {
        Preferences.Set(RepeatModeKey, (int)repeatMode);
    }
}
