using Gtk;
using Mono.Unix;
using Pinta.Docking;
using Pinta.Core;
using Pinta.Gui.Widgets;

namespace Pinta
{
	public class ColorPalettePad : IDockPad
	{
		public void Initialize (DockFrame workspace, Menu padMenu)
		{
			DockItem palette_item = workspace.AddItem ("Palette");
			ColorPaletteWidget palette = new ColorPaletteWidget () { Name = "palette" };

			palette_item.Label = Catalog.GetString ("Palette");
			palette_item.Content = palette;
			palette_item.Icon = PintaCore.Resources.GetIcon ("Pinta.png");
			palette_item.DefaultLocation = "Toolbox/Bottom";
			palette_item.Behavior |= DockItemBehavior.CantClose;
			palette_item.DefaultWidth = 35;

			Gtk.ToggleAction show_palette = padMenu.AppendToggleAction ("Palette", Catalog.GetString ("Palette"), null, "Pinta.png");
			show_palette.Activated += delegate { palette_item.Visible = show_palette.Active; };
			palette_item.VisibleChanged += delegate { show_palette.Active = palette_item.Visible; };

			palette.Initialize ();
			show_palette.Active = palette_item.Visible;
		}
	}
}
