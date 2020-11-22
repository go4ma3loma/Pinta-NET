using System;
using System.Collections.Generic;

namespace Pinta.Core
{
	public class PaintBrushManager : IEnumerable<BasePaintBrush>
	{
		private List<BasePaintBrush> paint_brushes = new List<BasePaintBrush> ();

		public event EventHandler<BrushEventArgs> BrushAdded;
		public event EventHandler<BrushEventArgs> BrushRemoved;

		/// <summary>
		/// Register a new brush.
		/// </summary>
		public void AddPaintBrush (BasePaintBrush paintBrush)
		{
			paint_brushes.Add (paintBrush);
			paint_brushes.Sort (new BrushSorter ());
			OnBrushAdded (paintBrush);
		}

		/// <summary>
		/// Remove a brush type.
		/// </summary>
		public void RemoveInstanceOfPaintBrush (System.Type paintBrush)
		{
			foreach (BasePaintBrush brush in paint_brushes) {
				if (brush.GetType () == paintBrush) {
					paint_brushes.Remove (brush);
					paint_brushes.Sort (new BrushSorter ());
					OnBrushRemoved (brush);
					return;
				}
			}
		}

		#region IEnumerable<BasePaintBrush> implementation
		public IEnumerator<BasePaintBrush> GetEnumerator ()
		{
			return paint_brushes.GetEnumerator ();
		}
		#endregion

		#region IEnumerable implementation
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return paint_brushes.GetEnumerator ();
		}
		#endregion

		private void OnBrushAdded (BasePaintBrush brush)
		{
			var handler = BrushAdded;
			if (handler != null)
				handler (this, new BrushEventArgs (brush));
		}

		private void OnBrushRemoved (BasePaintBrush brush)
		{
			var handler = BrushRemoved;
			if (handler != null)
				handler (this, new BrushEventArgs (brush));
		}

		class BrushSorter : Comparer<BasePaintBrush>
		{
			public override int Compare (BasePaintBrush x, BasePaintBrush y)
			{
				var xstr = x.Priority == 0 ? x.Name : x.Priority.ToString ();
				var ystr = y.Priority == 0 ? y.Name : y.Priority.ToString ();

				return string.Compare (xstr, ystr);
			}
		}
	}
}

