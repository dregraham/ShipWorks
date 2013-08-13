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
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory2 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            System.Windows.Forms.Label labelHeaders;
            System.Windows.Forms.TableLayoutPanel authPanel;
            System.Windows.Forms.Label authLabelPrefix;
            System.Windows.Forms.Label authLabelSuffix;
            System.Windows.Forms.Label basicAuthUserLabel;
            System.Windows.Forms.Label basicAuthPasswordLabel;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HitUrlTaskEditor));
            this.urlTextBox = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.verbLabel = new System.Windows.Forms.Label();
            this.authLabel = new System.Windows.Forms.Label();
            this.basicAuthPanel = new System.Windows.Forms.TableLayoutPanel();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.userNameTextBox = new System.Windows.Forms.TextBox();
            this.asBodyLabel = new System.Windows.Forms.Label();
            this.headersGrid = new ShipWorks.UI.Controls.NameValueGrid();
            this.headersPanel = new System.Windows.Forms.TableLayoutPanel();
            requestUrlPanel = new System.Windows.Forms.FlowLayoutPanel();
            requestToLabel = new System.Windows.Forms.Label();
            sendALabel = new System.Windows.Forms.Label();
            verbUrlPanel = new System.Windows.Forms.TableLayoutPanel();
            labelHeaders = new System.Windows.Forms.Label();
            authPanel = new System.Windows.Forms.TableLayoutPanel();
            authLabelPrefix = new System.Windows.Forms.Label();
            authLabelSuffix = new System.Windows.Forms.Label();
            basicAuthUserLabel = new System.Windows.Forms.Label();
            basicAuthPasswordLabel = new System.Windows.Forms.Label();
            verbUrlPanel.SuspendLayout();
            authPanel.SuspendLayout();
            this.basicAuthPanel.SuspendLayout();
            this.headersPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTemplate
            // 
            this.labelTemplate.Location = new System.Drawing.Point(4, 185);
            this.labelTemplate.Margin = new System.Windows.Forms.Padding(0);
            this.labelTemplate.Size = new System.Drawing.Size(78, 13);
            this.labelTemplate.Text = "Using template";
            // 
            // templateCombo
            // 
            this.templateCombo.Location = new System.Drawing.Point(85, 182);
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
            verbUrlPanel.Location = new System.Drawing.Point(4, -2);
            verbUrlPanel.Name = "verbUrlPanel";
            verbUrlPanel.RowCount = 1;
            verbUrlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            verbUrlPanel.Size = new System.Drawing.Size(518, 27);
            verbUrlPanel.TabIndex = 5;
            // 
            // urlTextBox
            // 
            this.urlTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.urlTextBox.Location = new System.Drawing.Point(126, 3);
            this.urlTextBox.MaxLength = 32767;
            this.urlTextBox.Name = "urlTextBox";
            this.urlTextBox.Size = new System.Drawing.Size(389, 21);
            this.urlTextBox.TabIndex = 2;
            this.urlTextBox.TokenSelectionMode = ShipWorks.Templates.Tokens.TokenSelectionMode.Paste;
            this.urlTextBox.TokenSuggestionFactory = commonTokenSuggestionsFactory2;
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
            this.verbLabel.Click += new System.EventHandler(this.OnVerbLabelClick);
            // 
            // labelHeaders
            // 
            labelHeaders.Anchor = System.Windows.Forms.AnchorStyles.None;
            labelHeaders.AutoSize = true;
            labelHeaders.Location = new System.Drawing.Point(0, 53);
            labelHeaders.Margin = new System.Windows.Forms.Padding(0);
            labelHeaders.Name = "labelHeaders";
            labelHeaders.Size = new System.Drawing.Size(71, 13);
            labelHeaders.TabIndex = 0;
            labelHeaders.Text = "With headers";
            labelHeaders.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // authPanel
            // 
            authPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            authPanel.ColumnCount = 4;
            authPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            authPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            authPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            authPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            authPanel.Controls.Add(authLabelPrefix, 0, 0);
            authPanel.Controls.Add(this.authLabel, 1, 0);
            authPanel.Controls.Add(authLabelSuffix, 2, 0);
            authPanel.Controls.Add(this.basicAuthPanel, 3, 0);
            authPanel.Location = new System.Drawing.Point(4, 28);
            authPanel.Name = "authPanel";
            authPanel.RowCount = 1;
            authPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            authPanel.Size = new System.Drawing.Size(518, 27);
            authPanel.TabIndex = 23;
            // 
            // authLabelPrefix
            // 
            authLabelPrefix.Anchor = System.Windows.Forms.AnchorStyles.None;
            authLabelPrefix.AutoSize = true;
            authLabelPrefix.Location = new System.Drawing.Point(0, 7);
            authLabelPrefix.Margin = new System.Windows.Forms.Padding(0);
            authLabelPrefix.Name = "authLabelPrefix";
            authLabelPrefix.Size = new System.Drawing.Size(29, 13);
            authLabelPrefix.TabIndex = 0;
            authLabelPrefix.Text = "With";
            authLabelPrefix.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // authLabel
            // 
            this.authLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.authLabel.AutoSize = true;
            this.authLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.authLabel.ForeColor = System.Drawing.Color.Blue;
            this.authLabel.Location = new System.Drawing.Point(29, 7);
            this.authLabel.Margin = new System.Windows.Forms.Padding(0);
            this.authLabel.Name = "authLabel";
            this.authLabel.Size = new System.Drawing.Size(19, 13);
            this.authLabel.TabIndex = 1;
            this.authLabel.Text = "no";
            this.authLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.authLabel.Click += new System.EventHandler(this.OnAuthLabelClick);
            // 
            // authLabelSuffix
            // 
            authLabelSuffix.Anchor = System.Windows.Forms.AnchorStyles.None;
            authLabelSuffix.AutoSize = true;
            authLabelSuffix.Location = new System.Drawing.Point(48, 7);
            authLabelSuffix.Margin = new System.Windows.Forms.Padding(0);
            authLabelSuffix.Name = "authLabelSuffix";
            authLabelSuffix.Size = new System.Drawing.Size(76, 13);
            authLabelSuffix.TabIndex = 0;
            authLabelSuffix.Text = "authentication";
            authLabelSuffix.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // basicAuthPanel
            // 
            this.basicAuthPanel.ColumnCount = 4;
            this.basicAuthPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.basicAuthPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.basicAuthPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.basicAuthPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.basicAuthPanel.Controls.Add(this.passwordTextBox, 3, 0);
            this.basicAuthPanel.Controls.Add(basicAuthUserLabel, 0, 0);
            this.basicAuthPanel.Controls.Add(basicAuthPasswordLabel, 2, 0);
            this.basicAuthPanel.Controls.Add(this.userNameTextBox, 1, 0);
            this.basicAuthPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicAuthPanel.Location = new System.Drawing.Point(124, 0);
            this.basicAuthPanel.Margin = new System.Windows.Forms.Padding(0);
            this.basicAuthPanel.Name = "basicAuthPanel";
            this.basicAuthPanel.RowCount = 1;
            this.basicAuthPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.basicAuthPanel.Size = new System.Drawing.Size(394, 27);
            this.basicAuthPanel.TabIndex = 2;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordTextBox.Location = new System.Drawing.Point(259, 3);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(132, 21);
            this.passwordTextBox.TabIndex = 2;
            this.passwordTextBox.TextChanged += new System.EventHandler(this.OnPasswordTextChanged);
            // 
            // basicAuthUserLabel
            // 
            basicAuthUserLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            basicAuthUserLabel.AutoSize = true;
            basicAuthUserLabel.Location = new System.Drawing.Point(0, 7);
            basicAuthUserLabel.Margin = new System.Windows.Forms.Padding(0);
            basicAuthUserLabel.Name = "basicAuthUserLabel";
            basicAuthUserLabel.Size = new System.Drawing.Size(42, 13);
            basicAuthUserLabel.TabIndex = 0;
            basicAuthUserLabel.Text = "as user";
            basicAuthUserLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // basicAuthPasswordLabel
            // 
            basicAuthPasswordLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            basicAuthPasswordLabel.AutoSize = true;
            basicAuthPasswordLabel.Location = new System.Drawing.Point(180, 7);
            basicAuthPasswordLabel.Margin = new System.Windows.Forms.Padding(0);
            basicAuthPasswordLabel.Name = "basicAuthPasswordLabel";
            basicAuthPasswordLabel.Size = new System.Drawing.Size(76, 13);
            basicAuthPasswordLabel.TabIndex = 0;
            basicAuthPasswordLabel.Text = "with password";
            basicAuthPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // userNameTextBox
            // 
            this.userNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.userNameTextBox.Location = new System.Drawing.Point(45, 3);
            this.userNameTextBox.Name = "userNameTextBox";
            this.userNameTextBox.Size = new System.Drawing.Size(132, 21);
            this.userNameTextBox.TabIndex = 1;
            this.userNameTextBox.TextChanged += new System.EventHandler(this.OnUserNameTextChanged);
            // 
            // asBodyLabel
            // 
            this.asBodyLabel.AutoSize = true;
            this.asBodyLabel.Location = new System.Drawing.Point(341, 185);
            this.asBodyLabel.Margin = new System.Windows.Forms.Padding(0);
            this.asBodyLabel.Name = "asBodyLabel";
            this.asBodyLabel.Size = new System.Drawing.Size(104, 13);
            this.asBodyLabel.TabIndex = 0;
            this.asBodyLabel.Text = "as the request body";
            this.asBodyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // headersGrid
            // 
            this.headersGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headersGrid.Location = new System.Drawing.Point(74, 3);
            this.headersGrid.Name = "headersGrid";
            this.headersGrid.Size = new System.Drawing.Size(441, 113);
            this.headersGrid.TabIndex = 21;
            this.headersGrid.Values = ((System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<string, string>>)(resources.GetObject("headersGrid.Values")));
            this.headersGrid.DataChanged += new System.EventHandler<System.EventArgs>(this.OnHeadersGridDataChanged);
            // 
            // headersPanel
            // 
            this.headersPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.headersPanel.ColumnCount = 2;
            this.headersPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.headersPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.headersPanel.Controls.Add(this.headersGrid, 1, 0);
            this.headersPanel.Controls.Add(labelHeaders, 0, 0);
            this.headersPanel.Location = new System.Drawing.Point(4, 57);
            this.headersPanel.Name = "headersPanel";
            this.headersPanel.RowCount = 1;
            this.headersPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.headersPanel.Size = new System.Drawing.Size(518, 119);
            this.headersPanel.TabIndex = 22;
            // 
            // HitUrlTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(authPanel);
            this.Controls.Add(this.headersPanel);
            this.Controls.Add(this.asBodyLabel);
            this.Controls.Add(verbUrlPanel);
            this.Controls.Add(requestUrlPanel);
            this.Name = "HitUrlTaskEditor";
            this.Size = new System.Drawing.Size(522, 210);
            this.Controls.SetChildIndex(requestUrlPanel, 0);
            this.Controls.SetChildIndex(verbUrlPanel, 0);
            this.Controls.SetChildIndex(this.asBodyLabel, 0);
            this.Controls.SetChildIndex(this.headersPanel, 0);
            this.Controls.SetChildIndex(this.labelTemplate, 0);
            this.Controls.SetChildIndex(this.templateCombo, 0);
            this.Controls.SetChildIndex(authPanel, 0);
            verbUrlPanel.ResumeLayout(false);
            verbUrlPanel.PerformLayout();
            authPanel.ResumeLayout(false);
            authPanel.PerformLayout();
            this.basicAuthPanel.ResumeLayout(false);
            this.basicAuthPanel.PerformLayout();
            this.headersPanel.ResumeLayout(false);
            this.headersPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label verbLabel;
        private System.Windows.Forms.Label asBodyLabel;
        private Templates.Tokens.TemplateTokenTextBox urlTextBox;
        private ShipWorks.UI.Controls.NameValueGrid headersGrid;
        private System.Windows.Forms.TableLayoutPanel headersPanel;
        private System.Windows.Forms.Label authLabel;
        private System.Windows.Forms.TableLayoutPanel basicAuthPanel;
        private System.Windows.Forms.TextBox userNameTextBox;
        private System.Windows.Forms.TextBox passwordTextBox;

    }
}
