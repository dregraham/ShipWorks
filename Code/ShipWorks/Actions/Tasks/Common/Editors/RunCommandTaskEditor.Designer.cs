namespace ShipWorks.Actions.Tasks.Common.Editors
{
    partial class RunCommandTaskEditor
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
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory1 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            this.executeLabel = new System.Windows.Forms.Label();
            this.tokenizedExecuteCommand = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.shouldTimeout = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timeoutInMinutes = new System.Windows.Forms.NumericUpDown();
            this.labelRun = new System.Windows.Forms.Label();
            this.comboCardinality = new System.Windows.Forms.ComboBox();
            this.panelCardinality = new System.Windows.Forms.Panel();
            this.panelStandard = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutInMinutes)).BeginInit();
            this.panelCardinality.SuspendLayout();
            this.panelStandard.SuspendLayout();
            this.SuspendLayout();
            // 
            // executeLabel
            // 
            this.executeLabel.AutoSize = true;
            this.executeLabel.Location = new System.Drawing.Point(3, 1);
            this.executeLabel.Name = "executeLabel";
            this.executeLabel.Size = new System.Drawing.Size(287, 13);
            this.executeLabel.TabIndex = 1;
            this.executeLabel.Text = "Enter anything you would at a Windows command prompt:";
            // 
            // tokenizedExecuteCommand
            // 
            this.tokenizedExecuteCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tokenizedExecuteCommand.Location = new System.Drawing.Point(6, 19);
            this.tokenizedExecuteCommand.MaxLength = 32767;
            this.tokenizedExecuteCommand.Multiline = true;
            this.tokenizedExecuteCommand.Name = "tokenizedExecuteCommand";
            this.tokenizedExecuteCommand.ShowTokenOptions = false;
            this.tokenizedExecuteCommand.Size = new System.Drawing.Size(432, 105);
            this.tokenizedExecuteCommand.TabIndex = 2;
            this.tokenizedExecuteCommand.TokenSuggestionFactory = commonTokenSuggestionsFactory1;
            // 
            // shouldTimeout
            // 
            this.shouldTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.shouldTimeout.AutoSize = true;
            this.shouldTimeout.Checked = true;
            this.shouldTimeout.CheckState = System.Windows.Forms.CheckState.Checked;
            this.shouldTimeout.Location = new System.Drawing.Point(7, 132);
            this.shouldTimeout.Name = "shouldTimeout";
            this.shouldTimeout.Size = new System.Drawing.Size(197, 17);
            this.shouldTimeout.TabIndex = 3;
            this.shouldTimeout.Text = "Abort if the script takes longer than";
            this.shouldTimeout.UseVisualStyleBackColor = true;
            this.shouldTimeout.CheckedChanged += new System.EventHandler(this.OnShouldTimeoutChecked);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(244, 133);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "minutes";
            // 
            // timeoutInMinutes
            // 
            this.timeoutInMinutes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.timeoutInMinutes.Location = new System.Drawing.Point(201, 131);
            this.timeoutInMinutes.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.timeoutInMinutes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.timeoutInMinutes.Name = "timeoutInMinutes";
            this.timeoutInMinutes.Size = new System.Drawing.Size(42, 21);
            this.timeoutInMinutes.TabIndex = 5;
            this.timeoutInMinutes.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.timeoutInMinutes.ValueChanged += new System.EventHandler(this.OnTimeoutValueChanged);
            // 
            // labelRun
            // 
            this.labelRun.AutoSize = true;
            this.labelRun.Location = new System.Drawing.Point(3, 4);
            this.labelRun.Name = "labelRun";
            this.labelRun.Size = new System.Drawing.Size(97, 13);
            this.labelRun.TabIndex = 6;
            this.labelRun.Text = "Run the command:";
            // 
            // comboCardinality
            // 
            this.comboCardinality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboCardinality.FormattingEnabled = true;
            this.comboCardinality.Items.AddRange(new object[] {
            "Single command",
            "Command for each order in the filter"});
            this.comboCardinality.Location = new System.Drawing.Point(103, 1);
            this.comboCardinality.Name = "comboCardinality";
            this.comboCardinality.Size = new System.Drawing.Size(243, 21);
            this.comboCardinality.TabIndex = 7;
            // 
            // panelCardinality
            // 
            this.panelCardinality.Controls.Add(this.comboCardinality);
            this.panelCardinality.Controls.Add(this.labelRun);
            this.panelCardinality.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCardinality.Location = new System.Drawing.Point(0, 0);
            this.panelCardinality.Name = "panelCardinality";
            this.panelCardinality.Size = new System.Drawing.Size(441, 30);
            this.panelCardinality.TabIndex = 8;
            // 
            // panelStandard
            // 
            this.panelStandard.Controls.Add(this.executeLabel);
            this.panelStandard.Controls.Add(this.tokenizedExecuteCommand);
            this.panelStandard.Controls.Add(this.timeoutInMinutes);
            this.panelStandard.Controls.Add(this.shouldTimeout);
            this.panelStandard.Controls.Add(this.label1);
            this.panelStandard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStandard.Location = new System.Drawing.Point(0, 30);
            this.panelStandard.Name = "panelStandard";
            this.panelStandard.Size = new System.Drawing.Size(441, 157);
            this.panelStandard.TabIndex = 9;
            // 
            // RunCommandTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelStandard);
            this.Controls.Add(this.panelCardinality);
            this.Name = "RunCommandTaskEditor";
            this.Size = new System.Drawing.Size(441, 187);
            ((System.ComponentModel.ISupportInitialize)(this.timeoutInMinutes)).EndInit();
            this.panelCardinality.ResumeLayout(false);
            this.panelCardinality.PerformLayout();
            this.panelStandard.ResumeLayout(false);
            this.panelStandard.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label executeLabel;
        private Templates.Tokens.TemplateTokenTextBox tokenizedExecuteCommand;
        private System.Windows.Forms.CheckBox shouldTimeout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown timeoutInMinutes;
        private System.Windows.Forms.Label labelRun;
        private System.Windows.Forms.ComboBox comboCardinality;
        private System.Windows.Forms.Panel panelCardinality;
        private System.Windows.Forms.Panel panelStandard;

    }
}
