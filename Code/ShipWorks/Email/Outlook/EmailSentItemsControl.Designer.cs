namespace ShipWorks.Email.Outlook
{
    partial class EmailSentItemsControl
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
            this.view = new System.Windows.Forms.Button();
            this.panelMessageControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // delete
            // 
            this.delete.Location = new System.Drawing.Point(5, 46);
            this.delete.TabIndex = 1;
            // 
            // entityGrid
            // 
            this.entityGrid.RowActivated += new Divelements.SandGrid.GridRowEventHandler(this.OnRowActivated);
            // 
            // panelMessageControls
            // 
            this.panelMessageControls.Controls.Add(this.view);
            this.panelMessageControls.Size = new System.Drawing.Size(124, 80);
            this.panelMessageControls.Controls.SetChildIndex(this.delete, 0);
            this.panelMessageControls.Controls.SetChildIndex(this.view, 0);
            // 
            // view
            // 
            this.view.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.view.Image = global::ShipWorks.Properties.Resources.view;
            this.view.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.view.Location = new System.Drawing.Point(5, 17);
            this.view.Name = "view";
            this.view.Size = new System.Drawing.Size(115, 23);
            this.view.TabIndex = 0;
            this.view.Text = "View";
            this.view.UseVisualStyleBackColor = true;
            this.view.Click += new System.EventHandler(this.OnViewMessage);
            // 
            // EmailSentItemsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "EmailSentItemsControl";
            this.panelMessageControls.ResumeLayout(false);
            this.panelMessageControls.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button view;

    }
}
