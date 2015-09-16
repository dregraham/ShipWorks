using Interapptive.Shared.Messaging;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.UI
{
    partial class RatingPanel
    {
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
            this.rateControl.AutoHeight = true;
            this.rateControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rateControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rateControl.Location = new System.Drawing.Point(0, 0);
            this.rateControl.Name = "rateControl";
            this.rateControl.ShowAllRates = true;
            this.rateControl.Size = new System.Drawing.Size(476, 165);
            this.rateControl.TabIndex = 0;
            // 
            // RatesPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rateControl);
            this.Name = "RatesPanel";
            this.Size = new System.Drawing.Size(476, 184);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private RateControl rateControl;
    }
}
