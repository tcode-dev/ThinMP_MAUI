using CommunityToolkit.Maui.Markup;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Resources.Strings;
using ThinMPm.ViewModels;
using ThinMPm.Views.Img;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.Popup;

public class PlaylistPopup : ContentPage
{
    private readonly PlaylistPopupViewModel _viewModel;
    private readonly VerticalStackLayout _contentContainer;
    private TaskCompletionSource<PlaylistPopupResult?>? _taskCompletionSource;

    public PlaylistPopup(PlaylistPopupViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = viewModel;

        Shell.SetNavBarIsVisible(this, false);
        BackgroundColor = Color.FromArgb("#80000000");

        _contentContainer = new VerticalStackLayout
        {
            Spacing = 0,
            WidthRequest = 300,
            BackgroundColor = ColorConstants.SecondaryBackgroundColor
        };
        _contentContainer.Clip = new Microsoft.Maui.Controls.Shapes.RoundRectangleGeometry
        {
            CornerRadius = 14,
            Rect = new Rect(0, 0, 300, 400)
        };

        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += OnBackgroundTapped;

        var backgroundLayout = new Grid
        {
            Children =
            {
                new ContentView
                {
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill,
                    GestureRecognizers = { tapGesture }
                },
                new ContentView
                {
                    Content = _contentContainer,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                }
            }
        };

        Content = backgroundLayout;
    }

    public async Task<PlaylistPopupResult?> ShowAsync()
    {
        _taskCompletionSource = new TaskCompletionSource<PlaylistPopupResult?>();

        await _viewModel.LoadAsync();
        UpdateContent();

        return await _taskCompletionSource.Task;
    }

    private void UpdateContent()
    {
        _contentContainer.Children.Clear();

        if (_viewModel.Playlists.Count > 0)
        {
            ShowPlaylistList();
        }
        else
        {
            ShowCreatePlaylist();
        }

        UpdateClipRect();
    }

    private void ShowPlaylistList()
    {
        var header = CreatePlaylistListHeader();
        _contentContainer.Children.Add(header);

        foreach (var playlist in _viewModel.Playlists)
        {
            var row = CreatePlaylistRow(playlist);
            _contentContainer.Children.Add(row);
        }
    }

    private void ShowCreatePlaylist()
    {
        var header = CreateNewPlaylistHeader();
        _contentContainer.Children.Add(header);

        var inputContainer = new VerticalStackLayout
        {
            Padding = new Thickness(LayoutConstants.SpacingMedium),
            Spacing = LayoutConstants.SpacingMedium
        };

        var titleLabel = new PrimaryText
        {
            Text = AppResources.PlaylistName,
            HorizontalTextAlignment = TextAlignment.Center,
            FontSize = 16
        };

        var entry = new Entry
        {
            Placeholder = AppResources.PlaylistEnter,
            BackgroundColor = ColorConstants.PrimaryBackgroundColor,
            TextColor = ColorConstants.PrimaryTextColor,
            PlaceholderColor = ColorConstants.SecondaryTextColor
        };
        entry.SetBinding(Entry.TextProperty, nameof(PlaylistPopupViewModel.PlaylistName));

        inputContainer.Children.Add(titleLabel);
        inputContainer.Children.Add(entry);

        _contentContainer.Children.Add(inputContainer);
    }

