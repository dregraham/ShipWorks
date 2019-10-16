namespace ShipWorks.Actions.Tasks.Common.Editors
{
    partial class CreateLabelTaskEditor
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
            this.allowMultiShipmentsLabel = new System.Windows.Forms.Label();
            this.allowMultiShipments = new System.Windows.Forms.CheckBox();
            this.allowProcessedShipmentsLabel = new System.Windows.Forms.Label();
            this.allowProcessedShipments = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // allowMultiShipmentsLabel
            // 
            this.allowMultiShipmentsLabel.AutoSize = true;
            this.allowMultiShipmentsLabel.Location = new System.Drawing.Point(3, 3);
            this.allowMultiShipmentsLabel.Name = "allowMultiShipmentsLabel";
            this.allowMultiShipmentsLabel.Size = new System.Drawing.Size(42, 13);
            this.allowMultiShipmentsLabel.TabIndex = 1;
            this.allowMultiShipmentsLabel.Text = "Create labels for orders with multiple shipments";
            // 
            // allowMultiShipments
            // 
            this.allowMultiShipments.AutoSize = true;
            this.allowMultiShipments.Location = new System.Drawing.Point(3, 3);
            this.allowMultiShipments.Name = "allowMultiShipments";
            this.allowMultiShipments.Size = new System.Drawing.Size(42, 13);
            this.allowMultiShipments.TabIndex = 0;
            // 
            // allowProcessedShipmentsLabel
            // 
            this.allowProcessedShipmentsLabel.AutoSize = true;
            this.allowProcessedShipmentsLabel.Location = new System.Drawing.Point(3, 3);
            this.allowProcessedShipmentsLabel.Name = "allowProcessedShipmentsLabel";
            this.allowProcessedShipmentsLabel.Size = new System.Drawing.Size(42, 13);
            this.allowProcessedShipmentsLabel.TabIndex = 3;
            this.allowProcessedShipmentsLabel.Text = "Create labels for orders with already processed shipments";
            // 
            // allowProcessedShipments
            // 
            this.allowProcessedShipments.AutoSize = true;
            this.allowProcessedShipments.Location = new System.Drawing.Point(3, 3);
            this.allowProcessedShipments.Name = "allowProcessedShipments";
            this.allowProcessedShipments.Size = new System.Drawing.Size(42, 13);
            this.allowProcessedShipments.TabIndex = 2;
            this.allowProcessedShipments.Text = "Status:";
            // 
            // CreateLabelTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.allowMultiShipmentsLabel);
            this.Controls.Add(this.allowMultiShipments);
            this.Controls.Add(this.allowProcessedShipmentsLabel);
            this.Controls.Add(this.allowProcessedShipments);
            this.Name = "CreateLabelTaskEditor";
            this.Size = new System.Drawing.Size(299, 26);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label allowMultiShipmentsLabel;
        private System.Windows.Forms.CheckBox allowMultiShipments;
        private System.Windows.Forms.Label allowProcessedShipmentsLabel;
        private System.Windows.Forms.CheckBox allowProcessedShipments;
    }
}
