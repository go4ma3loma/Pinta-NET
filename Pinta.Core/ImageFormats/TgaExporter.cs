using System;
using System.IO;
using Cairo;

namespace Pinta.Core
{
	public class TgaExporter: IImageExporter
	{
		private struct TgaHeader
		{
			public byte idLength;            // Image ID Field Length
			public byte cmapType;            // Color Map Type
			public byte imageType;           // Image Type

			public ushort cmapIndex;         // First Entry Index
			public ushort cmapLength;        // Color Map Length
			public byte cmapEntrySize;       // Color Map Entry Size

			public ushort xOrigin;           // X-origin of Image
			public ushort yOrigin;           // Y-origin of Image
			public ushort imageWidth;        // Image Width
			public ushort imageHeight;       // Image Height
			public byte pixelDepth;          // Pixel Depth
			public byte imageDesc;           // Image Descriptor

			public void WriteTo (BinaryWriter output)
			{
				output.Write (this.idLength);
				output.Write (this.cmapType);
				output.Write (this.imageType);

				output.Write (this.cmapIndex);
				output.Write (this.cmapLength);
				output.Write (this.cmapEntrySize);

				output.Write (this.xOrigin);
				output.Write (this.yOrigin);
				output.Write (this.imageWidth);
				output.Write (this.imageHeight);
				output.Write (this.pixelDepth);
				output.Write (this.imageDesc);
			}
		}

		/// <summary>
		/// The image ID field contents. It is important for this field to be non-empty, since
		/// GDK incorrectly identifies the mime type as image/x-win-bitmap if the idLength
		/// value is 0 (see bug #987641).
		/// </summary>
		private const string ImageIdField = "Created by Pinta .NET";
		
		// For now, we only export in uncompressed ARGB32 format. If someone requests this functionality,
		// we can always add more through an export dialog.
		public void Export (Document document, string fileName, Gtk.Window parent) {
			ImageSurface surf = document.GetFlattenedImage (); // Assumes the surface is in ARGB32 format
			BinaryWriter writer = new BinaryWriter (new FileStream (fileName, FileMode.Create, FileAccess.Write));
	
			try {
				TgaHeader header = new TgaHeader();

				header.idLength = (byte) (ImageIdField.Length + 1);
				header.cmapType = 0;
				header.imageType = 2; // uncompressed RGB
				header.cmapIndex = 0;
				header.cmapLength = 0;
				header.cmapEntrySize = 0;
				header.xOrigin = 0;
				header.yOrigin = 0;
				header.imageWidth = (ushort) surf.Width;
				header.imageHeight = (ushort) surf.Height;
				header.pixelDepth = 32;
				header.imageDesc = 8; // 32-bit, lower-left origin, which is weird but hey...
				header.WriteTo (writer);

				writer.Write(ImageIdField);
				
				byte[] data = surf.Data;
				
				// It just so happens that the Cairo ARGB32 internal representation matches
				// the TGA format, except vertically-flipped. In little-endian, of course.
				for (int y = surf.Height - 1; y >= 0; y--)
					writer.Write (data, surf.Stride * y, surf.Stride);
			} finally {
				(surf as IDisposable).Dispose ();
				writer.Close ();
			}
		}
	}
}
