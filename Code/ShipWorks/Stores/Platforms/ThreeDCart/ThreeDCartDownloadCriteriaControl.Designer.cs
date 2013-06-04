namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    partial class ThreeDCartDownloadCriteriaControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThreeDCartDownloadCriteriaControl));
            this.checkBoxDownloadModifiedOrders = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.downloadModifiedOrdersNumberOfDays = new System.Windows.Forms.ComboBox();
            this.labelDownloadModifiedNumberOfDays = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // checkBoxDownloadModifiedOrders
            // 
            this.checkBoxDownloadModifiedOrders.AutoSize = true;
            this.checkBoxDownloadModifiedOrders.Location = new System.Drawing.Point(10, 11);
            this.checkBoxDownloadModifiedOrders.Name = "checkBoxDownloadModifiedOrders";
            this.checkBoxDownloadModifiedOrders.Size = new System.Drawing.Size(205, 17);
            this.checkBoxDownloadModifiedOrders.TabIndex = 31;
            this.checkBoxDownloadModifiedOrders.Text = "Check for modified or skipped orders.";
            this.checkBoxDownloadModifiedOrders.UseVisualStyleBackColor = true;
            this.checkBoxDownloadModifiedOrders.CheckedChanged += new System.EventHandler(this.OnCheckBoxDownloadModifiedOrdersCheckedChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.DimGray;
            this.label4.Location = new System.Drawing.Point(31, 62);
            this.label4.MaximumSize = new System.Drawing.Size(450, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(443, 39);
            this.label4.TabIndex = 30;
            this.label4.Text = resources.GetString("label4.Text");
            // 
            // downloadModifiedOrdersNumberOfDays
            // 
            this.downloadModifiedOrdersNumberOfDays.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.downloadModifiedOrdersNumberOfDays.FormattingEnabled = true;
            this.downloadModifiedOrdersNumberOfDays.Location = new System.Drawing.Point(192, 32);
            this.downloadModifiedOrdersNumberOfDays.Name = "downloadModifiedOrdersNumberOfDays";
            this.downloadModifiedOrdersNumberOfDays.Size = new System.Drawing.Size(88, 21);
            this.downloadModifiedOrdersNumberOfDays.TabIndex = 29;
            // 
            // labelDownloadModifiedNumberOfDays
            // 
            this.labelDownloadModifiedNumberOfDays.AutoSize = true;
            this.labelDownloadModifiedNumberOfDays.Location = new System.Drawing.Point(31, 35);
            this.labelDownloadModifiedNumberOfDays.Name = "labelDownloadModifiedNumberOfDays";
            this.labelDownloadModifiedNumberOfDays.Size = new System.Drawing.Size(155, 13);
            this.labelDownloadModifiedNumberOfDays.TabIndex = 28;
            this.labelDownloadModifiedNumberOfDays.Text = "Number of days back to check:";
            // 
            // ThreeDCartDownloadCriteriaControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxDownloadModifiedOrders);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.downloadModifiedOrdersNumberOfDays);
            this.Controls.Add(this.labelDownloadModifiedNumberOfDays);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ThreeDCartDownloadCriteriaControl";
            this.Size = new System.Drawing.Size(489, 112);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxDownloadModifiedOrders;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox downloadModifiedOrdersNumberOfDays;
        private System.Windows.Forms.Label labelDownloadModifiedNumberOfDays;
    }
}
