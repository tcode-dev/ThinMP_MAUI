using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ThinMPm.Contracts.Models;
using ThinMPm.Contracts.Services;

namespace ThinMPm.ViewModels;

public partial class PlayerViewModel : ObservableObject
{
    private readonly IPlayerService _playerService;

    [ObservableProperty]
    private ISongModel? currentSong;

    [ObservableProperty]
    private bool isPlaying;

    [ObservableProperty]
    private bool isActive;

    public PlayerViewModel(IPlayerService playerService)
    {
        _playerService = playerService;

        _playerService.NowPlayingItemChanged += HandleNowPlayingItemChanged;
        _playerService.IsPlayingChanged += HandleIsPlayingChanged;
    }

    private void HandleNowPlayingItemChanged(ISongModel? song)
    {
        CurrentSong = song;
    }

    private void HandleIsPlayingChanged(bool isPlaying)
    {
        IsPlaying = isPlaying;
        IsActive = true;
    }

    [RelayCommand]
    private void Play()
    {
        _playerService.Play();
    }

    [RelayCommand]
    private void Pause()
    {
        _playerService.Pause();
    }

    [RelayCommand]
    private void Next()
    {
        _playerService.Next();
    }
}
