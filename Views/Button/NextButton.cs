using System.Windows.Input;
using ThinMPm.Constants;

namespace ThinMPm.Views.Button;

public class NextButton : BaseButton
{
    private const string IconSkipNext = "skipnext";

    public NextButton(EventHandler<TappedEventArgs> onTapped, double iconSize = LayoutConstants.ButtonSmall)
        : base(IconSkipNext, onTapped, iconSize)
    {
    }

    public NextButton(ICommand command, double iconSize = LayoutConstants.ButtonSmall)
        : base(IconSkipNext, (s, e) => command.Execute(null), iconSize)
    {
    }
}
