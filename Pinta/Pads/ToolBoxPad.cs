using Gtk;
using Mono.Unix;
using Pinta.Docking;
using Pinta.Core;
using Pinta.Gui.Widgets;

namespace Pinta
{
	public class ToolBoxPad : IDockPad
	{
		public void Initialize (DockFrame workspace, Menu padMenu)
		{
			DockItem toolbox_item = workspace.AddItem ("Toolbox");
			ToolBoxWidget toolbox = new ToolBoxWidget () { Name = "toolbox" };

			toolbox_item.Label = Catalog.GetString ("Tools");
			toolbox_item.Content = toolbox;
			toolbox_item.Icon = PintaCore.Resources.GetIcon ("Tools.Pencil.png");
			toolbox_item.Behavior |= DockItemBehavior.CantClose;
			toolbox_item.DefaultWidth = 35;
            

			Gtk.ToggleAction show_toolbox = padMenu.AppendToggleAction ("Tools", Catalog.GetString ("Tools"), null, "Tools.Pencil.png");
			show_toolbox.Activated += delegate { toolbox_item.Visible = show_toolbox.Active; };
			toolbox_item.VisibleChanged += delegate { show_toolbox.Active = toolbox_item.Visible; };

			show_toolbox.Active = toolbox_item.Visible;
		}
	}
}
