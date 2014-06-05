using System;
using Gtk;

namespace WidgetLibrary
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class ChoosenFilesWidget : Gtk.Bin
	{
		public ChoosenFilesWidget ()
		{
			this.Build ();

			TreeViewColumn SelectedCollumn = new TreeViewColumn ();
			SelectedCollumn.Title = "";

			TreeViewColumn FilenameCollumn = new TreeViewColumn ();
			FilenameCollumn.Title = "Filename";

			TreeViewColumn PathCollumn = new TreeViewColumn ();
			PathCollumn.Title = "Filepath";


			FileTreeView.AppendColumn (SelectedCollumn);
			FileTreeView.AppendColumn (FilenameCollumn);
			FileTreeView.AppendColumn (PathCollumn);

			CellRendererToggle cell = new CellRendererToggle ();
			cell.Activatable = true;
			SelectedCollumn.PackStart = false;
			SelectedCollumn.SetAttributes (cell, 1);

		}
	}
}

