using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Contracts.Utils;
using ThinMPm.Views.Background;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.Header;

public class DetailHeader : ContentView
{
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(
            nameof(Title),
            typeof(string),
            typeof(DetailHeader),
            default(string));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public DetailHeader()
    {
        var platformUtil = Application.Current!.Handler!.MauiContext!.Services.GetRequiredService<IPlatformUtil>();
        var appBarHeight = platformUtil.GetAppBarHeight();

        HeightRequest = appBarHeight;

        var layout = new AbsoluteLayout
        {
            SafeAreaEdges = SafeAreaEdges.None,
        };

        var blurBackground = new BlurBackgroundView();
        AbsoluteLayout.SetLayoutFlags(blurBackground, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(blurBackground, new Rect(0, 0, 1, appBarHeight));

        var contentGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(50),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(50)
            },
            Children =
            {
                new Button
                {
                    Text = "←",
                    FontSize = 18,
                    BackgroundColor = Colors.Transparent
                }.Column(0),

                new PrimaryTitle()
                    .Bind(Label.TextProperty, nameof(Title), source: this)
                    .Column(1),
            }
        };
        AbsoluteLayout.SetLayoutFlags(contentGrid, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(contentGrid, new Rect(0, 0, 1, appBarHeight));

        layout.Children.Add(blurBackground);
        layout.Children.Add(contentGrid);

        Content = layout;
    }
}
