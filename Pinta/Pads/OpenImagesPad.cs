using Pinta.Docking;
using Mono.Unix;
using Gtk;
using Pinta.Core;
using Pinta.Gui.Widgets;

namespace Pinta
{
	public class OpenImagesPad : IDockPad
	{
		public void Initialize (DockFrame workspace, Menu padMenu)
		{
			DockItem open_images_item = workspace.AddItem ("Images");
			open_images_item.DefaultLocation = "Layers/Bottom";
			open_images_item.Label = Catalog.GetString ("Images");
			open_images_item.Content = new OpenImagesListWidget ();
			open_images_item.Icon = PintaCore.Resources.GetIcon ("Menu.Effects.Default.png");
            open_images_item.DefaultVisible = false;
            open_images_item.DefaultWidth = 100;
			open_images_item.Behavior |= DockItemBehavior.CantClose;

			ToggleAction show_open_images = padMenu.AppendToggleAction ("Images", Catalog.GetString ("Images"), null, null);

			show_open_images.Activated += delegate {
				open_images_item.Visible = show_open_images.Active;
			};

			open_images_item.VisibleChanged += delegate {
				show_open_images.Active = open_images_item.Visible;
			};
		}
	}
}

