using System;
using Gtk;
using Mono.Unix;
using Pinta.Core;

namespace Pinta.Actions
{
	class ExitProgramAction : IActionHandler
	{
		#region IActionHandler Members
		public void Initialize ()
		{
			PintaCore.Actions.File.Exit.Activated += Activated;
		}

		public void Uninitialize ()
		{
			PintaCore.Actions.File.Exit.Activated -= Activated;
		}
		#endregion

		private void Activated (object sender, EventArgs e)
		{
			while (PintaCore.Workspace.HasOpenDocuments) {
				int count = PintaCore.Workspace.OpenDocuments.Count;

				PintaCore.Actions.File.Close.Activate ();

				// If we still have the same number of open documents,
				// the user cancelled on a Save prompt.
				if (count == PintaCore.Workspace.OpenDocuments.Count)
					return;
			}

			// Let everyone know we are quitting
			PintaCore.Actions.File.RaiseBeforeQuit ();

			Application.Quit ();
		}
	}
}
