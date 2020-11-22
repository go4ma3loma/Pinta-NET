﻿using System;
using System.Collections.Generic;
using Gtk;

namespace Pinta.Core
{
	public class DocumentWorkspaceHistory
	{
		private Document document;
		List<BaseHistoryItem> history = new List<BaseHistoryItem> ();
		int historyPointer = -1;

		internal DocumentWorkspaceHistory (Document document)
		{
			this.document = document;
			ListStore = new ListStore (typeof (BaseHistoryItem));
		}

		public Gtk.ListStore ListStore { get; private set; }
		
		public int Pointer {
			get { return historyPointer; }
		}
		
		public BaseHistoryItem Current {
			get { 
				if (historyPointer > -1 && historyPointer < history.Count)
					return history[historyPointer]; 
				else
					return null;
			}
		}
		
		public void PushNewItem (BaseHistoryItem newItem)
		{
			
			//Remove all old redos starting from the end of the list
			for (int i = history.Count - 1; i >= 0; i--) {
			
				BaseHistoryItem item = history[i];
				
				if (item.State == HistoryItemState.Redo) {
					history.RemoveAt(i);
					item.Dispose();
					//Remove from ListStore
					ListStore.Remove (ref item.Id);
					
				} else if (item.State == HistoryItemState.Undo) {
					break;
				}
			}
		
			//Add new undo to ListStore
			newItem.Id = ListStore.AppendValues (newItem);
			history.Add (newItem);
			historyPointer = history.Count - 1;
			
			if (newItem.CausesDirty)
				document.IsDirty = true;
				
			if (history.Count > 1) {
				PintaCore.Actions.Edit.Undo.Sensitive = true;
				CanUndo = true;
			}
				
			PintaCore.Actions.Edit.Redo.Sensitive = false;
			CanRedo = false;
			PintaCore.History.OnHistoryItemAdded (newItem);
		}
		
		public void Undo ()
		{
			if (historyPointer < 0) {
				throw new InvalidOperationException ("Undo stack is empty");
			} else {
				BaseHistoryItem item = history[historyPointer];
				item.Undo ();
				item.State = HistoryItemState.Redo;
				ListStore.SetValue (item.Id, 0, item);
				history[historyPointer] = item;
				historyPointer--;
			}	
			
			if (historyPointer == 0) {
				document.IsDirty = false;
				PintaCore.Actions.Edit.Undo.Sensitive = false;
				CanUndo = false;
			}
			
			PintaCore.Actions.Edit.Redo.Sensitive = true;
			CanRedo = true;
			PintaCore.History.OnActionUndone ();
		}
		
		public void Redo ()
		{
			if (historyPointer >= history.Count - 1)
				throw new InvalidOperationException ("Redo stack is empty");

			historyPointer++;
			BaseHistoryItem item = history[historyPointer];
			item.Redo ();
			item.State = HistoryItemState.Undo;
			ListStore.SetValue (item.Id, 0, item);
			history[historyPointer] = item;

			if (historyPointer == history.Count - 1) {
				PintaCore.Actions.Edit.Redo.Sensitive = false;
				CanRedo = false;
			}
				
			if (item.CausesDirty)
				document.IsDirty = true;

			if (history.Count > 1) {
				PintaCore.Actions.Edit.Undo.Sensitive = true;
				CanUndo = true;
			}

			PintaCore.History.OnActionRedone ();
		}
		
		public void Clear ()
		{
			history.ForEach (delegate(BaseHistoryItem item) { item.Dispose (); } );
			history.Clear();	
			ListStore.Clear ();	
			historyPointer = -1;

			document.IsDirty = false;
			PintaCore.Actions.Edit.Redo.Sensitive = false;
			PintaCore.Actions.Edit.Undo.Sensitive = false;
			
			CanRedo = false;
			CanUndo = false;
		}
		
		public bool CanRedo { get; private set; }
		public bool CanUndo { get; private set; }
	}
}
