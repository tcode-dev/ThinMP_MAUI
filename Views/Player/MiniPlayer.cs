using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Utils;
using ThinMPm.ViewModels;
using ThinMPm.Views.Background;
using ThinMPm.Views.Button;
using ThinMPm.Views.Img;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.Player;

public class MiniPlayer : ContentView
{
    public MiniPlayer()
    {
        var services = Application.Current!.Handler!.MauiContext!.Services;
        var playerViewModel = services.GetRequiredService<MiniPlayerViewModel>();
        var platformUtil = services.GetRequiredService<IPlatformUtil>();
        var bottomBarHeight = platformUtil.GetBottomBarHeight();

        BindingContext = playerViewModel;
        HeightRequest = bottomBarHeight;
        InputTransparent = false;
        this.Bind(IsVisibleProperty, nameof(MiniPlayerViewModel.IsActive));

        var layout = new AbsoluteLayout
        {
            SafeAreaEdges = SafeAreaEdges.None,
            InputTransparent = false,
            CascadeInputTransparent = false,
        };

        var blurBackground = new BlurBackgroundView();
        AbsoluteLayout.SetLayoutFlags(blurBackground, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(blurBackground, new Rect(0, 0, 1, bottomBarHeight));

        var contentGrid = new Grid
        {
            Padding = new Thickness(LayoutConstants.SpacingLarge, 0, LayoutConstants.SpacingLarge, 0),
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Auto),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Auto),
                new ColumnDefinition(GridLength.Auto)
            },
            InputTransparent = false,
            CascadeInputTransparent = false,
        };
        contentGrid.GestureRecognizers.Add(new TapGestureRecognizer());
        AbsoluteLayout.SetLayoutFlags(contentGrid, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(contentGrid, new Rect(0, 0, 1, bottomBarHeight));

        var artwork = new ArtworkImage()
            .Bind(ArtworkImage.ImageIdProperty, $"{nameof(MiniPlayerViewModel.CurrentSong)}.{nameof(ISongModel.ImageId)}")
            .Width(LayoutConstants.ImageSize)
            .Height(LayoutConstants.ImageSize)
            .Column(0);
        artwork.GestureRecognizers.Add(CreateNavigationGesture());
        contentGrid.Children.Add(artwork);

        var songTitle = new PrimaryText()
            .Bind(Label.TextProperty, $"{nameof(MiniPlayerViewModel.CurrentSong)}.{nameof(ISongModel.Name)}")
            .CenterVertical()
            .Margins(left: LayoutConstants.SpacingMedium)
            .Column(1);
        songTitle.GestureRecognizers.Add(CreateNavigationGesture());
        contentGrid.Children.Add(songTitle);

        var playButton = new PlayButton(playerViewModel.PlayCommand);
        playButton.Column(2);
        playButton.Bind(IsVisibleProperty, nameof(MiniPlayerViewModel.IsPlaying), converter: new InverseBoolConverter());
        contentGrid.Children.Add(playButton);

        var pauseButton = new PauseButton(playerViewModel.PauseCommand);
        pauseButton.Column(2);
        pauseButton.Bind(IsVisibleProperty, nameof(MiniPlayerViewModel.IsPlaying));
        contentGrid.Children.Add(pauseButton);

        var nextButton = new NextButton(playerViewModel.NextCommand)
        {
            Margin = new Thickness(LayoutConstants.SpacingMedium, 0, 0, 0)
        };
        nextButton.Column(3);
        contentGrid.Children.Add(nextButton);

        layout.Children.Add(blurBackground);
        layout.Children.Add(contentGrid);

        Content = layout;
    }

    private static TapGestureRecognizer CreateNavigationGesture()
    {
        var gesture = new TapGestureRecognizer();
        gesture.Tapped += async (s, e) => await Shell.Current.GoToAsync("PlayerPage");
        return gesture;
    }

    private class InverseBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            return value is bool b ? !b : value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
