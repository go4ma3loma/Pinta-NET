using System;
using Gtk;
using Pinta.Core;

namespace Pinta.Actions
{
	public class PrintDocumentAction : IActionHandler
	{
		#region IActionHandler implementation

		public void Initialize ()
		{
			PintaCore.Actions.File.Print.Activated += HandleActivated;
		}

		public void Uninitialize ()
		{
			PintaCore.Actions.File.Print.Activated -= HandleActivated;
		}

		#endregion
		
		void HandleActivated (object sender, EventArgs e)
		{
			// Commit any pending changes.
			PintaCore.Tools.Commit ();

			var op = new PrintOperation ();
			op.BeginPrint += HandleBeginPrint;
			op.DrawPage += HandleDrawPage;

			var result = op.Run (PrintOperationAction.PrintDialog, PintaCore.Chrome.MainWindow);

			if (result == PrintOperationResult.Apply) {
				// TODO - save print settings.
			} else if (result == PrintOperationResult.Error) {
				// TODO - show a proper dialog.
				System.Console.WriteLine ("Printing error");
			}
		}

		void HandleDrawPage (object o, DrawPageArgs args)
		{
			var doc = PintaCore.Workspace.ActiveDocument;

			// TODO - support scaling to fit page, centering image, etc.

			using (var surface = doc.GetFlattenedImage ()) {
				using (var context = args.Context.CairoContext) {
					context.SetSourceSurface (surface, 0, 0);
					context.Paint ();
				}
			}
		}

		void HandleBeginPrint (object o, BeginPrintArgs args)
		{
			PrintOperation op = (PrintOperation)o;
			op.NPages = 1;
		}
	}
}

