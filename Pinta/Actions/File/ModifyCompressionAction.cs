using System;
using Gtk;
using Mono.Unix;
using Pinta.Core;

namespace Pinta.Actions
{
	class ModifyCompressionAction : IActionHandler
	{
		#region IActionHandler Members
		public void Initialize ()
		{
			PintaCore.Actions.File.ModifyCompression += Activated;
		}

		public void Uninitialize ()
		{
			PintaCore.Actions.File.ModifyCompression -= Activated;
		}
		#endregion

		private void Activated (object sender, ModifyCompressionEventArgs e)
		{
			JpegCompressionDialog dlg = new JpegCompressionDialog (e.Quality, e.ParentWindow);

			try {
				if (dlg.Run () == (int)Gtk.ResponseType.Ok)
					e.Quality = dlg.GetCompressionLevel ();
				else
					e.Cancel = true;
			} finally {
				dlg.Destroy ();
			}
		}
	}
}
