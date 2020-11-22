using System;
using Gtk;
using Pinta.Core;

namespace Pinta
{
    class FileUnsupportedFormatDialog : Gtk.Dialog
    {
		private Label description_label;

		public FileUnsupportedFormatDialog(Window parent) : base("Pinta", parent, DialogFlags.Modal | DialogFlags.DestroyWithParent)
		{
			Build();

			TransientFor = parent;
		}

		public void SetMessage(string message)
		{
			description_label.Markup = message;
		}

		private void Build()
		{
			var hbox = new HBox();
			hbox.Spacing = 6;
			hbox.BorderWidth = 12;

			var error_icon = new Image();
			error_icon.Pixbuf = PintaCore.Resources.GetIcon(Stock.DialogError, 32);
			error_icon.Yalign = 0;
			hbox.PackStart(error_icon, false, false, 0);

			var vbox = new VBox();
			vbox.Spacing = 6;

			description_label = new Label();
			description_label.Wrap = true;
			description_label.Xalign = 0;
			vbox.PackStart(description_label, false, false, 0);

			hbox.Add(vbox);
			this.VBox.Add(hbox);

			//PintaCore.System.ImageFormats.Formats

			var ok_button = new Button(Gtk.Stock.Ok);
			ok_button.CanDefault = true;
			AddActionWidget(ok_button, ResponseType.Ok);

			DefaultWidth = 600;
			DefaultHeight = 142;

			ShowAll();
		}

	}
}
