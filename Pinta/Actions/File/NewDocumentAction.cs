using System;
using Pinta.Core;

namespace Pinta.Actions
{
	class NewDocumentAction : IActionHandler
	{
		#region IActionHandler Members
		public void Initialize ()
		{
			PintaCore.Actions.File.New.Activated += Activated;
		}

		public void Uninitialize ()
		{
			PintaCore.Actions.File.New.Activated -= Activated;
		}
		#endregion

		private void Activated (object sender, EventArgs e)
		{
			int imgWidth = 0;
			int imgHeight = 0;
			var bg_type = NewImageDialog.BackgroundType.White;
            var using_clipboard = true;
			
			// Try to get the dimensions of an image on the clipboard
			// for the initial width and height values on the NewImageDialog
			if (!GetClipboardImageSize (out imgWidth, out imgHeight))
			{
				// An image was not on the clipboard,
				// so use saved dimensions from settings
				imgWidth = PintaCore.Settings.GetSetting<int> ("new-image-width", 800);
				imgHeight = PintaCore.Settings.GetSetting<int> ("new-image-height", 600);
				bg_type = PintaCore.Settings.GetSetting<NewImageDialog.BackgroundType> (
					"new-image-bg", NewImageDialog.BackgroundType.White);
                using_clipboard = false;
            }

			var dialog = new NewImageDialog (imgWidth, imgHeight, bg_type, using_clipboard);

			int response = dialog.Run ();

			if (response == (int)Gtk.ResponseType.Ok) {
				PintaCore.Workspace.NewDocument (new Gdk.Size (dialog.NewImageWidth, dialog.NewImageHeight), dialog.NewImageBackground);

				PintaCore.Settings.PutSetting ("new-image-width", dialog.NewImageWidth);
				PintaCore.Settings.PutSetting ("new-image-height", dialog.NewImageHeight);
				PintaCore.Settings.PutSetting ("new-image-bg", dialog.NewImageBackgroundType);
				PintaCore.Settings.SaveSettings ();
			}

			dialog.Destroy ();
		}

		/// <summary>
		/// Gets the width and height of an image on the clipboard,
		/// if available.
		/// </summary>
		/// <param name="width">Destination for the image width.</param>
		/// <param name="height">Destination for the image height.</param>
		/// <returns>True if dimensions were available, false otherwise.</returns>
		private static bool GetClipboardImageSize (out int width, out int height)
		{
			bool clipboardUsed = false;
			width = height = 0;

			Gtk.Clipboard cb = Gtk.Clipboard.Get (Gdk.Atom.Intern ("CLIPBOARD", false));
			if (cb.WaitIsImageAvailable ())
			{
				Gdk.Pixbuf image = cb.WaitForImage ();
				if (image != null)
				{
					clipboardUsed = true;
					width = image.Width;
					height = image.Height;
					image.Dispose ();
				}
			}

			cb.Dispose ();

			return clipboardUsed;
		}
	}
}
