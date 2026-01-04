using CommunityToolkit.Maui.Markup;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Resources.Strings;
using ThinMPm.ViewModels;
using ThinMPm.Views.List;
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
            Padding = new Thickness(0, 0, LayoutConstants.SpacingLarge, LayoutConstants.SpacingLarge)
        };

        var border = new Border
        {
            Content = _contentContainer,
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 14 },
            StrokeThickness = 0,
            VerticalOptions = LayoutOptions.Center
        };
        border.SetAppThemeColor(Border.BackgroundColorProperty, ColorConstants.SecondaryBackgroundColorLight, ColorConstants.SecondaryBackgroundColorDark);
        Grid.SetColumn(border, 1);

        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += OnBackgroundTapped;

        var backgroundLayout = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(15, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(70, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(15, GridUnitType.Star) }
            },
            Children =
            {
                new ContentView
                {
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill,
                    GestureRecognizers = { tapGesture }
                }.Column(0).ColumnSpan(3),
                border
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
    }

    private void ShowPlaylistList()
    {
        var header = CreatePlaylistListHeader();
        _contentContainer.Children.Add(header);

        var playlistList = new PlaylistList(OnPlaylistTapped);
        playlistList.ItemsSource = _viewModel.Playlists;
        _contentContainer.Children.Add(playlistList);
    }

    private void ShowCreatePlaylist()
    {
        var header = CreateNewPlaylistHeader();
        _contentContainer.Children.Add(header);

        var inputContainer = new VerticalStackLayout
        {
            Padding = new Thickness(LayoutConstants.SpacingLarge, 0, 0, 0),
            Spacing = LayoutConstants.SpacingLarge
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
        };
        entry.SetAppThemeColor(Entry.BackgroundColorProperty, ColorConstants.PrimaryBackgroundColorLight, ColorConstants.PrimaryBackgroundColorDark);
        entry.SetAppThemeColor(Entry.TextColorProperty, ColorConstants.PrimaryTextColorLight, ColorConstants.PrimaryTextColorDark);
        entry.SetAppThemeColor(Entry.PlaceholderColorProperty, ColorConstants.SecondaryTextColorLight, ColorConstants.SecondaryTextColorDark);
        entry.SetBinding(Entry.TextProperty, nameof(PlaylistPopupViewModel.PlaylistName));

        inputContainer.Children.Add(titleLabel);
        inputContainer.Children.Add(entry);

        _contentContainer.Children.Add(inputContainer);
    }

    private View CreatePlaylistListHeader()
    {
        var header = new FlexLayout
        {
            JustifyContent = Microsoft.Maui.Layouts.FlexJustify.SpaceAround,
            AlignItems = Microsoft.Maui.Layouts.FlexAlignItems.Center,
            HeightRequest = LayoutConstants.HeightLarge
        };

        var newPlaylistButton = new Microsoft.Maui.Controls.Button
        {
            Text = AppResources.PlaylistCreate,
            BackgroundColor = Colors.Transparent,
            TextColor = Colors.DodgerBlue
        };
        newPlaylistButton.Clicked += OnNewPlaylistClicked;
        header.Children.Add(newPlaylistButton);

        var cancelButton = new Microsoft.Maui.Controls.Button
        {
            Text = AppResources.Cancel,
            BackgroundColor = Colors.Transparent,
            TextColor = Colors.DodgerBlue
        };
        cancelButton.Clicked += OnCancelClicked;
        header.Children.Add(cancelButton);

        return header;
    }

    private View CreateNewPlaylistHeader()
    {
        var header = new FlexLayout
        {
            JustifyContent = Microsoft.Maui.Layouts.FlexJustify.SpaceAround,
            AlignItems = Microsoft.Maui.Layouts.FlexAlignItems.Center,
            HeightRequest = LayoutConstants.HeightLarge
        };

        var doneButton = new Microsoft.Maui.Controls.Button
        {
            Text = AppResources.Done,
            BackgroundColor = Colors.Transparent,
            TextColor = Colors.DodgerBlue
        };
        doneButton.Clicked += OnCreateDoneClicked;
        header.Children.Add(doneButton);

        var cancelButton = new Microsoft.Maui.Controls.Button
        {
            Text = AppResources.Cancel,
            BackgroundColor = Colors.Transparent,
            TextColor = Colors.DodgerBlue
        };
        cancelButton.Clicked += OnCreateCancelClicked;
        header.Children.Add(cancelButton);

        return header;
    }

    private void OnPlaylistTapped(object? sender, TappedEventArgs e)
    {
        if (sender is BindableObject bindable && bindable.BindingContext is IPlaylistModel playlist)
        {
            ClosePopup(new PlaylistPopupResult { Action = PlaylistPopupAction.Select, SelectedPlaylist = playlist });
        }
    }

    private void OnBackgroundTapped(object? sender, TappedEventArgs e)
    {
        ClosePopup(null);
    }

    private void OnNewPlaylistClicked(object? sender, EventArgs e)
    {
        _contentContainer.Children.Clear();
        ShowCreatePlaylist();
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
