namespace ShipWorks.Stores.Platforms.Ebay.OrderCombining
{
    partial class EbayCombineOrderControl
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
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer1 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            Divelements.SandGrid.GridRow gridRow1 = new Divelements.SandGrid.GridRow();
            this.enableCheckBox = new System.Windows.Forms.CheckBox();
            this.combinedPanel = new System.Windows.Forms.Panel();
            this.eBayDetailsPanel = new System.Windows.Forms.Panel();
            this.shippingServiceComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.taxTextBox = new ShipWorks.UI.Controls.NumericTextBox();
            this.taxShippingCheckBox = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.taxStateComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.shippingTextBox = new ShipWorks.UI.Controls.MoneyTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.sandGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnOrder = new Divelements.SandGrid.GridColumn();
            this.gridDateTimeColumn1 = new Divelements.SandGrid.Specialized.GridDateTimeColumn();
            this.gridColumnItemNumber = new Divelements.SandGrid.GridColumn();
            this.gridColumnItemTitle = new Divelements.SandGrid.GridColumn();
            this.gridDecimalColumn1 = new Divelements.SandGrid.Specialized.GridDecimalColumn();
            this.buyerLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.combinedPanel.SuspendLayout();
            this.eBayDetailsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // enableCheckBox
            // 
            this.enableCheckBox.AutoSize = true;
            this.enableCheckBox.Location = new System.Drawing.Point(10, 8);
            this.enableCheckBox.Name = "enableCheckBox";
            this.enableCheckBox.Size = new System.Drawing.Size(15, 14);
            this.enableCheckBox.TabIndex = 7;
            this.enableCheckBox.UseVisualStyleBackColor = true;
            this.enableCheckBox.CheckedChanged += new System.EventHandler(this.OnEnableChecked);
            // 
            // combinedPanel
            // 
            this.combinedPanel.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.combinedPanel.Controls.Add(this.eBayDetailsPanel);
            this.combinedPanel.Controls.Add(this.pictureBox1);
            this.combinedPanel.Controls.Add(this.sandGrid);
            this.combinedPanel.Controls.Add(this.buyerLabel);
            this.combinedPanel.Location = new System.Drawing.Point(27, 0);
            this.combinedPanel.Name = "combinedPanel";
            this.combinedPanel.Size = new System.Drawing.Size(470, 235);
            this.combinedPanel.TabIndex = 8;
            // 
            // eBayDetailsPanel
            // 
            this.eBayDetailsPanel.Controls.Add(this.shippingServiceComboBox);
            this.eBayDetailsPanel.Controls.Add(this.label3);
            this.eBayDetailsPanel.Controls.Add(this.taxTextBox);
            this.eBayDetailsPanel.Controls.Add(this.taxShippingCheckBox);
            this.eBayDetailsPanel.Controls.Add(this.label4);
            this.eBayDetailsPanel.Controls.Add(this.taxStateComboBox);
            this.eBayDetailsPanel.Controls.Add(this.label1);
            this.eBayDetailsPanel.Controls.Add(this.shippingTextBox);
            this.eBayDetailsPanel.Controls.Add(this.label2);
            this.eBayDetailsPanel.Location = new System.Drawing.Point(34, 129);
            this.eBayDetailsPanel.Name = "eBayDetailsPanel";
            this.eBayDetailsPanel.Size = new System.Drawing.Size(433, 103);
            this.eBayDetailsPanel.TabIndex = 15;
            // 
            // shippingServiceComboBox
            // 
            this.shippingServiceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.shippingServiceComboBox.FormattingEnabled = true;
            this.shippingServiceComboBox.Location = new System.Drawing.Point(143, 1);
            this.shippingServiceComboBox.Name = "shippingServiceComboBox";
            this.shippingServiceComboBox.Size = new System.Drawing.Size(232, 21);
            this.shippingServiceComboBox.TabIndex = 24;
            this.shippingServiceComboBox.SelectedIndexChanged += new System.EventHandler(this.OnShippingMethodChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(48, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "&Shipping Service:";
            // 
            // taxTextBox
            // 
            this.taxTextBox.Location = new System.Drawing.Point(314, 55);
            this.taxTextBox.Name = "taxTextBox";
            this.taxTextBox.Size = new System.Drawing.Size(39, 21);
            this.taxTextBox.TabIndex = 22;
            this.taxTextBox.Text = "0.000";
            this.taxTextBox.TextChanged += new System.EventHandler(this.OnTaxChanged);
            // 
            // taxShippingCheckBox
            // 
            this.taxShippingCheckBox.AutoSize = true;
            this.taxShippingCheckBox.Location = new System.Drawing.Point(143, 82);
            this.taxShippingCheckBox.Name = "taxShippingCheckBox";
            this.taxShippingCheckBox.Size = new System.Drawing.Size(195, 17);
            this.taxShippingCheckBox.TabIndex = 21;
            this.taxShippingCheckBox.Text = "Apply tax to shipping and handling.";
            this.taxShippingCheckBox.UseVisualStyleBackColor = true;
            this.taxShippingCheckBox.CheckedChanged += new System.EventHandler(this.OnTaxShippingChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(353, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(18, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "%";
            // 
            // taxStateComboBox
            // 
            this.taxStateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.taxStateComboBox.FormattingEnabled = true;
            this.taxStateComboBox.Location = new System.Drawing.Point(143, 55);
            this.taxStateComboBox.Name = "taxStateComboBox";
            this.taxStateComboBox.Size = new System.Drawing.Size(165, 21);
            this.taxStateComboBox.TabIndex = 19;
            this.taxStateComboBox.SelectedIndexChanged += new System.EventHandler(this.OnTaxStateChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(80, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Sales Tax:";
            // 
            // shippingTextBox
            // 
            this.shippingTextBox.Amount = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            this.shippingTextBox.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.shippingTextBox.Location = new System.Drawing.Point(143, 28);
            this.shippingTextBox.Name = "shippingTextBox";
            this.shippingTextBox.Size = new System.Drawing.Size(60, 21);
            this.shippingTextBox.TabIndex = 15;
            this.shippingTextBox.Text = "$0.00";
            this.shippingTextBox.TextChanged += new System.EventHandler(this.OnShippingChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "&Shipping and Handling:";
            // 
            // sandGrid
            // 
            this.sandGrid.AllowMultipleSelection = false;
            this.sandGrid.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sandGrid.CheckBoxes = true;
            this.sandGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnOrder,
            this.gridDateTimeColumn1,
            this.gridColumnItemNumber,
            this.gridColumnItemTitle,
            this.gridDecimalColumn1});
            this.sandGrid.ImageTextSeparation = 1;
            this.sandGrid.KeyboardEditMode = Divelements.SandGrid.KeyboardEditMode.EditOnF2;
            this.sandGrid.Location = new System.Drawing.Point(34, 31);
            this.sandGrid.MouseEditMode = Divelements.SandGrid.MouseEditMode.DelayedSingleClick;
            this.sandGrid.Name = "sandGrid";
            this.sandGrid.NullRepresentation = "";
            this.sandGrid.Renderer = windowsXPRenderer1;
            this.sandGrid.RowDragBehavior = Divelements.SandGrid.RowDragBehavior.InitiateDragDrop;
            gridRow1.Cells.AddRange(new Divelements.SandGrid.GridCell[] {
            new Divelements.SandGrid.GridCell("123456"),
            new Divelements.SandGrid.GridCell("9/18/2009"),
            new Divelements.SandGrid.GridCell("100212104"),
            new Divelements.SandGrid.GridCell("Tweezers"),
            new Divelements.SandGrid.GridCell("$1.00"),
            new Divelements.SandGrid.GridCell("$2.50")});
            gridRow1.Expanded = true;
            this.sandGrid.Rows.AddRange(new Divelements.SandGrid.GridRow[] {
            gridRow1,
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell("32423"),
                        new Divelements.SandGrid.GridCell("09/25/2009"),
                        new Divelements.SandGrid.GridCell("1212155"),
                        new Divelements.SandGrid.GridCell("Speaker"),
                        new Divelements.SandGrid.GridCell("$100.00"),
                        new Divelements.SandGrid.GridCell("$22.40")}),
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell("3432423"),
                        new Divelements.SandGrid.GridCell("9/21/2009"),
                        new Divelements.SandGrid.GridCell("1212123"),
                        new Divelements.SandGrid.GridCell("Pet Rock"),
                        new Divelements.SandGrid.GridCell("$42.00"),
                        new Divelements.SandGrid.GridCell("$8.75")})});
            this.sandGrid.ShadeAlternateRows = true;
            this.sandGrid.Size = new System.Drawing.Size(433, 92);
            this.sandGrid.SortColumn = this.gridColumnItemTitle;
            this.sandGrid.SortDirection = System.ComponentModel.ListSortDirection.Descending;
            this.sandGrid.StretchPrimaryGrid = false;
            this.sandGrid.TabIndex = 8;
            this.sandGrid.AfterCheck += new Divelements.SandGrid.GridRowCheckEventHandler(this.OnAfterRowCheck);
            // 
            // gridColumnOrder
            // 
            this.gridColumnOrder.AllowEditing = false;
            this.gridColumnOrder.AllowReorder = false;
            this.gridColumnOrder.ForeColorSource = Divelements.SandGrid.CellForeColorSource.RowCell;
            this.gridColumnOrder.HeaderText = "Order #";
            this.gridColumnOrder.Width = 90;
            // 
            // gridDateTimeColumn1
            // 
            this.gridDateTimeColumn1.DataFormatString = "{0:d}";
            this.gridDateTimeColumn1.EditorType = typeof(Divelements.SandGrid.GridDateTimeEditor);
            this.gridDateTimeColumn1.HeaderText = "Date";
            this.gridDateTimeColumn1.Width = 69;
            // 
            // gridColumnItemNumber
            // 
            this.gridColumnItemNumber.AllowEditing = false;
            this.gridColumnItemNumber.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnItemNumber.HeaderText = "Item #";
            this.gridColumnItemNumber.Width = 88;
            // 
            // gridColumnItemTitle
            // 
            this.gridColumnItemTitle.AllowEditing = false;
            this.gridColumnItemTitle.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnItemTitle.HeaderText = "Item Title";
            this.gridColumnItemTitle.Width = 117;
            // 
            // gridDecimalColumn1
            // 
            this.gridDecimalColumn1.DataFormatString = "{0:c}";
            this.gridDecimalColumn1.HeaderText = "Total";
            this.gridDecimalColumn1.Width = 65;
            // 
            // buyerLabel
            // 
            this.buyerLabel.AutoSize = true;
            this.buyerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.buyerLabel.Location = new System.Drawing.Point(31, 8);
            this.buyerLabel.Name = "buyerLabel";
            this.buyerLabel.Size = new System.Drawing.Size(97, 13);
            this.buyerLabel.TabIndex = 7;
            this.buyerLabel.Text = "buyerNameHere";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ShipWorks.Properties.Resources.customer24;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(24, 24);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // EbayCombineOrderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.combinedPanel);
            this.Controls.Add(this.enableCheckBox);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "EbayCombineOrderControl";
            this.Size = new System.Drawing.Size(500, 236);
            this.Load += new System.EventHandler(this.OnLoad);
            this.combinedPanel.ResumeLayout(false);
            this.combinedPanel.PerformLayout();
            this.eBayDetailsPanel.ResumeLayout(false);
            this.eBayDetailsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox enableCheckBox;
        private System.Windows.Forms.Panel combinedPanel;
        private Divelements.SandGrid.SandGrid sandGrid;
        private Divelements.SandGrid.GridColumn gridColumnOrder;
        private Divelements.SandGrid.GridColumn gridColumnItemNumber;
        private Divelements.SandGrid.GridColumn gridColumnItemTitle;
        private System.Windows.Forms.Label buyerLabel;
        private Divelements.SandGrid.Specialized.GridDateTimeColumn gridDateTimeColumn1;
        private Divelements.SandGrid.Specialized.GridDecimalColumn gridDecimalColumn1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel eBayDetailsPanel;
        private ShipWorks.UI.Controls.MoneyTextBox shippingTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox taxShippingCheckBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox taxStateComboBox;
        private ShipWorks.UI.Controls.NumericTextBox taxTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox shippingServiceComboBox;

    }
}
