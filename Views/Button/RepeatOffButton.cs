using System.Windows.Input;
using ThinMPm.Constants;

namespace ThinMPm.Views.Button;

public class RepeatOffButton : BaseButton
{
    private const string IconRepeat = "repeat";

    public RepeatOffButton(EventHandler<TappedEventArgs> onTapped, double iconSize = LayoutConstants.ButtonMedium)
        : base(IconRepeat, onTapped, iconSize)
    {
    }

    public RepeatOffButton(ICommand command, double iconSize = LayoutConstants.ButtonMedium)
        : base(IconRepeat, (s, e) => command.Execute(null), iconSize)
    {
    }
}
