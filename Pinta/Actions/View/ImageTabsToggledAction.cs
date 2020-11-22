using System;
using Pinta.Core;
using Gtk;

namespace Pinta.Actions
{
    class ImageTabsToggledAction : IActionHandler
	{
		#region IActionHandler Members
		public void Initialize ()
		{
			PintaCore.Actions.View.ImageTabs.Toggled += Activated;
		}

		public void Uninitialize ()
		{
            PintaCore.Actions.View.ImageTabs.Toggled -= Activated;
		}
		#endregion

		private void Activated (object sender, EventArgs e)
		{
			Pinta.Docking.DockNotebook.DockNotebookManager.TabStripVisible = ((ToggleAction)sender).Active;
		}
	}
}
