namespace ShipWorks.Shipping.Carriers.BestRate
{
    partial class CounterRateProcessingWizardPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CounterRateProcessingWizardPage));
            this.availableRatePanel = new System.Windows.Forms.Panel();
            this.ignoreCounterRates = new System.Windows.Forms.CheckBox();
            this.moreExpensiveAvailableRate = new System.Windows.Forms.Label();
            this.useAvailableRateLink = new System.Windows.Forms.LinkLabel();
            this.availableRateClickLabel = new System.Windows.Forms.Label();
            this.bestRateDescription = new System.Windows.Forms.Label();
            this.availableRatePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // availableRatePanel
            // 
            this.availableRatePanel.Controls.Add(this.ignoreCounterRates);
            this.availableRatePanel.Controls.Add(this.moreExpensiveAvailableRate);
            this.availableRatePanel.Controls.Add(this.useAvailableRateLink);
            this.availableRatePanel.Controls.Add(this.availableRateClickLabel);
            this.availableRatePanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.availableRatePanel.Location = new System.Drawing.Point(0, 61);
            this.availableRatePanel.Name = "availableRatePanel";
            this.availableRatePanel.Size = new System.Drawing.Size(488, 43);
            this.availableRatePanel.TabIndex = 0;
            // 
            // ignoreCounterRates
            // 
            this.ignoreCounterRates.AutoSize = true;
            this.ignoreCounterRates.Location = new System.Drawing.Point(6, 21);
            this.ignoreCounterRates.Name = "ignoreCounterRates";
            this.ignoreCounterRates.Size = new System.Drawing.Size(358, 17);
            this.ignoreCounterRates.TabIndex = 3;
            this.ignoreCounterRates.Text = "Don\'t help me save money for the rest of the shipments in this batch.";
            this.ignoreCounterRates.UseVisualStyleBackColor = true;
            // 
            // moreExpensiveAvailableRate
            // 
            this.moreExpensiveAvailableRate.AutoSize = true;
            this.moreExpensiveAvailableRate.Location = new System.Drawing.Point(52, 5);
            this.moreExpensiveAvailableRate.Name = "moreExpensiveAvailableRate";
            this.moreExpensiveAvailableRate.Size = new System.Drawing.Size(436, 13);
            this.moreExpensiveAvailableRate.TabIndex = 1;
            this.moreExpensiveAvailableRate.Text = "if you would rather use your existing {ProviderName} account that costs {Amount} " +
    "more.";
            // 
            // useAvailableRateLink
            // 
            this.useAvailableRateLink.AutoSize = true;
            this.useAvailableRateLink.Location = new System.Drawing.Point(27, 5);
            this.useAvailableRateLink.Name = "useAvailableRateLink";
            this.useAvailableRateLink.Size = new System.Drawing.Size(29, 13);
            this.useAvailableRateLink.TabIndex = 2;
            this.useAvailableRateLink.TabStop = true;
            this.useAvailableRateLink.Text = "here";
            this.useAvailableRateLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnUseAvailableRateLinkClicked);
            // 
            // availableRateClickLabel
            // 
            this.availableRateClickLabel.AutoSize = true;
            this.availableRateClickLabel.Location = new System.Drawing.Point(3, 5);
            this.availableRateClickLabel.Name = "availableRateClickLabel";
            this.availableRateClickLabel.Size = new System.Drawing.Size(28, 13);
            this.availableRateClickLabel.TabIndex = 0;
            this.availableRateClickLabel.Text = "Click";
            // 
            // bestRateDescription
            // 
            this.bestRateDescription.Location = new System.Drawing.Point(3, 0);
            this.bestRateDescription.Name = "bestRateDescription";
            this.bestRateDescription.Size = new System.Drawing.Size(484, 52);
            this.bestRateDescription.TabIndex = 1;
            this.bestRateDescription.Text = resources.GetString("bestRateDescription.Text");
            // 
            // CounterRateProcessingWizardPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bestRateDescription);
            this.Controls.Add(this.availableRatePanel);
            this.Description = "Sign up for {ProviderName} account";
            this.Name = "CounterRateProcessingWizardPage";
            this.Size = new System.Drawing.Size(501, 349);
            this.Title = "ShipWorks found a way to save you money!";
            this.availableRatePanel.ResumeLayout(false);
            this.availableRatePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel availableRatePanel;
        private System.Windows.Forms.CheckBox ignoreCounterRates;
        private System.Windows.Forms.Label moreExpensiveAvailableRate;
        private System.Windows.Forms.LinkLabel useAvailableRateLink;
        private System.Windows.Forms.Label availableRateClickLabel;
        private System.Windows.Forms.Label bestRateDescription;
    }
}
