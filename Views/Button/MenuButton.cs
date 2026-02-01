using ThinMPm.Constants;

namespace ThinMPm.Views.Button;

public class MenuButton : BaseButton
{
    private const string MenuIcon = "more";
    private static bool _isProcessing;

    public MenuButton(Func<Task> onTapped, double iconSize = LayoutConstants.ButtonExtraSmall)
        : base(MenuIcon, WrapWithGuard(onTapped), iconSize)
    {
    }

    private static EventHandler<TappedEventArgs> WrapWithGuard(Func<Task> onTapped)
    {
        return async (s, e) =>
        {
            if (_isProcessing) return;

            _isProcessing = true;

            try
            {
                await onTapped();
            }
            finally
            {
                _isProcessing = false;
            }
        };
    }
}
