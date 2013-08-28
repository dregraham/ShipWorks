namespace ShipWorks.Actions.Tasks.Common.Editors
{
    partial class WebRequestTaskEditor
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.FlowLayoutPanel requestUrlPanel;
            System.Windows.Forms.TableLayoutPanel verbUrlPanel;
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory1 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            System.Windows.Forms.Label sendALabel;
            System.Windows.Forms.Label labelHeaders;
            System.Windows.Forms.TableLayoutPanel authPanel;
            System.Windows.Forms.Label authLabelPrefix;
            System.Windows.Forms.Label basicAuthUserLabel;
            System.Windows.Forms.Label basicAuthPasswordLabel;
            System.Windows.Forms.Label labelSendA2;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebRequestTaskEditor));
            this.urlFormat = new System.Windows.Forms.Label();
            this.urlTextBox = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.verbPanel = new System.Windows.Forms.Panel();
            this.verbLabel = new System.Windows.Forms.Label();
            this.requestToLabel = new System.Windows.Forms.Label();
            this.authTypePanel = new System.Windows.Forms.Panel();
            this.authLabelSuffix = new System.Windows.Forms.Label();
            this.authLabel = new System.Windows.Forms.Label();
            this.basicAuthPanel = new System.Windows.Forms.TableLayoutPanel();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.userNameTextBox = new System.Windows.Forms.TextBox();
            this.headersGrid = new ShipWorks.UI.Controls.NameValueGrid();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.cardinalityLabel = new System.Windows.Forms.Label();
            requestUrlPanel = new System.Windows.Forms.FlowLayoutPanel();
            verbUrlPanel = new System.Windows.Forms.TableLayoutPanel();
            sendALabel = new System.Windows.Forms.Label();
            labelHeaders = new System.Windows.Forms.Label();
            authPanel = new System.Windows.Forms.TableLayoutPanel();
            authLabelPrefix = new System.Windows.Forms.Label();
            basicAuthUserLabel = new System.Windows.Forms.Label();
            basicAuthPasswordLabel = new System.Windows.Forms.Label();
            labelSendA2 = new System.Windows.Forms.Label();
            verbUrlPanel.SuspendLayout();
            this.verbPanel.SuspendLayout();
            authPanel.SuspendLayout();
            this.authTypePanel.SuspendLayout();
            this.basicAuthPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // labelTemplate
            // 
            this.labelTemplate.Location = new System.Drawing.Point(29, 251);
            this.labelTemplate.Margin = new System.Windows.Forms.Padding(0);
            this.labelTemplate.Size = new System.Drawing.Size(82, 13);
            this.labelTemplate.Text = "Using template:";
            // 
            // templateCombo
            // 
            this.templateCombo.Location = new System.Drawing.Point(110, 248);
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
            // verbUrlPanel
            // 
            verbUrlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            verbUrlPanel.ColumnCount = 2;
            verbUrlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            verbUrlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            verbUrlPanel.Controls.Add(this.urlFormat, 1, 1);
            verbUrlPanel.Controls.Add(this.urlTextBox, 1, 0);
            verbUrlPanel.Controls.Add(this.verbPanel, 0, 0);
            verbUrlPanel.Location = new System.Drawing.Point(4, -2);
            verbUrlPanel.Name = "verbUrlPanel";
            verbUrlPanel.RowCount = 2;
            verbUrlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            verbUrlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            verbUrlPanel.Size = new System.Drawing.Size(579, 46);
            verbUrlPanel.TabIndex = 5;
            // 
            // urlFormat
            // 
            this.urlFormat.AutoSize = true;
            this.urlFormat.Font = new System.Drawing.Font("Tahoma", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.urlFormat.Location = new System.Drawing.Point(128, 27);
            this.urlFormat.Name = "urlFormat";
            this.urlFormat.Size = new System.Drawing.Size(285, 11);
            this.urlFormat.TabIndex = 24;
            this.urlFormat.Text = "e.g. http://www.example.org/orders.php?orderid={//Order/Number}";
            // 
            // urlTextBox
            // 
            this.urlTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.urlTextBox.Location = new System.Drawing.Point(128, 3);
            this.urlTextBox.MaxLength = 32767;
            this.urlTextBox.Name = "urlTextBox";
            this.urlTextBox.Size = new System.Drawing.Size(448, 21);
            this.urlTextBox.TabIndex = 2;
            this.urlTextBox.TokenSelectionMode = ShipWorks.Templates.Tokens.TokenSelectionMode.Paste;
            this.urlTextBox.TokenSuggestionFactory = commonTokenSuggestionsFactory1;
            this.urlTextBox.TextChanged += new System.EventHandler(this.OnUrlTextChanged);
            // 
            // verbPanel
            // 
            this.verbPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.verbPanel.Controls.Add(sendALabel);
            this.verbPanel.Controls.Add(this.verbLabel);
            this.verbPanel.Controls.Add(this.requestToLabel);
            this.verbPanel.Location = new System.Drawing.Point(0, 4);
            this.verbPanel.Margin = new System.Windows.Forms.Padding(0);
            this.verbPanel.Name = "verbPanel";
            this.verbPanel.Size = new System.Drawing.Size(125, 18);
            this.verbPanel.TabIndex = 29;
            // 
            // sendALabel
            // 
            sendALabel.AutoSize = true;
            sendALabel.Location = new System.Drawing.Point(0, 3);
            sendALabel.Margin = new System.Windows.Forms.Padding(0);
            sendALabel.Name = "sendALabel";
            sendALabel.Size = new System.Drawing.Size(40, 13);
            sendALabel.TabIndex = 5;
            sendALabel.Text = "Send a";
            sendALabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // verbLabel
            // 
            this.verbLabel.AutoSize = true;
            this.verbLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.verbLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.verbLabel.ForeColor = System.Drawing.Color.Blue;
            this.verbLabel.Location = new System.Drawing.Point(37, 3);
            this.verbLabel.Margin = new System.Windows.Forms.Padding(0);
            this.verbLabel.Name = "verbLabel";
            this.verbLabel.Size = new System.Drawing.Size(32, 13);
            this.verbLabel.TabIndex = 7;
            this.verbLabel.Text = "VERB";
            this.verbLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.verbLabel.SizeChanged += new System.EventHandler(this.OnVerbLabelSizeChanged);
            this.verbLabel.TextChanged += new System.EventHandler(this.OnVerbLabelTextChanged);
            this.verbLabel.Click += new System.EventHandler(this.OnVerbLabelClick);
            // 
            // requestToLabel
            // 
            this.requestToLabel.AutoSize = true;
            this.requestToLabel.Location = new System.Drawing.Point(66, 3);
            this.requestToLabel.Margin = new System.Windows.Forms.Padding(0);
            this.requestToLabel.Name = "requestToLabel";
            this.requestToLabel.Size = new System.Drawing.Size(61, 13);
            this.requestToLabel.TabIndex = 6;
            this.requestToLabel.Text = "request to:";
            this.requestToLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelHeaders
            // 
            labelHeaders.AutoSize = true;
            labelHeaders.Location = new System.Drawing.Point(4, 82);
            labelHeaders.Margin = new System.Windows.Forms.Padding(0);
            labelHeaders.Name = "labelHeaders";
            labelHeaders.Size = new System.Drawing.Size(75, 13);
            labelHeaders.TabIndex = 0;
            labelHeaders.Text = "With headers:";
            labelHeaders.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // authPanel
            // 
            authPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            authPanel.ColumnCount = 2;
            authPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            authPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            authPanel.Controls.Add(this.authTypePanel, 0, 0);
            authPanel.Controls.Add(this.basicAuthPanel, 1, 0);
            authPanel.Location = new System.Drawing.Point(4, 46);
            authPanel.Name = "authPanel";
            authPanel.RowCount = 1;
            authPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            authPanel.Size = new System.Drawing.Size(579, 27);
            authPanel.TabIndex = 23;
            // 
            // authTypePanel
            // 
            this.authTypePanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.authTypePanel.Controls.Add(this.authLabelSuffix);
            this.authTypePanel.Controls.Add(this.authLabel);
            this.authTypePanel.Controls.Add(authLabelPrefix);
            this.authTypePanel.Location = new System.Drawing.Point(0, 4);
            this.authTypePanel.Margin = new System.Windows.Forms.Padding(0);
            this.authTypePanel.Name = "authTypePanel";
            this.authTypePanel.Size = new System.Drawing.Size(114, 18);
            this.authTypePanel.TabIndex = 30;
            // 
            // authLabelSuffix
            // 
            this.authLabelSuffix.AutoSize = true;
            this.authLabelSuffix.Location = new System.Drawing.Point(42, 3);
            this.authLabelSuffix.Margin = new System.Windows.Forms.Padding(0);
            this.authLabelSuffix.Name = "authLabelSuffix";
            this.authLabelSuffix.Size = new System.Drawing.Size(76, 13);
            this.authLabelSuffix.TabIndex = 3;
            this.authLabelSuffix.Text = "authentication";
            this.authLabelSuffix.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // authLabel
            // 
            this.authLabel.AutoSize = true;
            this.authLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.authLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.authLabel.ForeColor = System.Drawing.Color.Blue;
            this.authLabel.Location = new System.Drawing.Point(26, 3);
            this.authLabel.Margin = new System.Windows.Forms.Padding(0);
            this.authLabel.Name = "authLabel";
            this.authLabel.Size = new System.Drawing.Size(19, 13);
            this.authLabel.TabIndex = 2;
            this.authLabel.Text = "no";
            this.authLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.authLabel.SizeChanged += new System.EventHandler(this.OnAuthLabelSizeChanged);
            this.authLabel.Click += new System.EventHandler(this.OnAuthLabelClick);
            // 
            // authLabelPrefix
            // 
            authLabelPrefix.AutoSize = true;
            authLabelPrefix.Location = new System.Drawing.Point(0, 3);
            authLabelPrefix.Margin = new System.Windows.Forms.Padding(0);
            authLabelPrefix.Name = "authLabelPrefix";
            authLabelPrefix.Size = new System.Drawing.Size(29, 13);
            authLabelPrefix.TabIndex = 1;
            authLabelPrefix.Text = "With";
            authLabelPrefix.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.basicAuthPanel.Location = new System.Drawing.Point(114, 0);
            this.basicAuthPanel.Margin = new System.Windows.Forms.Padding(0);
            this.basicAuthPanel.Name = "basicAuthPanel";
            this.basicAuthPanel.RowCount = 1;
            this.basicAuthPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.basicAuthPanel.Size = new System.Drawing.Size(465, 27);
            this.basicAuthPanel.TabIndex = 2;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordTextBox.Location = new System.Drawing.Point(298, 3);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(164, 21);
            this.passwordTextBox.TabIndex = 2;
            this.passwordTextBox.UseSystemPasswordChar = true;
            this.passwordTextBox.TextChanged += new System.EventHandler(this.OnPasswordTextChanged);
            // 
            // basicAuthUserLabel
            // 
            basicAuthUserLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            basicAuthUserLabel.AutoSize = true;
            basicAuthUserLabel.Location = new System.Drawing.Point(0, 7);
            basicAuthUserLabel.Margin = new System.Windows.Forms.Padding(0);
            basicAuthUserLabel.Name = "basicAuthUserLabel";
            basicAuthUserLabel.Size = new System.Drawing.Size(46, 13);
            basicAuthUserLabel.TabIndex = 0;
            basicAuthUserLabel.Text = "as user:";
            basicAuthUserLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // basicAuthPasswordLabel
            // 
            basicAuthPasswordLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            basicAuthPasswordLabel.AutoSize = true;
            basicAuthPasswordLabel.Location = new System.Drawing.Point(215, 7);
            basicAuthPasswordLabel.Margin = new System.Windows.Forms.Padding(0);
            basicAuthPasswordLabel.Name = "basicAuthPasswordLabel";
            basicAuthPasswordLabel.Size = new System.Drawing.Size(80, 13);
            basicAuthPasswordLabel.TabIndex = 0;
            basicAuthPasswordLabel.Text = "with password:";
            basicAuthPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // userNameTextBox
            // 
            this.userNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.userNameTextBox.Location = new System.Drawing.Point(49, 3);
            this.userNameTextBox.Name = "userNameTextBox";
            this.userNameTextBox.Size = new System.Drawing.Size(163, 21);
            this.userNameTextBox.TabIndex = 1;
            this.userNameTextBox.TextChanged += new System.EventHandler(this.OnUserNameTextChanged);
            // 
            // labelSendA2
            // 
            labelSendA2.AutoSize = true;
            labelSendA2.Location = new System.Drawing.Point(4, 228);
            labelSendA2.Name = "labelSendA2";
            labelSendA2.Size = new System.Drawing.Size(40, 13);
            labelSendA2.TabIndex = 28;
            labelSendA2.Text = "Send a";
            // 
            // headersGrid
            // 
            this.headersGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.headersGrid.Location = new System.Drawing.Point(32, 100);
            this.headersGrid.Name = "headersGrid";
            this.headersGrid.Size = new System.Drawing.Size(551, 113);
            this.headersGrid.TabIndex = 21;
            this.headersGrid.Values = ((System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<string, string>>)(resources.GetObject("headersGrid.Values")));
            this.headersGrid.DataChanged += new System.EventHandler<System.EventArgs>(this.OnHeadersGridDataChanged);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // cardinalityLabel
            // 
            this.cardinalityLabel.AutoSize = true;
            this.cardinalityLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cardinalityLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cardinalityLabel.ForeColor = System.Drawing.Color.Blue;
            this.cardinalityLabel.Location = new System.Drawing.Point(41, 228);
            this.cardinalityLabel.Margin = new System.Windows.Forms.Padding(0);
            this.cardinalityLabel.Name = "cardinalityLabel";
            this.cardinalityLabel.Size = new System.Drawing.Size(74, 13);
            this.cardinalityLabel.TabIndex = 30;
            this.cardinalityLabel.Text = "single request";
            this.cardinalityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cardinalityLabel.Click += new System.EventHandler(this.OnCardinalityLabelClick);
            // 
            // WebRequestTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.Controls.Add(labelSendA2);
            this.Controls.Add(this.cardinalityLabel);
            this.Controls.Add(labelHeaders);
            this.Controls.Add(this.headersGrid);
            this.Controls.Add(authPanel);
            this.Controls.Add(verbUrlPanel);
            this.Controls.Add(requestUrlPanel);
            this.Name = "WebRequestTaskEditor";
            this.Size = new System.Drawing.Size(583, 281);
            this.Controls.SetChildIndex(requestUrlPanel, 0);
            this.Controls.SetChildIndex(verbUrlPanel, 0);
            this.Controls.SetChildIndex(authPanel, 0);
            this.Controls.SetChildIndex(this.labelTemplate, 0);
            this.Controls.SetChildIndex(this.templateCombo, 0);
            this.Controls.SetChildIndex(this.headersGrid, 0);
            this.Controls.SetChildIndex(labelHeaders, 0);
            this.Controls.SetChildIndex(this.cardinalityLabel, 0);
            this.Controls.SetChildIndex(labelSendA2, 0);
            verbUrlPanel.ResumeLayout(false);
            verbUrlPanel.PerformLayout();
            this.verbPanel.ResumeLayout(false);
            this.verbPanel.PerformLayout();
            authPanel.ResumeLayout(false);
            this.authTypePanel.ResumeLayout(false);
            this.authTypePanel.PerformLayout();
            this.basicAuthPanel.ResumeLayout(false);
            this.basicAuthPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Templates.Tokens.TemplateTokenTextBox urlTextBox;
        private ShipWorks.UI.Controls.NameValueGrid headersGrid;
        private System.Windows.Forms.TableLayoutPanel basicAuthPanel;
        private System.Windows.Forms.TextBox userNameTextBox;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label urlFormat;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Panel verbPanel;
        private System.Windows.Forms.Label verbLabel;
        private System.Windows.Forms.Label requestToLabel;
        private System.Windows.Forms.Panel authTypePanel;
        private System.Windows.Forms.Label authLabel;
        private System.Windows.Forms.Label authLabelSuffix;
        private System.Windows.Forms.Label cardinalityLabel;

    }
}
