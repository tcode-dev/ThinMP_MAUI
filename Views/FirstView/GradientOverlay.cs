using ThinMPm.Constants;

namespace ThinMPm.Views.FirstView;

public class GradientOverlay : BoxView
{
    public GradientOverlay()
    {
        Background = new LinearGradientBrush
        {
            StartPoint = new Point(0, 0),
            EndPoint = new Point(0, 1),
            GradientStops = new GradientStopCollection
            {
                new GradientStop { Color = Colors.Transparent, Offset = 0.0f },
                new GradientStop { Color = Colors.Transparent, Offset = 0.5f },
                new GradientStop { Color = ColorConstants.GetGradientColor(), Offset = 1.0f }
            }
        };
    }
}
