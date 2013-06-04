namespace ShipWorks.Templates.Emailing
{
    partial class EmailComposerDlg
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
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer1 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            this.bccText = new System.Windows.Forms.TextBox();
            this.labelSendFrom = new System.Windows.Forms.Label();
            this.labelTemplate = new System.Windows.Forms.Label();
            this.subject = new System.Windows.Forms.TextBox();
            this.labelSubject = new System.Windows.Forms.Label();
            this.send = new System.Windows.Forms.Button();
            this.close = new System.Windows.Forms.Button();
            this.emailAccounts = new System.Windows.Forms.Button();
            this.edge2 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.ccText = new System.Windows.Forms.TextBox();
            this.toText = new System.Windows.Forms.TextBox();
            this.emailAccount = new System.Windows.Forms.ComboBox();
            this.panelBody = new System.Windows.Forms.Panel();
            this.htmlEditor = new ShipWorks.UI.Controls.Html.HtmlEditor();
            this.toButton = new ShipWorks.UI.Controls.DropDownButton();
            this.ccButton = new ShipWorks.UI.Controls.DropDownButton();
            this.bccButton = new ShipWorks.UI.Controls.DropDownButton();
            this.composerHeaderGroup = new ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.messageList = new Divelements.SandGrid.SandGrid();
            this.gridColumnMessageNumber = new Divelements.SandGrid.GridColumn();
            this.gridColumnMessageSubject = new Divelements.SandGrid.GridColumn();
            this.kryptonStatusPanel = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.labelMessageCount = new System.Windows.Forms.Label();
            this.templateList = new ShipWorks.Templates.Controls.TemplateComboBox();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.panelBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.composerHeaderGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.composerHeaderGroup.Panel)).BeginInit();
            this.composerHeaderGroup.Panel.SuspendLayout();
            this.composerHeaderGroup.SuspendLayout();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.kryptonStatusPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.kryptonStatusPanel.Panel)).BeginInit();
            this.kryptonStatusPanel.Panel.SuspendLayout();
            this.kryptonStatusPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // bccText
            // 
            this.bccText.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.bccText.Location = new System.Drawing.Point(64, 89);
            this.bccText.Name = "bccText";
            this.bccText.Size = new System.Drawing.Size(476, 21);
            this.bccText.TabIndex = 8;
            // 
            // labelSendFrom
            // 
            this.labelSendFrom.AutoSize = true;
            this.labelSendFrom.Location = new System.Drawing.Point(23, 12);
            this.labelSendFrom.Name = "labelSendFrom";
            this.labelSendFrom.Size = new System.Drawing.Size(35, 13);
            this.labelSendFrom.TabIndex = 0;
            this.labelSendFrom.Text = "From:";
            // 
            // labelTemplate
            // 
            this.labelTemplate.AutoSize = true;
            this.labelTemplate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelTemplate.Location = new System.Drawing.Point(11, 11);
            this.labelTemplate.Name = "labelTemplate";
            this.labelTemplate.Size = new System.Drawing.Size(55, 13);
            this.labelTemplate.TabIndex = 0;
            this.labelTemplate.Text = "Template:";
            // 
            // subject
            // 
            this.subject.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.subject.Location = new System.Drawing.Point(64, 114);
            this.fieldLengthProvider.SetMaxLengthSource(this.subject, ShipWorks.Data.Utility.EntityFieldLengthSource.EmailSubject);
            this.subject.Name = "subject";
            this.subject.Size = new System.Drawing.Size(476, 21);
            this.subject.TabIndex = 10;
            this.subject.TextChanged += new System.EventHandler(this.OnSubjectTextChanged);
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
            // send
            // 
            this.send.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.send.Location = new System.Drawing.Point(618, 526);
            this.send.Name = "send";
            this.send.Size = new System.Drawing.Size(75, 23);
            this.send.TabIndex = 12;
            this.send.Text = "Send";
            this.send.UseVisualStyleBackColor = true;
            this.send.Click += new System.EventHandler(this.OnSend);
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(699, 526);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 13;
            this.close.Text = "Cancel";
            this.close.UseVisualStyleBackColor = true;
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
            // edge2
            // 
            this.edge2.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.edge2.AutoSize = false;
            this.edge2.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.ControlRibbon;
            this.edge2.Location = new System.Drawing.Point(10, 143);
            this.edge2.Name = "edge2";
            this.edge2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.edge2.Size = new System.Drawing.Size(530, 1);
            this.edge2.TabIndex = 11;
            this.edge2.Text = "kryptonBorderEdge1";
            // 
            // ccText
            // 
            this.ccText.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ccText.Location = new System.Drawing.Point(64, 63);
            this.ccText.Name = "ccText";
            this.ccText.Size = new System.Drawing.Size(476, 21);
            this.ccText.TabIndex = 6;
            // 
            // toText
            // 
            this.toText.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.toText.Location = new System.Drawing.Point(64, 36);
            this.toText.Name = "toText";
            this.toText.Size = new System.Drawing.Size(476, 21);
            this.toText.TabIndex = 4;
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
            // panelBody
            // 
            this.panelBody.BackColor = System.Drawing.Color.Transparent;
            this.panelBody.Controls.Add(this.toText);
            this.panelBody.Controls.Add(this.htmlEditor);
            this.panelBody.Controls.Add(this.emailAccounts);
            this.panelBody.Controls.Add(this.emailAccount);
            this.panelBody.Controls.Add(this.bccText);
            this.panelBody.Controls.Add(this.toButton);
            this.panelBody.Controls.Add(this.labelSubject);
            this.panelBody.Controls.Add(this.subject);
            this.panelBody.Controls.Add(this.ccButton);
            this.panelBody.Controls.Add(this.edge2);
            this.panelBody.Controls.Add(this.labelSendFrom);
            this.panelBody.Controls.Add(this.ccText);
            this.panelBody.Controls.Add(this.bccButton);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 0);
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(5);
            this.panelBody.Size = new System.Drawing.Size(548, 446);
            this.panelBody.TabIndex = 0;
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
            this.htmlEditor.Size = new System.Drawing.Size(532, 290);
            this.htmlEditor.TabIndex = 12;
            // 
            // toButton
            // 
            this.toButton.AutoSize = true;
            this.toButton.Location = new System.Drawing.Point(9, 36);
            this.toButton.Name = "toButton";
            this.toButton.Size = new System.Drawing.Size(49, 23);
            this.toButton.SplitButton = false;
            this.toButton.TabIndex = 3;
            this.toButton.Text = "To";
            this.toButton.UseVisualStyleBackColor = true;
            // 
            // ccButton
            // 
            this.ccButton.AutoSize = true;
            this.ccButton.Location = new System.Drawing.Point(9, 63);
            this.ccButton.Name = "ccButton";
            this.ccButton.Size = new System.Drawing.Size(49, 23);
            this.ccButton.SplitButton = false;
            this.ccButton.TabIndex = 5;
            this.ccButton.Text = "CC";
            this.ccButton.UseVisualStyleBackColor = true;
            // 
            // bccButton
            // 
            this.bccButton.AutoSize = true;
            this.bccButton.Location = new System.Drawing.Point(9, 89);
            this.bccButton.Name = "bccButton";
            this.bccButton.Size = new System.Drawing.Size(49, 23);
            this.bccButton.SplitButton = false;
            this.bccButton.TabIndex = 7;
            this.bccButton.Text = "BCC";
            this.bccButton.UseVisualStyleBackColor = true;
            // 
            // composerHeaderGroup
            // 
            this.composerHeaderGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.composerHeaderGroup.HeaderVisibleSecondary = false;
            this.composerHeaderGroup.Location = new System.Drawing.Point(0, 0);
            this.composerHeaderGroup.Name = "composerHeaderGroup";
            this.composerHeaderGroup.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            // 
            // composerHeaderGroup.Panel
            // 
            this.composerHeaderGroup.Panel.Controls.Add(this.panelBody);
            this.composerHeaderGroup.Size = new System.Drawing.Size(556, 481);
            this.composerHeaderGroup.StateCommon.HeaderPrimary.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders) ((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.composerHeaderGroup.StateCommon.HeaderPrimary.Content.ShortText.Color1 = System.Drawing.Color.FromArgb(((int) (((byte) (80)))), ((int) (((byte) (80)))), ((int) (((byte) (80)))));
            this.composerHeaderGroup.StateCommon.OverlayHeaders = ComponentFactory.Krypton.Toolkit.InheritBool.False;
            this.composerHeaderGroup.StateNormal.Back.Color1 = System.Drawing.Color.FromArgb(((int) (((byte) (243)))), ((int) (((byte) (243)))), ((int) (((byte) (243)))));
            this.composerHeaderGroup.StateNormal.Back.Color2 = System.Drawing.Color.White;
            this.composerHeaderGroup.StateNormal.Back.ColorAngle = 180F;
            this.composerHeaderGroup.StateNormal.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.Linear;
            this.composerHeaderGroup.StateNormal.Border.Color1 = System.Drawing.Color.Gray;
            this.composerHeaderGroup.StateNormal.Border.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            this.composerHeaderGroup.StateNormal.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders) ((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.composerHeaderGroup.StateNormal.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            this.composerHeaderGroup.StateNormal.Border.Rounding = 8;
            this.composerHeaderGroup.StateNormal.Border.Width = 1;
            this.composerHeaderGroup.StateNormal.HeaderPrimary.Content.LongText.Color1 = System.Drawing.Color.Green;
            this.composerHeaderGroup.StateNormal.HeaderPrimary.Content.LongText.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.composerHeaderGroup.StateNormal.HeaderPrimary.Content.LongText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.composerHeaderGroup.TabIndex = 233;
            this.composerHeaderGroup.Text = "Message 1";
            this.composerHeaderGroup.ValuesPrimary.Description = "";
            this.composerHeaderGroup.ValuesPrimary.Heading = "Message 1";
            this.composerHeaderGroup.ValuesPrimary.Image = null;
            this.composerHeaderGroup.ValuesSecondary.Description = "";
            this.composerHeaderGroup.ValuesSecondary.Heading = "Description";
            this.composerHeaderGroup.ValuesSecondary.Image = null;
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.Location = new System.Drawing.Point(14, 39);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.messageList);
            this.splitContainer.Panel1.Controls.Add(this.kryptonStatusPanel);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.composerHeaderGroup);
            this.splitContainer.Size = new System.Drawing.Size(760, 481);
            this.splitContainer.SplitterDistance = 200;
            this.splitContainer.TabIndex = 2;
            // 
            // messageList
            // 
            this.messageList.AllowMultipleSelection = false;
            this.messageList.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnMessageNumber,
            this.gridColumnMessageSubject});
            this.messageList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messageList.EnableSearching = false;
            this.messageList.ImageTextSeparation = 4;
            this.messageList.Location = new System.Drawing.Point(0, 0);
            this.messageList.Name = "messageList";
            this.messageList.Renderer = windowsXPRenderer1;
            this.messageList.RowDragBehavior = Divelements.SandGrid.RowDragBehavior.InitiateDragDrop;
            this.messageList.Size = new System.Drawing.Size(200, 461);
            this.messageList.StretchPrimaryGrid = false;
            this.messageList.TabIndex = 0;
            this.messageList.WhitespaceClickBehavior = Divelements.SandGrid.WhitespaceClickBehavior.None;
            this.messageList.SelectionChanged += new Divelements.SandGrid.SelectionChangedEventHandler(this.OnSelectedMessageChanged);
            // 
            // gridColumnMessageNumber
            // 
            this.gridColumnMessageNumber.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnMessageNumber.AutoSortType = Divelements.SandGrid.ColumnAutoSortType.None;
            this.gridColumnMessageNumber.HeaderText = "#";
            this.gridColumnMessageNumber.MinimumWidth = 20;
            this.gridColumnMessageNumber.Width = 20;
            // 
            // gridColumnMessageSubject
            // 
            this.gridColumnMessageSubject.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnMessageSubject.AutoSortType = Divelements.SandGrid.ColumnAutoSortType.None;
            this.gridColumnMessageSubject.HeaderText = "Subject";
            this.gridColumnMessageSubject.Width = 176;
            // 
            // kryptonStatusPanel
            // 
            this.kryptonStatusPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.kryptonStatusPanel.GroupBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridHeaderColumnSheet;
            this.kryptonStatusPanel.GroupBorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.ButtonStandalone;
            this.kryptonStatusPanel.Location = new System.Drawing.Point(0, 461);
            this.kryptonStatusPanel.Name = "kryptonStatusPanel";
            this.kryptonStatusPanel.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            // 
            // kryptonStatusPanel.Panel
            // 
            this.kryptonStatusPanel.Panel.Controls.Add(this.labelMessageCount);
            this.kryptonStatusPanel.Size = new System.Drawing.Size(200, 20);
            this.kryptonStatusPanel.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders) (((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
                        | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonStatusPanel.TabIndex = 2;
            // 
            // labelMessageCount
            // 
            this.labelMessageCount.AutoSize = true;
            this.labelMessageCount.BackColor = System.Drawing.Color.Transparent;
            this.labelMessageCount.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (64)))), ((int) (((byte) (64)))), ((int) (((byte) (64)))));
            this.labelMessageCount.Location = new System.Drawing.Point(1, 1);
            this.labelMessageCount.Name = "labelMessageCount";
            this.labelMessageCount.Size = new System.Drawing.Size(67, 13);
            this.labelMessageCount.TabIndex = 0;
            this.labelMessageCount.Text = "Messages: 2";
            // 
            // templateList
            // 
            this.templateList.DropDownHeight = 300;
            this.templateList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.templateList.FormattingEnabled = true;
            this.templateList.IntegralHeight = false;
            this.templateList.Location = new System.Drawing.Point(67, 8);
            this.templateList.Name = "templateList";
            this.templateList.Size = new System.Drawing.Size(253, 21);
            this.templateList.TabIndex = 1;
            this.templateList.SelectedTemplateChanged += new System.EventHandler(this.OnChangeTemplate);
            // 
            // EmailComposerDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(788, 561);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.templateList);
            this.Controls.Add(this.close);
            this.Controls.Add(this.send);
            this.Controls.Add(this.labelTemplate);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(564, 445);
            this.Name = "EmailComposerDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Compose Email";
            this.Load += new System.EventHandler(this.OnLoad);
            this.Shown += new System.EventHandler(this.OnShown);
            this.panelBody.ResumeLayout(false);
            this.panelBody.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.composerHeaderGroup.Panel)).EndInit();
            this.composerHeaderGroup.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.composerHeaderGroup)).EndInit();
            this.composerHeaderGroup.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.kryptonStatusPanel.Panel)).EndInit();
            this.kryptonStatusPanel.Panel.ResumeLayout(false);
            this.kryptonStatusPanel.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.kryptonStatusPanel)).EndInit();
            this.kryptonStatusPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ShipWorks.UI.Controls.Html.HtmlEditor htmlEditor;
        private System.Windows.Forms.TextBox bccText;
        private System.Windows.Forms.Label labelSendFrom;
        private System.Windows.Forms.Label labelTemplate;
        private System.Windows.Forms.TextBox subject;
        private System.Windows.Forms.Label labelSubject;
        private System.Windows.Forms.Button send;
        private System.Windows.Forms.Button close;
        private ShipWorks.Templates.Controls.TemplateComboBox templateList;
        private System.Windows.Forms.Button emailAccounts;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge edge2;
        private ShipWorks.UI.Controls.DropDownButton bccButton;
        private ShipWorks.UI.Controls.DropDownButton ccButton;
        private System.Windows.Forms.TextBox ccText;
        private ShipWorks.UI.Controls.DropDownButton toButton;
        private System.Windows.Forms.TextBox toText;
        private System.Windows.Forms.ComboBox emailAccount;
        private System.Windows.Forms.Panel panelBody;
        private ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup composerHeaderGroup;
        private System.Windows.Forms.SplitContainer splitContainer;
        private Divelements.SandGrid.SandGrid messageList;
        private Divelements.SandGrid.GridColumn gridColumnMessageSubject;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup kryptonStatusPanel;
        private System.Windows.Forms.Label labelMessageCount;
        private Divelements.SandGrid.GridColumn gridColumnMessageNumber;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;

    }
}