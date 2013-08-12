namespace ShipWorks.Actions.Tasks.Common.Editors
{
    partial class HitUrlTaskEditor
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
            System.Windows.Forms.FlowLayoutPanel requestUrlPanel;
            System.Windows.Forms.Label requestToLabel;
            System.Windows.Forms.Label sendALabel;
            System.Windows.Forms.TableLayoutPanel verbUrlPanel;
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory1 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            this.urlTextBox = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.verbLabel = new System.Windows.Forms.Label();
            this.asBodyLabel = new System.Windows.Forms.Label();
            requestUrlPanel = new System.Windows.Forms.FlowLayoutPanel();
            requestToLabel = new System.Windows.Forms.Label();
            sendALabel = new System.Windows.Forms.Label();
            verbUrlPanel = new System.Windows.Forms.TableLayoutPanel();
            verbUrlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTemplate
            // 
            this.labelTemplate.Location = new System.Drawing.Point(4, 103);
            this.labelTemplate.Margin = new System.Windows.Forms.Padding(0);
            this.labelTemplate.Size = new System.Drawing.Size(78, 13);
            this.labelTemplate.Text = "Using template";
            // 
            // templateCombo
            // 
            this.templateCombo.Location = new System.Drawing.Point(85, 100);
            this.templateCombo.TabIndex = 20;
            // 
            // requestUrlPanel
            // 
            requestUrlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            requestUrlPanel.AutoSize = true;
            requestUrlPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            requestUrlPanel.Location = new System.Drawing.Point(3, 33);
            requestUrlPanel.Name = "requestUrlPanel";
            requestUrlPanel.Size = new System.Drawing.Size(0, 0);
            requestUrlPanel.TabIndex = 4;
            requestUrlPanel.WrapContents = false;
            // 
            // requestToLabel
            // 
            requestToLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            requestToLabel.AutoSize = true;
            requestToLabel.Location = new System.Drawing.Point(66, 7);
            requestToLabel.Margin = new System.Windows.Forms.Padding(0);
            requestToLabel.Name = "requestToLabel";
            requestToLabel.Size = new System.Drawing.Size(57, 13);
            requestToLabel.TabIndex = 0;
            requestToLabel.Text = "request to";
            requestToLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sendALabel
            // 
            sendALabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            sendALabel.AutoSize = true;
            sendALabel.Location = new System.Drawing.Point(0, 7);
            sendALabel.Margin = new System.Windows.Forms.Padding(0);
            sendALabel.Name = "sendALabel";
            sendALabel.Size = new System.Drawing.Size(40, 13);
            sendALabel.TabIndex = 0;
            sendALabel.Text = "Send a";
            sendALabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // verbUrlPanel
            // 
            verbUrlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            verbUrlPanel.ColumnCount = 4;
            verbUrlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            verbUrlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            verbUrlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            verbUrlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            verbUrlPanel.Controls.Add(this.urlTextBox, 3, 0);
            verbUrlPanel.Controls.Add(sendALabel, 0, 0);
            verbUrlPanel.Controls.Add(this.verbLabel, 1, 0);
            verbUrlPanel.Controls.Add(requestToLabel, 2, 0);
            verbUrlPanel.Location = new System.Drawing.Point(4, 0);
            verbUrlPanel.Name = "verbUrlPanel";
            verbUrlPanel.RowCount = 1;
            verbUrlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            verbUrlPanel.Size = new System.Drawing.Size(501, 27);
            verbUrlPanel.TabIndex = 5;
            // 
            // urlTextBox
            // 
            this.urlTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.urlTextBox.Location = new System.Drawing.Point(126, 3);
            this.urlTextBox.MaxLength = 32767;
            this.urlTextBox.Name = "urlTextBox";
            this.urlTextBox.Size = new System.Drawing.Size(372, 21);
            this.urlTextBox.TabIndex = 2;
            this.urlTextBox.TokenSelectionMode = ShipWorks.Templates.Tokens.TokenSelectionMode.Paste;
            this.urlTextBox.TokenSuggestionFactory = commonTokenSuggestionsFactory1;
            this.urlTextBox.TextChanged += new System.EventHandler(this.OnUrlTextChanged);
            // 
            // verbLabel
            // 
            this.verbLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.verbLabel.AutoSize = true;
            this.verbLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.verbLabel.ForeColor = System.Drawing.Color.Blue;
            this.verbLabel.Location = new System.Drawing.Point(40, 7);
            this.verbLabel.Margin = new System.Windows.Forms.Padding(0);
            this.verbLabel.Name = "verbLabel";
            this.verbLabel.Size = new System.Drawing.Size(26, 13);
            this.verbLabel.TabIndex = 1;
            this.verbLabel.Text = "GET";
            this.verbLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.verbLabel.Click += new System.EventHandler(this.OnClickVerbLabel);
            // 
            // asBodyLabel
            // 
            this.asBodyLabel.AutoSize = true;
            this.asBodyLabel.Location = new System.Drawing.Point(341, 103);
            this.asBodyLabel.Margin = new System.Windows.Forms.Padding(0);
            this.asBodyLabel.Name = "asBodyLabel";
            this.asBodyLabel.Size = new System.Drawing.Size(108, 13);
            this.asBodyLabel.TabIndex = 0;
            this.asBodyLabel.Text = "as the request body.";
            this.asBodyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // HitUrlTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.asBodyLabel);
            this.Controls.Add(verbUrlPanel);
            this.Controls.Add(requestUrlPanel);
            this.Name = "HitUrlTaskEditor";
            this.Size = new System.Drawing.Size(505, 131);
            this.Controls.SetChildIndex(requestUrlPanel, 0);
            this.Controls.SetChildIndex(verbUrlPanel, 0);
            this.Controls.SetChildIndex(this.labelTemplate, 0);
            this.Controls.SetChildIndex(this.templateCombo, 0);
            this.Controls.SetChildIndex(this.asBodyLabel, 0);
            verbUrlPanel.ResumeLayout(false);
            verbUrlPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label verbLabel;
        private System.Windows.Forms.Label asBodyLabel;
        private Templates.Tokens.TemplateTokenTextBox urlTextBox;

    }
}
