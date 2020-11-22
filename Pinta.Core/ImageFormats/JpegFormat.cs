using System;
using System.IO;
using Gdk;

namespace Pinta.Core
{
	public class JpegFormat: GdkPixbufFormat
	{
		//The identifier of the setting that is used to remember the previously
		//saved JPG compression quality, even when Pinta is restarted.
		private const string JpgCompressionQualitySetting = "jpg-quality";

		//The default JPG compression quality to use when no saved setting is loaded. This will usually
		//occur when Pinta is first run on a machine, although there are other possibile cases as well.
		private const int defaultQuality = 85;

		public JpegFormat()
			: base ("jpeg")
		{
		}

		protected override void DoSave(Pixbuf pb, string fileName, string fileType, Gtk.Window parent)
		{
			//Load the JPG compression quality, but use the default value if there is no saved value.
			int level = PintaCore.Settings.GetSetting<int>(JpgCompressionQualitySetting, defaultQuality);

			//Check to see if the Document has been saved before.
			if (!PintaCore.Workspace.ActiveDocument.HasBeenSavedInSession)
			{
				//Show the user the JPG export compression quality dialog, with the default
				//value being the one loaded in (or the default value if it was not saved).
				level = PintaCore.Actions.File.RaiseModifyCompression(level, parent);

				if (level == -1)
					throw new OperationCanceledException ();
			}

			//Store the "previous" JPG compression quality value (before saving with it).
			PintaCore.Settings.PutSetting(JpgCompressionQualitySetting, level);

			//Save the file.
			pb.SavevUtf8(fileName, fileType, new string[] { "quality", null }, new string[] { level.ToString(), null });
		}
	}
}
