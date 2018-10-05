namespace ShipWorks.OrderLookup.ShipmentHistory.Controls
{
    partial class ShipmentHistoryPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShipmentHistoryPanel));
            this.shipmentPanel = new System.Windows.Forms.Panel();
            this.kryptonHeader = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.kryptonGroup = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.searchBox = new ShipWorks.UI.Controls.Krypton.WatermarkKryptonTextBox();
            this.buttonEndSearch = new ComponentFactory.Krypton.Toolkit.ButtonSpecAny();
            ((System.ComponentModel.ISupportInitialize) (this.kryptonGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.kryptonGroup.Panel)).BeginInit();
            this.kryptonGroup.Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // shipmentPanel
            // 
            this.shipmentPanel.BackColor = System.Drawing.Color.White;
            this.shipmentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shipmentPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.shipmentPanel.Location = new System.Drawing.Point(1, 62);
            this.shipmentPanel.Padding = new System.Windows.Forms.Padding(1, 0, 1, 1);
            this.shipmentPanel.Size = new System.Drawing.Size(731, 468);
            this.shipmentPanel.Name = "shipmentPanel";
            this.shipmentPanel.TabIndex = 0;
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
            this.kryptonHeader.Values.Heading = "Today's Shipments for ...";
            this.kryptonHeader.Values.Description = "";
            this.kryptonHeader.Values.Image = ShipWorks.Properties.Resources.box_closed16;
            // 
            // kryptonGroup
            // 
            this.kryptonGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonGroup.GroupBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.HeaderPrimary;
            this.kryptonGroup.Location = new System.Drawing.Point(0, 0);
            this.kryptonGroup.Name = "kryptonGroup";
            this.kryptonGroup.TabIndex = 1;
            //
            // kryptonGroup.Panel
            //
            this.kryptonGroup.Panel.Controls.Add(this.kryptonHeader);
            this.kryptonGroup.Panel.Controls.Add(this.searchBox);
            this.kryptonGroup.Size = new System.Drawing.Size(733, 30);
            this.kryptonGroup.TabIndex = 1;
            // 
            // searchBox
            // 
            this.searchBox.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchBox.ButtonSpecs.AddRange(new ComponentFactory.Krypton.Toolkit.ButtonSpecAny[] {
            this.buttonEndSearch});
            this.searchBox.Location = new System.Drawing.Point(526, 3);
            this.searchBox.MaxLength = 3998;
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(181, 23);
            this.searchBox.TabIndex = 0;
            this.searchBox.WaterColor = System.Drawing.SystemColors.GrayText;
            this.searchBox.WaterText = "Search Shipments";
            this.searchBox.WordWrap = false;
            this.searchBox.TextChanged += OnSearchTextBoxTextChanged;
            // 
            // buttonEndSearch
            // 
            this.buttonEndSearch.Edge = ComponentFactory.Krypton.Toolkit.PaletteRelativeEdgeAlign.Far;
            this.buttonEndSearch.Enabled = ComponentFactory.Krypton.Toolkit.ButtonEnabled.False;
            this.buttonEndSearch.Image = ((System.Drawing.Image) (resources.GetObject("buttonEndSearch.Image")));
            this.buttonEndSearch.ImageStates.ImageDisabled = ((System.Drawing.Image) (resources.GetObject("buttonEndSearch.ImageStates.ImageDisabled")));
            this.buttonEndSearch.Type = ComponentFactory.Krypton.Toolkit.PaletteButtonSpecStyle.Close;
            this.buttonEndSearch.UniqueName = "370ABDC6B9F24E16370ABDC6B9F24E16";
            this.buttonEndSearch.Click += new System.EventHandler(this.OnEndSearch);
            // 
            // ShipmentHistoryPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.shipmentPanel);
            this.Controls.Add(this.kryptonGroup);
            this.Name = "ShipmentHistoryPanel";
            this.Size = new System.Drawing.Size(733, 531);
            ((System.ComponentModel.ISupportInitialize) (this.kryptonGroup.Panel)).EndInit();
            this.kryptonGroup.Panel.ResumeLayout(false);
            this.kryptonGroup.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.kryptonGroup)).EndInit();
            this.kryptonGroup.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel shipmentPanel;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup kryptonGroup;
        private ShipWorks.UI.Controls.Krypton.WatermarkKryptonTextBox searchBox;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecAny buttonEndSearch;
    }
}
