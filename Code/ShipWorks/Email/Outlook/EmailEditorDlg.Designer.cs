namespace ShipWorks.Email.Outlook
{
    partial class EmailEditorDlg
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
            this.components = new System.ComponentModel.Container();
            this.cancel = new System.Windows.Forms.Button();
            this.headerGroup = new ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup();
            this.panelBody = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.to = new System.Windows.Forms.TextBox();
            this.emailAccounts = new System.Windows.Forms.Button();
            this.emailAccount = new System.Windows.Forms.ComboBox();
            this.bcc = new System.Windows.Forms.TextBox();
            this.labelSubject = new System.Windows.Forms.Label();
            this.subject = new System.Windows.Forms.TextBox();
            this.edge2 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.labelSendFrom = new System.Windows.Forms.Label();
            this.cc = new System.Windows.Forms.TextBox();
            this.ok = new System.Windows.Forms.Button();
            this.htmlEditor = new ShipWorks.UI.Controls.Html.HtmlEditor();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            ((System.ComponentModel.ISupportInitialize) (this.headerGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.headerGroup.Panel)).BeginInit();
            this.headerGroup.Panel.SuspendLayout();
            this.headerGroup.SuspendLayout();
            this.panelBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(582, 426);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 2;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // headerGroup
            // 
            this.headerGroup.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.headerGroup.HeaderVisibleSecondary = false;
            this.headerGroup.Location = new System.Drawing.Point(8, 8);
            this.headerGroup.Name = "headerGroup";
            this.headerGroup.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            // 
            // headerGroup.Panel
            // 
            this.headerGroup.Panel.Controls.Add(this.panelBody);
            this.headerGroup.Size = new System.Drawing.Size(649, 412);
            this.headerGroup.StateCommon.HeaderPrimary.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders) ((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.headerGroup.StateCommon.HeaderPrimary.Content.ShortText.Color1 = System.Drawing.Color.FromArgb(((int) (((byte) (80)))), ((int) (((byte) (80)))), ((int) (((byte) (80)))));
            this.headerGroup.StateCommon.OverlayHeaders = ComponentFactory.Krypton.Toolkit.InheritBool.False;
            this.headerGroup.StateNormal.Back.Color1 = System.Drawing.Color.FromArgb(((int) (((byte) (243)))), ((int) (((byte) (243)))), ((int) (((byte) (243)))));
            this.headerGroup.StateNormal.Back.Color2 = System.Drawing.Color.White;
            this.headerGroup.StateNormal.Back.ColorAngle = 180F;
            this.headerGroup.StateNormal.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Linear;
            this.headerGroup.StateNormal.Border.Color1 = System.Drawing.Color.Gray;
            this.headerGroup.StateNormal.Border.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.headerGroup.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders) ((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.headerGroup.StateNormal.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            this.headerGroup.StateNormal.Border.Rounding = 8;
            this.headerGroup.StateNormal.Border.Width = 1;
            this.headerGroup.StateNormal.HeaderPrimary.Content.LongText.Color1 = System.Drawing.Color.Green;
            this.headerGroup.StateNormal.HeaderPrimary.Content.LongText.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.headerGroup.StateNormal.HeaderPrimary.Content.LongText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.headerGroup.TabIndex = 0;
            this.headerGroup.Text = "Message 1";
            this.headerGroup.ValuesPrimary.Description = "";
            this.headerGroup.ValuesPrimary.Heading = "Message 1";
            this.headerGroup.ValuesPrimary.Image = null;
            this.headerGroup.ValuesSecondary.Description = "";
            this.headerGroup.ValuesSecondary.Heading = "Description";
            this.headerGroup.ValuesSecondary.Image = null;
            // 
            // panelBody
            // 
            this.panelBody.BackColor = System.Drawing.Color.Transparent;
            this.panelBody.Controls.Add(this.label3);
            this.panelBody.Controls.Add(this.label2);
            this.panelBody.Controls.Add(this.label1);
            this.panelBody.Controls.Add(this.to);
            this.panelBody.Controls.Add(this.htmlEditor);
            this.panelBody.Controls.Add(this.emailAccounts);
            this.panelBody.Controls.Add(this.emailAccount);
            this.panelBody.Controls.Add(this.bcc);
            this.panelBody.Controls.Add(this.labelSubject);
            this.panelBody.Controls.Add(this.subject);
            this.panelBody.Controls.Add(this.edge2);
            this.panelBody.Controls.Add(this.labelSendFrom);
            this.panelBody.Controls.Add(this.cc);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 0);
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(5);
            this.panelBody.Size = new System.Drawing.Size(641, 377);
            this.panelBody.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "BCC:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "CC:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "To:";
            // 
            // to
            // 
            this.to.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.to.Location = new System.Drawing.Point(64, 36);
            this.to.Name = "to";
            this.to.Size = new System.Drawing.Size(569, 21);
            this.to.TabIndex = 4;
            // 
            // emailAccounts
            // 
            this.emailAccounts.Location = new System.Drawing.Point(347, 8);
            this.emailAccounts.Name = "emailAccounts";
            this.emailAccounts.Size = new System.Drawing.Size(105, 23);
            this.emailAccounts.TabIndex = 2;
            this.emailAccounts.Text = "Email Accounts...";
            this.emailAccounts.UseVisualStyleBackColor = true;
            this.emailAccounts.Click += new System.EventHandler(this.OnManageEmailAccounts);
            // 
            // emailAccount
            // 
            this.emailAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.emailAccount.FormattingEnabled = true;
            this.emailAccount.Location = new System.Drawing.Point(64, 9);
            this.emailAccount.Name = "emailAccount";
            this.emailAccount.Size = new System.Drawing.Size(278, 21);
            this.emailAccount.TabIndex = 1;
            // 
            // bcc
            // 
            this.bcc.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.bcc.Location = new System.Drawing.Point(64, 89);
            this.bcc.Name = "bcc";
            this.bcc.Size = new System.Drawing.Size(569, 21);
            this.bcc.TabIndex = 8;
            // 
            // labelSubject
            // 
            this.labelSubject.AutoSize = true;
            this.labelSubject.Location = new System.Drawing.Point(11, 117);
            this.labelSubject.Name = "labelSubject";
            this.labelSubject.Size = new System.Drawing.Size(47, 13);
            this.labelSubject.TabIndex = 9;
            this.labelSubject.Text = "Subject:";
            // 
            // subject
            // 
            this.subject.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.subject.Location = new System.Drawing.Point(64, 114);
            this.fieldLengthProvider.SetMaxLengthSource(this.subject, ShipWorks.Data.Utility.EntityFieldLengthSource.EmailSubject);
            this.subject.Name = "subject";
            this.subject.Size = new System.Drawing.Size(569, 21);
            this.subject.TabIndex = 10;
            // 
            // edge2
            // 
            this.edge2.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.edge2.AutoSize = false;
            this.edge2.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.ControlRibbon;
            this.edge2.Location = new System.Drawing.Point(10, 143);
            this.edge2.Name = "edge2";
            this.edge2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.edge2.Size = new System.Drawing.Size(623, 1);
            this.edge2.TabIndex = 11;
            this.edge2.Text = "kryptonBorderEdge1";
            // 
            // labelSendFrom
            // 
            this.labelSendFrom.AutoSize = true;
            this.labelSendFrom.Location = new System.Drawing.Point(23, 13);
            this.labelSendFrom.Name = "labelSendFrom";
            this.labelSendFrom.Size = new System.Drawing.Size(35, 13);
            this.labelSendFrom.TabIndex = 0;
            this.labelSendFrom.Text = "From:";
            // 
            // cc
            // 
            this.cc.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cc.Location = new System.Drawing.Point(64, 63);
            this.cc.Name = "cc";
            this.cc.Size = new System.Drawing.Size(569, 21);
            this.cc.TabIndex = 6;
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(501, 426);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 1;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // htmlEditor
            // 
            this.htmlEditor.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.htmlEditor.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.htmlEditor.Html = "<HTML><HEAD>\r\n<META content=\"MSHTML 6.00.6001.18294\" name=GENERATOR></HEAD>\r\n<BOD" +
                "Y></BODY></HTML>";
            this.htmlEditor.Location = new System.Drawing.Point(8, 150);
            this.htmlEditor.Name = "htmlEditor";
            this.htmlEditor.Size = new System.Drawing.Size(625, 221);
            this.htmlEditor.TabIndex = 12;
            // 
            // EmailEditorDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(669, 461);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.headerGroup);
            this.Controls.Add(this.cancel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EmailEditorDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Compose Email";
            this.Load += new System.EventHandler(this.OnLoad);
            this.Shown += new System.EventHandler(this.OnShown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
            ((System.ComponentModel.ISupportInitialize) (this.headerGroup.Panel)).EndInit();
            this.headerGroup.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.headerGroup)).EndInit();
            this.headerGroup.ResumeLayout(false);
            this.panelBody.ResumeLayout(false);
            this.panelBody.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancel;
        private ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup headerGroup;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.TextBox to;
        private ShipWorks.UI.Controls.Html.HtmlEditor htmlEditor;
        private System.Windows.Forms.Button emailAccounts;
        private System.Windows.Forms.ComboBox emailAccount;
        private System.Windows.Forms.TextBox bcc;
        private System.Windows.Forms.Label labelSubject;
        private System.Windows.Forms.TextBox subject;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge edge2;
        private System.Windows.Forms.Label labelSendFrom;
        private System.Windows.Forms.TextBox cc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button ok;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
    }
}