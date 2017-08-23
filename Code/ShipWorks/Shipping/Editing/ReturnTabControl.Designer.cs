namespace ShipWorks.Shipping.Editing
{
    partial class ReturnTabControl
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
            this.sectionContents = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.itemsGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnDescription = new Divelements.SandGrid.GridColumn();
            this.groupSelectedContent = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.multiValueTextBox2 = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelsku = new System.Windows.Forms.Label();
            this.multiValueTextBox1 = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelcode = new System.Windows.Forms.Label();
            this.code = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.weight = new ShipWorks.UI.Controls.WeightControl();
            this.labelQuantity = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.labelWeight = new System.Windows.Forms.Label();
            this.quantity = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.name = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.delete = new System.Windows.Forms.Button();
            this.add = new System.Windows.Forms.Button();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.sectionContents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionContents.ContentPanel)).BeginInit();
            this.sectionContents.ContentPanel.SuspendLayout();
            this.groupSelectedContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
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
            this.sectionContents.Location = new System.Drawing.Point(6, 4);
            this.sectionContents.Name = "sectionContents";
            this.sectionContents.SectionName = "Contents";
            this.sectionContents.SettingsKey = "{7fa47a04-8bd5-4ea5-a891-ab22cffa8e17}";
            this.sectionContents.Size = new System.Drawing.Size(572, 497);
            this.sectionContents.TabIndex = 1;
            // 
            // itemsGrid
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
            this.groupSelectedContent.Controls.Add(this.label1);
            this.groupSelectedContent.Controls.Add(this.multiValueTextBox2);
            this.groupSelectedContent.Controls.Add(this.labelsku);
            this.groupSelectedContent.Controls.Add(this.multiValueTextBox1);
            this.groupSelectedContent.Controls.Add(this.labelcode);
            this.groupSelectedContent.Controls.Add(this.code);
            this.groupSelectedContent.Controls.Add(this.weight);
            this.groupSelectedContent.Controls.Add(this.labelQuantity);
            this.groupSelectedContent.Controls.Add(this.labelName);
            this.groupSelectedContent.Controls.Add(this.labelWeight);
            this.groupSelectedContent.Controls.Add(this.quantity);
            this.groupSelectedContent.Controls.Add(this.name);
            this.groupSelectedContent.Location = new System.Drawing.Point(8, 132);
            this.groupSelectedContent.Name = "groupSelectedContent";
            this.groupSelectedContent.Size = new System.Drawing.Size(551, 328);
            this.groupSelectedContent.TabIndex = 3;
            this.groupSelectedContent.TabStop = false;
            this.groupSelectedContent.Text = "Selected Content";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(60, 188);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Notes:";
            // 
            // multiValueTextBox2
            // 
            this.multiValueTextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.multiValueTextBox2.Location = new System.Drawing.Point(105, 188);
            this.multiValueTextBox2.MaxLength = 14;
            this.fieldLengthProvider.SetMaxLengthSource(this.multiValueTextBox2, ShipWorks.Data.Utility.EntityFieldLengthSource.ReturnItemNote);
            this.multiValueTextBox2.Multiline = true;
            this.multiValueTextBox2.Name = "multiValueTextBox2";
            this.multiValueTextBox2.Size = new System.Drawing.Size(436, 127);
            this.multiValueTextBox2.TabIndex = 10;
            // 
            // labelsku
            // 
            this.labelsku.AutoSize = true;
            this.labelsku.Location = new System.Drawing.Point(69, 80);
            this.labelsku.Name = "labelsku";
            this.labelsku.Size = new System.Drawing.Size(30, 13);
            this.labelsku.TabIndex = 9;
            this.labelsku.Text = "SKU:";
            // 
            // multiValueTextBox1
            // 
            this.multiValueTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.multiValueTextBox1.Location = new System.Drawing.Point(105, 77);
            this.multiValueTextBox1.MaxLength = 14;
            this.fieldLengthProvider.SetMaxLengthSource(this.multiValueTextBox1, ShipWorks.Data.Utility.EntityFieldLengthSource.ReturnItemSku);
            this.multiValueTextBox1.Name = "multiValueTextBox1";
            this.multiValueTextBox1.Size = new System.Drawing.Size(436, 21);
            this.multiValueTextBox1.TabIndex = 8;
            // 
            // labelcode
            // 
            this.labelcode.AutoSize = true;
            this.labelcode.Location = new System.Drawing.Point(63, 107);
            this.labelcode.Name = "labelcode";
            this.labelcode.Size = new System.Drawing.Size(36, 13);
            this.labelcode.TabIndex = 7;
            this.labelcode.Text = "Code:";
            // 
            // code
            // 
            this.code.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.code.Location = new System.Drawing.Point(105, 104);
            this.code.MaxLength = 14;
            this.fieldLengthProvider.SetMaxLengthSource(this.code, ShipWorks.Data.Utility.EntityFieldLengthSource.ReturnItemCode);
            this.code.Name = "code";
            this.code.Size = new System.Drawing.Size(436, 21);
            this.code.TabIndex = 6;
            // 
            // weight
            // 
            this.weight.AutoSize = true;
            this.weight.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.weight.BackColor = System.Drawing.Color.Transparent;
            this.weight.ConfigureTelemetryEntityCounts = null;
            this.weight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.weight.Location = new System.Drawing.Point(105, 158);
            this.weight.Name = "weight";
            this.weight.RangeMax = 300D;
            this.weight.RangeMin = 0D;
            this.weight.Size = new System.Drawing.Size(269, 24);
            this.weight.TabIndex = 5;
            this.weight.Weight = 0D;
            // 
            // labelQuantity
            // 
            this.labelQuantity.AutoSize = true;
            this.labelQuantity.Location = new System.Drawing.Point(46, 134);
            this.labelQuantity.Name = "labelQuantity";
            this.labelQuantity.Size = new System.Drawing.Size(53, 13);
            this.labelQuantity.TabIndex = 0;
            this.labelQuantity.Text = "Quantity:";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(61, 54);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(38, 13);
            this.labelName.TabIndex = 2;
            this.labelName.Text = "Name:";
            // 
            // labelWeight
            // 
            this.labelWeight.AutoSize = true;
            this.labelWeight.Location = new System.Drawing.Point(54, 159);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(45, 13);
            this.labelWeight.TabIndex = 4;
            this.labelWeight.Text = "Weight:";
            // 
            // quantity
            // 
            this.quantity.Location = new System.Drawing.Point(105, 131);
            this.quantity.MaxLength = 14;
            this.fieldLengthProvider.SetMaxLengthSource(this.quantity, ShipWorks.Data.Utility.EntityFieldLengthSource.ReturnItemQuantity);
            this.quantity.Name = "quantity";
            this.quantity.Size = new System.Drawing.Size(95, 21);
            this.quantity.TabIndex = 1;
            // 
            // name
            // 
            this.name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.name.Location = new System.Drawing.Point(105, 51);
            this.name.MaxLength = 300;
            this.fieldLengthProvider.SetMaxLengthSource(this.name, ShipWorks.Data.Utility.EntityFieldLengthSource.ReturnItemName);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(436, 21);
            this.name.TabIndex = 3;
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
            // ReturnTabControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.sectionContents);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ReturnTabControl";
            this.Size = new System.Drawing.Size(581, 509);
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
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelWeight;
        private ShipWorks.UI.Controls.MultiValueTextBox quantity;
        private ShipWorks.UI.Controls.MultiValueTextBox name;
        private Divelements.SandGrid.GridColumn gridColumnDescription;
        private ShipWorks.UI.Controls.WeightControl weight;
        protected ShipWorks.UI.Controls.CollapsibleGroupControl sectionContents;
        protected System.Windows.Forms.GroupBox groupSelectedContent;
        protected System.Windows.Forms.Button add;
        protected System.Windows.Forms.Button delete;
        protected Divelements.SandGrid.SandGrid itemsGrid;
        protected ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private System.Windows.Forms.Label label1;
        private UI.Controls.MultiValueTextBox multiValueTextBox2;
        private System.Windows.Forms.Label labelsku;
        private UI.Controls.MultiValueTextBox multiValueTextBox1;
        private System.Windows.Forms.Label labelcode;
        private UI.Controls.MultiValueTextBox code;
    }
}
