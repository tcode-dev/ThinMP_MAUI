using System.Windows.Input;
using ThinMPm.Constants;

namespace ThinMPm.Views.Button;

public class RepeatOneButton : BaseButton
{
    private const string IconRepeatOne = "repeatoneon";

    public RepeatOneButton(EventHandler<TappedEventArgs> onTapped, double iconSize = LayoutConstants.ButtonMedium)
        : base(IconRepeatOne, onTapped, iconSize)
    {
    }

    public RepeatOneButton(ICommand command, double iconSize = LayoutConstants.ButtonMedium)
        : base(IconRepeatOne, (s, e) => command.Execute(null), iconSize)
    {
    }
}
