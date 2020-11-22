using System;
using System.Collections.Generic;
using System.Threading;
using Cairo;

namespace Pinta.Core
{
	public class AdjustmentsActions
	{
		public List<Gtk.Action> Actions { get; private set; }
		
		public AdjustmentsActions ()
		{
			Actions = new List<Gtk.Action> ();
		}

		#region Initialization
		public void CreateMainMenu (Gtk.Menu menu)
		{
            //try tools Gtk
		}
		#endregion

		#region Public Methods
		public void ToggleActionsSensitive (bool sensitive)
		{
			foreach (Gtk.Action a in Actions)
				a.Sensitive = sensitive;
		}
		#endregion
	}
}
