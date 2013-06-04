namespace ShipWorks.Shipping.Carriers
{
    partial class MultiSelectReturnsControl
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
            this.labelMulti = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelMulti
            // 
            this.labelMulti.AutoSize = true;
            this.labelMulti.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelMulti.Location = new System.Drawing.Point(3, 3);
            this.labelMulti.Name = "labelMulti";
            this.labelMulti.Size = new System.Drawing.Size(207, 13);
            this.labelMulti.TabIndex = 0;
            this.labelMulti.Text = "(Multiple shipping providers are selected.)";
            // 
            // MultiSelectReturnsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelMulti);
            this.Name = "MultiSelectReturnsControl";
            this.Size = new System.Drawing.Size(422, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelMulti;
    }
}
