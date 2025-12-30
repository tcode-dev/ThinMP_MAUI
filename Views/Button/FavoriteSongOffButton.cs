using System.Windows.Input;
using ThinMPm.Constants;

namespace ThinMPm.Views.Button;

public class FavoriteSongOffButton : BaseButton
{
    private const string IconFavoriteBorder = "favoriteborder";

    public FavoriteSongOffButton(EventHandler<TappedEventArgs> onTapped, double iconSize = LayoutConstants.ButtonMedium)
        : base(IconFavoriteBorder, onTapped, iconSize)
    {
    }

    public FavoriteSongOffButton(ICommand command, double iconSize = LayoutConstants.ButtonMedium)
        : base(IconFavoriteBorder, (s, e) => command.Execute(null), iconSize)
    {
    }
}
