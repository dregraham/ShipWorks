namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    partial class Express1SingleSourceControl
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
            ShipWorks.UI.Controls.LinkControl singleSourceLearnMore;
            System.Windows.Forms.Label singleSourceLabel;
            this.singleSourceCheckBox = new System.Windows.Forms.CheckBox();
            singleSourceLearnMore = new ShipWorks.UI.Controls.LinkControl();
            singleSourceLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // singleSourceLearnMore
            // 
            singleSourceLearnMore.AutoSize = true;
            singleSourceLearnMore.Cursor = System.Windows.Forms.Cursors.Hand;
            singleSourceLearnMore.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            singleSourceLearnMore.ForeColor = System.Drawing.Color.Blue;
            singleSourceLearnMore.Location = new System.Drawing.Point(342, 24);
            singleSourceLearnMore.Name = "singleSourceLearnMore";
            singleSourceLearnMore.Size = new System.Drawing.Size(65, 13);
            singleSourceLearnMore.TabIndex = 9;
            singleSourceLearnMore.Text = "(Learn how)";
            singleSourceLearnMore.Click += new System.EventHandler(this.OnSingleSourceLearnMore);
            // 
            // singleSourceCheckBox
            // 
            this.singleSourceCheckBox.AutoSize = true;
            this.singleSourceCheckBox.Location = new System.Drawing.Point(20, 23);
            this.singleSourceCheckBox.Name = "singleSourceCheckBox";
            this.singleSourceCheckBox.Size = new System.Drawing.Size(325, 17);
            this.singleSourceCheckBox.TabIndex = 8;
            this.singleSourceCheckBox.Text = "My Express1 account supports shipping with all USPS services.";
            this.singleSourceCheckBox.UseVisualStyleBackColor = true;
            // 
            // singleSourceLabel
            // 
            singleSourceLabel.AutoSize = true;
            singleSourceLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            singleSourceLabel.Location = new System.Drawing.Point(5, 2);
            singleSourceLabel.Name = "singleSourceLabel";
            singleSourceLabel.Size = new System.Drawing.Size(110, 13);
            singleSourceLabel.TabIndex = 7;
            singleSourceLabel.Text = "Available Services";
            // 
            // Express1SingleSourceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(singleSourceLearnMore);
            this.Controls.Add(this.singleSourceCheckBox);
            this.Controls.Add(singleSourceLabel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Express1SingleSourceControl";
            this.Size = new System.Drawing.Size(421, 49);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected System.Windows.Forms.CheckBox singleSourceCheckBox;
    }
}
