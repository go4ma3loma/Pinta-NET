using System;
using Gtk;
using System.Reflection;
using Mono.Unix;

namespace Pinta
{
	internal class VersionInformationTabPage : VBox
	{
		private ListStore data = null;
		private CellRenderer cellRenderer = new CellRendererText ();

		public VersionInformationTabPage ()
		{
			TreeView treeView = new TreeView ();

			TreeViewColumn treeViewColumnTitle = new TreeViewColumn (Catalog.GetString ("Title"), cellRenderer, "text", 0);
			treeViewColumnTitle.FixedWidth = 200;
			treeViewColumnTitle.Sizing = TreeViewColumnSizing.Fixed;
			treeViewColumnTitle.Resizable = true;
			treeView.AppendColumn (treeViewColumnTitle);

			TreeViewColumn treeViewColumnVersion = new TreeViewColumn (Catalog.GetString ("Version"), cellRenderer, "text", 1);
			treeView.AppendColumn (treeViewColumnVersion);

			TreeViewColumn treeViewColumnPath = new TreeViewColumn (Catalog.GetString ("Path"), cellRenderer, "text", 2);
			treeView.AppendColumn (treeViewColumnPath);

			treeView.RulesHint = true;

			data = new ListStore (typeof (string), typeof (string), typeof (string));
			treeView.Model = data;

			ScrolledWindow scrolledWindow = new ScrolledWindow ();
			scrolledWindow.Add (treeView);
			scrolledWindow.ShadowType = ShadowType.In;

			BorderWidth = 6;

			PackStart (scrolledWindow, true, true, 0);

			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies ()) {
				try {
					AssemblyName assemblyName = assembly.GetName ();
					data.AppendValues (assemblyName.Name, assemblyName.Version.ToString (), System.IO.Path.GetFullPath (assembly.Location));
				} catch { }
			}

			data.SetSortColumnId (0, SortType.Ascending);
		}

		protected override void OnDestroyed ()
		{
			if (cellRenderer != null) {
				cellRenderer.Destroy ();
				cellRenderer = null;
			}

			if (data != null) {
				data.Dispose ();
				data = null;
			}

			base.OnDestroyed ();
		}
	}
}
