
// This file has been generated by the GUI designer. Do not modify.
namespace WidgetLibrary
{
	public partial class DragDropWidget
	{
		private global::Gtk.EventBox eventbox1;
		private global::Gtk.Image image1;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget WidgetLibrary.DragDropWidget
			global::Stetic.BinContainer.Attach (this);
			this.Name = "WidgetLibrary.DragDropWidget";
			// Container child WidgetLibrary.DragDropWidget.Gtk.Container+ContainerChild
			this.eventbox1 = new global::Gtk.EventBox ();
			this.eventbox1.Name = "eventbox1";
			this.eventbox1.BorderWidth = ((uint)(3));
			// Container child eventbox1.Gtk.Container+ContainerChild
			this.image1 = new global::Gtk.Image ();
			this.image1.Name = "image1";
			this.image1.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("WidgetLibrary.dragAndDropBox.png");
			this.eventbox1.Add (this.image1);
			this.Add (this.eventbox1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
		}
	}
}
