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
            System.Windows.Forms.FlowLayoutPanel requestUrlPanel;
            System.Windows.Forms.Label labelAs;
            System.Windows.Forms.Label labelSendA2;
            System.Windows.Forms.Label labelRequestOptions;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory1 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebRequestTaskEditor));
            this.urlFormat = new System.Windows.Forms.Label();
            this.urlTextBox = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.userNameTextBox = new System.Windows.Forms.TextBox();
            this.headersGrid = new ShipWorks.UI.Controls.NameValueGrid();
            this.requestPanel = new System.Windows.Forms.Panel();
            this.includeExtraHeaders = new System.Windows.Forms.CheckBox();
            this.panelBasicAuth = new System.Windows.Forms.Panel();
            this.useBasicAuth = new System.Windows.Forms.CheckBox();
            this.comboVerb = new System.Windows.Forms.ComboBox();
            this.comboCardinality = new System.Windows.Forms.ComboBox();
            requestUrlPanel = new System.Windows.Forms.FlowLayoutPanel();
            labelAs = new System.Windows.Forms.Label();
            labelSendA2 = new System.Windows.Forms.Label();
            labelRequestOptions = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            this.requestPanel.SuspendLayout();
            this.panelBasicAuth.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTemplate
            // 
            this.labelTemplate.Location = new System.Drawing.Point(102, 33);
            this.labelTemplate.Margin = new System.Windows.Forms.Padding(0);
            this.labelTemplate.Size = new System.Drawing.Size(55, 13);
            this.labelTemplate.Text = "Template:";
            // 
            // templateCombo
            // 
            this.templateCombo.Location = new System.Drawing.Point(158, 30);
            this.templateCombo.Size = new System.Drawing.Size(281, 21);
            this.templateCombo.TabIndex = 20;
            // 
            // requestUrlPanel
            // 
            requestUrlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            requestUrlPanel.AutoSize = true;
            requestUrlPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            requestUrlPanel.Location = new System.Drawing.Point(2, 34);
            requestUrlPanel.Name = "requestUrlPanel";
            requestUrlPanel.Size = new System.Drawing.Size(0, 0);
            requestUrlPanel.TabIndex = 4;
            requestUrlPanel.WrapContents = false;
            // 
            // labelAs
            // 
            labelAs.AutoSize = true;
            labelAs.Location = new System.Drawing.Point(341, 4);
            labelAs.Margin = new System.Windows.Forms.Padding(0);
            labelAs.Name = "labelAs";
            labelAs.Size = new System.Drawing.Size(27, 13);
            labelAs.TabIndex = 5;
            labelAs.Text = "as a";
            labelAs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSendA2
            // 
            labelSendA2.AutoSize = true;
            labelSendA2.Location = new System.Drawing.Point(3, 4);
            labelSendA2.Name = "labelSendA2";
            labelSendA2.Size = new System.Drawing.Size(94, 13);
            labelSendA2.TabIndex = 28;
            labelSendA2.Text = "Send the request:";
            // 
            // labelRequestOptions
            // 
            labelRequestOptions.AutoSize = true;
            labelRequestOptions.Location = new System.Drawing.Point(3, 69);
            labelRequestOptions.Margin = new System.Windows.Forms.Padding(0);
            labelRequestOptions.Name = "labelRequestOptions";
            labelRequestOptions.Size = new System.Drawing.Size(89, 13);
            labelRequestOptions.TabIndex = 24;
            labelRequestOptions.Text = "Request options:";
            labelRequestOptions.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(6, 6);
            label2.Margin = new System.Windows.Forms.Padding(0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(59, 13);
            label2.TabIndex = 26;
            label2.Text = "Username:";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(8, 32);
            label3.Margin = new System.Windows.Forms.Padding(0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(57, 13);
            label3.TabIndex = 27;
            label3.Text = "Password:";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(3, 4);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(73, 13);
            label4.TabIndex = 30;
            label4.Text = "Request URL:";
            // 
            // urlFormat
            // 
            this.urlFormat.AutoSize = true;
            this.urlFormat.Font = new System.Drawing.Font("Tahoma", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.urlFormat.ForeColor = System.Drawing.Color.DimGray;
            this.urlFormat.Location = new System.Drawing.Point(28, 48);
            this.urlFormat.Name = "urlFormat";
            this.urlFormat.Size = new System.Drawing.Size(285, 11);
            this.urlFormat.TabIndex = 24;
            this.urlFormat.Text = "e.g. http://www.example.org/orders.php?orderid={//Order/Number}";
            // 
            // urlTextBox
            // 
            this.urlTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.urlTextBox.Location = new System.Drawing.Point(30, 24);
            this.urlTextBox.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.urlTextBox.MaxLength = 32767;
            this.urlTextBox.Name = "urlTextBox";
            this.urlTextBox.Size = new System.Drawing.Size(534, 21);
            this.urlTextBox.TabIndex = 2;
            this.urlTextBox.TokenSelectionMode = ShipWorks.Templates.Tokens.TokenSelectionMode.Paste;
            this.urlTextBox.TokenSuggestionFactory = commonTokenSuggestionsFactory1;
            this.urlTextBox.TextChanged += new System.EventHandler(this.OnUrlTextChanged);
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(68, 29);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(259, 21);
            this.passwordTextBox.TabIndex = 2;
            this.passwordTextBox.UseSystemPasswordChar = true;
            this.passwordTextBox.TextChanged += new System.EventHandler(this.OnPasswordTextChanged);
            // 
            // userNameTextBox
            // 
            this.userNameTextBox.Location = new System.Drawing.Point(68, 3);
            this.userNameTextBox.Name = "userNameTextBox";
            this.userNameTextBox.Size = new System.Drawing.Size(259, 21);
            this.userNameTextBox.TabIndex = 1;
            this.userNameTextBox.TextChanged += new System.EventHandler(this.OnUserNameTextChanged);
            // 
            // headersGrid
            // 
            this.headersGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.headersGrid.Location = new System.Drawing.Point(48, 188);
            this.headersGrid.Name = "headersGrid";
            this.headersGrid.Size = new System.Drawing.Size(516, 113);
            this.headersGrid.TabIndex = 21;
            this.headersGrid.Values = ((System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<string, string>>)(resources.GetObject("headersGrid.Values")));
            this.headersGrid.Visible = false;
            this.headersGrid.DataChanged += new System.EventHandler<System.EventArgs>(this.OnHeadersGridDataChanged);
            // 
            // requestPanel
            // 
            this.requestPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.requestPanel.Controls.Add(label4);
            this.requestPanel.Controls.Add(this.urlFormat);
            this.requestPanel.Controls.Add(this.includeExtraHeaders);
            this.requestPanel.Controls.Add(this.urlTextBox);
            this.requestPanel.Controls.Add(this.panelBasicAuth);
            this.requestPanel.Controls.Add(this.useBasicAuth);
            this.requestPanel.Controls.Add(labelRequestOptions);
            this.requestPanel.Controls.Add(this.headersGrid);
            this.requestPanel.Location = new System.Drawing.Point(0, 57);
            this.requestPanel.Name = "requestPanel";
            this.requestPanel.Size = new System.Drawing.Size(580, 307);
            this.requestPanel.TabIndex = 31;
            // 
            // includeExtraHeaders
            // 
            this.includeExtraHeaders.AutoSize = true;
            this.includeExtraHeaders.Location = new System.Drawing.Point(30, 168);
            this.includeExtraHeaders.Name = "includeExtraHeaders";
            this.includeExtraHeaders.Size = new System.Drawing.Size(160, 17);
            this.includeExtraHeaders.TabIndex = 29;
            this.includeExtraHeaders.Text = "Include extra HTTP headers";
            this.includeExtraHeaders.UseVisualStyleBackColor = true;
            this.includeExtraHeaders.CheckedChanged += new System.EventHandler(this.OnChangeIncludeHttpHeaders);
            // 
            // panelBasicAuth
            // 
            this.panelBasicAuth.Controls.Add(this.userNameTextBox);
            this.panelBasicAuth.Controls.Add(label3);
            this.panelBasicAuth.Controls.Add(this.passwordTextBox);
            this.panelBasicAuth.Controls.Add(label2);
            this.panelBasicAuth.Location = new System.Drawing.Point(41, 105);
            this.panelBasicAuth.Name = "panelBasicAuth";
            this.panelBasicAuth.Size = new System.Drawing.Size(341, 56);
            this.panelBasicAuth.TabIndex = 28;
            this.panelBasicAuth.Visible = false;
            // 
            // useBasicAuth
            // 
            this.useBasicAuth.AutoSize = true;
            this.useBasicAuth.Location = new System.Drawing.Point(30, 87);
            this.useBasicAuth.Name = "useBasicAuth";
            this.useBasicAuth.Size = new System.Drawing.Size(143, 17);
            this.useBasicAuth.TabIndex = 25;
            this.useBasicAuth.Text = "Use basic authentication";
            this.useBasicAuth.UseVisualStyleBackColor = true;
            this.useBasicAuth.CheckedChanged += new System.EventHandler(this.OnChangeUseBasicAuth);
            // 
            // comboVerb
            // 
            this.comboVerb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboVerb.FormattingEnabled = true;
            this.comboVerb.Location = new System.Drawing.Point(368, 1);
            this.comboVerb.Name = "comboVerb";
            this.comboVerb.Size = new System.Drawing.Size(71, 21);
            this.comboVerb.TabIndex = 33;
            // 
            // comboCardinality
            // 
            this.comboCardinality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboCardinality.FormattingEnabled = true;
            this.comboCardinality.Location = new System.Drawing.Point(103, 1);
            this.comboCardinality.Name = "comboCardinality";
            this.comboCardinality.Size = new System.Drawing.Size(232, 21);
            this.comboCardinality.TabIndex = 32;
            // 
            // WebRequestTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.Controls.Add(this.comboVerb);
            this.Controls.Add(labelAs);
            this.Controls.Add(this.comboCardinality);
            this.Controls.Add(this.requestPanel);
            this.Controls.Add(labelSendA2);
            this.Controls.Add(requestUrlPanel);
            this.Name = "WebRequestTaskEditor";
            this.Size = new System.Drawing.Size(583, 370);
            this.Controls.SetChildIndex(requestUrlPanel, 0);
            this.Controls.SetChildIndex(labelSendA2, 0);
            this.Controls.SetChildIndex(this.requestPanel, 0);
            this.Controls.SetChildIndex(this.comboCardinality, 0);
            this.Controls.SetChildIndex(this.labelTemplate, 0);
            this.Controls.SetChildIndex(this.templateCombo, 0);
            this.Controls.SetChildIndex(labelAs, 0);
            this.Controls.SetChildIndex(this.comboVerb, 0);
            this.requestPanel.ResumeLayout(false);
            this.requestPanel.PerformLayout();
            this.panelBasicAuth.ResumeLayout(false);
            this.panelBasicAuth.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Templates.Tokens.TemplateTokenTextBox urlTextBox;
        private ShipWorks.UI.Controls.NameValueGrid headersGrid;
        private System.Windows.Forms.TextBox userNameTextBox;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label urlFormat;
        private System.Windows.Forms.Panel requestPanel;
        private System.Windows.Forms.CheckBox useBasicAuth;
        private System.Windows.Forms.ComboBox comboCardinality;
        private System.Windows.Forms.CheckBox includeExtraHeaders;
        private System.Windows.Forms.Panel panelBasicAuth;
        private System.Windows.Forms.ComboBox comboVerb;

    }
}
