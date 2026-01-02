using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Utils;
using ThinMPm.Extensions;
using ThinMPm.ViewModels;
using ThinMPm.Converters;
using ThinMPm.Views.Button;
using ThinMPm.Views.FirstView;
using ThinMPm.Views.Img;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.Page;

class PlayerPage : ContentPage
{
    public PlayerPage(PlayerViewModel vm, IPlatformUtil platformUtil)
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

        if (BindingContext is PlayerViewModel vm)
        {
            vm.Load();
        }

        var window = Application.Current?.Windows.FirstOrDefault();
        if (window != null)
        {
            window.Deactivated += OnWindowDeactivated;
            window.Activated += OnWindowActivated;
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (BindingContext is PlayerViewModel vm)
        {
            vm.Unload();
        }

        var window = Application.Current?.Windows.FirstOrDefault();
        if (window != null)
        {
            window.Deactivated -= OnWindowDeactivated;
            window.Activated -= OnWindowActivated;
        }
    }

    private void OnWindowDeactivated(object? sender, EventArgs e)
    {
        if (BindingContext is PlayerViewModel vm)
        {
            vm.StopTimer();
        }
    }

    private void OnWindowActivated(object? sender, EventArgs e)
    {
        if (BindingContext is PlayerViewModel vm && vm.IsPlaying)
        {
            vm.StartTimer();
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
        .Bind(ArtworkImage.ImageIdProperty, $"{nameof(PlayerViewModel.CurrentSong)}.{nameof(ISongModel.ImageId)}");

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
        var blurBackground = new BlurredImageView()
            .BlurRadius(LayoutConstants.BlurRadius)
            .Bind(BlurredImageView.ImageIdProperty, $"{nameof(PlayerViewModel.CurrentSong)}.{nameof(ISongModel.ImageId)}");
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
        }.Bind(Label.TextProperty, $"{nameof(PlayerViewModel.CurrentSong)}.{nameof(ISongModel.Name)}");

        var artistName = new SecondaryText
        {
            FontSize = 16,
            HorizontalTextAlignment = TextAlignment.Center
        }.Bind(Label.TextProperty, $"{nameof(PlayerViewModel.CurrentSong)}.{nameof(ISongModel.ArtistName)}");

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
            MinimumTrackColor = ColorConstants.PrimaryTextColor,
            MaximumTrackColor = ColorConstants.LineColor,
            ThumbColor = ColorConstants.PrimaryTextColor
        }.Bind(Slider.ValueProperty, nameof(PlayerViewModel.CurrentTime));

