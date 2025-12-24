using ThinMPm.Constants;
using ThinMPm.Contracts.Services;

namespace ThinMPm.Services;

public class PreferenceService : IPreferenceService
{
    private const string ShuffleModeKey = "shuffle_mode";

    public ShuffleMode GetShuffleMode()
    {
        var raw = Preferences.Get(ShuffleModeKey, (int)ShuffleMode.Off);
        return ShuffleModeExtensions.OfRaw(raw) ?? ShuffleMode.Off;
    }

    public void SetShuffleMode(ShuffleMode shuffleMode)
    {
        Preferences.Set(ShuffleModeKey, (int)shuffleMode);
    }
}
