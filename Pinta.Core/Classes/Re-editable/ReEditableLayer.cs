using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cairo;

namespace Pinta.Core
{
	public class ReEditableLayer
	{
		Layer actualLayer;

		//Whether or not the actualLayer has already been setup.
		private bool isLayerSetup = false;

		private UserLayer parent;

		private bool inTheLoop = false;

		public bool InTheLoop { get { return inTheLoop; } }

		public Layer Layer
		{
			get
			{
				if (!isLayerSetup)
				{
					SetupLayer();
				}

				return actualLayer;
			}

			set
			{
				actualLayer = value;
			}
		}

		public bool IsLayerSetup
		{
			get
			{
				return isLayerSetup;
			}
		}

		/// <summary>
		/// Creates a new ReEditableLayer for drawing and editing on separately from the rest of the image.
		/// </summary>
		/// <param name="passedParent">The parent UserLayer that the ReEditableLayer will be associated with.</param>
		public ReEditableLayer(UserLayer passedParent)
		{
			parent = passedParent;

			TryAddLayer();
		}

		/// <summary>
		/// Safely and temporarily removes the ReEditableLayer from its parent UserLayer, withdrawing it from being drawn anywhere.
		/// 
		/// Due to history, cloning, etc. of editable shapes, ReEditableLayers need to be able to be removed from
		/// and added back into the system of drawn layers.
		/// </summary>
		public void TryRemoveLayer()
		{
			//Safely remove it by checking to make sure it is indeed in the loop first.
			if (parent.ReEditableLayers.Contains(this))
			{
				parent.ReEditableLayers.Remove(this);
			}

			inTheLoop = false;
		}

		/// <summary>
		/// Temporarily adds the ReEditableLayer back into its parent UserLayer, allowing it to be drawn again. Automatically avoids adding duplicates.
		/// 
		/// Due to history, cloning, etc. of editable shapes, ReEditableLayers need to be able to be removed from
		/// and added back into the system of drawn layers.
		/// </summary>
		public void TryAddLayer()
		{
			//Avoid duplicates.
			if (!parent.ReEditableLayers.Contains(this))
			{
				parent.ReEditableLayers.Add(this);
			}

			inTheLoop = true;
		}

		/// <summary>
		/// Setup the Layer based on the parent UserLayer's Surface.
		/// </summary>
		private void SetupLayer()
		{
			actualLayer = new Layer(new Cairo.ImageSurface(parent.Surface.Format, parent.Surface.Width, parent.Surface.Height));

			isLayerSetup = true;
		}
	}
}
