using CommunityToolkit.Maui.Markup;
using ThinMPm.Resources.Strings;
using ThinMPm.Views.Button;

namespace ThinMPm.Views.Header;

public abstract class ListMenuHeader : ListHeader
{
    protected abstract string EditPageRoute { get; }

    public ListMenuHeader()
    {
        contentGrid.Children.Add(
            new MenuButton(OnMenuClicked).Column(2)
        );
    }

    private async Task OnMenuClicked()
    {
        var page = Application.Current?.Windows.FirstOrDefault()?.Page;
        if (page == null) return;

        var result = await page.DisplayActionSheetAsync(null, AppResources.Cancel, null, AppResources.Edit);

        if (result == AppResources.Edit)
        {
            await Shell.Current.GoToAsync(EditPageRoute);
        }
    }
}
