using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Utils;
using ThinMPm.ViewModels;
using ThinMPm.Views.Background;
using ThinMPm.Views.Behaviors;
using ThinMPm.Views.Img;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.Player;

public class MiniPlayer : ContentView
{
    private const string IconPlay = "playarrow";
    private const string IconPause = "pause";
    private const string IconSkipNext = "skipnext";

    public MiniPlayer()
    {
        var services = Application.Current!.Handler!.MauiContext!.Services;
        var playerViewModel = services.GetRequiredService<PlayerViewModel>();
        var platformUtil = services.GetRequiredService<IPlatformUtil>();
        var bottomBarHeight = platformUtil.GetBottomBarHeight();

        BindingContext = playerViewModel;
        HeightRequest = bottomBarHeight;
        this.Bind(IsVisibleProperty, nameof(PlayerViewModel.IsActive));

        var layout = new AbsoluteLayout();

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
            }
        };
        AbsoluteLayout.SetLayoutFlags(contentGrid, AbsoluteLayoutFlags.WidthProportional);
        AbsoluteLayout.SetLayoutBounds(contentGrid, new Rect(0, 0, 1, bottomBarHeight));

        var artwork = new ArtworkImage()
            .Bind(ArtworkImage.ImageIdProperty, $"{nameof(PlayerViewModel.CurrentSong)}.{nameof(ISongModel.ImageId)}")
            .Width(LayoutConstants.ImageSize)
            .Height(LayoutConstants.ImageSize)
            .Column(0);
        artwork.GestureRecognizers.Add(CreateNavigationGesture());
        contentGrid.Children.Add(artwork);

        var songTitle = new PrimaryText()
            .Bind(Label.TextProperty, $"{nameof(PlayerViewModel.CurrentSong)}.{nameof(ISongModel.Name)}")
            .CenterVertical()
            .Margins(left: LayoutConstants.SpacingMedium)
            .Column(1);
        songTitle.GestureRecognizers.Add(CreateNavigationGesture());
        contentGrid.Children.Add(songTitle);

        var playPauseButton = new Image
        {
            WidthRequest = 40,
            HeightRequest = 40,
            VerticalOptions = LayoutOptions.Center
        }.Column(2);
        playPauseButton.Behaviors.Add(new IconColorBehavior());
        playPauseButton.SetBinding(Image.SourceProperty, new Binding(nameof(PlayerViewModel.IsPlaying), converter: new PlayPauseImageConverter()));

        var playPauseTapGesture = new TapGestureRecognizer();
        playPauseTapGesture.SetBinding(TapGestureRecognizer.CommandProperty, nameof(PlayerViewModel.TogglePlayPauseCommand));
        playPauseButton.GestureRecognizers.Add(playPauseTapGesture);

        contentGrid.Children.Add(playPauseButton);

        var skipNextButton = new Image
        {
            Source = IconSkipNext,
            WidthRequest = 40,
            HeightRequest = 40,
            VerticalOptions = LayoutOptions.Center,
            Margin = new Thickness(LayoutConstants.SpacingMedium, 0, 0, 0)
        }.Column(3);
        skipNextButton.Behaviors.Add(new IconColorBehavior());

        var skipNextTapGesture = new TapGestureRecognizer();
        skipNextTapGesture.SetBinding(TapGestureRecognizer.CommandProperty, nameof(PlayerViewModel.NextCommand));
        skipNextButton.GestureRecognizers.Add(skipNextTapGesture);

        contentGrid.Children.Add(skipNextButton);

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

    private class PlayPauseImageConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            return value is true ? IconPause : IconPlay;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
