using System;
using Gtk;
using Mono.Unix;
using Pinta.Core;

namespace Pinta.Actions
{
	class LayerPropertiesAction : IActionHandler
	{
		#region IActionHandler Members
		public void Initialize ()
		{
			PintaCore.Actions.Layers.Properties.Activated += Activated;
		}

		public void Uninitialize ()
		{
			PintaCore.Actions.Layers.Properties.Activated -= Activated;
		}
		#endregion

		private void Activated (object sender, EventArgs e)
		{
			var dialog = new LayerPropertiesDialog ();

			int response = dialog.Run ();

			if (response == (int)Gtk.ResponseType.Ok
				&& dialog.AreLayerPropertiesUpdated) {

				var historyMessage = GetLayerPropertyUpdateMessage (
						dialog.InitialLayerProperties,
						dialog.UpdatedLayerProperties);

				var historyItem = new UpdateLayerPropertiesHistoryItem (
					"Menu.Layers.LayerProperties.png",
					historyMessage,
					PintaCore.Layers.CurrentLayerIndex,
					dialog.InitialLayerProperties,
					dialog.UpdatedLayerProperties);

				PintaCore.Workspace.ActiveWorkspace.History.PushNewItem (historyItem);

				PintaCore.Workspace.ActiveWorkspace.Invalidate ();

			} else {

				var layer = PintaCore.Workspace.ActiveDocument.CurrentUserLayer;
				var selectionLayer = PintaCore.Workspace.ActiveDocument.SelectionLayer;
				var initial = dialog.InitialLayerProperties;
				initial.SetProperties (layer);
				if (selectionLayer != null)
					initial.SetProperties (selectionLayer);

				if ((layer.Opacity != initial.Opacity) || (layer.BlendMode != initial.BlendMode) || (layer.Hidden != initial.Hidden)) 
					PintaCore.Workspace.ActiveWorkspace.Invalidate ();
			}

			dialog.Destroy ();
		}

		private string GetLayerPropertyUpdateMessage (LayerProperties initial, LayerProperties updated)
		{

			string ret = null;
			int count = 0;

			if (updated.Opacity != initial.Opacity) {
				ret = Catalog.GetString ("Layer Opacity");
				count++;
			}

			if (updated.Name != initial.Name) {
				ret = Catalog.GetString ("Rename Layer");
				count++;
			}

			if (updated.Hidden != initial.Hidden) {
				ret = (updated.Hidden) ? Catalog.GetString ("Hide Layer") : Catalog.GetString ("Show Layer");
				count++;
			}

			if (ret == null || count > 1)
				ret = Catalog.GetString ("Layer Properties");

			return ret;
		}
	}
}
