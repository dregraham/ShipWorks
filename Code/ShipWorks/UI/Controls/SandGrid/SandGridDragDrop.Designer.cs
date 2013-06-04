namespace ShipWorks.UI.Controls.SandGrid
{
    partial class SandGridDragDrop
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.hoverTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // hoverTimer
            // 
            this.hoverTimer.Interval = 1000;
            this.hoverTimer.Tick += new System.EventHandler(this.OnDragHoverTimer);
            // 
            // SandGridTree
            // 
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.OnDragOver);
            this.DragLeave += new System.EventHandler(this.OnDragLeave);
            this.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.OnBeginDrag);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer hoverTimer;
    }
}
