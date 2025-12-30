using System.Windows.Input;
using ThinMPm.Constants;

namespace ThinMPm.Views.Button;

public class ShuffleOffButton : BaseButton
{
    private const string IconShuffle = "shuffle";

    public ShuffleOffButton(EventHandler<TappedEventArgs> onTapped, double iconSize = LayoutConstants.ButtonMedium)
        : base(IconShuffle, onTapped, iconSize)
    {
    }

    public ShuffleOffButton(ICommand command, double iconSize = LayoutConstants.ButtonMedium)
        : base(IconShuffle, (s, e) => command.Execute(null), iconSize)
    {
    }
}
