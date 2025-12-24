using ThinMPm.Constants;

namespace ThinMPm.Contracts.Services;

public interface IPreferenceService
{
    ShuffleMode GetShuffleMode();
    void SetShuffleMode(ShuffleMode shuffleMode);
}
