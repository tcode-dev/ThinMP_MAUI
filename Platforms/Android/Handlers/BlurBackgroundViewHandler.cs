using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using ThinMPm.Constants;
using ThinMPm.Views.Background;
using AndroidView = Android.Views.View;

namespace ThinMPm.Platforms.Android.Handlers;

public class BlurBackgroundViewHandler : ViewHandler<BlurBackgroundView, AndroidView>
{
    public static IPropertyMapper<BlurBackgroundView, BlurBackgroundViewHandler> PropertyMapper =
        new PropertyMapper<BlurBackgroundView, BlurBackgroundViewHandler>(ViewHandler.ViewMapper);

    public BlurBackgroundViewHandler() : base(PropertyMapper)
    {
    }

    protected override AndroidView CreatePlatformView()
    {
        var view = new AndroidView(Context);
        var backgroundColor = ColorConstants.GetSecondaryBackgroundColor();
        view.SetBackgroundColor(backgroundColor.ToPlatform());
        return view;
    }
}
