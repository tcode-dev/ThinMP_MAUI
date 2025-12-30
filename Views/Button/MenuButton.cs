using ThinMPm.Constants;

namespace ThinMPm.Views.Button;

public class MenuButton : BaseButton
{
    private const string MenuIcon = "more";

    public MenuButton(EventHandler<TappedEventArgs> onTapped, double iconSize = LayoutConstants.ButtonExtraSmall)
        : base(MenuIcon, onTapped, iconSize)
    {
    }
}
