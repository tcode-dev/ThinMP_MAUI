using System.Windows.Input;
using ThinMPm.Constants;

namespace ThinMPm.Views.Button;

public class ShuffleOnButton : BaseButton
{
    private const string IconShuffleOn = "shuffleon";

    public ShuffleOnButton(EventHandler<TappedEventArgs> onTapped, double iconSize = LayoutConstants.ButtonMedium)
        : base(IconShuffleOn, onTapped, iconSize)
    {
    }

    public ShuffleOnButton(ICommand command, double iconSize = LayoutConstants.ButtonMedium)
        : base(IconShuffleOn, (s, e) => command.Execute(null), iconSize)
    {
    }
}
