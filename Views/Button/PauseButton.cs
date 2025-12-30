using System.Windows.Input;
using ThinMPm.Constants;

namespace ThinMPm.Views.Button;

public class PauseButton : BaseButton
{
    private const string IconPause = "pause";

    public PauseButton(EventHandler<TappedEventArgs> onTapped, double iconSize = LayoutConstants.ButtonSmall)
        : base(IconPause, onTapped, iconSize)
    {
    }

    public PauseButton(ICommand command, double iconSize = LayoutConstants.ButtonSmall)
        : base(IconPause, (s, e) => command.Execute(null), iconSize)
    {
    }
}
