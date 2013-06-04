namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount
{
    partial class UpsShipmentCharacteristicsControl
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
            this.labelShipmentCharacteristics = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ground = new ShipWorks.UI.Controls.NumericTextBox();
            this.air = new ShipWorks.UI.Controls.NumericTextBox();
            this.labelAir = new System.Windows.Forms.Label();
            this.international = new ShipWorks.UI.Controls.NumericTextBox();
            this.labelInternational = new System.Windows.Forms.Label();
            this.labelInstructions = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelShipmentCharacteristics
            // 
            this.labelShipmentCharacteristics.AutoSize = true;
            this.labelShipmentCharacteristics.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelShipmentCharacteristics.Location = new System.Drawing.Point(0, 0);
            this.labelShipmentCharacteristics.Name = "labelShipmentCharacteristics";
            this.labelShipmentCharacteristics.Size = new System.Drawing.Size(149, 13);
            this.labelShipmentCharacteristics.TabIndex = 0;
            this.labelShipmentCharacteristics.Text = "Shipment Characteristics";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(53, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Ground:";
            // 
            // ground
            // 
            this.ground.Location = new System.Drawing.Point(105, 68);
            this.ground.Name = "ground";
            this.ground.Size = new System.Drawing.Size(100, 21);
            this.ground.TabIndex = 2;
            // 
            // air
            // 
            this.air.Location = new System.Drawing.Point(105, 95);
            this.air.Name = "air";
            this.air.Size = new System.Drawing.Size(100, 21);
            this.air.TabIndex = 4;
            // 
            // labelAir
            // 
            this.labelAir.AutoSize = true;
            this.labelAir.Location = new System.Drawing.Point(75, 98);
            this.labelAir.Name = "labelAir";
            this.labelAir.Size = new System.Drawing.Size(24, 13);
            this.labelAir.TabIndex = 3;
            this.labelAir.Text = "Air:";
            // 
            // international
            // 
            this.international.Location = new System.Drawing.Point(105, 122);
            this.international.Name = "international";
            this.international.Size = new System.Drawing.Size(100, 21);
            this.international.TabIndex = 6;
            // 
            // labelInternational
            // 
            this.labelInternational.AutoSize = true;
            this.labelInternational.Location = new System.Drawing.Point(26, 125);
            this.labelInternational.Name = "labelInternational";
            this.labelInternational.Size = new System.Drawing.Size(73, 13);
            this.labelInternational.TabIndex = 5;
            this.labelInternational.Text = "International:";
            // 
            // labelInstructions
            // 
            this.labelInstructions.Location = new System.Drawing.Point(5, 28);
            this.labelInstructions.Name = "labelInstructions";
            this.labelInstructions.Size = new System.Drawing.Size(529, 37);
            this.labelInstructions.TabIndex = 7;
            this.labelInstructions.Text = "Identify the characteristics of your shipments. How many Ground, Air, or International" +
    " shipments will you have for a week?";
            // 
            // UpsShipmentCharacteristicsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelInstructions);
            this.Controls.Add(this.international);
            this.Controls.Add(this.labelInternational);
            this.Controls.Add(this.air);
            this.Controls.Add(this.labelAir);
            this.Controls.Add(this.ground);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelShipmentCharacteristics);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UpsShipmentCharacteristicsControl";
            this.Size = new System.Drawing.Size(513, 296);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelShipmentCharacteristics;
        private System.Windows.Forms.Label label2;
        private UI.Controls.NumericTextBox ground;
        private UI.Controls.NumericTextBox air;
        private System.Windows.Forms.Label labelAir;
        private UI.Controls.NumericTextBox international;
        private System.Windows.Forms.Label labelInternational;
        private System.Windows.Forms.Label labelInstructions;

    }
}
