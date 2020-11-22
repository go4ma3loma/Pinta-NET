using System;

namespace Pinta.Core
{
	public class DeleteLayerHistoryItem : BaseHistoryItem
	{
		private int layer_index;
		private UserLayer layer;

		public DeleteLayerHistoryItem(string icon, string text, UserLayer layer, int layerIndex) : base(icon, text)
		{
			layer_index = layerIndex;
			this.layer = layer;
		}

		public override void Undo ()
		{
			PintaCore.Layers.Insert (layer, layer_index);

			// Make new layer the current layer
			PintaCore.Layers.SetCurrentLayer (layer);

			layer = null;
		}

		public override void Redo ()
		{
			// Store the layer for "undo"
			layer = PintaCore.Layers[layer_index];
			
			PintaCore.Layers.DeleteLayer (layer_index, false);
		}

		public override void Dispose ()
		{
			if (layer != null)
				(layer.Surface as IDisposable).Dispose ();
		}
	}
}
