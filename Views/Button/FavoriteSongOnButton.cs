using System.Windows.Input;
using ThinMPm.Constants;

namespace ThinMPm.Views.Button;

public class FavoriteSongOnButton : BaseButton
{
    private const string IconFavorite = "favorite";

    public FavoriteSongOnButton(EventHandler<TappedEventArgs> onTapped, double iconSize = LayoutConstants.ButtonMedium)
        : base(IconFavorite, onTapped, iconSize)
    {
    }

    public FavoriteSongOnButton(ICommand command, double iconSize = LayoutConstants.ButtonMedium)
        : base(IconFavorite, (s, e) => command.Execute(null), iconSize)
    {
    }
}
