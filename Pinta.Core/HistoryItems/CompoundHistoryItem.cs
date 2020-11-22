using Cairo;
using System;
using System.Collections.Generic;

namespace Pinta.Core
{
	public class CompoundHistoryItem : BaseHistoryItem
	{
		protected List<BaseHistoryItem> history_stack = new List<BaseHistoryItem> ();
		private List<ImageSurface> snapshots;

		public CompoundHistoryItem () : base ()
		{
		}
		
		public CompoundHistoryItem (string icon, string text) : base (icon, text)
		{
		}
		
		public void Push (BaseHistoryItem item)
		{
			history_stack.Add (item);
		}
		
		public override void Undo ()
		{
			for (int i = history_stack.Count - 1; i >= 0; i--)
				history_stack[i].Undo ();
		}

		public override void Redo ()
		{
			// We want to redo the actions in the
			// opposite order than the undo order
			foreach (var item in history_stack)
				item.Redo ();
		}

		public override void Dispose ()
		{
			foreach (var item in history_stack)
				item.Dispose ();
		}

		public void StartSnapshotOfImage ()
		{
			snapshots = new List<ImageSurface> ();
			foreach (UserLayer item in PintaCore.Workspace.ActiveDocument.UserLayers) {
				snapshots.Add (item.Surface.Clone ());
			}
		}

		public void FinishSnapshotOfImage ()
		{
			for (int i = 0; i < snapshots.Count; ++i) {
				history_stack.Add (new SimpleHistoryItem (string.Empty, string.Empty, snapshots[i], i));
			}
			snapshots.Clear ();
		}
	}
}
