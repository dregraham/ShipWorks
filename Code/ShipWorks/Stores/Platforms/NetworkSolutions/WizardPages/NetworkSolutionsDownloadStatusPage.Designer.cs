namespace ShipWorks.Stores.Platforms.NetworkSolutions.WizardPages
{
    partial class NetworkSolutionsDownloadStatusPage
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
            this.statuses = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // statuses
            // 
            this.statuses.CheckOnClick = true;
            this.statuses.FormattingEnabled = true;
            this.statuses.Location = new System.Drawing.Point(38, 77);
            this.statuses.Name = "statuses";
            this.statuses.Size = new System.Drawing.Size(220, 116);
            this.statuses.TabIndex = 21;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(366, 54);
            this.label1.TabIndex = 22;
            this.label1.Text = "ShipWorks downloads Network Solutions orders by their order statuses.  Select the" +
                " order statuses ShipWorks will download each time. \r\n\r\nThis selection can be cha" +
                "nged later.";
            // 
            // NetworkSolutionsDownloadStatusPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statuses);
            this.Description = "Select order statuses to be downloaded.";
            this.Name = "NetworkSolutionsDownloadStatusPage";
            this.Size = new System.Drawing.Size(381, 220);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox statuses;
        private System.Windows.Forms.Label label1;
    }
}
