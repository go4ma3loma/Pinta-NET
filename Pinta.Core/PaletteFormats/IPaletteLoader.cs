using System;
using Mono.Addins;
using Cairo;
using System.Collections.Generic;

namespace Pinta.Core
{
	[TypeExtensionPoint]
	public interface IPaletteLoader
	{
		List<Color> Load (string fileName);
	}
}
