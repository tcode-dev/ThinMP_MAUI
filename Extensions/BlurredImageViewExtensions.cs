using ThinMPm.Views.Img;

namespace ThinMPm.Extensions;

public static class BlurredImageViewExtensions
{
    public static TBlurredImageView BlurRadius<TBlurredImageView>(this TBlurredImageView view, float radius)
        where TBlurredImageView : BlurredImageView
    {
        view.BlurRadius = radius;
        return view;
    }
}
