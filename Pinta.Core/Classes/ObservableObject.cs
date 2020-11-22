using System;
using System.ComponentModel;

namespace Pinta.Core
{

	public abstract class ObservableObject
	{
		public ObservableObject ()
		{
		}
				
		public event PropertyChangedEventHandler PropertyChanged;
				
		protected void SetValue<T> (string propertyName, ref T member, T value)
		{
			member = value;			
			FirePropertyChanged (propertyName);
		}
		
		protected void FirePropertyChanged (string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));			
		}
	}
}
