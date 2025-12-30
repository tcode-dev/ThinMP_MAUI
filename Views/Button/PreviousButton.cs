using System.Windows.Input;
using ThinMPm.Constants;

namespace ThinMPm.Views.Button;

public class PreviousButton : BaseButton
{
    private const string IconSkipPrevious = "skipprevious";

    public PreviousButton(EventHandler<TappedEventArgs> onTapped, double iconSize = LayoutConstants.ButtonSmall)
        : base(IconSkipPrevious, onTapped, iconSize)
    {
    }

    public PreviousButton(ICommand command, double iconSize = LayoutConstants.ButtonSmall)
        : base(IconSkipPrevious, (s, e) => command.Execute(null), iconSize)
    {
    }
}
