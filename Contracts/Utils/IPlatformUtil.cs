namespace ThinMPm.Contracts.Utils;

public interface IPlatformUtil
{
    double GetStatusBarHeight();
    double GetAppBarHeight();
    double GetMainAppBarHeight();
    double GetBottomSafeAreaHeight();
    double GetBottomBarHeight();
}