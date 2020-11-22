using System;
using Gtk;
using Mono.Unix;

namespace Pinta.Core
{
	public class AddinActions
	{
		private Menu addins_menu;

		public Gtk.Action AddinManager { get; private set; }

		public AddinActions ()
		{
			AddinManager = new Gtk.Action ("AddinManager", Catalog.GetString ("Add-in Manager"),
			                               null, "Menu.Edit.Addins.png");
		}

		/// <summary>
		/// Adds a new item to the Add-ins menu.
		/// </summary>
		public void AddMenuItem (Widget item)
		{
			addins_menu.Add (item);
		}

		/// <summary>
		/// Removes an item from the Add-ins menu.
		/// </summary>
		public void RemoveMenuItem (Widget item)
		{
			addins_menu.Remove (item);
		}

		#region Initialization
		public void CreateMainMenu (Gtk.Menu menu)
		{
			addins_menu = menu;

			menu.Append (AddinManager.CreateMenuItem ());
			menu.AppendSeparator ();
		}
		#endregion
	}
}

