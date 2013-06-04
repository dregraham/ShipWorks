namespace ShipWorks.Actions.Tasks
{
    partial class ActionTaskFlowDlg
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelCondition = new System.Windows.Forms.Label();
            this.restrictFilter = new System.Windows.Forms.CheckBox();
            this.labelWhenSkipped = new System.Windows.Forms.Label();
            this.whenError = new System.Windows.Forms.ComboBox();
            this.labelNextStep = new System.Windows.Forms.Label();
            this.whenSuccess = new System.Windows.Forms.ComboBox();
            this.labelWhenSuccess = new System.Windows.Forms.Label();
            this.whenSkipped = new System.Windows.Forms.ComboBox();
            this.labelWhenError = new System.Windows.Forms.Label();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.restrictFilterCombo = new ShipWorks.Filters.Controls.FilterComboBox();
            this.SuspendLayout();
            // 
            // labelCondition
            // 
            this.labelCondition.AutoSize = true;
            this.labelCondition.BackColor = System.Drawing.Color.Transparent;
            this.labelCondition.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelCondition.Location = new System.Drawing.Point(8, 11);
            this.labelCondition.Name = "labelCondition";
            this.labelCondition.Size = new System.Drawing.Size(60, 13);
            this.labelCondition.TabIndex = 0;
            this.labelCondition.Text = "Condition";
            // 
            // restrictFilter
            // 
            this.restrictFilter.AutoSize = true;
            this.restrictFilter.BackColor = System.Drawing.Color.Transparent;
            this.restrictFilter.Location = new System.Drawing.Point(26, 32);
            this.restrictFilter.Name = "restrictFilter";
            this.restrictFilter.Size = new System.Drawing.Size(145, 17);
            this.restrictFilter.TabIndex = 1;
            this.restrictFilter.Text = "Only run if the order is in";
            this.restrictFilter.UseVisualStyleBackColor = false;
            this.restrictFilter.CheckedChanged += new System.EventHandler(this.OnConditionEnabledChanged);
            // 
            // labelWhenSkipped
            // 
            this.labelWhenSkipped.AutoSize = true;
            this.labelWhenSkipped.Location = new System.Drawing.Point(24, 143);
            this.labelWhenSkipped.Name = "labelWhenSkipped";
            this.labelWhenSkipped.Size = new System.Drawing.Size(158, 13);
            this.labelWhenSkipped.TabIndex = 8;
            this.labelWhenSkipped.Text = "When skipped due to condition:";
            // 
            // whenError
            // 
            this.whenError.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.whenError.FormattingEnabled = true;
            this.whenError.Location = new System.Drawing.Point(185, 113);
            this.whenError.Name = "whenError";
            this.whenError.Size = new System.Drawing.Size(274, 21);
            this.whenError.TabIndex = 7;
            // 
            // labelNextStep
            // 
            this.labelNextStep.AutoSize = true;
            this.labelNextStep.BackColor = System.Drawing.Color.Transparent;
            this.labelNextStep.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelNextStep.Location = new System.Drawing.Point(8, 67);
            this.labelNextStep.Name = "labelNextStep";
            this.labelNextStep.Size = new System.Drawing.Size(62, 13);
            this.labelNextStep.TabIndex = 3;
            this.labelNextStep.Text = "Next Step";
            // 
            // whenSuccess
            // 
            this.whenSuccess.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.whenSuccess.FormattingEnabled = true;
            this.whenSuccess.Location = new System.Drawing.Point(185, 86);
            this.whenSuccess.Name = "whenSuccess";
            this.whenSuccess.Size = new System.Drawing.Size(274, 21);
            this.whenSuccess.TabIndex = 5;
            // 
            // labelWhenSuccess
            // 
            this.labelWhenSuccess.AutoSize = true;
            this.labelWhenSuccess.Location = new System.Drawing.Point(36, 89);
            this.labelWhenSuccess.Name = "labelWhenSuccess";
            this.labelWhenSuccess.Size = new System.Drawing.Size(146, 13);
            this.labelWhenSuccess.TabIndex = 4;
            this.labelWhenSuccess.Text = "When completed successfully:";
            // 
            // whenSkipped
            // 
            this.whenSkipped.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.whenSkipped.FormattingEnabled = true;
            this.whenSkipped.Location = new System.Drawing.Point(185, 140);
            this.whenSkipped.Name = "whenSkipped";
            this.whenSkipped.Size = new System.Drawing.Size(274, 21);
            this.whenSkipped.TabIndex = 9;
            // 
            // labelWhenError
            // 
            this.labelWhenError.AutoSize = true;
            this.labelWhenError.Location = new System.Drawing.Point(67, 116);
            this.labelWhenError.Name = "labelWhenError";
            this.labelWhenError.Size = new System.Drawing.Size(115, 13);
            this.labelWhenError.TabIndex = 6;
            this.labelWhenError.Text = "When an error occurs:";
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(303, 185);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 10;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(384, 185);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 11;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // restrictFilterCombo
            // 
            this.restrictFilterCombo.AllowQuickFilter = true;
            this.restrictFilterCombo.DropDownHeight = 300;
            this.restrictFilterCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.restrictFilterCombo.Enabled = false;
            this.restrictFilterCombo.FormattingEnabled = true;
            this.restrictFilterCombo.IntegralHeight = false;
            this.restrictFilterCombo.Location = new System.Drawing.Point(168, 30);
            this.restrictFilterCombo.Name = "restrictFilterCombo";
            this.restrictFilterCombo.Size = new System.Drawing.Size(204, 21);
            this.restrictFilterCombo.TabIndex = 2;
            // 
            // ActionTaskFlowDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(471, 220);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.labelWhenError);
            this.Controls.Add(this.whenSkipped);
            this.Controls.Add(this.whenSuccess);
            this.Controls.Add(this.labelWhenSuccess);
            this.Controls.Add(this.labelNextStep);
            this.Controls.Add(this.whenError);
            this.Controls.Add(this.labelWhenSkipped);
            this.Controls.Add(this.restrictFilterCombo);
            this.Controls.Add(this.labelCondition);
            this.Controls.Add(this.restrictFilter);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ActionTaskFlowDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Task Flow";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ShipWorks.Filters.Controls.FilterComboBox restrictFilterCombo;
        private System.Windows.Forms.Label labelCondition;
        private System.Windows.Forms.CheckBox restrictFilter;
        private System.Windows.Forms.Label labelWhenSkipped;
        private System.Windows.Forms.ComboBox whenError;
        private System.Windows.Forms.Label labelNextStep;
        private System.Windows.Forms.ComboBox whenSuccess;
        private System.Windows.Forms.Label labelWhenSuccess;
        private System.Windows.Forms.ComboBox whenSkipped;
        private System.Windows.Forms.Label labelWhenError;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
    }
}