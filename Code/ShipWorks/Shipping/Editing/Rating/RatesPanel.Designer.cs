using Interapptive.Shared.Messaging;

namespace ShipWorks.Shipping.Editing.Rating
{
    partial class RatesPanel
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
            if (disposing)
            {
                uspsAccountConvertedToken?.Dispose();

                if (components != null)
                {
                    components.Dispose();
                }
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
            this.labelRates = new System.Windows.Forms.Label();
            this.rateControl = new ShipWorks.Shipping.Editing.Rating.RateControl();
            this.SuspendLayout();
            // 
            // labelRates
            // 
            this.labelRates.AutoSize = true;
            this.labelRates.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRates.Location = new System.Drawing.Point(0, 2);
            this.labelRates.Name = "labelRates";
            this.labelRates.Size = new System.Drawing.Size(40, 13);
            this.labelRates.TabIndex = 7;
            this.labelRates.Text = "Rates";
            // 
            // rateControl
            // 
            this.rateControl.ActionLinkVisible = false;
            this.rateControl.AutoHeight = true;
            this.rateControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rateControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rateControl.Location = new System.Drawing.Point(0, 19);
            this.rateControl.Name = "rateControl";
            this.rateControl.ShowAllRates = true;
            this.rateControl.Size = new System.Drawing.Size(476, 165);
            this.rateControl.TabIndex = 0;
            this.rateControl.SizeChanged += new System.EventHandler(this.OnRateControlSizeChanged);
            // 
            // RatesPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelRates);
            this.Controls.Add(this.rateControl);
            this.Name = "RatesPanel";
            this.Size = new System.Drawing.Size(476, 184);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RateControl rateControl;
        private System.Windows.Forms.Label labelRates;
    }
}
