using MonoMac.AppKit;
using CocosSharp;


namespace TrainJam2015.Mac
{
	class Program : NSApplicationDelegate
	{
		static void Main (string[] args)
		{
			NSApplication.Init ();

			var application = NSApplication.SharedApplication;
			application.Delegate = new Program ();
			application.Run ();
		}

		public override void DidFinishLaunching (MonoMac.Foundation.NSNotification notification)
		{
			var application = new CCApplication (false, new CCSize (1024f, 768f)) {
				ApplicationDelegate = new AppDelegate ()
			};
			application.StartGame();
		}

		public override bool ApplicationShouldTerminateAfterLastWindowClosed (NSApplication sender)
		{
			return true;
		}
	}
}

