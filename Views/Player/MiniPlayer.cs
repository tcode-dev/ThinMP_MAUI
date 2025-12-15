using CommunityToolkit.Maui.Markup;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Utils;
using ThinMPm.ViewModels;
using ThinMPm.Views.Img;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.Player;

public class MiniPlayer : Grid
{
    public MiniPlayer()
    {
        var services = Application.Current!.Handler!.MauiContext!.Services;
        var playerViewModel = services.GetRequiredService<PlayerViewModel>();
        var platformUtil = services.GetRequiredService<IPlatformUtil>();

        BindingContext = playerViewModel;
        HeightRequest = platformUtil.GetBottomBarHeight();
        BackgroundColor = ColorConstants.GetBackgroundColor();
        this.Bind(IsVisibleProperty, nameof(PlayerViewModel.IsActive));

        Padding = new Thickness(LayoutConstants.SpacingLarge, 0, LayoutConstants.SpacingLarge, 0);
        ColumnDefinitions = new ColumnDefinitionCollection
        {
            new ColumnDefinition(GridLength.Auto),
            new ColumnDefinition(GridLength.Star),
            new ColumnDefinition(GridLength.Auto),
            new ColumnDefinition(GridLength.Auto)
        };

        Children.Add(
            new ArtworkImage()
                .Bind(ArtworkImage.ImageIdProperty, $"{nameof(PlayerViewModel.CurrentSong)}.{nameof(ISongModel.ImageId)}")
                .Width(40)
                .Height(40)
                .Column(0)
        );

        Children.Add(
            new PrimaryText()
                .Bind(Label.TextProperty, $"{nameof(PlayerViewModel.CurrentSong)}.{nameof(ISongModel.Name)}")
                .CenterVertical()
                .Margins(left: LayoutConstants.SpacingMedium)
                .Column(1)
        );

        var playPauseButton = new Label
        {
            FontFamily = IconConstants.FontFamily,
            FontSize = 40,
            TextColor = ColorConstants.GetPrimaryTextColor(),
            VerticalOptions = LayoutOptions.Center
        }.Column(2);
        playPauseButton.SetBinding(Label.TextProperty, new Binding(nameof(PlayerViewModel.IsPlaying), converter: new PlayPauseIconConverter()));

        var playPauseTapGesture = new TapGestureRecognizer();
        playPauseTapGesture.SetBinding(TapGestureRecognizer.CommandProperty, nameof(PlayerViewModel.TogglePlayPauseCommand));
        playPauseButton.GestureRecognizers.Add(playPauseTapGesture);

        Children.Add(playPauseButton);

        var skipNextButton = new Label
        {
            FontFamily = IconConstants.FontFamily,
            Text = IconConstants.SkipNext,
            FontSize = 40,
            TextColor = ColorConstants.GetPrimaryTextColor(),
            VerticalOptions = LayoutOptions.Center,
            Margin = new Thickness(LayoutConstants.SpacingMedium, 0, 0, 0)
        }.Column(3);

        var skipNextTapGesture = new TapGestureRecognizer();
        skipNextTapGesture.SetBinding(TapGestureRecognizer.CommandProperty, nameof(PlayerViewModel.NextCommand));
        skipNextButton.GestureRecognizers.Add(skipNextTapGesture);

        Children.Add(skipNextButton);
    }

    private class PlayPauseIconConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            return value is true ? IconConstants.Pause : IconConstants.Play;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
