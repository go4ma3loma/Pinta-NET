using System;
using Gtk;
using Mono.Unix;
using Pinta.Core;

namespace Pinta.Actions
{
	class PasteAction : IActionHandler
	{
		#region IActionHandler Members
		public void Initialize ()
		{
			PintaCore.Actions.Edit.Paste.Activated += Activated;
		}

		public void Uninitialize ()
		{
			PintaCore.Actions.Edit.Paste.Activated -= Activated;
		}
		#endregion

		private void Activated (object sender, EventArgs e)
		{
			// If no documents are open, activate the
			// PasteIntoNewImage action and abort this Paste action.
			if (!PintaCore.Workspace.HasOpenDocuments)
			{
				PintaCore.Actions.Edit.PasteIntoNewImage.Activate();
				return;
			}

            var doc = PintaCore.Workspace.ActiveDocument;

			// Get the scroll position in canvas co-ordinates
			Gtk.Viewport view = (Gtk.Viewport)doc.Workspace.Canvas.Parent;
			Cairo.PointD canvasPos = doc.Workspace.WindowPointToCanvas (
				view.Hadjustment.Value,
				view.Vadjustment.Value);

			// Paste into the active document.
			// The 'false' argument indicates that paste should be
			// performed into the current (not a new) layer.
			doc.Paste (false, (int) canvasPos.X, (int) canvasPos.Y);
		}
	}
}
