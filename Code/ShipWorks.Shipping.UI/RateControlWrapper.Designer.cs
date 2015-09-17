namespace ShipWorks.Shipping.UI
{
    partial class RateControlWrapper
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
            this.rateControl = new ShipWorks.Shipping.Editing.Rating.RateControl();
            this.SuspendLayout();
            // 
            // rateControl
            // 
            this.rateControl.ActionLinkVisible = false;
            this.rateControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rateControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rateControl.Location = new System.Drawing.Point(0, 0);
            this.rateControl.Name = "rateControl";
            this.rateControl.RestrictedRateCount = 5;
            this.rateControl.ShowAllRates = true;
            this.rateControl.ShowSingleRate = false;
            this.rateControl.Size = new System.Drawing.Size(584, 241);
            this.rateControl.TabIndex = 0;
            // 
            // RateControlWrapper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rateControl);
            this.Name = "RateControlWrapper";
            this.Size = new System.Drawing.Size(584, 241);
            this.ResumeLayout(false);

        }

        #endregion

        private Editing.Rating.RateControl rateControl;
    }
}
