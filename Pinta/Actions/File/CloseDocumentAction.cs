using System;
using Gtk;
using Mono.Unix;
using Pinta.Core;

namespace Pinta.Actions
{
	class CloseDocumentAction : IActionHandler
	{
		private const string markup = "<span weight=\"bold\" size=\"larger\">{0}</span>\n\n{1}";

		#region IActionHandler Members
		public void Initialize ()
		{
			PintaCore.Actions.File.Close.Activated += Activated;
		}

		public void Uninitialize ()
		{
			PintaCore.Actions.File.Close.Activated -= Activated;
		}
		#endregion

		private void Activated (object sender, EventArgs e)
		{
			// Commit any pending changes
			PintaCore.Tools.Commit ();

			// If it's not dirty, just close it
			if (!PintaCore.Workspace.ActiveDocument.IsDirty) {
				PintaCore.Workspace.CloseActiveDocument ();
				return;
			}

			var primary = Catalog.GetString ("Save the changes to image \"{0}\" before closing?");
			var secondary = Catalog.GetString ("If you don't save, all changes will be permanently lost.");
			var message = string.Format (markup, primary, secondary);

			var md = new MessageDialog (PintaCore.Chrome.MainWindow, DialogFlags.Modal,
						    MessageType.Warning, ButtonsType.None, true,
						    message, System.IO.Path.GetFileName (PintaCore.Workspace.ActiveDocument.Filename));

			// Use the standard button order for each OS.
			if (PintaCore.System.OperatingSystem == OS.Windows) {
				md.AddButton (Stock.Save, ResponseType.Yes);
				md.AddButton (Catalog.GetString ("Close _without saving"), ResponseType.No);
				md.AddButton (Stock.Cancel, ResponseType.Cancel);
			}
			else {
				md.AddButton (Catalog.GetString ("Close _without saving"), ResponseType.No);
				md.AddButton (Stock.Cancel, ResponseType.Cancel);
				md.AddButton (Stock.Save, ResponseType.Yes);
			}

			// so that user won't accidentally overwrite
			md.DefaultResponse = ResponseType.Cancel;

			ResponseType response = (ResponseType)md.Run ();
			md.Destroy ();

			if (response == ResponseType.Yes) {
				PintaCore.Workspace.ActiveDocument.Save (false);

				// If the image is still dirty, the user
				// must have cancelled the Save dialog
				if (!PintaCore.Workspace.ActiveDocument.IsDirty)
					PintaCore.Workspace.CloseActiveDocument ();
			} else if (response == ResponseType.No) {
				PintaCore.Workspace.CloseActiveDocument ();
			}
		}
	}
}
