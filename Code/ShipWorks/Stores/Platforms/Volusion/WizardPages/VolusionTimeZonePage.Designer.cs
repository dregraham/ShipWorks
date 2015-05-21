namespace ShipWorks.Stores.Platforms.Volusion.WizardPages
{
    partial class VolusionTimeZonePage
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
            this.timeZoneControl = new ShipWorks.Stores.Platforms.Volusion.VolusionTimeZoneControl();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.statuses = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timeZoneControl
            // 
            this.timeZoneControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeZoneControl.Location = new System.Drawing.Point(8, 16);
            this.timeZoneControl.Name = "timeZoneControl";
            this.timeZoneControl.Size = new System.Drawing.Size(508, 100);
            this.timeZoneControl.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 147);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(366, 54);
            this.label2.TabIndex = 42;
            this.label2.Text = "ShipWorks downloads Volusion orders by their order statuses.  Select the order st" +
    "atuses ShipWorks will download each time. \r\n\r\nThis selection can be changed late" +
    "r.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 13);
            this.label1.TabIndex = 41;
            this.label1.Text = "Download Order Statuses";
            // 
            // statuses
            // 
            this.statuses.CheckOnClick = true;
            this.statuses.FormattingEnabled = true;
            this.statuses.Location = new System.Drawing.Point(48, 204);
            this.statuses.Name = "statuses";
            this.statuses.Size = new System.Drawing.Size(216, 116);
            this.statuses.TabIndex = 40;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 13);
            this.label3.TabIndex = 43;
            this.label3.Text = "Volusion Time Zone";
            // 
            // VolusionTimeZonePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statuses);
            this.Controls.Add(this.timeZoneControl);
            this.Name = "VolusionTimeZonePage";
            this.Size = new System.Drawing.Size(525, 333);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private VolusionTimeZoneControl timeZoneControl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox statuses;
        private System.Windows.Forms.Label label3;
    }
}
