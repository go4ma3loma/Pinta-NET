using System;
using System.IO;
using Pinta.Resources;

namespace Pinta
{
	public class ResourceManager
	{
		public Gdk.Pixbuf GetIcon (string name)
		{
			return GetIcon (name, 16);
		}

		public Gdk.Pixbuf GetIcon (string name, int size)
		{
			return ResourceLoader.GetIcon (name, size);
		}

        public Stream GetResourceIconStream (string name)
        {
            return ResourceLoader.GetResourceIconStream (name);
        }
    }
}
