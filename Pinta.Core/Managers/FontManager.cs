using System;
using System.Collections.Generic;
using System.Linq;
using Gdk;
using Pango;

namespace Pinta.Core
{
	public class FontManager
	{
		private List<FontFamily> families;

		private List<int> default_font_sizes = new List<int> (new int[] { 6, 7, 8, 9, 10, 11, 12, 14, 16,
				18, 20, 22, 24, 26, 28, 32, 36, 40, 44,
				48, 54, 60, 66, 72, 80, 88, 96 });

		public FontManager ()
		{
			families = new List<FontFamily> ();

			using (Pango.Context c = PangoHelper.ContextGet ())
				families.AddRange (c.Families);
		}

		public List<string> GetInstalledFonts ()
		{
			return families.Select (f => f.Name).ToList ();
		}

		public FontFamily GetFamily (string fontname)
		{
			return families.Find (f => f.Name == fontname);
		}

		public List<int> GetSizes (FontFamily family)
		{
			return GetSizes (family.Faces[0]);
		}

		unsafe public List<int> GetSizes (FontFace fontFace)
		{
			int sizes;
			int nsizes;

			// Query for supported sizes for this font
			fontFace.ListSizes (out sizes, out nsizes);

			if (nsizes == 0)
				return default_font_sizes;

			List<int> result = new List<int> ();

			for (int i = 0; i < nsizes; i++)
				result.Add (*(&sizes + 4 * i));

			return result;
		}
		
	}
}
