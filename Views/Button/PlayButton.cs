using System.Windows.Input;
using ThinMPm.Constants;

namespace ThinMPm.Views.Button;

public class PlayButton : BaseButton
{
    private const string IconPlay = "playarrow";

    public PlayButton(EventHandler<TappedEventArgs> onTapped, double iconSize = LayoutConstants.ButtonSmall)
        : base(IconPlay, onTapped, iconSize)
    {
    }

    public PlayButton(ICommand command, double iconSize = LayoutConstants.ButtonSmall)
        : base(IconPlay, (s, e) => command.Execute(null), iconSize)
    {
    }
}
