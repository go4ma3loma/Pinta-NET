using System;
using Mono.Addins;

namespace Pinta.Core
{
	[TypeExtensionPoint]
	public interface IImageImporter
	{
		/// <summary>
		/// Imports a document into Pinta.
		/// </summary>
		/// <param name='filename'>The name of the file to be imported.</param>
		/// <param name='parent'>
		/// Window to be used as a parent for any dialogs that are shown.
		/// </param>
		void Import (string filename, Gtk.Window parent);

		/// <summary>
		/// Returns a thumbnail of an image.
		/// If the format provides an efficient way to load a smaller version of
		/// the image, it is suggested to use that method to load a thumbnail
		/// no larger than the given width and height parameters. Otherwise, the
		/// returned pixbuf will need to be rescaled by the calling code if it
		/// exceeds the maximum size.
		/// </summary>
		/// <param name='filename'>The name of the file to be imported.</param>
		/// <param name='maxWidth'>The maximum width of the thumbnail.</param>
		/// <param name='maxHeight'>The maximum height of the thumbnail.</param>
		/// <param name='parent'>
		/// Window to be used as a parent for any dialogs that are shown.
		/// </param>
		/// <returns>The thumbnail, or null if the image could not be loaded.</returns>
		Gdk.Pixbuf LoadThumbnail (string filename, int maxWidth, int maxHeight,
		                          Gtk.Window parent);
	}
}
