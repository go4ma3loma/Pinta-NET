using System;

namespace Pinta.Core
{

	public class LayerProperties
	{
		
		public LayerProperties (string name, bool hidden, double opacity, BlendMode blendmode)
		{
			this.Opacity = opacity;			
			this.Hidden = hidden;
			this.Name = name;
			this.BlendMode = blendmode;
		}
				
		public string Name { get; private set; }				
		public bool Hidden { get; private set; }				
		public double Opacity { get; private set; }
		public BlendMode BlendMode { get; private set; }

		public void SetProperties (Layer layer)
		{
			layer.Name = Name;
			layer.Opacity = Opacity;
			layer.Hidden = Hidden;
			layer.BlendMode = BlendMode;
		}
	}
}
