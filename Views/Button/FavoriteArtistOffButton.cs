using System.Windows.Input;
using ThinMPm.Constants;

namespace ThinMPm.Views.Button;

public class FavoriteArtistOffButton : BaseButton
{
    private const string IconPerson = "person";

    public FavoriteArtistOffButton(EventHandler<TappedEventArgs> onTapped, double iconSize = LayoutConstants.ButtonMedium)
        : base(IconPerson, onTapped, iconSize)
    {
    }

    public FavoriteArtistOffButton(ICommand command, double iconSize = LayoutConstants.ButtonMedium)
        : base(IconPerson, (s, e) => command.Execute(null), iconSize)
    {
    }
}
