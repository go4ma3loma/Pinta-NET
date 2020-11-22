using System;
using Pinta.Core;
using Gtk;

namespace Pinta.Actions
{
	class ToolBarToggledAction : IActionHandler
	{
		#region IActionHandler Members
		public void Initialize ()
		{
			PintaCore.Actions.View.ToolBar.Toggled += Activated;
		}

		public void Uninitialize ()
		{
			PintaCore.Actions.View.ToolBar.Toggled -= Activated;
		}
		#endregion

		private void Activated (object sender, EventArgs e)
		{
			PintaCore.Chrome.MainToolBar.Visible = ((ToggleAction)sender).Active;
		}
	}
}
