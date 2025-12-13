namespace ThinMPm.Views.FirstView;

public class GradientOverlay : BoxView
{
    public GradientOverlay()
    {
        var isDark = Application.Current?.RequestedTheme == AppTheme.Dark;
        var gradientColor = isDark ? Colors.Black : Colors.White;

        Background = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(0, 1),
            GradientStops = new GradientStopCollection
            {
                new GradientStop { Color = Colors.Transparent, Offset = 0.0f },
                new GradientStop { Color = Colors.Transparent, Offset = 0.5f },
                new GradientStop { Color = gradientColor, Offset = 1.0f }
            }
        };
    }
}
