using System;
using Pinta.Core;

namespace Pinta.Actions
{
	class CloseAllDocumentsAction : IActionHandler
	{
		#region IActionHandler Members
		public void Initialize ()
		{
			PintaCore.Actions.Window.CloseAll.Activated += Activated;
		}

		public void Uninitialize ()
		{
			PintaCore.Actions.Window.CloseAll.Activated -= Activated;
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
		}
	}
}
