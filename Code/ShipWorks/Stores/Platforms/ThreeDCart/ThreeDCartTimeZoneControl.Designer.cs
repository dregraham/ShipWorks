namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    partial class ThreeDCartTimeZoneControl
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
            this.labelInfo2 = new System.Windows.Forms.Label();
            this.labelInfo1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // timeZone
            //
            this.timeZone.Location = new System.Drawing.Point(24, 40);
            //
            // labelInfo2
            //
            this.labelInfo2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelInfo2.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelInfo2.Location = new System.Drawing.Point(23, 73);
            this.labelInfo2.Name = "labelInfo2";
            this.labelInfo2.Size = new System.Drawing.Size(444, 27);
            this.labelInfo2.TabIndex = 27;
            this.labelInfo2.Text = "This setting can be found in the Store Settings of your 3dcart Admin Area.";
            //
            // labelInfo1
            //
            this.labelInfo1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelInfo1.Location = new System.Drawing.Point(5, 6);
            this.labelInfo1.Name = "labelInfo1";
            this.labelInfo1.Size = new System.Drawing.Size(461, 27);
            this.labelInfo1.TabIndex = 28;
            this.labelInfo1.Text = "To accurately download orders and display order dates, ShipWorks needs to know th" +
    "e timezone that your 3dcart store is configured to use:";
            //
            // ThreeDCartTimeZoneControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelInfo1);
            this.Controls.Add(this.labelInfo2);
            this.Name = "ThreeDCartTimeZoneControl";
            this.Size = new System.Drawing.Size(469, 116);
            this.Controls.SetChildIndex(this.timeZone, 0);
            this.Controls.SetChildIndex(this.labelInfo2, 0);
            this.Controls.SetChildIndex(this.labelInfo1, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelInfo2;
        private System.Windows.Forms.Label labelInfo1;
    }
}
