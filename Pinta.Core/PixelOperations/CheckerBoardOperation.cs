using System;

namespace Pinta.Core
{
	public class CheckerBoardOperation
	{
		public int opacity = 255;

		public CheckerBoardOperation (double opacity)
		{
			this.opacity = (int)(opacity * 255);
		}

		public ColorBgra Apply (ColorBgra color, int checkerX, int checkerY)
		{
			int b = color.B;
			int g = color.G;
			int r = color.R;
			int a = ApplyOpacity (color.A);

			int v = ((checkerX ^ checkerY) & 8) * 8 + 191;
			a = a + (a >> 7);
			int vmia = v * (256 - a);

			r = ((r * a) + vmia) >> 8;
			g = ((g * a) + vmia) >> 8;
			b = ((b * a) + vmia) >> 8;

			return ColorBgra.FromUInt32 ((uint)b + ((uint)g << 8) + ((uint)r << 16) + 0xff000000);
		}

		private byte ApplyOpacity (byte a)
		{
			int r = a;
			r = r * this.opacity + 0x80;
			r = ((((r) >> 8) + (r)) >> 8);
			return (byte)r;
		}
	}
}
