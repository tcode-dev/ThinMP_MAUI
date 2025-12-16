using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using ThinMPm.Constants;
using ThinMPm.Views.Background;
using AView = Android.Views.View;

namespace ThinMPm.Platforms.Android.Handlers;

public class BlurBackgroundViewHandler : ViewHandler<BlurBackgroundView, AView>
{
    public static IPropertyMapper<BlurBackgroundView, BlurBackgroundViewHandler> PropertyMapper =
        new PropertyMapper<BlurBackgroundView, BlurBackgroundViewHandler>(ViewHandler.ViewMapper);

    public BlurBackgroundViewHandler() : base(PropertyMapper)
    {
    }

    protected override AView CreatePlatformView()
    {
        var view = new AView(Context);
        var backgroundColor = ColorConstants.GetBackgroundColor();
        view.SetBackgroundColor(backgroundColor.ToPlatform());
        return view;
    }
}
