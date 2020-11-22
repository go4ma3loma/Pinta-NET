using System;
using Gtk;
using Cairo;

namespace Pinta.Core
{
	public class PaletteManager
	{
		private Color primary;
		private Color secondary;
		private Palette palette;

		public Color PrimaryColor {
			get { return primary; }
			set {
				if (!primary.Equals (value)) {
					primary = value;
					OnPrimaryColorChanged ();
				}
			}
		}

		public Color SecondaryColor {
			get { return secondary; }
			set {
				if (!secondary.Equals (value)) {
					secondary = value;
					OnSecondaryColorChanged ();
				}
			}
		}
		
		public Palette CurrentPalette {
			get {
				if (palette == null) {
					palette = Palette.GetDefault ();
				}
				
				return palette;
			}
		}
		
		public PaletteManager ()
		{
			PrimaryColor = new Color (0, 0, 0);
			SecondaryColor = new Color (1, 1, 1);
		}

		public void DoKeyRelease (object o, KeyReleaseEventArgs e)
		{
			if (e.Event.Key.ToString().ToUpper() == "X") {
				Color temp = PintaCore.Palette.PrimaryColor;
				PintaCore.Palette.PrimaryColor = PintaCore.Palette.SecondaryColor;
				PintaCore.Palette.SecondaryColor = temp;
			}
		}

		#region Protected Methods
		protected void OnPrimaryColorChanged ()
		{
			if (PrimaryColorChanged != null)
				PrimaryColorChanged.Invoke (this, EventArgs.Empty);
		}

		protected void OnSecondaryColorChanged ()
		{
			if (SecondaryColorChanged != null)
				SecondaryColorChanged.Invoke (this, EventArgs.Empty);
		}
		#endregion
		
		#region Events
		public event EventHandler PrimaryColorChanged;
		public event EventHandler SecondaryColorChanged;
		#endregion
	}
}
