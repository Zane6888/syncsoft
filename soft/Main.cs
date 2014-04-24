using System;
using Gtk;

namespace syncsoft
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			RSelectionWindow win = new RSelectionWindow ();
			win.Show ();
			Application.Run ();
		}
	}
}
