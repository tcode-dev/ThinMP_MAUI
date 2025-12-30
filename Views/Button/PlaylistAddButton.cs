using System.Windows.Input;
using ThinMPm.Constants;

namespace ThinMPm.Views.Button;

public class PlaylistAddButton : BaseButton
{
    private const string IconPlaylistAdd = "playlistadd";

    public PlaylistAddButton(EventHandler<TappedEventArgs> onTapped, double iconSize = LayoutConstants.ButtonMedium)
        : base(IconPlaylistAdd, onTapped, iconSize)
    {
    }

    public PlaylistAddButton(ICommand command, double iconSize = LayoutConstants.ButtonMedium)
        : base(IconPlaylistAdd, (s, e) => command.Execute(null), iconSize)
    {
    }
}
