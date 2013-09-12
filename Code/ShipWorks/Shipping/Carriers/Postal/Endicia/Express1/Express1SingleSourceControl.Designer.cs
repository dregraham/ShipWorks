namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
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
            this.singleSourceLearnMore = new ShipWorks.UI.Controls.LinkControl();
            this.checkBoxExpress1SingleSource = new System.Windows.Forms.CheckBox();
            this.labelExpress1SingleSource = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // singleSourceLearnMore
            // 
            this.singleSourceLearnMore.AutoSize = true;
            this.singleSourceLearnMore.Cursor = System.Windows.Forms.Cursors.Hand;
            this.singleSourceLearnMore.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.singleSourceLearnMore.ForeColor = System.Drawing.Color.Blue;
            this.singleSourceLearnMore.Location = new System.Drawing.Point(342, 24);
            this.singleSourceLearnMore.Name = "singleSourceLearnMore";
            this.singleSourceLearnMore.Size = new System.Drawing.Size(65, 13);
            this.singleSourceLearnMore.TabIndex = 9;
            this.singleSourceLearnMore.Text = "(Learn how)";
            this.singleSourceLearnMore.Click += new System.EventHandler(this.OnSingleSourceLearnMore);
            // 
            // checkBoxExpress1SingleSource
            // 
            this.checkBoxExpress1SingleSource.AutoSize = true;
            this.checkBoxExpress1SingleSource.Location = new System.Drawing.Point(20, 23);
            this.checkBoxExpress1SingleSource.Name = "checkBoxExpress1SingleSource";
            this.checkBoxExpress1SingleSource.Size = new System.Drawing.Size(325, 17);
            this.checkBoxExpress1SingleSource.TabIndex = 8;
            this.checkBoxExpress1SingleSource.Text = "My Express1 account supports shipping with all USPS services.";
            this.checkBoxExpress1SingleSource.UseVisualStyleBackColor = true;
            // 
            // labelExpress1SingleSource
            // 
            this.labelExpress1SingleSource.AutoSize = true;
            this.labelExpress1SingleSource.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelExpress1SingleSource.Location = new System.Drawing.Point(5, 2);
            this.labelExpress1SingleSource.Name = "labelExpress1SingleSource";
            this.labelExpress1SingleSource.Size = new System.Drawing.Size(110, 13);
            this.labelExpress1SingleSource.TabIndex = 7;
            this.labelExpress1SingleSource.Text = "Available Services";
            // 
            // Express1SingleSourceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.singleSourceLearnMore);
            this.Controls.Add(this.checkBoxExpress1SingleSource);
            this.Controls.Add(this.labelExpress1SingleSource);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "Express1SingleSourceControl";
            this.Size = new System.Drawing.Size(421, 49);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UI.Controls.LinkControl singleSourceLearnMore;
        private System.Windows.Forms.CheckBox checkBoxExpress1SingleSource;
        private System.Windows.Forms.Label labelExpress1SingleSource;
    }
}
