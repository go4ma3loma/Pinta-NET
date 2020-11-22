using System;
using Gtk;
using Pinta.Docking;

namespace Pinta
{
	public static class GtkExtensions
	{
		public static DockToolButton CreateDockToolBarItem (this Gtk.Action action)
		{
			DockToolButton item = new DockToolButton (action.StockId);
			action.ConnectProxy (item);
			
			item.Show ();
			item.TooltipText = action.Label;
			item.Label = string.Empty;
			item.Image.Show ();
			
			return item;
		}

		public static Gtk.ToolItem CreateToolBarItem (this Gtk.Action action)
		{
			Gtk.ToolItem item = (Gtk.ToolItem)action.CreateToolItem ();
			item.TooltipText = action.Label;
			return item;
		}

		public static void AppendItem (this Toolbar tb, ToolItem item)
		{
			item.Show ();
			tb.Insert (item, tb.NItems);
		}

	}
}
