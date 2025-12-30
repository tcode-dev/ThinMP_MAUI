using ThinMPm.Constants;

namespace ThinMPm.Views.Button;

public class BackButton : BaseButton
{
    private const string IconArrowBack = "arrowback";

    public BackButton() : base(IconArrowBack, async (s, e) => await Shell.Current.GoToAsync(".."), LayoutConstants.ButtonExtraSmall)
    {
    }
}
