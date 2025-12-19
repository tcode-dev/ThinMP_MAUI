using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Utils;
using ThinMPm.ViewModels;
using ThinMPm.Views.Behaviors;
using ThinMPm.Views.Button;
using ThinMPm.Views.FirstView;
using ThinMPm.Views.Img;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.Page;

class PlayerPage : ContentPage
{
    private const string IconSkipPrevious = "skipprevious";
    private const string IconSkipNext = "skipnext";
    private const string IconPlay = "playarrow";
    private const string IconPause = "pause";
    private const string IconRepeat = "repeat";
    private const string IconRepeatOn = "repeaton";
    private const string IconShuffle = "shuffle";
    private const string IconShuffleOn = "shuffleon";
    private const string IconPerson = "person";
    private const string IconFavorite = "favorite";
    private const string IconFavoriteBorder = "favoriteborder";
    private const string IconPlaylistAdd = "playlistadd";

    public PlayerPage(PlayerPageViewModel vm, IPlatformUtil platformUtil)
    {
        Shell.SetNavBarIsVisible(this, false);
        BindingContext = vm;

        var statusBarHeight = platformUtil.GetStatusBarHeight();
        var safeAreaBottom = platformUtil.GetBottomSafeAreaHeight();

        var layout = new AbsoluteLayout {
            SafeAreaEdges = SafeAreaEdges.None,
        };
        var backButton = new BackButton
        {
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Center
        };
        AbsoluteLayout.SetLayoutFlags(backButton, AbsoluteLayoutFlags.None);
        AbsoluteLayout.SetLayoutBounds(backButton, new Rect(0, statusBarHeight, 50, 50));

        var background = CreateBackgroundSection();
        AbsoluteLayout.SetLayoutFlags(background, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(background, new Rect(0, 0, 1, 0.5));

        // Main content
        var contentLayout = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition(GridLength.Star),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Star),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Star),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Star),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Star),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Star)
            }
        };

        var artworkSection = CreateArtworkSection();
        Grid.SetRow(artworkSection, 1);

        var songInfoSection = CreateSongInfoSection();
        Grid.SetRow(songInfoSection, 3);

        var progressSection = CreateProgressSection();
        Grid.SetRow(progressSection, 5);

        var playbackControls = CreatePlaybackControls();
        Grid.SetRow(playbackControls, 7);

        var secondaryControls = CreateSecondaryControls(safeAreaBottom);
        Grid.SetRow(secondaryControls, 9);

        contentLayout.Children.Add(artworkSection);
        contentLayout.Children.Add(songInfoSection);
        contentLayout.Children.Add(progressSection);
        contentLayout.Children.Add(playbackControls);
        contentLayout.Children.Add(secondaryControls);
        AbsoluteLayout.SetLayoutFlags(contentLayout, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(contentLayout, new Rect(0, 0, 1, 1));

        layout.Children.Add(background);
        layout.Children.Add(contentLayout);
        layout.Children.Add(backButton);
        Content = layout;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is PlayerPageViewModel vm)
        {
            vm.Load();
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (BindingContext is PlayerPageViewModel vm)
        {
            vm.Unload();
        }
    }

    private View CreateArtworkSection()
    {
        var artworkContainer = new Grid
        {
            Padding = new Thickness(LayoutConstants.SpacingLarge * 2, LayoutConstants.SpacingLarge, LayoutConstants.SpacingLarge * 2, LayoutConstants.SpacingLarge),
            HorizontalOptions = LayoutOptions.Center
        };

        var artwork = new ArtworkImage(4)
        .Bind(ArtworkImage.ImageIdProperty, $"{nameof(PlayerPageViewModel.CurrentSong)}.{nameof(ISongModel.ImageId)}");

        // Set size based on screen width
        artwork.SizeChanged += (s, e) =>
        {
            if (Application.Current?.Windows.FirstOrDefault()?.Width is double windowWidth && windowWidth > 0)
            {
                var size = windowWidth * 0.7;
                artwork.WidthRequest = size;
                artwork.HeightRequest = size;
            }
        };

        artworkContainer.Children.Add(artwork);

        return artworkContainer;
    }

    private View CreateBackgroundSection()
    {
        var layout = new AbsoluteLayout {
            SafeAreaEdges = SafeAreaEdges.None,
        };
        var blurBackground = new BlurredImageView().Bind(BlurredImageView.ImageIdProperty, $"{nameof(PlayerPageViewModel.CurrentSong)}.{nameof(ISongModel.ImageId)}");
        AbsoluteLayout.SetLayoutFlags(blurBackground, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(blurBackground, new Rect(0, 0, 1, 1));

        var gradientOverlay = new GradientOverlay();
        AbsoluteLayout.SetLayoutFlags(gradientOverlay, AbsoluteLayoutFlags.All);
        AbsoluteLayout.SetLayoutBounds(gradientOverlay, new Rect(0, 0, 1, 1));

        layout.Children.Add(blurBackground);
        layout.Children.Add(gradientOverlay);

        return layout;
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
        var previousButton = CreateImageButton(IconSkipPrevious, 88);
        var prevTap = new TapGestureRecognizer();
        prevTap.SetBinding(TapGestureRecognizer.CommandProperty, nameof(PlayerPageViewModel.PreviousCommand));
        previousButton.GestureRecognizers.Add(prevTap);
        previousButton.Column(0);

        // Play/Pause button
        var playPauseButton = new Image
        {
            WidthRequest = 100,
            HeightRequest = 100,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }.Column(1);
        playPauseButton.Behaviors.Add(new IconColorBehavior());
        playPauseButton.SetBinding(Image.SourceProperty, new Binding(nameof(PlayerPageViewModel.IsPlaying), converter: new PlayPauseImageConverter()));

        var playPauseTap = new TapGestureRecognizer();
        playPauseTap.SetBinding(TapGestureRecognizer.CommandProperty, nameof(PlayerPageViewModel.TogglePlayPauseCommand));
        playPauseButton.GestureRecognizers.Add(playPauseTap);

        // Next button
        var nextButton = CreateImageButton(IconSkipNext, 88);
        var nextTap = new TapGestureRecognizer();
        nextTap.SetBinding(TapGestureRecognizer.CommandProperty, nameof(PlayerPageViewModel.NextCommand));
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
        var repeatButton = new Image
        {
            WidthRequest = 50,
            HeightRequest = 50,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }.Column(0);
        repeatButton.Behaviors.Add(new IconColorBehavior());
        repeatButton.SetBinding(Image.SourceProperty, new Binding(nameof(PlayerPageViewModel.IsRepeatOn), converter: new RepeatImageConverter()));
        var repeatTap = new TapGestureRecognizer();
        repeatTap.SetBinding(TapGestureRecognizer.CommandProperty, nameof(PlayerPageViewModel.ToggleRepeatCommand));
        repeatButton.GestureRecognizers.Add(repeatTap);

        // Shuffle button
        var shuffleButton = new Image
        {
            WidthRequest = 50,
            HeightRequest = 50,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }.Column(1);
        shuffleButton.Behaviors.Add(new IconColorBehavior());
        shuffleButton.SetBinding(Image.SourceProperty, new Binding(nameof(PlayerPageViewModel.IsShuffleOn), converter: new ShuffleImageConverter()));
        var shuffleTap = new TapGestureRecognizer();
        shuffleTap.SetBinding(TapGestureRecognizer.CommandProperty, nameof(PlayerPageViewModel.ToggleShuffleCommand));
        shuffleButton.GestureRecognizers.Add(shuffleTap);

        // Artist button
        var artistButton = CreateImageButton(IconPerson, 50);
        var artistTap = new TapGestureRecognizer();
        artistTap.SetBinding(TapGestureRecognizer.CommandProperty, nameof(PlayerPageViewModel.GoToArtistCommand));
        artistButton.GestureRecognizers.Add(artistTap);
        artistButton.Column(2);

        // Favorite button
        var favoriteButton = new Image
        {
            WidthRequest = 50,
            HeightRequest = 50,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        }.Column(3);
        favoriteButton.Behaviors.Add(new IconColorBehavior());
        favoriteButton.SetBinding(Image.SourceProperty, new Binding(nameof(PlayerPageViewModel.IsFavorite), converter: new FavoriteImageConverter()));
        var favoriteTap = new TapGestureRecognizer();
        favoriteTap.SetBinding(TapGestureRecognizer.CommandProperty, nameof(PlayerPageViewModel.ToggleFavoriteCommand));
        favoriteButton.GestureRecognizers.Add(favoriteTap);

        // Add to playlist button
        var playlistButton = CreateImageButton(IconPlaylistAdd, 50);
        var playlistTap = new TapGestureRecognizer();
        playlistTap.SetBinding(TapGestureRecognizer.CommandProperty, nameof(PlayerPageViewModel.AddToPlaylistCommand));
        playlistButton.GestureRecognizers.Add(playlistTap);
        playlistButton.Column(4);

        secondaryContainer.Children.Add(repeatButton);
        secondaryContainer.Children.Add(shuffleButton);
        secondaryContainer.Children.Add(artistButton);
        secondaryContainer.Children.Add(favoriteButton);
        secondaryContainer.Children.Add(playlistButton);

        return secondaryContainer;
    }

    private static Image CreateImageButton(string source, int size)
    {
        var image = new Image
        {
            Source = source,
            WidthRequest = size,
            HeightRequest = size,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        image.Behaviors.Add(new IconColorBehavior());
        return image;
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

    private class RepeatImageConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            return value is true ? IconRepeatOn : IconRepeat;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    private class ShuffleImageConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            return value is true ? IconShuffleOn : IconShuffle;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    private class FavoriteImageConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            return value is true ? IconFavorite : IconFavoriteBorder;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
