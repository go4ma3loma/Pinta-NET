using Gtk;

namespace Pinta
{
	public class SpinButtonEntryDialog: Dialog
	{
		private SpinButton spinButton;
	
		public SpinButtonEntryDialog (string title, Window parent, string label, int min, int max, int current)
			: base (title, parent, DialogFlags.Modal, Stock.Cancel, ResponseType.Cancel, Stock.Ok, ResponseType.Ok)
		{
			BorderWidth = 6;
			VBox.Spacing = 3;
			HBox hbox = new HBox ();
			hbox.Spacing = 6;
			
			Label lbl = new Label (label);
			lbl.Xalign = 0;
			hbox.PackStart (lbl);
			
			spinButton = new SpinButton (min, max, 1);
			spinButton.Value = current;
			hbox.PackStart (spinButton);
			
			hbox.ShowAll ();
			VBox.Add (hbox);

			AlternativeButtonOrder = new int[] { (int) ResponseType.Ok, (int) ResponseType.Cancel };
			DefaultResponse = ResponseType.Ok;
			spinButton.ActivatesDefault = true;
		}
		
		public int GetValue ()
		{
			return spinButton.ValueAsInt;
		}
	}
}
