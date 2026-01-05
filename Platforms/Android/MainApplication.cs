using Android.App;
using Android.OS;
using Android.Runtime;
using ThinMPm.Platforms.Android.Audio;

namespace ThinMPm;

[Application]
public class MainApplication : MauiApplication, Android.App.Application.IActivityLifecycleCallbacks
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
	}

	public override void OnCreate()
	{
		base.OnCreate();
		RegisterActivityLifecycleCallbacks(this);
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

	public void OnActivityCreated(Activity activity, Bundle? savedInstanceState) { }

	public void OnActivityStarted(Activity activity) { }

	public void OnActivityResumed(Activity activity) { }

	public void OnActivityPaused(Activity activity) { }

	public void OnActivityStopped(Activity activity) { }

	public void OnActivitySaveInstanceState(Activity activity, Bundle outState) { }

	public void OnActivityDestroyed(Activity activity)
	{
		if (ApplicationContext == null) return;

		MusicPlayer.Dispose(ApplicationContext);
	}
}
