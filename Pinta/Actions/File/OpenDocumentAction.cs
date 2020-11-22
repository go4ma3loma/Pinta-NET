using System;
using Pinta.Core;
using Gtk;
using Mono.Unix;

namespace Pinta.Actions
{
	class OpenDocumentAction : IActionHandler
	{
		#region IActionHandler Members
		public void Initialize ()
		{
			PintaCore.Actions.File.Open.Activated += Activated;
		}

		public void Uninitialize ()
		{
			PintaCore.Actions.File.Open.Activated -= Activated;
		}
		#endregion

		private void Activated (object sender, EventArgs e)
		{
			var fcd = new Gtk.FileChooserDialog (Catalog.GetString ("Open Image File"), PintaCore.Chrome.MainWindow,
							    FileChooserAction.Open, Gtk.Stock.Cancel, Gtk.ResponseType.Cancel,
							    Gtk.Stock.Open, Gtk.ResponseType.Ok);

			// Add image files filter
			FileFilter ff = new FileFilter ();
            foreach (var format in PintaCore.System.ImageFormats.Formats) {
				if (!format.IsWriteOnly ()) {
					foreach (var ext in format.Extensions)
						ff.AddPattern (string.Format("*.{0}", ext));
				}
            }

			ff.Name = Catalog.GetString ("Image files");
			fcd.AddFilter (ff);

			FileFilter ff2 = new FileFilter ();
			ff2.Name = Catalog.GetString ("All files");
			ff2.AddPattern ("*.*");
			fcd.AddFilter (ff2);

			fcd.AlternativeButtonOrder = new int[] { (int)ResponseType.Ok, (int)ResponseType.Cancel };
            fcd.SetCurrentFolder (PintaCore.System.GetDialogDirectory ());
			fcd.SelectMultiple = true;

			fcd.AddImagePreview ();

			int response = fcd.Run ();

			if (response == (int)Gtk.ResponseType.Ok) {
				PintaCore.System.LastDialogDirectory = fcd.CurrentFolder;

				foreach (var file in fcd.Filenames)
					if (PintaCore.Workspace.OpenFile (file, fcd))
						RecentManager.Default.AddFull (fcd.Uri, PintaCore.System.RecentData);
			}

			fcd.Destroy ();
		}
	}
}
