using System;
using Mono.Addins;
using System.Collections.Generic;
using Cairo;

namespace Pinta.Core
{
	[TypeExtensionPoint]
	public interface IPaletteSaver
	{
		void Save (List<Color> colors, string fileName);
	}
}
