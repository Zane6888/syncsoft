using System;

namespace syncsoft
{
	public partial class RSelectionWindow : Gtk.Window
	{
		public RSelectionWindow () : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
		}
	}
}

