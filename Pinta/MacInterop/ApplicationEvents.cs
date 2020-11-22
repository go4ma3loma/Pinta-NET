using System;
using System.Collections.Generic;

namespace Pinta.MacInterop
{
	public static class ApplicationEvents
	{
		static object lockObj = new object ();
		
		#region Quit
		
		static EventHandler<ApplicationQuitEventArgs> quit;
		static IntPtr quitHandlerRef = IntPtr.Zero;
		
		public static event EventHandler<ApplicationQuitEventArgs> Quit {
			add {
				lock (lockObj) {
					quit += value;
					if (quitHandlerRef == IntPtr.Zero)
						quitHandlerRef = Carbon.InstallApplicationEventHandler (HandleQuit, CarbonEventApple.QuitApplication);
				}
			}
			remove {
				lock (lockObj) {
					quit -= value;
					if (quit == null && quitHandlerRef != IntPtr.Zero) {
						Carbon.RemoveEventHandler (quitHandlerRef);
						quitHandlerRef = IntPtr.Zero;
					}
				}
			}
		}
		
		static CarbonEventHandlerStatus HandleQuit (IntPtr callRef, IntPtr eventRef, IntPtr user_data)
		{
			var args = new ApplicationQuitEventArgs ();
			quit (null, args);
			return args.UserCancelled? CarbonEventHandlerStatus.UserCancelled : args.HandledStatus;
		}
		
		#endregion
		
		#region Reopen
		
		static EventHandler<ApplicationEventArgs> reopen;
		static IntPtr reopenHandlerRef = IntPtr.Zero;
		
		public static event EventHandler<ApplicationEventArgs> Reopen {
			add {
				lock (lockObj) {
					reopen += value;
					if (reopenHandlerRef == IntPtr.Zero)
						reopenHandlerRef = Carbon.InstallApplicationEventHandler (HandleReopen, CarbonEventApple.ReopenApplication);
				}
			}
			remove {
				lock (lockObj) {
					reopen -= value;
					if (reopen == null && reopenHandlerRef != IntPtr.Zero) {
						Carbon.RemoveEventHandler (reopenHandlerRef);
						reopenHandlerRef = IntPtr.Zero;
					}
				}
			}
		}
		
		static CarbonEventHandlerStatus HandleReopen (IntPtr callRef, IntPtr eventRef, IntPtr user_data)
		{
			var args = new ApplicationEventArgs ();
			reopen (null, args);
			return args.HandledStatus;
		}
		
		#endregion
		
		#region OpenDocuments
		
		static EventHandler<ApplicationDocumentEventArgs> openDocuments;
		static IntPtr openDocumentsHandlerRef = IntPtr.Zero;
		
		public static event EventHandler<ApplicationDocumentEventArgs> OpenDocuments {
			add {
				lock (lockObj) {
					openDocuments += value;
					if (openDocumentsHandlerRef == IntPtr.Zero)
						openDocumentsHandlerRef = Carbon.InstallApplicationEventHandler (HandleOpenDocuments, CarbonEventApple.OpenDocuments);
				}
			}
			remove {
				lock (lockObj) {
					openDocuments -= value;
					if (openDocuments == null && openDocumentsHandlerRef != IntPtr.Zero) {
						Carbon.RemoveEventHandler (openDocumentsHandlerRef);
						openDocumentsHandlerRef = IntPtr.Zero;
					}
				}
			}
		}
		
		static CarbonEventHandlerStatus HandleOpenDocuments (IntPtr callRef, IntPtr eventRef, IntPtr user_data)
		{
			try {
				var docs = Carbon.GetFileListFromEventRef (eventRef);
				var args = new ApplicationDocumentEventArgs (docs);
				openDocuments (null, args);
				return args.HandledStatus;
			} catch (Exception ex) {
				System.Console.WriteLine (ex);
				return CarbonEventHandlerStatus.NotHandled;
			}
		}
		
		#endregion
		
		#region OpenUrls
		
		static EventHandler<ApplicationUrlEventArgs> openUrls;
		static IntPtr openUrlsHandlerRef = IntPtr.Zero;
		
		public static event EventHandler<ApplicationUrlEventArgs> OpenUrls {
			add {
				lock (lockObj) {
					openUrls += value;
					if (openUrlsHandlerRef == IntPtr.Zero)
						openUrlsHandlerRef = Carbon.InstallApplicationEventHandler (HandleOpenUrls,
							new CarbonEventTypeSpec[] {
								//For some reason GetUrl doesn't take CarbonEventClass.AppleEvent
								//need to use GURL, GURL
								new CarbonEventTypeSpec (CarbonEventClass.Internet, (int)CarbonEventApple.GetUrl)
							}
						);
				}
			}
			remove {
				lock (lockObj) {
					openUrls -= value;
					if (openUrls == null && openUrlsHandlerRef != IntPtr.Zero) {
						Carbon.RemoveEventHandler (openUrlsHandlerRef);
						openUrlsHandlerRef = IntPtr.Zero;
					}
				}
			}
		}
		
		static CarbonEventHandlerStatus HandleOpenUrls (IntPtr callRef, IntPtr eventRef, IntPtr user_data)
		{
			try {
				var urls = Carbon.GetUrlListFromEventRef (eventRef);
				var args = new ApplicationUrlEventArgs (urls);
				openUrls (null, args);
				return args.HandledStatus;
			} catch (Exception ex) {
				System.Console.WriteLine (ex);
				return CarbonEventHandlerStatus.NotHandled;
			}
		}
		
		#endregion
	}
	
	public class ApplicationEventArgs : EventArgs
	{
		public bool Handled { get; set; }
		
		internal CarbonEventHandlerStatus HandledStatus {
			get {
				return Handled? CarbonEventHandlerStatus.Handled : CarbonEventHandlerStatus.NotHandled;
			}
		}
	}
	
	public class ApplicationQuitEventArgs : ApplicationEventArgs
	{
		public bool UserCancelled { get; set; }
	}
	
	public class ApplicationDocumentEventArgs : ApplicationEventArgs
	{
		public ApplicationDocumentEventArgs (IDictionary<string,int> documents)
		{
			this.Documents = documents;
		}		
		
		public IDictionary<string,int> Documents { get; private set; }
	}
	
	public class ApplicationUrlEventArgs : ApplicationEventArgs
	{
		public ApplicationUrlEventArgs (IList<string> urls)
		{
			this.Urls = urls;
		}		
		
		public IList<string> Urls { get; private set; }
	}
}

