using System;
using Gtk;
using Mono.Unix;
using Pinta.Core;
using Mono.Addins.Gui;

namespace Pinta.Actions
{
	class AboutDialogAction : IActionHandler
	{
		#region IActionHandler Members
		public void Initialize ()
		{
			PintaCore.Actions.Help.About.Activated += Activated;
		}

		public void Uninitialize ()
		{
			PintaCore.Actions.Help.About.Activated -= Activated;
		}
		#endregion

		private void Activated (object sender, EventArgs e)
		{
			AboutDialog dlg = new AboutDialog ();

			try {
				dlg.Run ();
			} finally {
				dlg.Destroy ();
			}
		}
	}
}
