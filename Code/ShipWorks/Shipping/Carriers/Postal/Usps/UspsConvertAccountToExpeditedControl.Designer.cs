namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    partial class UspsConvertAccountToExpeditedControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UspsConvertAccountToExpeditedControl));
            this.labelConversionDescription = new System.Windows.Forms.Label();
            this.labelHeader = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.learnMore = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // labelConversionDescription
            // 
            this.labelConversionDescription.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelConversionDescription.Location = new System.Drawing.Point(26, 29);
            this.labelConversionDescription.Name = "labelConversionDescription";
            this.labelConversionDescription.Size = new System.Drawing.Size(378, 82);
            this.labelConversionDescription.TabIndex = 0;
            this.labelConversionDescription.Text = resources.GetString("labelConversionDescription.Text");
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHeader.Location = new System.Drawing.Point(7, 4);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(105, 13);
            this.labelHeader.TabIndex = 3;
            this.labelHeader.Text = "Postage Discount";
            // 
            // linkLabel1
            // 
            this.linkLabel1.Location = new System.Drawing.Point(26, 140);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(378, 32);
            this.linkLabel1.TabIndex = 4;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Click here to add these discounted rates from IntuiShip through your existing Sta" +
    "mps.com account at no additional cost.";
            this.linkLabel1.Click += new System.EventHandler(this.OnStartSaving);
            // 
            // learnMore
            // 
            this.learnMore.AutoSize = true;
            this.learnMore.Location = new System.Drawing.Point(26, 111);
            this.learnMore.Name = "learnMore";
            this.learnMore.Size = new System.Drawing.Size(69, 13);
            this.learnMore.TabIndex = 5;
            this.learnMore.TabStop = true;
            this.learnMore.Text = "(Learn more)";
            this.learnMore.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLearnMore);
            // 
            // UspsConvertAccountToExpeditedControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.learnMore);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.labelHeader);
            this.Controls.Add(this.labelConversionDescription);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UspsConvertAccountToExpeditedControl";
            this.Size = new System.Drawing.Size(407, 185);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelConversionDescription;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel learnMore;
    }
}
