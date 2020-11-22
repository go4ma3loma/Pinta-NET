using System;
using Gtk;
using Pinta.Resources;

namespace Pinta.Core
{
	public static class PintaCore
	{
		public static LayerManager Layers { get; private set; }
		public static PaintBrushManager PaintBrushes { get; private set; }
		public static ToolManager Tools { get; private set; }
		public static ChromeManager Chrome { get; private set; }
		public static PaletteManager Palette { get; private set; }
		public static ResourceManager Resources { get; private set; }
		public static ActionManager Actions { get; private set; }
		public static WorkspaceManager Workspace { get; private set; }
		public static HistoryManager History { get; private set; }
		public static SystemManager System { get; private set; }
		public static LivePreviewManager LivePreview { get; private set; }
		public static SettingsManager Settings { get; private set; }
		public static EffectsManager Effects { get; private set; }

        public const string ApplicationVersion = "1.8";

		static PintaCore ()
		{
			Resources = new ResourceManager ();
			Actions = new ActionManager ();
			Workspace = new WorkspaceManager ();
			Layers = new LayerManager ();
			PaintBrushes = new PaintBrushManager ();
			Tools = new ToolManager ();
			History = new HistoryManager ();
			System = new SystemManager ();
			LivePreview = new LivePreviewManager ();
			Palette = new PaletteManager ();
			Settings = new SettingsManager ();
			Chrome = new ChromeManager ();
			Effects = new EffectsManager ();
		}
		
		public static void Initialize ()
		{
			Actions.RegisterHandlers ();
		}
	}
}
