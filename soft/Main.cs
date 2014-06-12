using System;
using Gtk;

namespace syncsoft
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();

            Raspberry.SendBroadcastDiscoverRaspberrys();
			MainWindow win = new MainWindow ();
            win.Show ();    

			Application.Run ();
		}
	}
}
