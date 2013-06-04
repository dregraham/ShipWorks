namespace ShipWorks.Data.Administration.SqlServerSetup
{
    partial class RebootRequiredPage
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
            this.openAfterRestart = new System.Windows.Forms.CheckBox();
            this.radioRebootLater = new System.Windows.Forms.RadioButton();
            this.radioRebootNow = new System.Windows.Forms.RadioButton();
            this.labelSuccess = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // openAfterRestart
            // 
            this.openAfterRestart.AutoSize = true;
            this.openAfterRestart.Checked = true;
            this.openAfterRestart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.openAfterRestart.Location = new System.Drawing.Point(76, 66);
            this.openAfterRestart.Name = "openAfterRestart";
            this.openAfterRestart.Size = new System.Drawing.Size(307, 17);
            this.openAfterRestart.TabIndex = 2;
            this.openAfterRestart.Text = "Automatically open ShipWorks after my computer restarts.";
            this.openAfterRestart.UseVisualStyleBackColor = true;
            // 
            // radioRebootLater
            // 
            this.radioRebootLater.AutoSize = true;
            this.radioRebootLater.Location = new System.Drawing.Point(56, 87);
            this.radioRebootLater.Name = "radioRebootLater";
            this.radioRebootLater.Size = new System.Drawing.Size(176, 17);
            this.radioRebootLater.TabIndex = 3;
            this.radioRebootLater.Text = "I will restart my computer later.";
            this.radioRebootLater.UseVisualStyleBackColor = true;
            this.radioRebootLater.CheckedChanged += new System.EventHandler(this.OnChangeRestartNow);
            // 
            // radioRebootNow
            // 
            this.radioRebootNow.AutoSize = true;
            this.radioRebootNow.Checked = true;
            this.radioRebootNow.Location = new System.Drawing.Point(56, 45);
            this.radioRebootNow.Name = "radioRebootNow";
            this.radioRebootNow.Size = new System.Drawing.Size(153, 17);
            this.radioRebootNow.TabIndex = 1;
            this.radioRebootNow.TabStop = true;
            this.radioRebootNow.Text = "Restart my computer now.";
            this.radioRebootNow.UseVisualStyleBackColor = true;
            this.radioRebootNow.CheckedChanged += new System.EventHandler(this.OnChangeRestartNow);
            // 
            // labelSuccess
            // 
            this.labelSuccess.Location = new System.Drawing.Point(24, 9);
            this.labelSuccess.Name = "labelSuccess";
            this.labelSuccess.Size = new System.Drawing.Size(472, 33);
            this.labelSuccess.TabIndex = 0;
            this.labelSuccess.Text = "The installation of {0} was successful, but your computer must be restarted befor" +
                "e continuing.";
            // 
            // RebootRequiredPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.openAfterRestart);
            this.Controls.Add(this.radioRebootLater);
            this.Controls.Add(this.radioRebootNow);
            this.Controls.Add(this.labelSuccess);
            this.Description = "{0} has been installed.";
            this.Name = "RebootRequiredPage";
            this.Size = new System.Drawing.Size(499, 157);
            this.Title = "Install {0}";
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox openAfterRestart;
        private System.Windows.Forms.RadioButton radioRebootLater;
        private System.Windows.Forms.RadioButton radioRebootNow;
        private System.Windows.Forms.Label labelSuccess;
    }
}
