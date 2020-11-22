using Gtk;
using Mono.Unix;
using Pinta.Docking;
using Pinta.Core;
using Pinta.Gui.Widgets;

namespace Pinta
{
	public class HistoryPad : IDockPad
	{
		public void Initialize (DockFrame workspace, Menu padMenu)
		{
			var history = new HistoryTreeView ();
			DockItem history_item = workspace.AddItem ("History");
			DockItemToolbar history_tb = history_item.GetToolbar (PositionType.Bottom);

			history_item.Label = Catalog.GetString ("History");
			history_item.DefaultLocation = "Images/Bottom";
			history_item.Content = history;
			history_item.Icon = PintaCore.Resources.GetIcon ("Menu.Layers.DuplicateLayer.png");
            history_item.DefaultWidth = 100;
			history_item.Behavior |= DockItemBehavior.CantClose;

			history_tb.Add (PintaCore.Actions.Edit.Undo.CreateDockToolBarItem ());
			history_tb.Add (PintaCore.Actions.Edit.Redo.CreateDockToolBarItem ());
			Gtk.ToggleAction show_history = padMenu.AppendToggleAction ("History", Catalog.GetString ("History"), null, "Menu.Layers.DuplicateLayer.png");
			show_history.Activated += delegate { history_item.Visible = show_history.Active; };
			history_item.VisibleChanged += delegate { show_history.Active = history_item.Visible; };

			show_history.Active = history_item.Visible;
		}
	}
}
