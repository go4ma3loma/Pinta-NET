using System;
using Mono.Addins;

namespace Pinta.Core
{
	[TypeExtensionPoint]
	public interface IImageExporter
	{
		void Export (Document document, string fileName, Gtk.Window parent);
	}
}
