namespace ShipWorks.Stores.Platforms.Amazon
{
    partial class AmazonStoreSettingsControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.weightsCheckedList = new System.Windows.Forms.CheckedListBox();
            this.sectionHeader = new ShipWorks.UI.Controls.SectionTitle();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelWeightSource = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.clearCookieButton = new System.Windows.Forms.Button();
            this.cookieLabel = new System.Windows.Forms.Label();
            this.sectionCookie = new ShipWorks.UI.Controls.SectionTitle();
            this.moveDownButton = new System.Windows.Forms.Button();
            this.moveUpButton = new System.Windows.Forms.Button();
            this.importInventoryControl = new ShipWorks.Stores.Platforms.Amazon.AmazonImportInventoryControl();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(21, 180);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(469, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "Amazon does not provide access to the Shipping weight.  Select the priority ShipW" +
                "orks should give to each weight field when downloading.";
            // 
            // weightsCheckedList
            // 
            this.weightsCheckedList.CheckOnClick = true;
            this.weightsCheckedList.FormattingEnabled = true;
            this.weightsCheckedList.Location = new System.Drawing.Point(47, 214);
            this.weightsCheckedList.Name = "weightsCheckedList";
            this.weightsCheckedList.Size = new System.Drawing.Size(173, 84);
            this.weightsCheckedList.TabIndex = 7;
            // 
            // sectionHeader
            // 
            this.sectionHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.sectionHeader.Location = new System.Drawing.Point(0, 0);
            this.sectionHeader.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
            this.sectionHeader.Name = "sectionHeader";
            this.sectionHeader.Size = new System.Drawing.Size(512, 22);
            this.sectionHeader.TabIndex = 13;
            this.sectionHeader.Text = "Item Weights";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(23, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(464, 28);
            this.label2.TabIndex = 15;
            this.label2.Text = "It is not necessary to complete this step, but without it ShipWorks will not be a" +
                "ble to retrieve product weights or images.";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(23, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(464, 50);
            this.label3.TabIndex = 14;
            this.label3.Text = "To retrieve product images and weights ShipWorks requires the ASIN of each item s" +
                "old.  To find the ASIN ShipWorks uses your Amazon inventory report to map each S" +
                "KU to its ASIN.";
            // 
            // labelWeightSource
            // 
            this.labelWeightSource.AutoSize = true;
            this.labelWeightSource.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelWeightSource.Location = new System.Drawing.Point(21, 162);
            this.labelWeightSource.Name = "labelWeightSource";
            this.labelWeightSource.Size = new System.Drawing.Size(89, 13);
            this.labelWeightSource.TabIndex = 19;
            this.labelWeightSource.Text = "Weight Source";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(22, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Inventory Import";
            // 
            // clearCookieButton
            // 
            this.clearCookieButton.Location = new System.Drawing.Point(29, 383);
            this.clearCookieButton.Name = "clearCookieButton";
            this.clearCookieButton.Size = new System.Drawing.Size(98, 23);
            this.clearCookieButton.TabIndex = 23;
            this.clearCookieButton.Text = "Clear Cookie";
            this.clearCookieButton.UseVisualStyleBackColor = true;
            this.clearCookieButton.Click += new System.EventHandler(this.OnClearCookie);
            // 
            // cookieLabel
            // 
            this.cookieLabel.Location = new System.Drawing.Point(28, 350);
            this.cookieLabel.Name = "cookieLabel";
            this.cookieLabel.Size = new System.Drawing.Size(421, 30);
            this.cookieLabel.TabIndex = 21;
            this.cookieLabel.Text = "Some download problems are caused by issues with the \"Amazon Download Cookie\".  I" +
                "f instructed by Interapptive, you should clear this cookie and try downloading a" +
                "gain.";
            // 
            // sectionCookie
            // 
            this.sectionCookie.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionCookie.Location = new System.Drawing.Point(3, 319);
            this.sectionCookie.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
            this.sectionCookie.Name = "sectionCookie";
            this.sectionCookie.Size = new System.Drawing.Size(512, 22);
            this.sectionCookie.TabIndex = 24;
            this.sectionCookie.Text = "Download Troubleshooting";
            // 
            // moveDownButton
            // 
            this.moveDownButton.Image = global::ShipWorks.Properties.Resources.arrow_down_blue;
            this.moveDownButton.Location = new System.Drawing.Point(226, 239);
            this.moveDownButton.Name = "moveDownButton";
            this.moveDownButton.Size = new System.Drawing.Size(36, 23);
            this.moveDownButton.TabIndex = 6;
            this.moveDownButton.UseVisualStyleBackColor = true;
            this.moveDownButton.Click += new System.EventHandler(this.OnDownClick);
            // 
            // moveUpButton
            // 
            this.moveUpButton.Image = global::ShipWorks.Properties.Resources.arrow_up_blue;
            this.moveUpButton.Location = new System.Drawing.Point(226, 212);
            this.moveUpButton.Name = "moveUpButton";
            this.moveUpButton.Size = new System.Drawing.Size(36, 23);
            this.moveUpButton.TabIndex = 5;
            this.moveUpButton.UseVisualStyleBackColor = true;
            this.moveUpButton.Click += new System.EventHandler(this.OnUpClick);
            // 
            // importInventoryControl
            // 
            this.importInventoryControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.importInventoryControl.Location = new System.Drawing.Point(47, 121);
            this.importInventoryControl.Name = "importInventoryControl";
            this.importInventoryControl.Size = new System.Drawing.Size(129, 24);
            this.importInventoryControl.TabIndex = 25;
            // 
            // AmazonStoreSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.importInventoryControl);
            this.Controls.Add(this.sectionCookie);
            this.Controls.Add(this.clearCookieButton);
            this.Controls.Add(this.cookieLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labelWeightSource);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.sectionHeader);
            this.Controls.Add(this.weightsCheckedList);
            this.Controls.Add(this.moveDownButton);
            this.Controls.Add(this.moveUpButton);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "AmazonStoreSettingsControl";
            this.Size = new System.Drawing.Size(512, 434);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button moveUpButton;
        private System.Windows.Forms.Button moveDownButton;
        private System.Windows.Forms.CheckedListBox weightsCheckedList;
        private ShipWorks.UI.Controls.SectionTitle sectionHeader;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelWeightSource;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button clearCookieButton;
        private System.Windows.Forms.Label cookieLabel;
        private ShipWorks.UI.Controls.SectionTitle sectionCookie;
        private AmazonImportInventoryControl importInventoryControl;
    }
}
