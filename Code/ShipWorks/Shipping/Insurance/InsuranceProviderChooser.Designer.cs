namespace ShipWorks.Shipping.Insurance
{
    partial class InsuranceProviderChooser
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
            this.carrierMessage = new System.Windows.Forms.Label();
            this.comboProvider = new System.Windows.Forms.ComboBox();
            this.linkShipWorks = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // carrierMessage
            // 
            this.carrierMessage.AutoSize = true;
            this.carrierMessage.ForeColor = System.Drawing.Color.DimGray;
            this.carrierMessage.Location = new System.Drawing.Point(191, 31);
            this.carrierMessage.Name = "carrierMessage";
            this.carrierMessage.Size = new System.Drawing.Size(93, 13);
            this.carrierMessage.TabIndex = 7;
            this.carrierMessage.Text = "(Carrier Message)";
            this.carrierMessage.Visible = false;
            // 
            // comboProvider
            // 
            this.comboProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboProvider.FormattingEnabled = true;
            this.comboProvider.Location = new System.Drawing.Point(1, 2);
            this.comboProvider.Name = "comboProvider";
            this.comboProvider.Size = new System.Drawing.Size(185, 21);
            this.comboProvider.TabIndex = 6;
            this.comboProvider.SelectedIndexChanged += new System.EventHandler(this.OnChangeProvider);
            // 
            // linkShipWorks
            // 
            this.linkShipWorks.AutoSize = true;
            this.linkShipWorks.LinkColor = System.Drawing.Color.CornflowerBlue;
            this.linkShipWorks.Location = new System.Drawing.Point(191, 5);
            this.linkShipWorks.Name = "linkShipWorks";
            this.linkShipWorks.Size = new System.Drawing.Size(109, 13);
            this.linkShipWorks.TabIndex = 8;
            this.linkShipWorks.TabStop = true;
            this.linkShipWorks.Text = "(See how you\'ll save)";
            this.linkShipWorks.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkShipWorks);
            // 
            // InsuranceProviderChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.linkShipWorks);
            this.Controls.Add(this.carrierMessage);
            this.Controls.Add(this.comboProvider);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "InsuranceProviderChooser";
            this.Size = new System.Drawing.Size(464, 51);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label carrierMessage;
        private System.Windows.Forms.ComboBox comboProvider;
        private System.Windows.Forms.LinkLabel linkShipWorks;
    }
}
