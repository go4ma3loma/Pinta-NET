using System;
using Gtk;
using Mono.Options;
using System.Collections.Generic;
using Pinta.Core;
using Mono.Unix;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Pinta
{
	class MainClass
	{
		[STAThread]
		public static void Main (string[] args)
		{
			string locale_dir = Path.Combine (SystemManager.GetDataRootDirectory (), "locale");

			try {
				Catalog.Init ("pinta", locale_dir);
			} catch (Exception ex) {
				Console.WriteLine (ex);
			}

			int threads = -1;
                        bool show_help = false;
                        bool show_version = false;
			
			var p = new OptionSet () {
                                { "h|help", Catalog.GetString("Show this message and exit."), v => show_help = v != null },
                                { "v|version", Catalog.GetString("Display the application version."), v => show_version = v != null },
				{ "rt|render-threads=", Catalog.GetString ("number of threads to use for rendering"), (int v) => threads = v }
			};

			List<string> extra;
			
			try {
				extra = p.Parse (args);
			} catch (OptionException e) {
				Console.WriteLine (e.Message);
                                ShowHelp (p);
				return;
			}

                        if (show_version)
                        {
                            Console.WriteLine (PintaCore.ApplicationVersion);
                            return;
                        }

                        if (show_help)
                        {
                            ShowHelp (p);
                            return;
                        }

			GLib.ExceptionManager.UnhandledException += new GLib.UnhandledExceptionHandler (ExceptionManager_UnhandledException);

			if (SystemManager.GetOperatingSystem () == OS.Windows) {
				SetWindowsGtkPath ();
			}
			
			Application.Init ();
			new MainWindow ();
			
			if (threads != -1)
				Pinta.Core.PintaCore.System.RenderThreads = threads;

			if (SystemManager.GetOperatingSystem () == OS.Mac) {
				RegisterForAppleEvents ();
			}

			OpenFilesFromCommandLine (extra);
			
			Application.Run ();
		}

                private static void ShowHelp (OptionSet p)
                {
                    Console.WriteLine (Catalog.GetString ("Usage: pinta [files]"));
                    Console.WriteLine ();
                    Console.WriteLine (Catalog.GetString ("Options: "));
                    p.WriteOptionDescriptions (Console.Out);
                }

		private static void OpenFilesFromCommandLine (List<string> extra)
		{
			// Ignore the process serial number parameter on Mac OS X
			if (PintaCore.System.OperatingSystem == OS.Mac && extra.Count > 0)
			{
				if (extra[0].StartsWith ("-psn_"))
				{
					extra.RemoveAt (0);
				}
			}

			if (extra.Count > 0)
			{
				foreach (var file in extra)
					PintaCore.Workspace.OpenFile (file);
			}
			else
			{
				// Create a blank document
				PintaCore.Workspace.NewDocument (new Gdk.Size (800, 600), new Cairo.Color (1, 1, 1));
			}
		}

		private static void ExceptionManager_UnhandledException (GLib.UnhandledExceptionArgs args)
		{
			Exception ex = (Exception)args.ExceptionObject;
			PintaCore.Chrome.ShowErrorDialog (PintaCore.Chrome.MainWindow,
			                                  string.Format ("{0}:\n{1}", "Unhandled exception", ex.Message),
			                                  ex.ToString ());
		}

		/// <summary>
		/// Registers for OSX-specific events, like quitting from the dock.
		/// </summary>
		static void RegisterForAppleEvents ()
		{
			MacInterop.ApplicationEvents.Quit += (sender, e) => {
				GLib.Timeout.Add (10, delegate {
					PintaCore.Actions.File.Exit.Activate ();
					return false;
				});
				e.Handled = true;
			};

			MacInterop.ApplicationEvents.Reopen += (sender, e) => {
				var window = PintaCore.Chrome.MainWindow;
				window.Deiconify ();
				window.Hide ();
				window.Show ();
				window.Present ();
				e.Handled = true;
			};

			MacInterop.ApplicationEvents.OpenDocuments += (sender, e) => {
				if (e.Documents != null) {
					GLib.Timeout.Add (10, delegate {
						foreach (string filename in e.Documents.Keys) {
							System.Console.Error.WriteLine ("Opening: {0}", filename);
							PintaCore.Workspace.OpenFile (filename);
						}
						return false;
					});
				}
				e.Handled = true;
			};
		}

		[DllImport ("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs (UnmanagedType.Bool)]
		static extern bool SetDllDirectory (string lpPathName);

		private static void SetWindowsGtkPath ()
		{
			string location = null;
			using (var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey (@"SOFTWARE\Xamarin\GtkSharp\InstallFolder")) {
				if (key != null) {
					location = key.GetValue (null) as string;
				}
			}

			if (location == null || !File.Exists (Path.Combine (location, "bin", "libgtk-win32-2.0-0.dll"))) {
				System.Console.Error.WriteLine ("Did not find registered GTK# installation");
				return;
			}

			var path = Path.Combine (location, @"bin");
			try {
				if (SetDllDirectory (path)) {
					return;
				}
			}
			catch (EntryPointNotFoundException) {
			}

			System.Console.Error.WriteLine ("Unable to set GTK# dll directory");
		}
	}
}
