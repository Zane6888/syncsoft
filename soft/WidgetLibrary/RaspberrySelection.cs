using System;
using Gtk;

namespace WidgetLibrary
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class RaspberrySelection : Gtk.Bin
	{
		public RaspberrySelection ()
		{
			this.Build ();

			#region TreeView füllen
			// Create a column for the date name
			Gtk.TreeViewColumn nameColumn = new Gtk.TreeViewColumn ();
			nameColumn.Title = "Name";
			
			// Create the text cell that will display the name
			Gtk.CellRendererText nameNameCell = new Gtk.CellRendererText ();
			nameColumn.PackStart (nameNameCell, true); // Add the cell to the column
			
			// Create a column for the ip 
			Gtk.TreeViewColumn ipColumn = new Gtk.TreeViewColumn ();
			ipColumn.Title = "IP - Address";
			Gtk.CellRendererText ipTitleCell = new Gtk.CellRendererText ();
			ipColumn.PackStart (ipTitleCell, true);
			
			// Create a column for the mac 
			Gtk.TreeViewColumn macColumn = new Gtk.TreeViewColumn ();
			macColumn.Title = "MAC - Address";
			Gtk.CellRendererText macTitleCell = new Gtk.CellRendererText ();
			macColumn.PackStart (macTitleCell, true); 

			// Create a column for the protocol 
			Gtk.TreeViewColumn protocolColumn = new Gtk.TreeViewColumn ();
			protocolColumn.Title = "Protocol";
			Gtk.CellRendererText protocolTitleCell = new Gtk.CellRendererText ();
			protocolColumn.PackStart (protocolTitleCell, true);
			
			// Create a column for the date of the last connection  
			Gtk.TreeViewColumn lastconColumn = new Gtk.TreeViewColumn ();
			lastconColumn.Title = "last connect";
			Gtk.CellRendererText lastconTitleCell = new Gtk.CellRendererText ();
			lastconColumn.PackStart (lastconTitleCell, true);


			// Add the columns to the TreeView
			devicesTreeView.AppendColumn (nameColumn);
			devicesTreeView.AppendColumn (ipColumn);
			devicesTreeView.AppendColumn (macColumn);
			devicesTreeView.AppendColumn (protocolColumn);
			devicesTreeView.AppendColumn (lastconColumn);


			// Tell the Cell Renderers which items in the model to display
			nameColumn.AddAttribute (nameNameCell, "text", 0);
			ipColumn.AddAttribute (ipTitleCell, "text", 1);
			macColumn.AddAttribute(macTitleCell, "text", 2);
			protocolColumn.AddAttribute(protocolTitleCell, "text", 3);
			lastconColumn.AddAttribute(lastconTitleCell, "text", 4);
			
			Gtk.ListStore raspberryListStore = new Gtk.ListStore (typeof (string), typeof (string), typeof (string), typeof (string), typeof (string));
			
			// Add some data to the store
			raspberryListStore.AppendValues ("Name", "IP", "MAC", "Protocol", "lastConnection");
			raspberryListStore.AppendValues ("Name", "IP", "MAC", "Protocol", "letzte Verbindung");


			// Assign the model to the TreeView
			devicesTreeView.Model = raspberryListStore;
			#endregion
		}

		protected void OnConnectButtonClicked (object sender, EventArgs e)
		{
			ListStore dataList = OnDevicesTreeViewCursorChanged();
			//SIMON --> hier hast du deinen Bereich! :-)
		}

		protected void OnDevicesTreeViewCursorChanged (object sender, EventArgs e) // current selected Raspberry
		{
			TreeSelection selection = (sender as TreeView).Selection;
			TreeModel model;
			TreeIter iter;

			ListStore dataLS = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string), typeof(string));

			// THE ITER WILL POINT TO THE SELECTED ROW
			if (selection.GetSelected (out model, out iter)) {
				string name = (model.GetValue (iter, 0).ToString ());
				string ip = (model.GetValue (iter, 1).ToString ());
				string mac = (model.GetValue (iter, 2).ToString ());
				string protocol = (model.GetValue (iter, 3).ToString ());
				string lastConn = (model.GetValue (iter, 4).ToString ());
		
				dataLS.AppendValues(name);
				dataLS.AppendValues(ip);
				dataLS.AppendValues(mac);
				dataLS.AppendValues(protocol);
				dataLS.AppendValues(lastConn);
			}

			return dataLS;
		}
	}
}

