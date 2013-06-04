namespace ShipWorks.Stores.Platforms.ThreeDCart.WizardPages
{
    partial class ThreeDCartTimeZonePage
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
            this.timeZoneControl = new ShipWorks.Stores.Platforms.ThreeDCart.ThreeDCartTimeZoneControl();
            this.SuspendLayout();
            // 
            // timeZoneControl
            // 
            this.timeZoneControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeZoneControl.Location = new System.Drawing.Point(8, 8);
            this.timeZoneControl.Name = "timeZoneControl";
            this.timeZoneControl.Size = new System.Drawing.Size(508, 100);
            this.timeZoneControl.TabIndex = 2;
            // 
            // ThreeDCartTimeZonePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.timeZoneControl);
            this.Name = "ThreeDCartTimeZonePage";
            this.Size = new System.Drawing.Size(544, 112);
            this.Title = "ThreeDCart Store Time Zone";
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.ResumeLayout(false);

        }

        #endregion

        private ThreeDCartTimeZoneControl timeZoneControl;
    }
}
