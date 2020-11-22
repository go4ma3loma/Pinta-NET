using System;
using Pinta.Core;

namespace Pinta.Actions
{
	class SaveDocumentAsAction : IActionHandler
	{
		#region IActionHandler Members
		public void Initialize ()
		{
			PintaCore.Actions.File.SaveAs.Activated += Activated;
		}

		public void Uninitialize ()
		{
			PintaCore.Actions.File.SaveAs.Activated -= Activated;
		}
		#endregion

		private void Activated (object sender, EventArgs e)
		{
			PintaCore.Workspace.ActiveDocument.Save (true);
		}
	}
}
