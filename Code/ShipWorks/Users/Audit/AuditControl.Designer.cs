namespace ShipWorks.Users.Audit
{
    partial class AuditControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            ShipWorks.UI.Controls.SandGrid.WindowsXPShipWorksRenderer windowsXPShipWorksRenderer1 = new ShipWorks.UI.Controls.SandGrid.WindowsXPShipWorksRenderer();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuditControl));
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.panelSearchButton = new System.Windows.Forms.Panel();
            this.pictureSearchCriteria = new System.Windows.Forms.PictureBox();
            this.labelSearchCriteria = new System.Windows.Forms.Label();
            this.panelSearchControls = new System.Windows.Forms.Panel();
            this.panelDate = new System.Windows.Forms.UserControl();
            this.panelRelatedTo = new System.Windows.Forms.Panel();
            this.relatedToBox = new System.Windows.Forms.CheckBox();
            this.panelUserLocked = new System.Windows.Forms.Panel();
            this.computerBox = new System.Windows.Forms.CheckBox();
            this.reasonCombo = new System.Windows.Forms.ComboBox();
            this.reasonBox = new System.Windows.Forms.CheckBox();
            this.computerCombo = new System.Windows.Forms.ComboBox();
            this.actionCombo = new System.Windows.Forms.ComboBox();
            this.actionBox = new System.Windows.Forms.CheckBox();
            this.dateBox = new System.Windows.Forms.CheckBox();
            this.userBox = new System.Windows.Forms.CheckBox();
            this.entityGrid = new ShipWorks.Data.Grid.Paging.PagedEntityGrid();
            this.userCombo = new ShipWorks.UI.Controls.ImageComboBox();
            this.infotipRelatedTo = new ShipWorks.UI.Controls.InfoTip();
            this.searchBox = new ShipWorks.UI.Controls.Krypton.WatermarkKryptonTextBox();
            this.buttonSearching = new ComponentFactory.Krypton.Toolkit.ButtonSpecAny();
            this.panelSearchButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureSearchCriteria)).BeginInit();
            this.panelSearchControls.SuspendLayout();
            this.panelRelatedTo.SuspendLayout();
            this.panelUserLocked.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Interval = 10000;
            this.timer.Tick += new System.EventHandler(this.OnTimer);
            // 
            // panelSearchButton
            // 
            this.panelSearchButton.Controls.Add(this.pictureSearchCriteria);
            this.panelSearchButton.Controls.Add(this.labelSearchCriteria);
            this.panelSearchButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearchButton.Location = new System.Drawing.Point(0, 0);
            this.panelSearchButton.Name = "panelSearchButton";
            this.panelSearchButton.Size = new System.Drawing.Size(550, 21);
            this.panelSearchButton.TabIndex = 0;
            // 
            // pictureSearchCriteria
            // 
            this.pictureSearchCriteria.Image = global::ShipWorks.Properties.Resources.view;
            this.pictureSearchCriteria.Location = new System.Drawing.Point(5, 3);
            this.pictureSearchCriteria.Name = "pictureSearchCriteria";
            this.pictureSearchCriteria.Size = new System.Drawing.Size(16, 16);
            this.pictureSearchCriteria.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureSearchCriteria.TabIndex = 11;
            this.pictureSearchCriteria.TabStop = false;
            // 
            // labelSearchCriteria
            // 
            this.labelSearchCriteria.AutoSize = true;
            this.labelSearchCriteria.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSearchCriteria.Location = new System.Drawing.Point(22, 4);
            this.labelSearchCriteria.Name = "labelSearchCriteria";
            this.labelSearchCriteria.Size = new System.Drawing.Size(91, 13);
            this.labelSearchCriteria.TabIndex = 0;
            this.labelSearchCriteria.Text = "Search Criteria";
            // 
            // panelSearchControls
            // 
            this.panelSearchControls.Controls.Add(this.panelDate);
            this.panelSearchControls.Controls.Add(this.userCombo);
            this.panelSearchControls.Controls.Add(this.panelRelatedTo);
            this.panelSearchControls.Controls.Add(this.panelUserLocked);
            this.panelSearchControls.Controls.Add(this.actionCombo);
            this.panelSearchControls.Controls.Add(this.actionBox);
            this.panelSearchControls.Controls.Add(this.dateBox);
            this.panelSearchControls.Controls.Add(this.userBox);
            this.panelSearchControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearchControls.Location = new System.Drawing.Point(0, 21);
            this.panelSearchControls.Name = "panelSearchControls";
            this.panelSearchControls.Size = new System.Drawing.Size(550, 77);
            this.panelSearchControls.TabIndex = 1;
            // 
            // panelDate
            // 
            this.panelDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDate.Enabled = false;
            this.panelDate.Location = new System.Drawing.Point(327, 23);
            this.panelDate.Name = "panelDate";
            this.panelDate.Size = new System.Drawing.Size(215, 27);
            this.panelDate.TabIndex = 6;
            // 
            // panelRelatedTo
            // 
            this.panelRelatedTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRelatedTo.Controls.Add(this.infotipRelatedTo);
            this.panelRelatedTo.Controls.Add(this.searchBox);
            this.panelRelatedTo.Controls.Add(this.relatedToBox);
            this.panelRelatedTo.Location = new System.Drawing.Point(248, 50);
            this.panelRelatedTo.Name = "panelRelatedTo";
            this.panelRelatedTo.Size = new System.Drawing.Size(299, 24);
            this.panelRelatedTo.TabIndex = 7;
            // 
            // relatedToBox
            // 
            this.relatedToBox.AutoSize = true;
            this.relatedToBox.Location = new System.Drawing.Point(3, 2);
            this.relatedToBox.Name = "relatedToBox";
            this.relatedToBox.Size = new System.Drawing.Size(82, 17);
            this.relatedToBox.TabIndex = 0;
            this.relatedToBox.Text = "Related To:";
            this.relatedToBox.UseVisualStyleBackColor = true;
            this.relatedToBox.CheckedChanged += new System.EventHandler(this.OnSearchCheckChanged);
            // 
            // panelUserLocked
            // 
            this.panelUserLocked.Controls.Add(this.computerBox);
            this.panelUserLocked.Controls.Add(this.reasonCombo);
            this.panelUserLocked.Controls.Add(this.reasonBox);
            this.panelUserLocked.Controls.Add(this.computerCombo);
            this.panelUserLocked.Location = new System.Drawing.Point(24, 25);
            this.panelUserLocked.Name = "panelUserLocked";
            this.panelUserLocked.Size = new System.Drawing.Size(221, 50);
            this.panelUserLocked.TabIndex = 2;
            // 
            // computerBox
            // 
            this.computerBox.AutoSize = true;
            this.computerBox.Location = new System.Drawing.Point(3, 3);
            this.computerBox.Name = "computerBox";
            this.computerBox.Size = new System.Drawing.Size(77, 17);
            this.computerBox.TabIndex = 0;
            this.computerBox.Text = "Computer:";
            this.computerBox.UseVisualStyleBackColor = true;
            this.computerBox.CheckedChanged += new System.EventHandler(this.OnSearchCheckChanged);
            // 
            // reasonCombo
            // 
            this.reasonCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.reasonCombo.Enabled = false;
            this.reasonCombo.FormattingEnabled = true;
            this.reasonCombo.Location = new System.Drawing.Point(87, 25);
            this.reasonCombo.Name = "reasonCombo";
            this.reasonCombo.Size = new System.Drawing.Size(121, 21);
            this.reasonCombo.TabIndex = 3;
            this.reasonCombo.SelectedIndexChanged += new System.EventHandler(this.OnSearchValueChanged);
            // 
            // reasonBox
            // 
            this.reasonBox.AutoSize = true;
            this.reasonBox.Location = new System.Drawing.Point(3, 27);
            this.reasonBox.Name = "reasonBox";
            this.reasonBox.Size = new System.Drawing.Size(66, 17);
            this.reasonBox.TabIndex = 2;
            this.reasonBox.Text = "Reason:";
            this.reasonBox.UseVisualStyleBackColor = true;
            this.reasonBox.CheckedChanged += new System.EventHandler(this.OnSearchCheckChanged);
            // 
            // computerCombo
            // 
            this.computerCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.computerCombo.Enabled = false;
            this.computerCombo.FormattingEnabled = true;
            this.computerCombo.Location = new System.Drawing.Point(87, 1);
            this.computerCombo.Name = "computerCombo";
            this.computerCombo.Size = new System.Drawing.Size(121, 21);
            this.computerCombo.TabIndex = 1;
            this.computerCombo.SelectedIndexChanged += new System.EventHandler(this.OnSearchValueChanged);
            // 
            // actionCombo
            // 
            this.actionCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.actionCombo.Enabled = false;
            this.actionCombo.FormattingEnabled = true;
            this.actionCombo.Location = new System.Drawing.Point(332, 1);
            this.actionCombo.Name = "actionCombo";
            this.actionCombo.Size = new System.Drawing.Size(154, 21);
            this.actionCombo.TabIndex = 4;
            this.actionCombo.SelectedIndexChanged += new System.EventHandler(this.OnSearchValueChanged);
            // 
            // actionBox
            // 
            this.actionBox.AutoSize = true;
            this.actionBox.Location = new System.Drawing.Point(251, 3);
            this.actionBox.Name = "actionBox";
            this.actionBox.Size = new System.Drawing.Size(60, 17);
            this.actionBox.TabIndex = 3;
            this.actionBox.Text = "Action:";
            this.actionBox.UseVisualStyleBackColor = true;
            this.actionBox.CheckedChanged += new System.EventHandler(this.OnSearchCheckChanged);
            // 
            // dateBox
            // 
            this.dateBox.AutoSize = true;
            this.dateBox.Location = new System.Drawing.Point(251, 28);
            this.dateBox.Name = "dateBox";
            this.dateBox.Size = new System.Drawing.Size(53, 17);
            this.dateBox.TabIndex = 5;
            this.dateBox.Text = "Date:";
            this.dateBox.UseVisualStyleBackColor = true;
            this.dateBox.CheckedChanged += new System.EventHandler(this.OnSearchCheckChanged);
            // 
            // userBox
            // 
            this.userBox.AutoSize = true;
            this.userBox.Location = new System.Drawing.Point(27, 3);
            this.userBox.Name = "userBox";
            this.userBox.Size = new System.Drawing.Size(52, 17);
            this.userBox.TabIndex = 0;
            this.userBox.Text = "User:";
            this.userBox.UseVisualStyleBackColor = true;
            this.userBox.CheckedChanged += new System.EventHandler(this.OnSearchCheckChanged);
            // 
            // entityGrid
            // 
            this.entityGrid.AllowMultipleSelection = false;
            this.entityGrid.DetailViewSettings = null;
            this.entityGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.entityGrid.EnableSearching = false;
            this.entityGrid.GridLines = Divelements.SandGrid.GridLinesDisplayType.Both;
            this.entityGrid.LiveResize = false;
            this.entityGrid.Location = new System.Drawing.Point(0, 98);
            this.entityGrid.Name = "entityGrid";
            this.entityGrid.NullRepresentation = "";
            this.entityGrid.Renderer = windowsXPShipWorksRenderer1;
            this.entityGrid.RowHighlightType = Divelements.SandGrid.RowHighlightType.Full;
            this.entityGrid.ShadeAlternateRows = true;
            this.entityGrid.Size = new System.Drawing.Size(550, 374);
            this.entityGrid.TabIndex = 2;
            this.entityGrid.RowActivated += new Divelements.SandGrid.GridRowEventHandler(this.OnRowActivate);
            // 
            // userCombo
            // 
            this.userCombo.Enabled = false;
            this.userCombo.FormattingEnabled = true;
            this.userCombo.Location = new System.Drawing.Point(111, 1);
            this.userCombo.Name = "userCombo";
            this.userCombo.Size = new System.Drawing.Size(121, 21);
            this.userCombo.TabIndex = 1;
            this.userCombo.SelectedIndexChanged += new System.EventHandler(this.OnSearchValueChanged);
            // 
            // infotipRelatedTo
            // 
            this.infotipRelatedTo.Caption = "You can enter an order number, customer name, email address, or any data you want" +
    " to find.";
            this.infotipRelatedTo.Location = new System.Drawing.Point(244, 6);
            this.infotipRelatedTo.Name = "infotipRelatedTo";
            this.infotipRelatedTo.Size = new System.Drawing.Size(12, 12);
            this.infotipRelatedTo.TabIndex = 21;
            this.infotipRelatedTo.Title = "Related To";
            // 
            // searchBox
            // 
            this.searchBox.ButtonSpecs.AddRange(new ComponentFactory.Krypton.Toolkit.ButtonSpecAny[] {
            this.buttonSearching});
            this.searchBox.Enabled = false;
            this.searchBox.Location = new System.Drawing.Point(84, 0);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(154, 20);
            this.searchBox.TabIndex = 1;
            this.searchBox.WaterColor = System.Drawing.SystemColors.GrayText;
            this.searchBox.WordWrap = false;
            this.searchBox.TextChanged += new System.EventHandler(this.OnSearchTextChanged);
            // 
            // buttonSearching
            // 
            this.buttonSearching.Edge = ComponentFactory.Krypton.Toolkit.PaletteRelativeEdgeAlign.Far;
            this.buttonSearching.Enabled = ComponentFactory.Krypton.Toolkit.ButtonEnabled.False;
            this.buttonSearching.Image = ((System.Drawing.Image)(resources.GetObject("buttonSearching.Image")));
            this.buttonSearching.ImageStates.ImageDisabled = ((System.Drawing.Image)(resources.GetObject("buttonSearching.ImageStates.ImageDisabled")));
            this.buttonSearching.Type = ComponentFactory.Krypton.Toolkit.PaletteButtonSpecStyle.Close;
            this.buttonSearching.UniqueName = "370ABDC6B9F24E16370ABDC6B9F24E16";
            // 
            // AuditControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.entityGrid);
            this.Controls.Add(this.panelSearchControls);
            this.Controls.Add(this.panelSearchButton);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "AuditControl";
            this.Size = new System.Drawing.Size(550, 472);
            this.panelSearchButton.ResumeLayout(false);
            this.panelSearchButton.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureSearchCriteria)).EndInit();
            this.panelSearchControls.ResumeLayout(false);
            this.panelSearchControls.PerformLayout();
            this.panelRelatedTo.ResumeLayout(false);
            this.panelRelatedTo.PerformLayout();
            this.panelUserLocked.ResumeLayout(false);
            this.panelUserLocked.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.Data.Grid.Paging.PagedEntityGrid entityGrid;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Panel panelSearchButton;
        private System.Windows.Forms.Panel panelSearchControls;
        private System.Windows.Forms.CheckBox actionBox;
        private System.Windows.Forms.ComboBox actionCombo;
        private System.Windows.Forms.ComboBox reasonCombo;
        private System.Windows.Forms.ComboBox computerCombo;
        private System.Windows.Forms.CheckBox reasonBox;
        private System.Windows.Forms.CheckBox computerBox;
        private System.Windows.Forms.CheckBox relatedToBox;
        private System.Windows.Forms.CheckBox dateBox;
        private System.Windows.Forms.CheckBox userBox;
        private System.Windows.Forms.Panel panelUserLocked;
        private System.Windows.Forms.Panel panelRelatedTo;
        private ShipWorks.UI.Controls.ImageComboBox userCombo;
        private System.Windows.Forms.PictureBox pictureSearchCriteria;
        private System.Windows.Forms.Label labelSearchCriteria;
        private System.Windows.Forms.UserControl panelDate;
        private ShipWorks.UI.Controls.Krypton.WatermarkKryptonTextBox searchBox;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecAny buttonSearching;
        private UI.Controls.InfoTip infotipRelatedTo;
    }
}
