namespace ShipWorks.Shipping.Carriers.FedEx
{
    partial class FedExExpressFreightControl
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
            this.freightLoadAndCount = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.freightInsideDelivery = new System.Windows.Forms.CheckBox();
            this.freightBookingNumber = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.labelLoadAndCount = new System.Windows.Forms.Label();
            this.freightInsidePickup = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // freightLoadAndCount
            // 
            this.freightLoadAndCount.Location = new System.Drawing.Point(113, 27);
            this.freightLoadAndCount.Name = "freightLoadAndCount";
            this.freightLoadAndCount.Size = new System.Drawing.Size(77, 20);
            this.freightLoadAndCount.TabIndex = 10;
            // 
            // freightInsideDelivery
            // 
            this.freightInsideDelivery.AutoSize = true;
            this.freightInsideDelivery.BackColor = System.Drawing.Color.White;
            this.freightInsideDelivery.Location = new System.Drawing.Point(96, 29);
            this.freightInsideDelivery.Name = "freightInsideDelivery";
            this.freightInsideDelivery.Size = new System.Drawing.Size(95, 17);
            this.freightInsideDelivery.TabIndex = 9;
            this.freightInsideDelivery.Text = "Inside Delivery";
            this.freightInsideDelivery.UseVisualStyleBackColor = false;
            // 
            // freightBookingNumber
            // 
            this.freightBookingNumber.Location = new System.Drawing.Point(113, 1);
            this.freightBookingNumber.Name = "freightBookingNumber";
            this.freightBookingNumber.Size = new System.Drawing.Size(173, 20);
            this.freightBookingNumber.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(-1, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Booking confirmation:";
            // 
            // labelLoadAndCount
            // 
            this.labelLoadAndCount.AutoSize = true;
            this.labelLoadAndCount.BackColor = System.Drawing.Color.White;
            this.labelLoadAndCount.Location = new System.Drawing.Point(20, 30);
            this.labelLoadAndCount.Name = "labelLoadAndCount";
            this.labelLoadAndCount.Size = new System.Drawing.Size(85, 13);
            this.labelLoadAndCount.TabIndex = 8;
            this.labelLoadAndCount.Text = "Load and count:";
            // 
            // freightInsidePickup
            // 
            this.freightInsidePickup.AutoSize = true;
            this.freightInsidePickup.BackColor = System.Drawing.Color.White;
            this.freightInsidePickup.Location = new System.Drawing.Point(2, 29);
            this.freightInsidePickup.Name = "freightInsidePickup";
            this.freightInsidePickup.Size = new System.Drawing.Size(90, 17);
            this.freightInsidePickup.TabIndex = 7;
            this.freightInsidePickup.Text = "Inside Pickup";
            this.freightInsidePickup.UseVisualStyleBackColor = false;
            // 
            // FedExExpressFreightControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.freightLoadAndCount);
            this.Controls.Add(this.freightInsideDelivery);
            this.Controls.Add(this.freightBookingNumber);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelLoadAndCount);
            this.Controls.Add(this.freightInsidePickup);
            this.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.Name = "FedExExpressFreightControl";
            this.Size = new System.Drawing.Size(487, 50);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UI.Controls.MultiValueTextBox freightLoadAndCount;
        private System.Windows.Forms.CheckBox freightInsideDelivery;
        private UI.Controls.MultiValueTextBox freightBookingNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelLoadAndCount;
        private System.Windows.Forms.CheckBox freightInsidePickup;
    }
}
