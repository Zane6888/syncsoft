using System;
using System.Drawing;
using System.Drawing.Design;
using Gdk;

namespace WidgetLibrary
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class RaspberrySelect : Gtk.Bin
	{
		public RaspberrySelect ()
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
			macColumn.PackStart (protocolTitleCell, true); 

			// Create a column for the date of the last connection  
			Gtk.TreeViewColumn lastconColumn = new Gtk.TreeViewColumn ();
			lastconColumn.Title = "last connect";
			Gtk.CellRendererText lastconTitleCell = new Gtk.CellRendererText ();
			lastconColumn.PackStart (lastconTitleCell, true);
			
			/* TODO merge bugs
			// Add the columns to the TreeView
			devicesTreeView.AppendColumn (nameColumn);
			devicesTreeView.AppendColumn (ipColumn);
			devicesTreeView.AppendColumn (macColumn);
			devicesTreeView.AppendColumn (protocolColumn);
			devicesTreeView.AppendColumn (lastconColumn);
			*/

            
			// Tell the Cell Renderers which items in the model to display
			nameColumn.AddAttribute (nameNameCell, "text", 0);
			ipColumn.AddAttribute (ipTitleCell, "text", 1);
			macColumn.AddAttribute(macTitleCell, "text", 2);
			protocolColumn.AddAttribute(protocolTitleCell, "text", 2);
			lastconColumn.AddAttribute(lastconTitleCell, "text", 2);
			
			
			
			
//			// Create a model that will hold two strings - Artist Name and Song Title
//			Gtk.ListStore stempsListStore = new Gtk.ListStore (typeof (string), typeof (string), typeof (string), typeof (string), typeof (string));
//			
//			// Add some data to the store
//			stempsListStore.AppendValues ("Datum", "Beschreibung", "Dauer", "Preis", "Bearbeiter");
//			
//			
//			List<String[]> stempsDetail = MainClass.connection.readStemps(1); // PARAMETER neu einfügen!
//			
//			foreach (string[] s in stempsDetail) {
//				stempsListStore.AppendValues (s[0], s[1], s[2], s[3], s[4]);
//			}
//			
//			
//			
//			// Assign the model to the TreeView
//			devicesTreeView.Model = stempsListStore;
			
			
#endregion

		}
	}
}

