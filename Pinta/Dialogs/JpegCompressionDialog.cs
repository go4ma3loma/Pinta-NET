using Gtk;
using Mono.Unix;
using Pinta.Gui.Widgets;

namespace Pinta
{
	public class JpegCompressionDialog : Dialog
	{
		private HScale compressionLevel;
	
		public JpegCompressionDialog (int defaultQuality, Gtk.Window parent)
			: base (Catalog.GetString ("JPEG Quality"), parent, DialogFlags.Modal | DialogFlags.DestroyWithParent,
				Stock.Cancel, ResponseType.Cancel, Stock.Ok, ResponseType.Ok)
		{
			this.BorderWidth = 6;
			this.VBox.Spacing = 3;
			VBox content = new VBox ();
			content.Spacing = 5;

			DefaultResponse = ResponseType.Ok;
			
			Label label = new Label (Catalog.GetString ("Quality: "));
			label.Xalign = 0;
			content.PackStart (label, false, false, 0);
			
			compressionLevel = new HScale (1, 100, 1);
			compressionLevel.Value = defaultQuality;
			content.PackStart (compressionLevel, false, false, 0);

			content.ShowAll ();
			this.VBox.Add (content);
			AlternativeButtonOrder = new int[] { (int) ResponseType.Ok, (int) ResponseType.Cancel };
		}
		
		public int GetCompressionLevel ()
		{
			return (int) compressionLevel.Value;
		}
	}
}
