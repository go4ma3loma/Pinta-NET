using System;

namespace Pinta.Core
{
	public sealed class IndexEventArgs : EventArgs {
        
		public int Index { get; private set; }
		
        public IndexEventArgs(int i) {
            Index = i;
        }
    }
}

