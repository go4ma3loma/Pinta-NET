using System;
using Pinta.Core;
using Pinta.Docking.Gui;

namespace Pinta
{
    class DocumentViewContent : IViewContent
    {
        private CanvasWindow canvas_window;

        public Document Document { get; private set; }

        public DocumentViewContent (Document document, CanvasWindow canvasWindow)
        {
            this.Document = document;
            this.canvas_window = canvasWindow;

            document.IsDirtyChanged += (o, e) => IsDirty = document.IsDirty;
            document.Renamed += (o, e) => { if (ContentNameChanged != null) ContentNameChanged (this, EventArgs.Empty); };
        }

        #region IViewContent Members
        public event EventHandler ContentNameChanged;
        public event EventHandler ContentChanged;
        public event EventHandler DirtyChanged;
        public event EventHandler BeforeSave;

        public string ContentName {
            get { return Document.Filename; }
            set { Document.Filename = value; }
        }

        public string UntitledName { get; set; }

        // We don't put icons on the tabs
        public string StockIconId {
            get { return string.Empty; }
        }

        public bool IsUntitled {
            get { return false; }
        }

        public bool IsViewOnly {
            get { return false; }
        }

        public bool IsFile {
            get { return true; }
        }

        public bool IsDirty {
            get { return Document.IsDirty; }
            set {
                if (DirtyChanged != null)
                    DirtyChanged (this, EventArgs.Empty);
            }
        }

        // can remove?
        public bool IsReadOnly {
            get { return false; }
        }

        public void Load (string fileName)
        {
        }

        public void LoadNew (System.IO.Stream content, string mimeType)
        {
        }

        public void Save (string fileName)
        {
        }

        public void Save ()
        {
        }

        public void DiscardChanges ()
        {
        }
        #endregion

        #region IBaseViewContent Members
        public IWorkbenchWindow WorkbenchWindow { get; set; }

        public Gtk.Widget Control {
            get { return canvas_window; }
        }

        public string TabPageLabel {
            get { return string.Empty; }
        }

        public object GetContent (Type type)
        {
            return null;
        }

        public bool CanReuseView (string fileName)
        {
            return false;
        }

        public void RedrawContent ()
        {
        }
        #endregion

        #region IDisposable Members
        public void Dispose ()
        {
            if (canvas_window != null)
                canvas_window.Dispose ();
        }
        #endregion
    }
}
