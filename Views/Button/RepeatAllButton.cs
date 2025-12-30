using System.Windows.Input;
using ThinMPm.Constants;

namespace ThinMPm.Views.Button;

public class RepeatAllButton : BaseButton
{
    private const string IconRepeatAll = "repeaton";

    public RepeatAllButton(EventHandler<TappedEventArgs> onTapped, double iconSize = LayoutConstants.ButtonMedium)
        : base(IconRepeatAll, onTapped, iconSize)
    {
    }

    public RepeatAllButton(ICommand command, double iconSize = LayoutConstants.ButtonMedium)
        : base(IconRepeatAll, (s, e) => command.Execute(null), iconSize)
    {
    }
}
