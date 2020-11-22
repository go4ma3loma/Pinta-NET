using System;
using Gtk;

namespace Pinta.Core
{
	public enum HistoryItemState { Undo, Redo }

	public class BaseHistoryItem : IDisposable
	{
		public string Icon { get; set; }
		public string Text { get; set; }
		public HistoryItemState State { get; set; }
		public TreeIter Id;
		public virtual bool CausesDirty { get { return true; } }
		
		public BaseHistoryItem ()
		{
		}
		
		public BaseHistoryItem (string icon, string text)
		{
			Icon = icon;
			Text = text;
			State = HistoryItemState.Undo;
		}
		
		public BaseHistoryItem (string icon, string text, HistoryItemState state)
		{
			Icon = icon;
			Text = text;
			State = state;
		}

		public virtual void Undo ()
		{
		}

		public virtual void Redo ()
		{
		}

        protected void Swap<T> (ref T x, ref T y)
        {
            T temp = x;
            x = y;
            y = temp;
        }

		#region IDisposable Members
		public virtual void Dispose ()
		{
		}
		#endregion
	}
}
