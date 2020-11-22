using System;
using Pinta.Core;

namespace Pinta.Actions
{
	class SaveDocumentAction : IActionHandler
	{
		#region IActionHandler Members
		public void Initialize ()
		{
			PintaCore.Actions.File.Save.Activated += Activated;
		}

		public void Uninitialize ()
		{
			PintaCore.Actions.File.Save.Activated -= Activated;
		}
		#endregion

		private void Activated (object sender, EventArgs e)
		{
			PintaCore.Workspace.ActiveDocument.Save (false);
		}
	}
}
