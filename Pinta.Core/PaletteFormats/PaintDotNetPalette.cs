using System;
using System.IO;
using Cairo;
using System.Collections.Generic;
using System.Globalization;

namespace Pinta.Core
{
	public class PaintDotNetPalette : IPaletteLoader, IPaletteSaver
	{
		public List<Color> Load (string fileName)
		{
			List<Color> colors = new List<Color> ();
			StreamReader reader = new StreamReader (fileName);

			try {
				string line = reader.ReadLine ();
				do {
					if (line.IndexOf (';') == 0)
						continue;

					uint color = uint.Parse (line.Substring (0, 8), NumberStyles.HexNumber);
					double b = (color & 0xff) / 255f;
					double g = ((color >> 8) & 0xff) / 255f;
					double r = ((color >> 16) & 0xff) / 255f;
					double a = (color >> 24) / 255f;
					colors.Add (new Color (r, g, b, a));
				} while ((line = reader.ReadLine ()) != null);

				return colors;
			} finally {
				reader.Close ();
			}
		}

		public void Save (List<Color> colors, string fileName)
		{
			StreamWriter writer = new StreamWriter (fileName);
			writer.WriteLine ("; Hexadecimal format: aarrggbb");

			foreach (Color color in colors) {
				byte a = (byte)(color.A * 255);
				byte r = (byte)(color.R * 255);
				byte g = (byte)(color.G * 255);
				byte b = (byte)(color.B * 255);
				writer.WriteLine ("{0:X}", (a << 24) | (r << 16) | (g << 8) | b);
			}
			writer.Close ();
		}
	}
}

