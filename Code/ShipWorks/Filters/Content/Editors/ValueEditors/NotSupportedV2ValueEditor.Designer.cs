namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    partial class NotSupportedV2ValueEditor
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
            this.labelDetails = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelDetails
            // 
            this.labelDetails.AutoSize = true;
            this.labelDetails.ForeColor = System.Drawing.Color.Crimson;
            this.labelDetails.Location = new System.Drawing.Point(3, 6);
            this.labelDetails.Name = "labelDetails";
            this.labelDetails.Size = new System.Drawing.Size(121, 13);
            this.labelDetails.TabIndex = 0;
            this.labelDetails.Text = "Not supported anymore";
            // 
            // NotSupportedV2ValueEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelDetails);
            this.Name = "NotSupportedV2ValueEditor";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelDetails;
    }
}
