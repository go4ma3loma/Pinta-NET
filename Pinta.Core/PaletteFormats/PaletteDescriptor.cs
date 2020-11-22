using System;
using Gtk;
using System.Text;
using Mono.Unix;

namespace Pinta.Core
{
	public sealed class PaletteDescriptor
	{
		public string[] Extensions { get; private set; }

		public IPaletteLoader Loader { get; private set; }

		public IPaletteSaver Saver { get; private set; }

		public FileFilter Filter { get; private set; }

		public PaletteDescriptor (string displayPrefix, string[] extensions, IPaletteLoader loader, IPaletteSaver saver)
		{
			if (extensions == null || (loader == null && saver == null)) {
				throw new ArgumentNullException ("Palette descriptor is initialized incorrectly");
			}

			this.Extensions = extensions;
			this.Loader = loader;
			this.Saver = saver;

			FileFilter ff = new FileFilter ();
			StringBuilder formatNames = new StringBuilder ();

			foreach (string ext in extensions) {
				if (formatNames.Length > 0)
					formatNames.Append (", ");

				string wildcard = string.Format ("*.{0}", ext);
				ff.AddPattern (wildcard);
				formatNames.Append (wildcard);
			}

			// Translators: {0} is the palette format (e.g. "GIMP") and {1} is a list of file extensions.
			ff.Name = string.Format (Catalog.GetString ("{0} palette ({1})"), displayPrefix, formatNames);
			this.Filter = ff;
		}

		public bool IsReadOnly ()
		{
			return Saver == null;
		}

		public bool IsWriteOnly ()
		{
			return Loader == null;
		}
	}
}
