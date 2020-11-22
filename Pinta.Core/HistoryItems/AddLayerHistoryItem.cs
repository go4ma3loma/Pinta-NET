using System;

namespace Pinta.Core
{
	public class AddLayerHistoryItem : BaseHistoryItem
	{
		private int layer_index;
		private UserLayer layer;

		public AddLayerHistoryItem (string icon, string text, int newLayerIndex) : base (icon, text)
		{
			layer_index = newLayerIndex;
		}

		public override void Undo ()
		{
			// Store the layer for "redo"
			layer = PintaCore.Layers[layer_index];
			
			PintaCore.Layers.DeleteLayer (layer_index, false);
		}

		public override void Redo ()
		{
			PintaCore.Layers.Insert (layer, layer_index);

			// Make new layer the current layer
			PintaCore.Layers.SetCurrentLayer (layer);

			layer = null;
		}

		public override void Dispose ()
		{
			if (layer != null)
				(layer.Surface as IDisposable).Dispose ();
		}
	}
}