    private View CreatePlaylistListHeader()
    {
        var header = new Grid
        {
            Padding = new Thickness(LayoutConstants.SpacingMedium),
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star)
            }
        };

        var newPlaylistButton = new Microsoft.Maui.Controls.Button
        {
            Text = AppResources.PlaylistCreate,
            BackgroundColor = Colors.Transparent,
            TextColor = Colors.DodgerBlue,
            HorizontalOptions = LayoutOptions.Start
        };
        newPlaylistButton.Clicked += OnNewPlaylistClicked;
        Grid.SetColumn(newPlaylistButton, 0);
        header.Children.Add(newPlaylistButton);

        var cancelButton = new Microsoft.Maui.Controls.Button
        {
            Text = AppResources.Cancel,
            BackgroundColor = Colors.Transparent,
            TextColor = Colors.DodgerBlue,
            HorizontalOptions = LayoutOptions.End
        };
        cancelButton.Clicked += OnCancelClicked;
        Grid.SetColumn(cancelButton, 1);
        header.Children.Add(cancelButton);

        return header;
    }

    private View CreateNewPlaylistHeader()
    {
        var header = new Grid
        {
            Padding = new Thickness(LayoutConstants.SpacingMedium),
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star)
            }
        };

        var doneButton = new Microsoft.Maui.Controls.Button
        {
            Text = AppResources.Done,
            BackgroundColor = Colors.Transparent,
            TextColor = Colors.DodgerBlue,
            HorizontalOptions = LayoutOptions.Start
        };
        doneButton.Clicked += OnCreateDoneClicked;
        Grid.SetColumn(doneButton, 0);
        header.Children.Add(doneButton);

        var cancelButton = new Microsoft.Maui.Controls.Button
        {
            Text = AppResources.Cancel,
            BackgroundColor = Colors.Transparent,
            TextColor = Colors.DodgerBlue,
            HorizontalOptions = LayoutOptions.End
        };
        cancelButton.Clicked += OnCreateCancelClicked;
        Grid.SetColumn(cancelButton, 1);
        header.Children.Add(cancelButton);

        return header;
    }

    private View CreatePlaylistRow(IPlaylistModel playlist)
    {
        var container = new Grid
        {
            Padding = new Thickness(LayoutConstants.SpacingMedium, LayoutConstants.SpacingSmall),
            ColumnDefinitions =
            {
                new ColumnDefinition(50),
                new ColumnDefinition(GridLength.Star)
            },
            ColumnSpacing = LayoutConstants.SpacingMedium
        };

        var artwork = new ArtworkImage(4)
        {
            WidthRequest = 50,
            HeightRequest = 50,
            ImageId = playlist.ImageId ?? string.Empty
        };
        Grid.SetColumn(artwork, 0);

        var nameLabel = new PrimaryText
        {
            Text = playlist.Name,
            VerticalTextAlignment = TextAlignment.Center,
            FontSize = 16
        };
        Grid.SetColumn(nameLabel, 1);

        container.Children.Add(artwork);
        container.Children.Add(nameLabel);

        var separator = new BoxView
        {
            HeightRequest = 1,
            Color = ColorConstants.LineColor,
            Margin = new Thickness(LayoutConstants.SpacingMedium + 50 + LayoutConstants.SpacingMedium, 0, 0, 0)
        };

        var stackLayout = new VerticalStackLayout
        {
            Children = { container, separator }
        };

        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += (s, e) => OnPlaylistSelected(playlist);
        stackLayout.GestureRecognizers.Add(tapGesture);

        return stackLayout;
    }

    private void UpdateClipRect()
    {
        _contentContainer.Dispatcher.Dispatch(() =>
        {
            var height = Math.Min(400, _contentContainer.Children.Sum(c => c.Height > 0 ? c.Height : 60));
            _contentContainer.Clip = new Microsoft.Maui.Controls.Shapes.RoundRectangleGeometry
            {
                CornerRadius = 14,
                Rect = new Rect(0, 0, 300, height > 0 ? height : 200)
            };
        });
    }

    private void OnBackgroundTapped(object? sender, TappedEventArgs e)
    {
        ClosePopup(null);
    }

    private void OnNewPlaylistClicked(object? sender, EventArgs e)
    {
        _contentContainer.Children.Clear();
        ShowCreatePlaylist();
        UpdateClipRect();
    }

    private void OnCancelClicked(object? sender, EventArgs e)
    {
        ClosePopup(null);
    }

    private void OnCreateCancelClicked(object? sender, EventArgs e)
    {
        if (_viewModel.Playlists.Count > 0)
        {
            _contentContainer.Children.Clear();
            ShowPlaylistList();
            UpdateClipRect();
        }
        else
        {
            ClosePopup(null);
        }
    }

    private void OnCreateDoneClicked(object? sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(_viewModel.PlaylistName))
        {
            ClosePopup(new PlaylistPopupResult { Action = PlaylistPopupAction.Create, PlaylistName = _viewModel.PlaylistName });
        }
    }

    private void OnPlaylistSelected(IPlaylistModel playlist)
    {
        ClosePopup(new PlaylistPopupResult { Action = PlaylistPopupAction.Select, SelectedPlaylist = playlist });
    }

    private async void ClosePopup(PlaylistPopupResult? result)
    {
        _taskCompletionSource?.TrySetResult(result);
        await Navigation.PopModalAsync();
    }
}

public enum PlaylistPopupAction
{
    Create,
    Select,
    Cancel
}

public class PlaylistPopupResult
{
    public PlaylistPopupAction Action { get; set; }
    public string? PlaylistName { get; set; }
    public IPlaylistModel? SelectedPlaylist { get; set; }
}
