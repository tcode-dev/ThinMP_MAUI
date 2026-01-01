using CommunityToolkit.Maui.Markup;
using ThinMPm.Views.Button;

namespace ThinMPm.Views.Header;

public class ListMenuHeader : ListHeader
{
    public event EventHandler? MenuClicked;

    public ListMenuHeader()
    {
        contentGrid.Children.Add(
            new MenuButton((s, e) => MenuClicked?.Invoke(this, EventArgs.Empty)).Column(2)
        );
    }
}
