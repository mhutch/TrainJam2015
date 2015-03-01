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
			new CCApplication { ApplicationDelegate = new AppDelegate () }.StartGame ();
		}

		public override bool ApplicationShouldTerminateAfterLastWindowClosed (NSApplication sender)
		{
			return true;
		}
	}
}