        slider.DragCompleted += (s, e) =>
        {
            if (BindingContext is PlayerViewModel vm)
            {
                vm.SeekCommand.Execute(slider.Value);
            }
        };

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
        }.Bind(Label.TextProperty, nameof(PlayerViewModel.CurrentTimeText)).Column(0);

        var durationLabel = new SecondaryText
        {
            FontSize = 12,
            HorizontalTextAlignment = TextAlignment.End
        }.Bind(Label.TextProperty, nameof(PlayerViewModel.DurationText)).Column(1);

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
        var previousButton = new PreviousButton((s, e) => (BindingContext as PlayerViewModel)?.PreviousCommand.Execute(null), LayoutConstants.ButtonLarge);
        previousButton.Column(0);

        // Play button
        var playButton = new PlayButton((s, e) => (BindingContext as PlayerViewModel)?.PlayCommand.Execute(null), LayoutConstants.ButtonExtraLarge);
        playButton.Column(1);
        playButton.Bind(IsVisibleProperty, nameof(PlayerViewModel.IsPlaying), converter: new InverseBoolConverter());

        // Pause button
        var pauseButton = new PauseButton((s, e) => (BindingContext as PlayerViewModel)?.PauseCommand.Execute(null), LayoutConstants.ButtonExtraLarge);
        pauseButton.Column(1);
        pauseButton.Bind(IsVisibleProperty, nameof(PlayerViewModel.IsPlaying));

        // Next button
        var nextButton = new NextButton((s, e) => (BindingContext as PlayerViewModel)?.NextCommand.Execute(null), LayoutConstants.ButtonLarge);
        nextButton.Column(2);

        controlsContainer.Children.Add(previousButton);
        controlsContainer.Children.Add(playButton);
        controlsContainer.Children.Add(pauseButton);
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

        // Repeat Off button
        var repeatOffButton = new RepeatOffButton((s, e) => (BindingContext as PlayerViewModel)?.ToggleRepeatCommand.Execute(null));
        repeatOffButton.Column(0);
        repeatOffButton.Bind(IsVisibleProperty, nameof(PlayerViewModel.RepeatMode), converter: new RepeatModeVisibilityConverter(RepeatMode.Off));

        // Repeat All button
        var repeatAllButton = new RepeatAllButton((s, e) => (BindingContext as PlayerViewModel)?.ToggleRepeatCommand.Execute(null));
        repeatAllButton.Column(0);
        repeatAllButton.Bind(IsVisibleProperty, nameof(PlayerViewModel.RepeatMode), converter: new RepeatModeVisibilityConverter(RepeatMode.All));

        // Repeat One button
        var repeatOneButton = new RepeatOneButton((s, e) => (BindingContext as PlayerViewModel)?.ToggleRepeatCommand.Execute(null));
        repeatOneButton.Column(0);
        repeatOneButton.Bind(IsVisibleProperty, nameof(PlayerViewModel.RepeatMode), converter: new RepeatModeVisibilityConverter(RepeatMode.One));

        // Shuffle Off button
        var shuffleOffButton = new ShuffleOffButton((s, e) => (BindingContext as PlayerViewModel)?.ToggleShuffleCommand.Execute(null));
        shuffleOffButton.Column(1);
        shuffleOffButton.Bind(IsVisibleProperty, nameof(PlayerViewModel.IsShuffleOn), converter: new InverseBoolConverter());

        // Shuffle On button
        var shuffleOnButton = new ShuffleOnButton((s, e) => (BindingContext as PlayerViewModel)?.ToggleShuffleCommand.Execute(null));
        shuffleOnButton.Column(1);
        shuffleOnButton.Bind(IsVisibleProperty, nameof(PlayerViewModel.IsShuffleOn));

        // Favorite artist Off button
        var favoriteArtistOffButton = new FavoriteArtistOffButton((s, e) => (BindingContext as PlayerViewModel)?.ToggleFavoriteArtistCommand.Execute(null));
        favoriteArtistOffButton.Column(2);
        favoriteArtistOffButton.Bind(IsVisibleProperty, nameof(PlayerViewModel.IsFavoriteArtist), converter: new InverseBoolConverter());

        // Favorite artist On button
        var favoriteArtistOnButton = new FavoriteArtistOnButton((s, e) => (BindingContext as PlayerViewModel)?.ToggleFavoriteArtistCommand.Execute(null));
        favoriteArtistOnButton.Column(2);
        favoriteArtistOnButton.Bind(IsVisibleProperty, nameof(PlayerViewModel.IsFavoriteArtist));

        // Favorite song Off button
        var favoriteSongOffButton = new FavoriteSongOffButton((s, e) => (BindingContext as PlayerViewModel)?.ToggleFavoriteCommand.Execute(null));
        favoriteSongOffButton.Column(3);
        favoriteSongOffButton.Bind(IsVisibleProperty, nameof(PlayerViewModel.IsFavorite), converter: new InverseBoolConverter());

        // Favorite song On button
        var favoriteSongOnButton = new FavoriteSongOnButton((s, e) => (BindingContext as PlayerViewModel)?.ToggleFavoriteCommand.Execute(null));
        favoriteSongOnButton.Column(3);
        favoriteSongOnButton.Bind(IsVisibleProperty, nameof(PlayerViewModel.IsFavorite));

        // Add to playlist button
        var playlistButton = new PlaylistAddButton((s, e) => (BindingContext as PlayerViewModel)?.AddToPlaylistCommand.Execute(null));
        playlistButton.Column(4);

        secondaryContainer.Children.Add(repeatOffButton);
        secondaryContainer.Children.Add(repeatAllButton);
        secondaryContainer.Children.Add(repeatOneButton);
        secondaryContainer.Children.Add(shuffleOffButton);
        secondaryContainer.Children.Add(shuffleOnButton);
        secondaryContainer.Children.Add(favoriteArtistOffButton);
        secondaryContainer.Children.Add(favoriteArtistOnButton);
        secondaryContainer.Children.Add(favoriteSongOffButton);
        secondaryContainer.Children.Add(favoriteSongOnButton);
        secondaryContainer.Children.Add(playlistButton);

        return secondaryContainer;
    }
}
