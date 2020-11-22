using System;
using Gtk;
using Mono.Unix;
using Pinta.Docking;
using Pinta.Docking.DockNotebook;

namespace Pinta
{
	public class CanvasPad : IDockPad
	{
        public DockNotebookContainer NotebookContainer { get; private set; }

		public void Initialize (DockFrame workspace, Menu padMenu)
		{
            var tab = new DockNotebook () {
                NavigationButtonsVisible = false
            };

            NotebookContainer = new DockNotebookContainer (tab, true);

            tab.InitSize ();

            var canvas_dock = workspace.AddItem ("Canvas");
            canvas_dock.Behavior = DockItemBehavior.Locked;
            canvas_dock.Expand = true;

            canvas_dock.DrawFrame = false;
            canvas_dock.Label = Catalog.GetString ("Canvas");
            canvas_dock.Content = NotebookContainer;
        }
	}
}
