using Microsoft.Maui.Layouts;
using ThinMPm.Constants;
using ThinMPm.Contracts.Utils;
using ThinMPm.Resources.Strings;
using ThinMPm.Views.Background;
using ThinMPm.Views.Button;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.Header;

public class EditHeader : ContentView
{
    private readonly BoxView solidBackground;
    private readonly BlurBackgroundView blurBackground;

    public event EventHandler? DoneClicked;

    public EditHeader()
    {
        var platformUtil = Application.Current!.Handler!.MauiContext!.Services.GetRequiredService<IPlatformUtil>();
        var appBarHeight = platformUtil.GetAppBarHeight();

        HeightRequest = appBarHeight;

        var layout = new AbsoluteLayout
        {
            SafeAreaEdges = SafeAreaEdges.None,
        };

        solidBackground = new BoxView
        {
            Color = ColorConstants.PrimaryBackgroundColor,
        };
        AbsoluteLayout.SetLayoutFlags(solidBackground, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(solidBackground, new Rect(0, 0, 1, appBarHeight));

        blurBackground = new BlurBackgroundView
        {
            Opacity = 0
        };
        AbsoluteLayout.SetLayoutFlags(blurBackground, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(blurBackground, new Rect(0, 0, 1, appBarHeight));

        var cancelButton = new TextButton(AppResources.Cancel, async (s, e) => await Shell.Current.GoToAsync(".."))
        {
            HorizontalOptions = LayoutOptions.Start,
            Margin = new Thickness(20, 0, 0, 0)
        };

        var title = new PrimaryTitle { Text = AppResources.Edit };

        var doneButton = new TextButton(AppResources.Done, (s, e) => DoneClicked?.Invoke(this, EventArgs.Empty))
        {
            HorizontalOptions = LayoutOptions.End,
            Margin = new Thickness(0, 0, 20, 0)
        };

        var contentGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Auto),
                new ColumnDefinition(GridLength.Star)
            },
            Children =
            {
                cancelButton,
                title,
                doneButton
            }
        };
        Grid.SetColumn(cancelButton, 0);
        Grid.SetColumn(title, 1);
        Grid.SetColumn(doneButton, 2);

        AbsoluteLayout.SetLayoutFlags(contentGrid, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(contentGrid, new Rect(0, 0, 1, appBarHeight));

        layout.Children.Add(solidBackground);
        layout.Children.Add(blurBackground);
        layout.Children.Add(contentGrid);

        Content = layout;
    }

    public async void ShowBlurBackground()
    {
        await Task.WhenAll(
            solidBackground.FadeToAsync(0, 300, Easing.CubicOut),
            blurBackground.FadeToAsync(1, 300, Easing.CubicOut)
        );
    }

    public async void ShowSolidBackground()
    {
        await Task.WhenAll(
            solidBackground.FadeToAsync(1, 300, Easing.CubicOut),
            blurBackground.FadeToAsync(0, 300, Easing.CubicOut)
        );
    }
}
