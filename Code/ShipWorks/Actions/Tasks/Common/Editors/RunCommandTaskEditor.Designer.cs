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
            ((System.ComponentModel.ISupportInitialize)(this.timeoutInMinutes)).BeginInit();
            this.SuspendLayout();
            // 
            // executeLabel
            // 
            this.executeLabel.AutoSize = true;
            this.executeLabel.Location = new System.Drawing.Point(3, 3);
            this.executeLabel.Name = "executeLabel";
            this.executeLabel.Size = new System.Drawing.Size(137, 13);
            this.executeLabel.TabIndex = 1;
            this.executeLabel.Text = "With the program or script:";
            // 
            // tokenizedExecuteCommand
            // 
            this.tokenizedExecuteCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tokenizedExecuteCommand.Location = new System.Drawing.Point(3, 19);
            this.tokenizedExecuteCommand.MaxLength = 32767;
            this.tokenizedExecuteCommand.Multiline = true;
            this.tokenizedExecuteCommand.Name = "tokenizedExecuteCommand";
            this.tokenizedExecuteCommand.Size = new System.Drawing.Size(435, 98);
            this.tokenizedExecuteCommand.TabIndex = 2;
            this.tokenizedExecuteCommand.TokenSuggestionFactory = commonTokenSuggestionsFactory1;
            // 
            // shouldTimeout
            // 
            this.shouldTimeout.AutoSize = true;
            this.shouldTimeout.Checked = true;
            this.shouldTimeout.CheckState = System.Windows.Forms.CheckState.Checked;
            this.shouldTimeout.Location = new System.Drawing.Point(6, 127);
            this.shouldTimeout.Name = "shouldTimeout";
            this.shouldTimeout.Size = new System.Drawing.Size(211, 17);
            this.shouldTimeout.TabIndex = 3;
            this.shouldTimeout.Text = "Stop if the command takes longer than";
            this.shouldTimeout.UseVisualStyleBackColor = true;
            this.shouldTimeout.CheckedChanged += new System.EventHandler(this.OnShouldTimeoutChecked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(264, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "minutes";
            // 
            // timeoutInMinutes
            // 
            this.timeoutInMinutes.Location = new System.Drawing.Point(216, 123);
            this.timeoutInMinutes.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.timeoutInMinutes.Name = "timeoutInMinutes";
            this.timeoutInMinutes.Size = new System.Drawing.Size(42, 21);
            this.timeoutInMinutes.TabIndex = 5;
            this.timeoutInMinutes.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.timeoutInMinutes.ValueChanged += new System.EventHandler(this.OnTimeoutValueChanged);
            // 
            // RunCommandTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.timeoutInMinutes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.shouldTimeout);
            this.Controls.Add(this.tokenizedExecuteCommand);
            this.Controls.Add(this.executeLabel);
            this.Name = "RunCommandTaskEditor";
            this.Size = new System.Drawing.Size(441, 154);
            ((System.ComponentModel.ISupportInitialize)(this.timeoutInMinutes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label executeLabel;
        private Templates.Tokens.TemplateTokenTextBox tokenizedExecuteCommand;
        private System.Windows.Forms.CheckBox shouldTimeout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown timeoutInMinutes;

    }
}
