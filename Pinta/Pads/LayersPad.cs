using Gtk;
using Mono.Unix;
using Pinta.Docking;
using Pinta.Core;
using Pinta.Gui.Widgets;

namespace Pinta
{
	public class LayersPad : IDockPad
	{
		public void Initialize (DockFrame workspace, Menu padMenu)
		{
			var layers = new LayersListWidget ();
			DockItem layers_item = workspace.AddItem ("Layers");
			DockItemToolbar layers_tb = layers_item.GetToolbar (PositionType.Bottom);

			layers_item.Label = Catalog.GetString ("Layers");
			layers_item.Content = layers;
			layers_item.Icon = PintaCore.Resources.GetIcon ("Menu.Layers.MergeLayerDown.png");
            layers_item.DefaultWidth = 100;
			layers_item.Behavior |= DockItemBehavior.CantClose;

			layers_tb.Add (PintaCore.Actions.Layers.AddNewLayer.CreateDockToolBarItem ());
			layers_tb.Add (PintaCore.Actions.Layers.DeleteLayer.CreateDockToolBarItem ());
			layers_tb.Add (PintaCore.Actions.Layers.DuplicateLayer.CreateDockToolBarItem ());
			layers_tb.Add (PintaCore.Actions.Layers.MergeLayerDown.CreateDockToolBarItem ());
			layers_tb.Add (PintaCore.Actions.Layers.MoveLayerUp.CreateDockToolBarItem ());
			layers_tb.Add (PintaCore.Actions.Layers.MoveLayerDown.CreateDockToolBarItem ());

			Gtk.ToggleAction show_layers = padMenu.AppendToggleAction ("Layers", Catalog.GetString ("Layers"), null, "Menu.Layers.MergeLayerDown.png");
			show_layers.Activated += delegate { layers_item.Visible = show_layers.Active; };
			layers_item.VisibleChanged += delegate { show_layers.Active = layers_item.Visible; };

			show_layers.Active = layers_item.Visible;

			PintaCore.Workspace.ActiveDocumentChanged += delegate { layers.Reset (); };
		}
	}
}
