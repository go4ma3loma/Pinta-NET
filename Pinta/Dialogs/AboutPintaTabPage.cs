using System;
using Gtk;
using Mono.Unix;
using Pinta.Core;

namespace Pinta
{
	internal class AboutPintaTabPage : VBox
	{
		public AboutPintaTabPage ()
		{
			Label label = new Label ();
			label.Markup = String.Format (
			    "<b>{0}</b>\n    {1}",
			    Catalog.GetString ("Version"),
                            PintaCore.ApplicationVersion);
			    
			HBox hBoxVersion = new HBox ();
			hBoxVersion.PackStart (label, false, false, 5);
			this.PackStart (hBoxVersion, false, true, 0);

			label = null;
			label = new Label ();
			label.Markup = string.Format ("<b>{0}</b>\n    {1}", Catalog.GetString ("License"), Catalog.GetString ("Released under the MIT X11 License."));
			HBox hBoxLicense = new HBox ();
			hBoxLicense.PackStart (label, false, false, 5);
			this.PackStart (hBoxLicense, false, true, 5);

			label = null;
			label = new Label ();
			label.Markup = string.Format ("<b>{0}</b>\n    (c) 2019-2020 {1}", Catalog.GetString ("Copyright"), Catalog.GetString ("by Pinta f o u e d"));
			HBox hBoxCopyright = new HBox ();
			hBoxCopyright.PackStart (label, false, false, 5);
			this.PackStart (hBoxCopyright, false, true, 5);

			this.ShowAll ();
		}
	}
}
