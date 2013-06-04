namespace ShipWorks.Stores.Platforms.Ebay.WizardPages
{
    partial class EBayOptionsPage
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
            this.downloadDetails = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // downloadDetails
            // 
            this.downloadDetails.AutoSize = true;
            this.downloadDetails.Location = new System.Drawing.Point(20, 15);
            this.downloadDetails.Name = "downloadDetails";
            this.downloadDetails.Size = new System.Drawing.Size(285, 17);
            this.downloadDetails.TabIndex = 4;
            this.downloadDetails.Text = "Download image URLs (to display images in templates)";
            this.downloadDetails.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.DimGray;
            this.label4.Location = new System.Drawing.Point(33, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(299, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "(This will add a significant amount of time to each download.)";
            // 
            // EBayTimeSpanPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.downloadDetails);
            this.Description = "Please enter the information below.";
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "EBayTimeSpanPage";
            this.Size = new System.Drawing.Size(615, 300);
            this.Title = "eBay Setup";
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox downloadDetails;
        private System.Windows.Forms.Label label4;
    }
}
