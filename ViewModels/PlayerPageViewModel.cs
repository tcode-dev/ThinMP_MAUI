using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public partial class PlayerPageViewModel : ObservableObject
{
    private readonly IPlayerService _playerService;
    private readonly IFavoriteSongService _favoriteSongService;
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
    private bool isRepeatOn;

    [ObservableProperty]
    private bool isShuffleOn;

    [ObservableProperty]
    private bool isFavorite;

    public PlayerPageViewModel(IPlayerService playerService, IFavoriteSongService favoriteSongService)
    {
        _playerService = playerService;
        _favoriteSongService = favoriteSongService;

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
        }
        IsPlaying = _playerService.GetIsPlaying();
        UpdateCurrentTime();
        StartTimer();
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
        if (IsPlaying)
        {
            UpdateCurrentTime();
        }
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
        }
    }

    private void HandleIsPlayingChanged(bool isPlaying)
    {
        IsPlaying = isPlaying;
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
        IsRepeatOn = !IsRepeatOn;
    }

    [RelayCommand]
    private void ToggleShuffle()
    {
        IsShuffleOn = !IsShuffleOn;
    }

    [RelayCommand]
    private async Task ToggleFavorite()
    {
        if (CurrentSong == null) return;

        await _favoriteSongService.ToggleAsync(CurrentSong.Id);
        IsFavorite = !IsFavorite;
    }

    [RelayCommand]
    private void GoToArtist()
    {
        // TODO: Navigate to artist detail
    }

    [RelayCommand]
    private void AddToPlaylist()
    {
        // TODO: Add to playlist
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }
}
