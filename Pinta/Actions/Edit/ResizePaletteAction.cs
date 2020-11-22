using System;
using Gtk;
using Mono.Unix;
using Pinta.Core;

namespace Pinta.Actions
{
	class ResizePaletteAction : IActionHandler
	{
		#region IActionHandler Members
		public void Initialize ()
		{
			PintaCore.Actions.Edit.ResizePalette.Activated += Activated;
		}

		public void Uninitialize ()
		{
			PintaCore.Actions.Edit.ResizePalette.Activated -= Activated;
		}
		#endregion

		private void Activated (object sender, EventArgs e)
		{
			SpinButtonEntryDialog dialog = new SpinButtonEntryDialog (Catalog.GetString ("Resize Palette"),
					PintaCore.Chrome.MainWindow, Catalog.GetString ("New palette size:"), 1, 96,
					PintaCore.Palette.CurrentPalette.Count);

			if (dialog.Run () == (int)ResponseType.Ok) {
				PintaCore.Palette.CurrentPalette.Resize (dialog.GetValue ());
			}

			dialog.Destroy ();
		}
	}
}
