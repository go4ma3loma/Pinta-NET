using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gtk;

namespace Pinta.Core
{
	public class HistoryManager
	{
		public Gtk.ListStore ListStore {
			get { return PintaCore.Workspace.ActiveWorkspace.History.ListStore; }
		}
		
		public int Pointer {
			get { return PintaCore.Workspace.ActiveWorkspace.History.Pointer; }
		}
		
		public BaseHistoryItem Current {
			get { return PintaCore.Workspace.ActiveWorkspace.History.Current; }
		}
		
		public void PushNewItem (BaseHistoryItem newItem)
		{
			PintaCore.Workspace.ActiveWorkspace.History.PushNewItem (newItem);
		}
		
		public void Undo ()
		{
			PintaCore.Workspace.ActiveWorkspace.History.Undo ();
		}
		
		public void Redo ()
		{
			PintaCore.Workspace.ActiveWorkspace.History.Redo ();
		}
		
		public void Clear ()
		{
			PintaCore.Workspace.ActiveWorkspace.History.Clear ();
		}
		
		#region Protected Methods
		protected internal void OnHistoryItemAdded (BaseHistoryItem item)
		{
			if (HistoryItemAdded != null)
				HistoryItemAdded (this, new HistoryItemAddedEventArgs (item));
		}

		protected internal void OnHistoryItemRemoved (BaseHistoryItem item)
		{
			if (HistoryItemRemoved != null)
				HistoryItemRemoved (this, new HistoryItemRemovedEventArgs (item));
		}

		protected internal void OnActionUndone ()
		{
			if (ActionUndone != null)
				ActionUndone (this, EventArgs.Empty);
		}

		protected internal void OnActionRedone ()
		{
			if (ActionRedone != null)
				ActionRedone (this, EventArgs.Empty);
		}
		#endregion
		 
		#region Events
		public event EventHandler<HistoryItemAddedEventArgs> HistoryItemAdded;
		public event EventHandler<HistoryItemRemovedEventArgs> HistoryItemRemoved;
		public event EventHandler ActionUndone;
		public event EventHandler ActionRedone;
		#endregion
	}
}
