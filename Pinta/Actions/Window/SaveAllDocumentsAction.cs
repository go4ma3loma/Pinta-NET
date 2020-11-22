using System;
using Pinta.Core;

namespace Pinta.Actions
{
	class SaveAllDocumentsAction : IActionHandler
	{
		#region IActionHandler Members
		public void Initialize ()
		{
			PintaCore.Actions.Window.SaveAll.Activated += Activated;
		}

		public void Uninitialize ()
		{
			PintaCore.Actions.Window.SaveAll.Activated -= Activated;
		}
		#endregion

		private void Activated (object sender, EventArgs e)
		{
			foreach (Document doc in PintaCore.Workspace.OpenDocuments) {
				if (!doc.IsDirty && doc.HasFile)
					continue;

				PintaCore.Workspace.SetActiveDocument (doc);

				// Loop through all of these until we get a cancel
				if (!doc.Save (false))
					break;
			}
		}
	}
}
