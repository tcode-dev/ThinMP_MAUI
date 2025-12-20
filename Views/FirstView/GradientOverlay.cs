using ThinMPm.Constants;

namespace ThinMPm.Views.FirstView;

public class GradientOverlay : BoxView
{
    public GradientOverlay()
    {
        var gradientColor = ColorConstants.GradientColor;
        Background = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(0, 1),
            GradientStops = new GradientStopCollection
            {
                new GradientStop { Color = gradientColor.WithAlpha(0), Offset = 0.0f },
                new GradientStop { Color = gradientColor.WithAlpha(0), Offset = 0.5f },
                new GradientStop { Color = gradientColor, Offset = 1.0f }
            }
        };
    }
}
