using System.Windows.Input;
using ThinMPm.Constants;

namespace ThinMPm.Views.Button;

public class FavoriteArtistOnButton : BaseButton
{
    private const string IconPersonOn = "personon";

    public FavoriteArtistOnButton(EventHandler<TappedEventArgs> onTapped, double iconSize = LayoutConstants.ButtonMedium)
        : base(IconPersonOn, onTapped, iconSize)
    {
    }

    public FavoriteArtistOnButton(ICommand command, double iconSize = LayoutConstants.ButtonMedium)
        : base(IconPersonOn, (s, e) => command.Execute(null), iconSize)
    {
    }
}
