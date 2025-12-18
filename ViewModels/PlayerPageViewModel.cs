using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public partial class PlayerPageViewModel : ObservableObject
{
    private readonly IPlayerService _playerService;

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

    public PlayerPageViewModel(IPlayerService playerService)
    {
        _playerService = playerService;

        _playerService.NowPlayingItemChanged += HandleNowPlayingItemChanged;
        _playerService.IsPlayingChanged += HandleIsPlayingChanged;
    }

    private void HandleNowPlayingItemChanged(ISongModel? song)
    {
        CurrentSong = song;
        if (song != null)
        {
            Duration = song.Duration;
            DurationText = FormatTime(song.Duration);
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
    private void ToggleFavorite()
    {
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
