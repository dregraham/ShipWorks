namespace ShipWorks.Actions.Tasks.Common.Editors
{
    partial class DownloadOrdersTaskEditor
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
            this.taskDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // taskDescription
            // 
            this.taskDescription.AutoSize = true;
            this.taskDescription.Location = new System.Drawing.Point(4, 4);
            this.taskDescription.Name = "taskDescription";
            this.taskDescription.Size = new System.Drawing.Size(376, 13);
            this.taskDescription.TabIndex = 0;
            this.taskDescription.Text = "Use the Settings tab to select the stores that orders should be downloaded for.";
            // 
            // DownloadOrdersTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.taskDescription);
            this.Name = "DownloadOrdersTaskEditor";
            this.Size = new System.Drawing.Size(397, 26);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label taskDescription;
    }
}
