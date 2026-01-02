namespace ThinMPm.Services;

public class DisplayInfoService
{
    public event EventHandler? DisplayInfoChanged;

    private DisplayOrientation _currentOrientation;

    public DisplayInfoService()
    {
        _currentOrientation = DeviceDisplay.MainDisplayInfo.Orientation;
        DeviceDisplay.MainDisplayInfoChanged += OnMainDisplayInfoChanged;
    }

    private void OnMainDisplayInfoChanged(object? sender, DisplayInfoChangedEventArgs e)
    {
        if (e.DisplayInfo.Orientation == _currentOrientation) return;

        _currentOrientation = e.DisplayInfo.Orientation;
        DisplayInfoChanged?.Invoke(this, EventArgs.Empty);
    }
}
