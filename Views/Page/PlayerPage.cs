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

namespace ThinMPm.Views.Page;

class PlayerPage : ContentPage
{
    public PlayerPage(PlayerPageViewModel vm, IPlatformUtil platformUtil)
    {
        Shell.SetNavBarIsVisible(this, false);
        BindingContext = vm;

        var statusBarHeight = platformUtil.GetStatusBarHeight();
        var safeAreaBottom = platformUtil.GetBottomSafeAreaHeight();

        var layout = new AbsoluteLayout
        {
            BackgroundColor = ColorConstants.GetPrimaryBackgroundColor()
        };

        // Blurred background
        var blurBackground = new BlurredImageView()
            .Bind(BlurredImageView.ImageIdProperty, $"{nameof(PlayerPageViewModel.CurrentSong)}.{nameof(ISongModel.ImageId)}");
        AbsoluteLayout.SetLayoutFlags(blurBackground, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(blurBackground, new Rect(0, 0, 1, 0.5));

        // Main content
        var contentLayout = new VerticalStackLayout
        {
            Spacing = 0,
            Children =
            {
                CreateHeader(statusBarHeight),
                CreateArtworkSection(),
                CreateSongInfoSection(),
                CreateProgressSection(),
                CreatePlaybackControls(),
                CreateSecondaryControls(safeAreaBottom)
            }
        };
        AbsoluteLayout.SetLayoutFlags(contentLayout, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(contentLayout, new Rect(0, 0, 1, 1));

        layout.Children.Add(blurBackground);
        layout.Children.Add(contentLayout);

        Content = layout;
    }

    private View CreateHeader(double statusBarHeight)
    {
        var header = new Grid
        {
            Padding = new Thickness(LayoutConstants.SpacingMedium, statusBarHeight, LayoutConstants.SpacingMedium, 0),
            HeightRequest = statusBarHeight + LayoutConstants.HeightMedium,
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Auto),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Auto)
            }
        };

        var backButton = new BackButton().Column(0);

        header.Children.Add(backButton);

