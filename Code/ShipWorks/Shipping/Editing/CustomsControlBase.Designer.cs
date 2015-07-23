namespace ShipWorks.Shipping.Editing
{
    partial class CustomsControlBase
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
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer1 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            this.sectionGeneral = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.label1 = new System.Windows.Forms.Label();
            this.customsValue = new ShipWorks.UI.Controls.MoneyTextBox();
            this.sectionContents = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.itemsGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnDescription = new Divelements.SandGrid.GridColumn();
            this.groupSelectedContent = new System.Windows.Forms.GroupBox();
            this.labelHarmonized = new System.Windows.Forms.Label();
            this.harmonizedCode = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.weight = new ShipWorks.UI.Controls.WeightControl();
            this.value = new ShipWorks.UI.Controls.MoneyTextBox();
            this.labelQuantity = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelWeight = new System.Windows.Forms.Label();
            this.labelValue = new System.Windows.Forms.Label();
            this.labelCountryOfOrigin = new System.Windows.Forms.Label();
            this.countryOfOrigin = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.quantity = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.description = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.delete = new System.Windows.Forms.Button();
            this.add = new System.Windows.Forms.Button();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.sectionGeneral)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionGeneral.ContentPanel)).BeginInit();
            this.sectionGeneral.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionContents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionContents.ContentPanel)).BeginInit();
            this.sectionContents.ContentPanel.SuspendLayout();
            this.groupSelectedContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // sectionGeneral
            // 
            this.sectionGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // sectionGeneral.ContentPanel
            // 
            this.sectionGeneral.ContentPanel.Controls.Add(this.label1);
            this.sectionGeneral.ContentPanel.Controls.Add(this.customsValue);
            this.sectionGeneral.ExtraText = "";
            this.sectionGeneral.Location = new System.Drawing.Point(6, 3);
            this.sectionGeneral.Name = "sectionGeneral";
            this.sectionGeneral.SectionName = "General";
            this.sectionGeneral.SettingsKey = "{ce5a918e-a23f-40cf-8029-7c03753fc946}";
            this.sectionGeneral.Size = new System.Drawing.Size(572, 65);
            this.sectionGeneral.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(15, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Value:";
            // 
            // customsValue
            // 
            this.customsValue.Amount = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            this.customsValue.IgnoreSet = false;
            this.customsValue.Location = new System.Drawing.Point(61, 7);
            this.customsValue.Name = "customsValue";
            this.customsValue.Size = new System.Drawing.Size(100, 21);
            this.customsValue.TabIndex = 0;
            this.customsValue.Text = "$0.00";
            // 
            // sectionContents
            // 
            this.sectionContents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // sectionContents.ContentPanel
            // 
            this.sectionContents.ContentPanel.Controls.Add(this.itemsGrid);
            this.sectionContents.ContentPanel.Controls.Add(this.groupSelectedContent);
            this.sectionContents.ContentPanel.Controls.Add(this.delete);
            this.sectionContents.ContentPanel.Controls.Add(this.add);
            this.sectionContents.ExtraText = "";
            this.sectionContents.Location = new System.Drawing.Point(6, 74);
            this.sectionContents.Name = "sectionContents";
            this.sectionContents.SectionName = "Contents";
            this.sectionContents.SettingsKey = "{7fa47a04-8bd5-4ea5-a891-ab22cffa8e17}";
            this.sectionContents.Size = new System.Drawing.Size(572, 362);
            this.sectionContents.TabIndex = 1;
            // 
            // sandGrid
            // 
            this.itemsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.itemsGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnDescription});
            this.itemsGrid.Location = new System.Drawing.Point(8, 10);
            this.itemsGrid.Name = "itemsGrid";
            this.itemsGrid.Renderer = windowsXPRenderer1;
            this.itemsGrid.Size = new System.Drawing.Size(486, 116);
            this.itemsGrid.TabIndex = 0;
            this.itemsGrid.SelectionChanged += new Divelements.SandGrid.SelectionChangedEventHandler(this.OnItemsGridChangeSelectedRow);
            // 
            // gridColumnDescription
            // 
            this.gridColumnDescription.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnDescription.HeaderText = "Description";
            this.gridColumnDescription.Width = 482;
            // 
            // groupSelectedContent
            // 
            this.groupSelectedContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupSelectedContent.BackColor = System.Drawing.Color.Transparent;
            this.groupSelectedContent.Controls.Add(this.labelHarmonized);
            this.groupSelectedContent.Controls.Add(this.harmonizedCode);
            this.groupSelectedContent.Controls.Add(this.weight);
            this.groupSelectedContent.Controls.Add(this.value);
            this.groupSelectedContent.Controls.Add(this.labelQuantity);
            this.groupSelectedContent.Controls.Add(this.labelDescription);
            this.groupSelectedContent.Controls.Add(this.labelWeight);
            this.groupSelectedContent.Controls.Add(this.labelValue);
            this.groupSelectedContent.Controls.Add(this.labelCountryOfOrigin);
            this.groupSelectedContent.Controls.Add(this.countryOfOrigin);
            this.groupSelectedContent.Controls.Add(this.quantity);
            this.groupSelectedContent.Controls.Add(this.description);
            this.groupSelectedContent.Location = new System.Drawing.Point(8, 132);
            this.groupSelectedContent.Name = "groupSelectedContent";
            this.groupSelectedContent.Size = new System.Drawing.Size(551, 190);
            this.groupSelectedContent.TabIndex = 3;
            this.groupSelectedContent.TabStop = false;
            this.groupSelectedContent.Text = "Selected Content";
            // 
            // labelHarmonized
            // 
            this.labelHarmonized.AutoSize = true;
            this.labelHarmonized.Location = new System.Drawing.Point(4, 135);
            this.labelHarmonized.Name = "labelHarmonized";
            this.labelHarmonized.Size = new System.Drawing.Size(95, 13);
            this.labelHarmonized.TabIndex = 8;
            this.labelHarmonized.Text = "Harmonized Code:";
            // 
            // harmonizedCode
            // 
            this.harmonizedCode.Location = new System.Drawing.Point(105, 132);
            this.fieldLengthProvider.SetMaxLengthSource(this.harmonizedCode, ShipWorks.Data.Utility.EntityFieldLengthSource.CustomsHarmonizedCode);
            this.harmonizedCode.Name = "harmonizedCode";
            this.harmonizedCode.Size = new System.Drawing.Size(95, 21);
            this.harmonizedCode.TabIndex = 9;
            this.harmonizedCode.TextChanged += new System.EventHandler(this.OnShipSenseFieldChanged);
            // 
            // weight
            // 
            this.weight.BackColor = System.Drawing.Color.Transparent;
            this.weight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.weight.Location = new System.Drawing.Point(105, 79);
            this.weight.Name = "weight";
            this.weight.RangeMax = 300D;
            this.weight.RangeMin = 0D;
            this.weight.Size = new System.Drawing.Size(218, 24);
            this.weight.TabIndex = 5;
            this.weight.Weight = 0D;
            this.weight.WeightChanged += new System.EventHandler(this.OnShipSenseFieldChanged);
            // 
            // value
            // 
            this.value.Amount = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            this.value.IgnoreSet = false;
            this.value.Location = new System.Drawing.Point(105, 105);
            this.value.Name = "value";
            this.value.Size = new System.Drawing.Size(95, 21);
            this.value.TabIndex = 7;
            this.value.Text = "$0.00";
            this.value.TextChanged += new System.EventHandler(this.OnShipSenseFieldChanged);
            this.value.Leave += new System.EventHandler(this.OnLeaveValueAffectingControl);
            // 
            // labelQuantity
            // 
            this.labelQuantity.AutoSize = true;
            this.labelQuantity.Location = new System.Drawing.Point(46, 27);
            this.labelQuantity.Name = "labelQuantity";
            this.labelQuantity.Size = new System.Drawing.Size(53, 13);
            this.labelQuantity.TabIndex = 0;
            this.labelQuantity.Text = "Quantity:";
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(35, 54);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(64, 13);
            this.labelDescription.TabIndex = 2;
            this.labelDescription.Text = "Description:";
            // 
            // labelWeight
            // 
            this.labelWeight.AutoSize = true;
            this.labelWeight.Location = new System.Drawing.Point(54, 80);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(45, 13);
            this.labelWeight.TabIndex = 4;
            this.labelWeight.Text = "Weight:";
            // 
            // labelValue
            // 
            this.labelValue.AutoSize = true;
            this.labelValue.Location = new System.Drawing.Point(62, 108);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(37, 13);
            this.labelValue.TabIndex = 6;
            this.labelValue.Text = "Value:";
            // 
            // labelCountryOfOrigin
            // 
            this.labelCountryOfOrigin.AutoSize = true;
            this.labelCountryOfOrigin.Location = new System.Drawing.Point(5, 162);
            this.labelCountryOfOrigin.Name = "labelCountryOfOrigin";
            this.labelCountryOfOrigin.Size = new System.Drawing.Size(94, 13);
            this.labelCountryOfOrigin.TabIndex = 10;
            this.labelCountryOfOrigin.Text = "Country of Origin:";
            // 
            // countryOfOrigin
            // 
            this.countryOfOrigin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.countryOfOrigin.FormattingEnabled = true;
            this.countryOfOrigin.Location = new System.Drawing.Point(105, 159);
            this.countryOfOrigin.Name = "countryOfOrigin";
            this.countryOfOrigin.PromptText = "(Multiple Values)";
            this.countryOfOrigin.Size = new System.Drawing.Size(160, 21);
            this.countryOfOrigin.TabIndex = 11;
            this.countryOfOrigin.SelectedIndexChanged += new System.EventHandler(this.OnShipSenseFieldChanged);
            // 
            // quantity
            // 
            this.quantity.Location = new System.Drawing.Point(105, 24);
            this.quantity.Name = "quantity";
            this.quantity.Size = new System.Drawing.Size(95, 21);
            this.quantity.TabIndex = 1;
            this.quantity.TextChanged += new System.EventHandler(this.OnShipSenseFieldChanged);
            this.quantity.Leave += new System.EventHandler(this.OnLeaveValueAffectingControl);
            // 
            // description
            // 
            this.description.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.description.Location = new System.Drawing.Point(105, 51);
            this.fieldLengthProvider.SetMaxLengthSource(this.description, ShipWorks.Data.Utility.EntityFieldLengthSource.CustomsDescription);
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(436, 21);
            this.description.TabIndex = 3;
            this.description.TextChanged += new System.EventHandler(this.OnDescriptionChanged);
            // 
            // delete
            // 
            this.delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.delete.Image = global::ShipWorks.Properties.Resources.delete16;
            this.delete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.delete.Location = new System.Drawing.Point(500, 37);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(64, 23);
            this.delete.TabIndex = 2;
            this.delete.Text = "Delete";
            this.delete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.delete.UseVisualStyleBackColor = true;
            this.delete.Click += new System.EventHandler(this.OnDelete);
            // 
            // add
            // 
            this.add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.add.Image = global::ShipWorks.Properties.Resources.add16;
            this.add.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.add.Location = new System.Drawing.Point(500, 10);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(64, 23);
            this.add.TabIndex = 1;
            this.add.Text = "  Add";
            this.add.UseVisualStyleBackColor = true;
            this.add.Click += new System.EventHandler(this.OnAdd);
            // 
            // CustomsControlBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.sectionGeneral);
            this.Controls.Add(this.sectionContents);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "CustomsControlBase";
            this.Size = new System.Drawing.Size(581, 480);
            ((System.ComponentModel.ISupportInitialize)(this.sectionGeneral.ContentPanel)).EndInit();
            this.sectionGeneral.ContentPanel.ResumeLayout(false);
            this.sectionGeneral.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionGeneral)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionContents.ContentPanel)).EndInit();
            this.sectionContents.ContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sectionContents)).EndInit();
            this.groupSelectedContent.ResumeLayout(false);
            this.groupSelectedContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelQuantity;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Label labelWeight;
        private System.Windows.Forms.Label labelValue;
        private System.Windows.Forms.Label labelCountryOfOrigin;
        private ShipWorks.UI.Controls.MultiValueComboBox countryOfOrigin;
        private ShipWorks.UI.Controls.MultiValueTextBox quantity;
        private ShipWorks.UI.Controls.MultiValueTextBox description;
        private Divelements.SandGrid.GridColumn gridColumnDescription;
        private ShipWorks.UI.Controls.MoneyTextBox value;
        private ShipWorks.UI.Controls.WeightControl weight;
        private System.Windows.Forms.Label labelHarmonized;
        private ShipWorks.UI.Controls.MultiValueTextBox harmonizedCode;
        protected ShipWorks.UI.Controls.CollapsibleGroupControl sectionContents;
        protected System.Windows.Forms.Label label1;
        protected ShipWorks.UI.Controls.MoneyTextBox customsValue;
        protected ShipWorks.UI.Controls.CollapsibleGroupControl sectionGeneral;
        protected System.Windows.Forms.GroupBox groupSelectedContent;
        protected System.Windows.Forms.Button add;
        protected System.Windows.Forms.Button delete;
        protected Divelements.SandGrid.SandGrid itemsGrid;
        protected ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
    }
}
