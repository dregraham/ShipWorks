namespace ShipWorks.Shipping.UI
{
    partial class RatingPanel
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
            this.rateControlWrapper = new ShipWorks.Shipping.UI.RateControlWrapper();
            this.SuspendLayout();
            // 
            // rateControlWrapper
            // 
            this.rateControlWrapper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rateControlWrapper.Location = new System.Drawing.Point(0, 0);
            this.rateControlWrapper.Name = "rateControlWrapper";
            this.rateControlWrapper.Size = new System.Drawing.Size(502, 153);
            this.rateControlWrapper.TabIndex = 0;
            // 
            // RatingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rateControlWrapper);
            this.Name = "RatingPanel";
            this.Size = new System.Drawing.Size(502, 153);
            this.ResumeLayout(false);

        }

        #endregion

        private RateControlWrapper rateControlWrapper;
    }
}
