using System.Drawing;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    partial class UspsProfileControl
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
            this.rateShop = new System.Windows.Forms.CheckBox();
            this.labelRateShop = new System.Windows.Forms.Label();
            this.stateRateShop = new System.Windows.Forms.CheckBox();
            this.groupLabels.SuspendLayout();
            this.groupBoxFrom.SuspendLayout();
            this.groupShipment.SuspendLayout();
            this.tabPage.SuspendLayout();
            this.groupReturns.SuspendLayout();
            this.groupInsurance.SuspendLayout();
            this.groupExpressMail.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupTo
            // 
            this.groupTo.Location = new System.Drawing.Point(9, 108);
            this.groupTo.Size = new System.Drawing.Size(417, 52);
            // 
            // groupLabels
            // 
            this.groupLabels.Location = new System.Drawing.Point(9, 442);
            this.groupLabels.Size = new System.Drawing.Size(417, 58);
            // 
            // groupBoxFrom
            // 
            this.groupBoxFrom.Controls.Add(this.rateShop);
            this.groupBoxFrom.Controls.Add(this.labelRateShop);
            this.groupBoxFrom.Controls.Add(this.stateRateShop);
            this.groupBoxFrom.Size = new System.Drawing.Size(417, 96);
            this.groupBoxFrom.Controls.SetChildIndex(this.senderState, 0);
            this.groupBoxFrom.Controls.SetChildIndex(this.originCombo, 0);
            this.groupBoxFrom.Controls.SetChildIndex(this.labelSender, 0);
            this.groupBoxFrom.Controls.SetChildIndex(this.kryptonBorderEdge1, 0);
            this.groupBoxFrom.Controls.SetChildIndex(this.stateRateShop, 0);
            this.groupBoxFrom.Controls.SetChildIndex(this.labelRateShop, 0);
            this.groupBoxFrom.Controls.SetChildIndex(this.rateShop, 0);
            // 
            // groupShipment
            // 
            this.groupShipment.Location = new System.Drawing.Point(9, 166);
            this.groupShipment.Size = new System.Drawing.Size(417, 270);
            // 
            // groupBoxCustoms
            // 
            this.groupBoxCustoms.Location = new System.Drawing.Point(9, 593);
            this.groupBoxCustoms.Size = new System.Drawing.Size(417, 54);
            // 
            // groupReturns
            // 
            this.groupReturns.Location = new System.Drawing.Point(9, 714);
            this.groupReturns.Size = new Size(417, 53);
            // 
            // groupInsurance
            // 
            this.groupInsurance.Location = new System.Drawing.Point(9, 505);
            this.groupInsurance.Size = new Size(417, 82);
            // 
            // groupExpressMail
            // 
            this.groupExpressMail.Location = new System.Drawing.Point(9, 654);
            this.groupExpressMail.Size = new Size(417, 53);
            // 
            // kryptonBorderEdge1
            // 
            this.kryptonBorderEdge1.Size = new System.Drawing.Size(1, 68);
            // 
            // rateShop
            // 
            this.rateShop.AutoSize = true;
            this.rateShop.Location = new System.Drawing.Point(110, 71);
            this.rateShop.Name = "rateShop";
            this.rateShop.Size = new System.Drawing.Size(188, 17);
            this.rateShop.TabIndex = 20;
            this.rateShop.Text = "Automatically use the USPS account with the least expensive rate";
            this.rateShop.UseVisualStyleBackColor = true;
            // 
            // labelRateShop
            // 
            this.labelRateShop.AutoSize = true;
            this.labelRateShop.Location = new System.Drawing.Point(41, 71);
            this.labelRateShop.Name = "labelRateShop";
            this.labelRateShop.Size = new System.Drawing.Size(61, 13);
            this.labelRateShop.TabIndex = 19;
            this.labelRateShop.Text = "Rate Shop:";
            // 
            // stateRateShop
            // 
            this.stateRateShop.AutoSize = true;
            this.stateRateShop.Checked = true;
            this.stateRateShop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.stateRateShop.Location = new System.Drawing.Point(9, 71);
            this.stateRateShop.Name = "stateRateShop";
            this.stateRateShop.Size = new System.Drawing.Size(15, 14);
            this.stateRateShop.TabIndex = 18;
            this.stateRateShop.UseVisualStyleBackColor = true;
            // 
            // UspsProfileControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UspsProfileControl";
            this.groupLabels.ResumeLayout(false);
            this.groupLabels.PerformLayout();
            this.groupBoxFrom.ResumeLayout(false);
            this.groupBoxFrom.PerformLayout();
            this.groupShipment.ResumeLayout(false);
            this.groupShipment.PerformLayout();
            this.tabPage.ResumeLayout(false);
            this.groupReturns.ResumeLayout(false);
            this.groupReturns.PerformLayout();
            this.groupInsurance.ResumeLayout(false);
            this.groupInsurance.PerformLayout();
            this.groupExpressMail.ResumeLayout(false);
            this.groupExpressMail.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox rateShop;
        private System.Windows.Forms.Label labelRateShop;
        private System.Windows.Forms.CheckBox stateRateShop;
    }
}
