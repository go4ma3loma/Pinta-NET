using System;
using System.IO;
using Cairo;
using System.Collections.Generic;
using System.Globalization;

namespace Pinta.Core
{
	public class PaintShopProPalette : IPaletteLoader, IPaletteSaver
	{
		public List<Color> Load (string fileName)
		{
			List<Color> colors = new List<Color> ();
			StreamReader reader = new StreamReader (fileName);
			string line = reader.ReadLine ();

			if (!line.StartsWith ("JASC-PAL"))
				throw new InvalidDataException("Not a valid PaintShopPro palette file.");

			line = reader.ReadLine (); // version

			int numberOfColors = int.Parse(reader.ReadLine());
			PintaCore.Palette.CurrentPalette.Resize (numberOfColors);

			while (!reader.EndOfStream) {
				line = reader.ReadLine ();
				string[] split = line.Split (' ');
				double r = int.Parse (split[0]) / 255f;
				double g = int.Parse (split[1]) / 255f;
				double b = int.Parse (split[2]) / 255f;
				colors.Add (new Color (r, g, b));
			}

			return colors;
		}

		public void Save (List<Color> colors, string fileName)
		{
			StreamWriter writer = new StreamWriter (fileName);
			writer.WriteLine ("JASC-PAL");
			writer.WriteLine ("0100");
			writer.WriteLine (colors.Count.ToString());

			foreach (Color color in colors) {
				writer.WriteLine ("{0} {1} {2}", (int) (color.R * 255), (int) (color.G * 255), (int) (color.B * 255));
			}

			writer.Close ();
		}
	}
}

