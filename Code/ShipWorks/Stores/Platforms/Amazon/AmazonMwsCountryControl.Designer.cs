namespace ShipWorks.Stores.Platforms.Amazon
{
    partial class AmazonMwsCountryControl
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
            this.countries = new System.Windows.Forms.ComboBox();
            this.selectCountryLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // countries
            // 
            this.countries.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.countries.FormattingEnabled = true;
            this.countries.Location = new System.Drawing.Point(36, 33);
            this.countries.Name = "countries";
            this.countries.Size = new System.Drawing.Size(208, 21);
            this.countries.TabIndex = 3;
            // 
            // selectCountryLabel
            // 
            this.selectCountryLabel.AutoSize = true;
            this.selectCountryLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectCountryLabel.Location = new System.Drawing.Point(23, 13);
            this.selectCountryLabel.Name = "selectCountryLabel";
            this.selectCountryLabel.Size = new System.Drawing.Size(205, 13);
            this.selectCountryLabel.TabIndex = 2;
            this.selectCountryLabel.Text = "What country are you selling from?";
            // 
            // AmazonMwsCountryControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.countries);
            this.Controls.Add(this.selectCountryLabel);
            this.Name = "AmazonMwsCountryControl";
            this.Size = new System.Drawing.Size(311, 68);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox countries;
        private System.Windows.Forms.Label selectCountryLabel;

    }
}
