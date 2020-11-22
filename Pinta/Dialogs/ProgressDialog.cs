using System;
using Pinta.Core;
using Gtk;

namespace Pinta
{
    public class ProgressDialog : Dialog, IProgressDialog
    {
        private Label label;
        private ProgressBar progress_bar;
        uint timeout_id;

        public ProgressDialog ()
            : base (string.Empty, PintaCore.Chrome.MainWindow, DialogFlags.Modal)
        {
            WindowPosition = WindowPosition.CenterOnParent;

            this.Build ();
            timeout_id = 0;
            Hide ();
        }

        public string Text
        {
            get { return label.Text; }
            set { label.Text = value; }
        }

        public double Progress
        {
            get { return progress_bar.Fraction; }
            set { progress_bar.Fraction = value; }
        }

        public event EventHandler<EventArgs> Canceled;

        void IProgressDialog.Show ()
        {
            timeout_id = GLib.Timeout.Add (500, () => {
                this.ShowAll ();
                timeout_id = 0;
                return false;
            });
        }

        void IProgressDialog.Hide ()
        {
            if (timeout_id != 0)
                GLib.Source.Remove (timeout_id);
            this.Hide ();
        }

        protected override void OnResponse (Gtk.ResponseType response_id)
        {
            if (Canceled != null)
                Canceled (this, EventArgs.Empty);
        }

        private void Build ()
        {
            VBox.BorderWidth = 2;
            VBox.Spacing = 6;

            label = new Label ();
            VBox.Add (label);

            progress_bar = new ProgressBar ();
            VBox.Add (progress_bar);

            AddButton (Gtk.Stock.Cancel, Gtk.ResponseType.Cancel);

            DefaultWidth = 400;
            DefaultHeight = 114;
        }
    }
}
