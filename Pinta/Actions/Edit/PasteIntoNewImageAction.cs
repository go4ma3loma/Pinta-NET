using System;
using Gtk;
using Mono.Unix;
using Pinta.Core;

namespace Pinta.Actions
{
	class PasteIntoNewImageAction : IActionHandler
	{
		#region IActionHandler Members
		public void Initialize ()
		{
			PintaCore.Actions.Edit.PasteIntoNewImage.Activated += Activated;
		}

		public void Uninitialize ()
		{
			PintaCore.Actions.Edit.PasteIntoNewImage.Activated -= Activated;
		}
		#endregion

		private void Activated (object sender, EventArgs e)
		{
			Gtk.Clipboard cb = Gtk.Clipboard.Get (Gdk.Atom.Intern ("CLIPBOARD", false));

			if (cb.WaitIsImageAvailable ()) {
				using (Gdk.Pixbuf image = cb.WaitForImage ()) {
					if (image != null) {
						Gdk.Size size = new Gdk.Size (image.Width, image.Height);

						PintaCore.Workspace.NewDocument (size, new Cairo.Color (0, 0, 0, 0));
						PintaCore.Actions.Edit.Paste.Activate ();
						PintaCore.Actions.Edit.Deselect.Activate ();
						return;
					}
				}
			}

			Pinta.Core.Document.ShowClipboardEmptyDialog ();
		}
	}
}
