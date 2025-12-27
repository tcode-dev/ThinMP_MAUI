using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;
using ThinMPm.Views.Popup;

namespace ThinMPm.ViewModels;

public partial class PlayerPageViewModel : ObservableObject
{
    private readonly IPlayerService _playerService;
    private readonly IFavoriteSongService _favoriteSongService;
    private readonly IFavoriteArtistService _favoriteArtistService;
    private readonly IPreferenceService _preferenceService;
    private readonly IPlaylistService _playlistService;
    private readonly Func<PlaylistPopup> _playlistPopupFactory;
    private IDispatcherTimer? _timer;

    [ObservableProperty]
    private ISongModel? currentSong;

    [ObservableProperty]
    private bool isPlaying;

    [ObservableProperty]
    private double currentTime;

    [ObservableProperty]
    private double duration;

    [ObservableProperty]
    private string currentTimeText = "00:00";

    [ObservableProperty]
    private string durationText = "00:00";

    [ObservableProperty]
    private RepeatMode repeatMode;

    [ObservableProperty]
    private bool isShuffleOn;

    [ObservableProperty]
    private bool isFavorite;

    [ObservableProperty]
    private bool isFavoriteArtist;

    public PlayerPageViewModel(IPlayerService playerService, IFavoriteSongService favoriteSongService, IFavoriteArtistService favoriteArtistService, IPreferenceService preferenceService, IPlaylistService playlistService, Func<PlaylistPopup> playlistPopupFactory)
    {
        _playerService = playerService;
        _favoriteSongService = favoriteSongService;
        _favoriteArtistService = favoriteArtistService;
        _preferenceService = preferenceService;
        _playlistService = playlistService;
        _playlistPopupFactory = playlistPopupFactory;

        _playerService.NowPlayingItemChanged += HandleNowPlayingItemChanged;
        _playerService.IsPlayingChanged += HandleIsPlayingChanged;
    }

    public async void Load()
    {
        var song = _playerService.GetCurrentSong();
        if (song != null)
        {
            CurrentSong = song;
            Duration = song.Duration;
            DurationText = FormatTime(song.Duration);
            IsFavorite = await _favoriteSongService.ExistsAsync(song.Id);
            IsFavoriteArtist = await _favoriteArtistService.ExistsAsync(song.ArtistId);
        }
        IsPlaying = _playerService.GetIsPlaying();
        RepeatMode = _preferenceService.GetRepeatMode();
        IsShuffleOn = _preferenceService.GetShuffleMode() == ShuffleMode.On;
        UpdateCurrentTime();
        if (IsPlaying)
        {
            StartTimer();
        }
    }

    public void Unload()
    {
        StopTimer();
    }

    private void StartTimer()
    {
        if (_timer != null) return;

        _timer = Application.Current?.Dispatcher.CreateTimer();
        if (_timer == null) return;

        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += OnTimerTick;
        _timer.Start();
    }

    private void StopTimer()
    {
        if (_timer == null) return;

        _timer.Stop();
        _timer.Tick -= OnTimerTick;
        _timer = null;
    }

    private void OnTimerTick(object? sender, EventArgs e)
    {
        UpdateCurrentTime();
    }

    private void UpdateCurrentTime()
    {
        _playerService.GetCurrentTime(time =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (Duration > 0)
                {
                    CurrentTime = time / Duration;
                }
                CurrentTimeText = FormatTime(time);
            });
        });
    }

    private async void HandleNowPlayingItemChanged(ISongModel? song)
    {
        CurrentSong = song;
        if (song != null)
        {
            Duration = song.Duration;
            DurationText = FormatTime(song.Duration);
            IsFavorite = await _favoriteSongService.ExistsAsync(song.Id);
            IsFavoriteArtist = await _favoriteArtistService.ExistsAsync(song.ArtistId);
        }
    }

    private void HandleIsPlayingChanged(bool isPlaying)
    {
        IsPlaying = isPlaying;
        if (isPlaying)
        {
            StartTimer();
        }
        else
        {
            StopTimer();
        }
    }

    private static string FormatTime(double seconds)
    {
        var timeSpan = TimeSpan.FromSeconds(seconds);
        return timeSpan.ToString(@"mm\:ss");
    }

    [RelayCommand]
    private void TogglePlayPause()
    {
        if (IsPlaying)
        {
            _playerService.Pause();
        }
        else
        {
            _playerService.Play();
        }
    }

    [RelayCommand]
    private void Previous()
    {
        // TODO: Implement when IPlayerService.Previous is added
    }

    [RelayCommand]
    private void Next()
    {
        _playerService.Next();
    }

    [RelayCommand]
    private void ToggleRepeat()
    {
        RepeatMode = RepeatMode switch
        {
            RepeatMode.Off => RepeatMode.All,
            RepeatMode.All => RepeatMode.One,
            RepeatMode.One => RepeatMode.Off,
            _ => RepeatMode.Off
        };
        _preferenceService.SetRepeatMode(RepeatMode);
        _playerService.SetRepeat(RepeatMode);
    }

    [RelayCommand]
    private void ToggleShuffle()
    {
        IsShuffleOn = !IsShuffleOn;
        var shuffleMode = IsShuffleOn ? ShuffleMode.On : ShuffleMode.Off;
        _preferenceService.SetShuffleMode(shuffleMode);
        _playerService.SetShuffle(shuffleMode);
    }

    [RelayCommand]
    private async Task ToggleFavorite()
    {
        if (CurrentSong == null) return;

        await _favoriteSongService.ToggleAsync(CurrentSong.Id);
        IsFavorite = !IsFavorite;
    }

    [RelayCommand]
    private async Task ToggleFavoriteArtist()
    {
        if (CurrentSong == null) return;

        await _favoriteArtistService.ToggleAsync(CurrentSong.ArtistId);
        IsFavoriteArtist = !IsFavoriteArtist;
    }

    [RelayCommand]
    private async Task AddToPlaylist()
    {
        if (CurrentSong == null) return;

        var page = Application.Current?.Windows.FirstOrDefault()?.Page;
        if (page == null) return;

        var popup = _playlistPopupFactory();
        await page.Navigation.PushModalAsync(popup);
        var result = await popup.ShowAsync();

        if (result != null)
        {
            switch (result.Action)
            {
                case PlaylistPopupAction.Create:
                    if (!string.IsNullOrWhiteSpace(result.PlaylistName))
                    {
                        var playlistId = await _playlistService.CreateAsync(result.PlaylistName);
                        await _playlistService.AddSongAsync(playlistId, CurrentSong.Id);
                    }
                    break;
                case PlaylistPopupAction.Select:
                    if (result.SelectedPlaylist != null)
                    {
                        await _playlistService.AddSongAsync(result.SelectedPlaylist.Id, CurrentSong.Id);
                    }
                    break;
            }
        }
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private void Seek(double value)
    {
        var seekTime = value * Duration;
        _playerService.Seek(seekTime);
        CurrentTime = value;
        CurrentTimeText = FormatTime(seekTime);
    }
}
