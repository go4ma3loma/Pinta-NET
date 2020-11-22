using System;
using Pinta.Core;
using Gtk;
using Mono.Unix;

namespace Pinta.Actions
{
	class OpenRecentAction : IActionHandler
	{
		#region IActionHandler Members
		public void Initialize ()
		{
			PintaCore.Actions.File.OpenRecent.ItemActivated += Activated;
		}

		public void Uninitialize ()
		{
			PintaCore.Actions.File.OpenRecent.ItemActivated -= Activated;
		}
		#endregion

		private void Activated (object sender, EventArgs e)
		{
			string fileUri = (sender as RecentAction).CurrentUri;

			PintaCore.Workspace.OpenFile (new Uri (fileUri).LocalPath);
		}
	}
}
