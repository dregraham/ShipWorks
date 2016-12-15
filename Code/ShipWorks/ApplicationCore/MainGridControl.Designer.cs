namespace ShipWorks.ApplicationCore
{
    partial class MainGridControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainGridControl));
            this.gridPanel = new System.Windows.Forms.Panel();
            this.gridColumnOrderNumber = new Divelements.SandGrid.GridColumn();
            this.gridColumnOrderDate = new Divelements.SandGrid.Specialized.GridDateTimeColumn();
            this.gridColumnBillFirstName = new Divelements.SandGrid.GridColumn();
            this.gridColumnBillLastName = new Divelements.SandGrid.GridColumn();
            this.gridColumnBillCompany = new Divelements.SandGrid.GridColumn();
            this.gridColumnBillCity = new Divelements.SandGrid.GridColumn();
            this.gridColumnBillState = new Divelements.SandGrid.GridColumn();
            this.gridColumnBillPostalCode = new Divelements.SandGrid.GridColumn();
            this.gridColumnBillCountry = new Divelements.SandGrid.GridColumn();
            this.gridColumnBillPhone = new Divelements.SandGrid.GridColumn();
            this.gridColumnBillEmail = new Divelements.SandGrid.GridColumn();
            this.gridColumnShipLastName = new Divelements.SandGrid.GridColumn();
            this.kryptonHeader = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.kryptonGroup = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.pictureSearchHourglass = new System.Windows.Forms.PictureBox();
            this.searchBox = new ShipWorks.UI.Controls.Krypton.WatermarkKryptonTextBox();
            this.buttonEndSearch = new ComponentFactory.Krypton.Toolkit.ButtonSpecAny();
            this.kryptonHeaderSearchContainer = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.buttonAdvancedSearch = new ComponentFactory.Krypton.Toolkit.ButtonSpecAny();
            this.kryptonBorderEdge1 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.kryptonBorderEdge2 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.kryptonBorderEdge3 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.borderAdvanced = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.filterEditor = new ShipWorks.Filters.Controls.FilterDefinitionEditor();
            ((System.ComponentModel.ISupportInitialize) (this.kryptonGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.kryptonGroup.Panel)).BeginInit();
            this.kryptonGroup.Panel.SuspendLayout();
            this.kryptonGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureSearchHourglass)).BeginInit();
            this.SuspendLayout();
            //
            // gridPanel
            //
            this.gridPanel.BackColor = System.Drawing.SystemColors.Control;
            this.gridPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridPanel.Location = new System.Drawing.Point(1, 62);
            this.gridPanel.Name = "gridPanel";
            this.gridPanel.Padding = new System.Windows.Forms.Padding(1, 0, 1, 1);
            this.gridPanel.Size = new System.Drawing.Size(731, 468);
            this.gridPanel.TabIndex = 0;
            //
            // gridColumnOrderNumber
            //
            this.gridColumnOrderNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            //
            // gridColumnOrderDate
            //
            this.gridColumnOrderDate.DataFormatString = "{0:d}";
            this.gridColumnOrderDate.EditorType = typeof(Divelements.SandGrid.GridDateTimeEditor);
            this.gridColumnOrderDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            //
            // gridColumnBillFirstName
            //
            this.gridColumnBillFirstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            //
            // gridColumnBillLastName
            //
            this.gridColumnBillLastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            //
            // gridColumnBillCompany
            //
            this.gridColumnBillCompany.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            //
            // gridColumnBillCity
            //
            this.gridColumnBillCity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            //
            // gridColumnBillState
            //
            this.gridColumnBillState.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            //
            // gridColumnBillPostalCode
            //
            this.gridColumnBillPostalCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            //
            // gridColumnBillCountry
            //
            this.gridColumnBillCountry.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            //
            // gridColumnBillPhone
            //
            this.gridColumnBillPhone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            //
            // gridColumnBillEmail
            //
            this.gridColumnBillEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            //
            // gridColumnShipLastName
            //
            this.gridColumnShipLastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            //
            // kryptonHeader
            //
            this.kryptonHeader.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonHeader.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
            this.kryptonHeader.Location = new System.Drawing.Point(0, 0);
            this.kryptonHeader.Name = "kryptonHeader";
            this.kryptonHeader.Size = new System.Drawing.Size(518, 29);
            this.kryptonHeader.StateCommon.Border.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.False;
            this.kryptonHeader.StateCommon.Border.DrawBorders = ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.None;
            this.kryptonHeader.StateCommon.Content.LongText.Color1 = System.Drawing.SystemColors.ControlDarkDark;
            this.kryptonHeader.StateCommon.Content.LongText.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.kryptonHeader.TabIndex = 0;
            this.kryptonHeader.Text = "Orders";
            this.kryptonHeader.Values.Description = "Searching";
            this.kryptonHeader.Values.Heading = "Orders";
            this.kryptonHeader.Values.Image = global::ShipWorks.Properties.Resources.order16;
            //
            // kryptonGroup
            //
            this.kryptonGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonGroup.GroupBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.HeaderPrimary;
            this.kryptonGroup.Location = new System.Drawing.Point(0, 0);
            this.kryptonGroup.Name = "kryptonGroup";
            //
            // kryptonGroup.Panel
            //
            this.kryptonGroup.Panel.Controls.Add(this.pictureSearchHourglass);
            this.kryptonGroup.Panel.Controls.Add(this.kryptonHeader);
            this.kryptonGroup.Panel.Controls.Add(this.searchBox);
            this.kryptonGroup.Panel.Controls.Add(this.kryptonHeaderSearchContainer);
            this.kryptonGroup.Size = new System.Drawing.Size(733, 30);
            this.kryptonGroup.TabIndex = 1;
            //
            // pictureSearchHourglass
            //
            this.pictureSearchHourglass.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureSearchHourglass.BackColor = System.Drawing.Color.Transparent;
            this.pictureSearchHourglass.Image = ((System.Drawing.Image) (resources.GetObject("pictureSearchHourglass.Image")));
            this.pictureSearchHourglass.Location = new System.Drawing.Point(449, 5);
            this.pictureSearchHourglass.Name = "pictureSearchHourglass";
            this.pictureSearchHourglass.Size = new System.Drawing.Size(16, 16);
            this.pictureSearchHourglass.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureSearchHourglass.TabIndex = 4;
            this.pictureSearchHourglass.TabStop = false;
            //
            // searchBox
            //
            this.searchBox.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchBox.ButtonSpecs.AddRange(new ComponentFactory.Krypton.Toolkit.ButtonSpecAny[] {
            this.buttonEndSearch});
            this.searchBox.Location = new System.Drawing.Point(526, 3);
            this.searchBox.MaxLength = 3998;
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(181, 20);
            this.searchBox.TabIndex = 0;
            this.searchBox.WaterColor = System.Drawing.SystemColors.GrayText;
            this.searchBox.WaterText = "Search All Orders";
            this.searchBox.WordWrap = false;
            //
            // buttonEndSearch
            //
            this.buttonEndSearch.Edge = ComponentFactory.Krypton.Toolkit.PaletteRelativeEdgeAlign.Far;
            this.buttonEndSearch.Enabled = ComponentFactory.Krypton.Toolkit.ButtonEnabled.False;
            this.buttonEndSearch.ExtraText = "";
            this.buttonEndSearch.Image = ((System.Drawing.Image) (resources.GetObject("buttonEndSearch.Image")));
            this.buttonEndSearch.ImageStates.ImageDisabled = ((System.Drawing.Image) (resources.GetObject("buttonEndSearch.ImageStates.ImageDisabled")));
            this.buttonEndSearch.Text = "";
            this.buttonEndSearch.Type = ComponentFactory.Krypton.Toolkit.PaletteButtonSpecStyle.Close;
            this.buttonEndSearch.UniqueName = "370ABDC6B9F24E16370ABDC6B9F24E16";
            this.buttonEndSearch.Click += new System.EventHandler(this.OnEndSearch);
            //
            // kryptonHeaderSearchContainer
            //
            this.kryptonHeaderSearchContainer.AllowButtonSpecToolTips = true;
            this.kryptonHeaderSearchContainer.ButtonSpecs.AddRange(new ComponentFactory.Krypton.Toolkit.ButtonSpecAny[] {
            this.buttonAdvancedSearch});
            this.kryptonHeaderSearchContainer.Dock = System.Windows.Forms.DockStyle.Right;
            this.kryptonHeaderSearchContainer.Location = new System.Drawing.Point(706, 0);
            this.kryptonHeaderSearchContainer.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.kryptonHeaderSearchContainer.Name = "kryptonHeaderSearchContainer";
            this.kryptonHeaderSearchContainer.Size = new System.Drawing.Size(25, 28);
            this.kryptonHeaderSearchContainer.StateCommon.Back.ColorStyle = ComponentFactory.Krypton.Toolkit.PaletteColorStyle.RoundedTopLight;
            this.kryptonHeaderSearchContainer.StateCommon.Border.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.False;
            this.kryptonHeaderSearchContainer.StateCommon.Border.DrawBorders = ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.None;
            this.kryptonHeaderSearchContainer.TabIndex = 1;
            this.kryptonHeaderSearchContainer.Values.Description = "";
            this.kryptonHeaderSearchContainer.Values.Heading = "";
            this.kryptonHeaderSearchContainer.Values.Image = null;
            //
            // buttonAdvancedSearch
            //
            this.buttonAdvancedSearch.ExtraText = "";
            this.buttonAdvancedSearch.Image = null;
            this.buttonAdvancedSearch.Style = ComponentFactory.Krypton.Toolkit.PaletteButtonStyle.Standalone;
            this.buttonAdvancedSearch.Text = "";
            this.buttonAdvancedSearch.ToolTipBody = "Opens and closes the Advanced Search area.";
            this.buttonAdvancedSearch.ToolTipStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.SuperTip;
            this.buttonAdvancedSearch.ToolTipTitle = "Advanced Search";
            this.buttonAdvancedSearch.Type = ComponentFactory.Krypton.Toolkit.PaletteButtonSpecStyle.ArrowDown;
            this.buttonAdvancedSearch.UniqueName = "0A0D0B4DF83C4A4A0A0D0B4DF83C4A4A";
            this.buttonAdvancedSearch.Click += new System.EventHandler(this.OnAdvancedSearch);
            //
            // kryptonBorderEdge1
            //
            this.kryptonBorderEdge1.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.HeaderPrimary;
            this.kryptonBorderEdge1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.kryptonBorderEdge1.Location = new System.Drawing.Point(0, 530);
            this.kryptonBorderEdge1.Name = "kryptonBorderEdge1";
            this.kryptonBorderEdge1.Size = new System.Drawing.Size(733, 1);
            this.kryptonBorderEdge1.TabIndex = 5;
            this.kryptonBorderEdge1.Text = "kryptonBorderEdge1";
            //
            // kryptonBorderEdge2
            //
            this.kryptonBorderEdge2.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.HeaderPrimary;
            this.kryptonBorderEdge2.Dock = System.Windows.Forms.DockStyle.Right;
            this.kryptonBorderEdge2.Location = new System.Drawing.Point(732, 30);
            this.kryptonBorderEdge2.Name = "kryptonBorderEdge2";
            this.kryptonBorderEdge2.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.kryptonBorderEdge2.Size = new System.Drawing.Size(1, 500);
            this.kryptonBorderEdge2.TabIndex = 6;
            this.kryptonBorderEdge2.Text = "kryptonBorderEdge2";
            //
            // kryptonBorderEdge3
            //
            this.kryptonBorderEdge3.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.HeaderPrimary;
            this.kryptonBorderEdge3.Dock = System.Windows.Forms.DockStyle.Left;
            this.kryptonBorderEdge3.Location = new System.Drawing.Point(0, 30);
            this.kryptonBorderEdge3.Name = "kryptonBorderEdge3";
            this.kryptonBorderEdge3.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.kryptonBorderEdge3.Size = new System.Drawing.Size(1, 500);
            this.kryptonBorderEdge3.TabIndex = 7;
            this.kryptonBorderEdge3.Text = "kryptonBorderEdge3";
            //
            // borderAdvanced
            //
            this.borderAdvanced.Dock = System.Windows.Forms.DockStyle.Top;
            this.borderAdvanced.Location = new System.Drawing.Point(1, 62);
            this.borderAdvanced.Name = "borderAdvanced";
            this.borderAdvanced.Size = new System.Drawing.Size(731, 1);
            this.borderAdvanced.TabIndex = 0;
            this.borderAdvanced.Text = "kryptonBorderEdge4";
            //
            // filterEditor
            //
            this.filterEditor.BackColor = System.Drawing.Color.White;
            this.filterEditor.Dock = System.Windows.Forms.DockStyle.Top;
            this.filterEditor.Location = new System.Drawing.Point(1, 30);
            this.filterEditor.Name = "filterEditor";
            this.filterEditor.Size = new System.Drawing.Size(731, 32);
            this.filterEditor.TabIndex = 2;
            this.filterEditor.RequiredHeightChanged += new System.EventHandler(this.OnAdvancedSearchRequiredHeightChanged);
            //
            // MainGridControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.borderAdvanced);
            this.Controls.Add(this.gridPanel);
            this.Controls.Add(this.filterEditor);
            this.Controls.Add(this.kryptonBorderEdge3);
            this.Controls.Add(this.kryptonBorderEdge2);
            this.Controls.Add(this.kryptonBorderEdge1);
            this.Controls.Add(this.kryptonGroup);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "MainGridControl";
            this.Size = new System.Drawing.Size(733, 531);
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize) (this.kryptonGroup.Panel)).EndInit();
            this.kryptonGroup.Panel.ResumeLayout(false);
            this.kryptonGroup.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.kryptonGroup)).EndInit();
            this.kryptonGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.pictureSearchHourglass)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel gridPanel;
        private Divelements.SandGrid.GridColumn gridColumnOrderNumber;
        private Divelements.SandGrid.Specialized.GridDateTimeColumn gridColumnOrderDate;
        private Divelements.SandGrid.GridColumn gridColumnBillFirstName;
        private Divelements.SandGrid.GridColumn gridColumnBillLastName;
        private Divelements.SandGrid.GridColumn gridColumnBillCompany;
        private Divelements.SandGrid.GridColumn gridColumnBillCity;
        private Divelements.SandGrid.GridColumn gridColumnBillState;
        private Divelements.SandGrid.GridColumn gridColumnBillPostalCode;
        private Divelements.SandGrid.GridColumn gridColumnBillCountry;
        private Divelements.SandGrid.GridColumn gridColumnBillPhone;
        private Divelements.SandGrid.GridColumn gridColumnBillEmail;
        private Divelements.SandGrid.GridColumn gridColumnShipLastName;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup kryptonGroup;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge1;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge2;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge3;
        private ShipWorks.UI.Controls.Krypton.WatermarkKryptonTextBox searchBox;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecAny buttonEndSearch;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeaderSearchContainer;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecAny buttonAdvancedSearch;
        private ShipWorks.Filters.Controls.FilterDefinitionEditor filterEditor;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge borderAdvanced;
        private System.Windows.Forms.PictureBox pictureSearchHourglass;
    }
}
