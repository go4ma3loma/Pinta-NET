using System;
using Mono.Addins.Gui;
using Pinta.Core;

namespace Pinta.Actions
{
	class AddinManagerAction : IActionHandler
	{
		#region IActionHandler Members
		public void Initialize ()
		{
			PintaCore.Actions.Addins.AddinManager.Activated += Activated;
		}

		public void Uninitialize ()
		{
			PintaCore.Actions.Addins.AddinManager.Activated -= Activated;
		}
		#endregion

		private void Activated (object sender, EventArgs e)
		{
			AddinManagerWindow.AllowInstall = true;

			AddinManagerWindow.Run (PintaCore.Chrome.MainWindow);

			//dlg.DeleteEvent += delegate { dlg.Destroy (); };
		}
	}
}