        return header;
    }

    private View CreateArtworkSection()
    {
        var artworkContainer = new Grid
        {
            Padding = new Thickness(LayoutConstants.SpacingLarge * 2, LayoutConstants.SpacingLarge, LayoutConstants.SpacingLarge * 2, LayoutConstants.SpacingLarge),
            HorizontalOptions = LayoutOptions.Center
        };

        var artwork = new ArtworkImage(15)
        {
            Aspect = Aspect.AspectFit,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }
        .Bind(ArtworkImage.ImageIdProperty, $"{nameof(PlayerPageViewModel.CurrentSong)}.{nameof(ISongModel.ImageId)}");

        // Set size based on screen width
        artwork.SizeChanged += (s, e) =>
        {
            if (Application.Current?.Windows.FirstOrDefault()?.Width is double windowWidth && windowWidth > 0)
            {
                var size = windowWidth - (LayoutConstants.SpacingLarge * 4);
                artwork.WidthRequest = size;
                artwork.HeightRequest = size;
            }
        };

        artworkContainer.Children.Add(artwork);

        return artworkContainer;
    }

    private View CreateSongInfoSection()
    {
        var songInfo = new VerticalStackLayout
        {
            Spacing = LayoutConstants.SpacingSmall,
            Padding = new Thickness(LayoutConstants.SpacingLarge, LayoutConstants.SpacingMedium),
            HorizontalOptions = LayoutOptions.Center
        };

        var songTitle = new PrimaryText
        {
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            HorizontalTextAlignment = TextAlignment.Center
        }.Bind(Label.TextProperty, $"{nameof(PlayerPageViewModel.CurrentSong)}.{nameof(ISongModel.Name)}");

        var artistName = new SecondaryText
        {
            FontSize = 16,
            HorizontalTextAlignment = TextAlignment.Center
        }.Bind(Label.TextProperty, $"{nameof(PlayerPageViewModel.CurrentSong)}.{nameof(ISongModel.ArtistName)}");

        songInfo.Children.Add(songTitle);
        songInfo.Children.Add(artistName);

        return songInfo;
    }

    private View CreateProgressSection()
    {
        var progressContainer = new VerticalStackLayout
        {
            Spacing = LayoutConstants.SpacingSmall,
            Padding = new Thickness(LayoutConstants.SpacingLarge, LayoutConstants.SpacingLarge)
        };

        var slider = new Slider
        {
            Minimum = 0,
            Maximum = 1,
            MinimumTrackColor = ColorConstants.GetPrimaryTextColor(),
            MaximumTrackColor = ColorConstants.GetLineColor(),
            ThumbColor = ColorConstants.GetPrimaryTextColor()
        }.Bind(Slider.ValueProperty, nameof(PlayerPageViewModel.CurrentTime));

        var timeContainer = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star)
            }
        };

        var currentTimeLabel = new SecondaryText
        {
            FontSize = 12,
            HorizontalTextAlignment = TextAlignment.Start
        }.Bind(Label.TextProperty, nameof(PlayerPageViewModel.CurrentTimeText)).Column(0);

        var durationLabel = new SecondaryText
        {
            FontSize = 12,
            HorizontalTextAlignment = TextAlignment.End
        }.Bind(Label.TextProperty, nameof(PlayerPageViewModel.DurationText)).Column(1);

        timeContainer.Children.Add(currentTimeLabel);
        timeContainer.Children.Add(durationLabel);

        progressContainer.Children.Add(slider);
        progressContainer.Children.Add(timeContainer);

        return progressContainer;
    }

    private View CreatePlaybackControls()
    {
        var controlsContainer = new Grid
        {
            Padding = new Thickness(LayoutConstants.SpacingLarge, LayoutConstants.SpacingMedium),
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star)
            },
            HorizontalOptions = LayoutOptions.Fill
        };

        // Previous button
        var previousButton = CreateIconButton(IconConstants.SkipPrevious, 50);
        var prevTap = new TapGestureRecognizer();
        prevTap.SetBinding(TapGestureRecognizer.CommandProperty, nameof(PlayerPageViewModel.PreviousCommand));
        previousButton.GestureRecognizers.Clear();
        previousButton.GestureRecognizers.Add(prevTap);
        previousButton.Column(0);

        // Play/Pause button
        var playPauseButton = new Label
        {
            FontFamily = IconConstants.FontFamily,
            FontSize = 70,
            TextColor = ColorConstants.GetPrimaryTextColor(),
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }.Column(1);
        playPauseButton.SetBinding(Label.TextProperty, new Binding(nameof(PlayerPageViewModel.IsPlaying), converter: new PlayPauseIconConverter()));

        var playPauseTap = new TapGestureRecognizer();
        playPauseTap.SetBinding(TapGestureRecognizer.CommandProperty, nameof(PlayerPageViewModel.TogglePlayPauseCommand));
        playPauseButton.GestureRecognizers.Add(playPauseTap);

        // Next button
        var nextButton = CreateIconButton(IconConstants.SkipNext, 50);
        var nextTap = new TapGestureRecognizer();
        nextTap.SetBinding(TapGestureRecognizer.CommandProperty, nameof(PlayerPageViewModel.NextCommand));
        nextButton.GestureRecognizers.Clear();
        nextButton.GestureRecognizers.Add(nextTap);
        nextButton.Column(2);

        controlsContainer.Children.Add(previousButton);
        controlsContainer.Children.Add(playPauseButton);
        controlsContainer.Children.Add(nextButton);

        return controlsContainer;
    }

    private View CreateSecondaryControls(double safeAreaBottom)
    {
        var secondaryContainer = new Grid
        {
            Padding = new Thickness(LayoutConstants.SpacingLarge, LayoutConstants.SpacingMedium, LayoutConstants.SpacingLarge, LayoutConstants.SpacingMedium + safeAreaBottom),
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star)
            },
            VerticalOptions = LayoutOptions.End
        };

        // Repeat button
        var repeatButton = new Label
        {
            FontFamily = IconConstants.FontFamily,
            Text = IconConstants.Repeat,
            FontSize = 24,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }.Column(0);
        repeatButton.SetBinding(Label.TextColorProperty, new Binding(nameof(PlayerPageViewModel.IsRepeatOn), converter: new ActiveColorConverter()));
        var repeatTap = new TapGestureRecognizer();
        repeatTap.SetBinding(TapGestureRecognizer.CommandProperty, nameof(PlayerPageViewModel.ToggleRepeatCommand));
        repeatButton.GestureRecognizers.Add(repeatTap);

        // Shuffle button
        var shuffleButton = new Label
        {
            FontFamily = IconConstants.FontFamily,
            Text = IconConstants.Shuffle,
            FontSize = 24,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }.Column(1);
        shuffleButton.SetBinding(Label.TextColorProperty, new Binding(nameof(PlayerPageViewModel.IsShuffleOn), converter: new ActiveColorConverter()));
        var shuffleTap = new TapGestureRecognizer();
        shuffleTap.SetBinding(TapGestureRecognizer.CommandProperty, nameof(PlayerPageViewModel.ToggleShuffleCommand));
        shuffleButton.GestureRecognizers.Add(shuffleTap);

        // Artist button
        var artistButton = CreateIconButton(IconConstants.Artist, 24);
        var artistTap = new TapGestureRecognizer();
        artistTap.SetBinding(TapGestureRecognizer.CommandProperty, nameof(PlayerPageViewModel.GoToArtistCommand));
        artistButton.GestureRecognizers.Clear();
        artistButton.GestureRecognizers.Add(artistTap);
        artistButton.Column(2);

        // Favorite button
        var favoriteButton = new Label
        {
            FontFamily = IconConstants.FontFamily,
            FontSize = 24,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }.Column(3);
        favoriteButton.SetBinding(Label.TextProperty, new Binding(nameof(PlayerPageViewModel.IsFavorite), converter: new FavoriteIconConverter()));
        favoriteButton.SetBinding(Label.TextColorProperty, new Binding(nameof(PlayerPageViewModel.IsFavorite), converter: new ActiveColorConverter()));
        var favoriteTap = new TapGestureRecognizer();
        favoriteTap.SetBinding(TapGestureRecognizer.CommandProperty, nameof(PlayerPageViewModel.ToggleFavoriteCommand));
        favoriteButton.GestureRecognizers.Add(favoriteTap);

        // Add to playlist button
        var playlistButton = CreateIconButton(IconConstants.Playlist, 24);
        var playlistTap = new TapGestureRecognizer();
        playlistTap.SetBinding(TapGestureRecognizer.CommandProperty, nameof(PlayerPageViewModel.AddToPlaylistCommand));
        playlistButton.GestureRecognizers.Clear();
        playlistButton.GestureRecognizers.Add(playlistTap);
        playlistButton.Column(4);

        secondaryContainer.Children.Add(repeatButton);
        secondaryContainer.Children.Add(shuffleButton);
        secondaryContainer.Children.Add(artistButton);
        secondaryContainer.Children.Add(favoriteButton);
        secondaryContainer.Children.Add(playlistButton);

        return secondaryContainer;
    }

    private static Label CreateIconButton(string icon, int fontSize)
    {
        var button = new Label
        {
            FontFamily = IconConstants.FontFamily,
            Text = icon,
            FontSize = fontSize,
            TextColor = ColorConstants.GetPrimaryTextColor(),
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        button.GestureRecognizers.Add(new TapGestureRecognizer());
        return button;
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

    private class FavoriteIconConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            return value is true ? IconConstants.Heart : IconConstants.HeartOutline;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    private class ActiveColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            return value is true ? ColorConstants.GetPrimaryTextColor() : ColorConstants.GetLineColor();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
