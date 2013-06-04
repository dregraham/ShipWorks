namespace ShipWorks.Email.Outlook
{
    partial class EmailSentViewerDlg
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
            this.close = new System.Windows.Forms.Button();
            this.headerGroup = new ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup();
            this.panelBody = new System.Windows.Forms.Panel();
            this.panelWithBorder = new System.Windows.Forms.Panel();
            this.htmlControl = new ShipWorks.UI.Controls.Html.HtmlControl();
            this.labelBCC = new System.Windows.Forms.Label();
            this.labelCC = new System.Windows.Forms.Label();
            this.labelTo = new System.Windows.Forms.Label();
            this.from = new System.Windows.Forms.TextBox();
            this.to = new System.Windows.Forms.TextBox();
            this.bcc = new System.Windows.Forms.TextBox();
            this.labelSubject = new System.Windows.Forms.Label();
            this.subject = new System.Windows.Forms.TextBox();
            this.edge2 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.labelSendFrom = new System.Windows.Forms.Label();
            this.cc = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize) (this.headerGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.headerGroup.Panel)).BeginInit();
            this.headerGroup.Panel.SuspendLayout();
            this.headerGroup.SuspendLayout();
            this.panelBody.SuspendLayout();
            this.panelWithBorder.SuspendLayout();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(526, 489);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 1;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
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
            this.headerGroup.Size = new System.Drawing.Size(593, 474);
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
            this.headerGroup.StateNormal.HeaderPrimary.Content.LongText.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.headerGroup.StateNormal.HeaderPrimary.Content.LongText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            this.headerGroup.TabIndex = 0;
            this.headerGroup.Text = "Thank you for your order!";
            this.headerGroup.ValuesPrimary.Description = "Sent: Today 12:45 AM";
            this.headerGroup.ValuesPrimary.Heading = "Thank you for your order!";
            this.headerGroup.ValuesPrimary.Image = null;
            this.headerGroup.ValuesSecondary.Description = "";
            this.headerGroup.ValuesSecondary.Heading = "Description";
            this.headerGroup.ValuesSecondary.Image = null;
            // 
            // panelBody
            // 
            this.panelBody.BackColor = System.Drawing.Color.Transparent;
            this.panelBody.Controls.Add(this.panelWithBorder);
            this.panelBody.Controls.Add(this.labelBCC);
            this.panelBody.Controls.Add(this.labelCC);
            this.panelBody.Controls.Add(this.labelTo);
            this.panelBody.Controls.Add(this.from);
            this.panelBody.Controls.Add(this.to);
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
            this.panelBody.Size = new System.Drawing.Size(585, 439);
            this.panelBody.TabIndex = 0;
            // 
            // panelWithBorder
            // 
            this.panelWithBorder.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelWithBorder.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelWithBorder.Controls.Add(this.htmlControl);
            this.panelWithBorder.Location = new System.Drawing.Point(10, 150);
            this.panelWithBorder.Name = "panelWithBorder";
            this.panelWithBorder.Size = new System.Drawing.Size(567, 283);
            this.panelWithBorder.TabIndex = 234;
            // 
            // htmlControl
            // 
            this.htmlControl.AllowActiveContent = false;
            this.htmlControl.AllowContextMenu = false;
            this.htmlControl.AllowNavigation = false;
            this.htmlControl.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.htmlControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htmlControl.Html = "";
            this.htmlControl.Location = new System.Drawing.Point(0, 0);
            this.htmlControl.Name = "htmlControl";
            this.htmlControl.OpenLinksInNewWindow = true;
            this.htmlControl.SelectionAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.htmlControl.SelectionBackColor = System.Drawing.Color.Empty;
            this.htmlControl.SelectionBold = false;
            this.htmlControl.SelectionBullets = false;
            this.htmlControl.SelectionFont = null;
            this.htmlControl.SelectionFontName = "Times New Roman";
            this.htmlControl.SelectionFontSize = 2;
            this.htmlControl.SelectionForeColor = System.Drawing.Color.Empty;
            this.htmlControl.SelectionItalic = false;
            this.htmlControl.SelectionNumbering = false;
            this.htmlControl.SelectionUnderline = false;
            this.htmlControl.ShowBorderGuides = true;
            this.htmlControl.ShowGlyphs = false;
            this.htmlControl.Size = new System.Drawing.Size(563, 279);
            this.htmlControl.TabIndex = 0;
            // 
            // labelBCC
            // 
            this.labelBCC.AutoSize = true;
            this.labelBCC.Location = new System.Drawing.Point(30, 92);
            this.labelBCC.Name = "labelBCC";
            this.labelBCC.Size = new System.Drawing.Size(31, 13);
            this.labelBCC.TabIndex = 6;
            this.labelBCC.Text = "BCC:";
            // 
            // labelCC
            // 
            this.labelCC.AutoSize = true;
            this.labelCC.Location = new System.Drawing.Point(35, 66);
            this.labelCC.Name = "labelCC";
            this.labelCC.Size = new System.Drawing.Size(25, 13);
            this.labelCC.TabIndex = 4;
            this.labelCC.Text = "CC:";
            // 
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Location = new System.Drawing.Point(35, 39);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(23, 13);
            this.labelTo.TabIndex = 2;
            this.labelTo.Text = "To:";
            // 
            // from
            // 
            this.from.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.from.BackColor = System.Drawing.Color.White;
            this.from.Location = new System.Drawing.Point(64, 9);
            this.from.Name = "from";
            this.from.ReadOnly = true;
            this.from.Size = new System.Drawing.Size(513, 21);
            this.from.TabIndex = 1;
            // 
            // to
            // 
            this.to.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.to.BackColor = System.Drawing.Color.White;
            this.to.Location = new System.Drawing.Point(64, 36);
            this.to.Name = "to";
            this.to.ReadOnly = true;
            this.to.Size = new System.Drawing.Size(513, 21);
            this.to.TabIndex = 3;
            // 
            // bcc
            // 
            this.bcc.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.bcc.BackColor = System.Drawing.Color.White;
            this.bcc.Location = new System.Drawing.Point(64, 89);
            this.bcc.Name = "bcc";
            this.bcc.ReadOnly = true;
            this.bcc.Size = new System.Drawing.Size(513, 21);
            this.bcc.TabIndex = 7;
            // 
            // labelSubject
            // 
            this.labelSubject.AutoSize = true;
            this.labelSubject.Location = new System.Drawing.Point(11, 117);
            this.labelSubject.Name = "labelSubject";
            this.labelSubject.Size = new System.Drawing.Size(47, 13);
            this.labelSubject.TabIndex = 8;
            this.labelSubject.Text = "Subject:";
            // 
            // subject
            // 
            this.subject.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.subject.BackColor = System.Drawing.Color.White;
            this.subject.Location = new System.Drawing.Point(64, 114);
            this.subject.Name = "subject";
            this.subject.ReadOnly = true;
            this.subject.Size = new System.Drawing.Size(513, 21);
            this.subject.TabIndex = 9;
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
            this.edge2.Size = new System.Drawing.Size(567, 1);
            this.edge2.TabIndex = 10;
            this.edge2.Text = "kryptonBorderEdge1";
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
            // cc
            // 
            this.cc.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cc.BackColor = System.Drawing.Color.White;
            this.cc.Location = new System.Drawing.Point(64, 63);
            this.cc.Name = "cc";
            this.cc.ReadOnly = true;
            this.cc.Size = new System.Drawing.Size(513, 21);
            this.cc.TabIndex = 5;
            // 
            // EmailSentViewerDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(613, 524);
            this.Controls.Add(this.headerGroup);
            this.Controls.Add(this.close);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EmailSentViewerDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "View Email";
            this.Load += new System.EventHandler(this.OnLoad);
            this.Shown += new System.EventHandler(this.OnShown);
            ((System.ComponentModel.ISupportInitialize) (this.headerGroup.Panel)).EndInit();
            this.headerGroup.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.headerGroup)).EndInit();
            this.headerGroup.ResumeLayout(false);
            this.panelBody.ResumeLayout(false);
            this.panelBody.PerformLayout();
            this.panelWithBorder.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button close;
        private ComponentFactory.Krypton.Toolkit.KryptonHeaderGroup headerGroup;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.Label labelBCC;
        private System.Windows.Forms.Label labelCC;
        private System.Windows.Forms.Label labelTo;
        private System.Windows.Forms.TextBox from;
        private System.Windows.Forms.TextBox to;
        private System.Windows.Forms.TextBox bcc;
        private System.Windows.Forms.Label labelSubject;
        private System.Windows.Forms.TextBox subject;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge edge2;
        private System.Windows.Forms.Label labelSendFrom;
        private System.Windows.Forms.TextBox cc;
        private ShipWorks.UI.Controls.Html.HtmlControl htmlControl;
        private System.Windows.Forms.Panel panelWithBorder;
    }
}