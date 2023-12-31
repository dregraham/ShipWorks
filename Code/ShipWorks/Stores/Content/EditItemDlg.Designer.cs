﻿using ShipWorks.UI.Controls;

namespace ShipWorks.Stores.Content
{
    partial class EditItemDlg
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
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.labelName = new System.Windows.Forms.Label();
            this.labelCode = new System.Windows.Forms.Label();
            this.labelQuantity = new System.Windows.Forms.Label();
            this.labelPrice = new System.Windows.Forms.Label();
            this.labelSku = new System.Windows.Forms.Label();
            this.labelLocation = new System.Windows.Forms.Label();
            this.labelCost = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelWeight = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.edge2 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.edge1 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.labelAttributes = new System.Windows.Forms.Label();
            this.attributeGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnDescription = new Divelements.SandGrid.GridColumn();
            this.gridColumnPrice1 = new Divelements.SandGrid.GridColumn();
            this.gridLinkEdit = new Divelements.SandGrid.Specialized.GridHyperlinkColumn();
            this.gridLinkDelete = new Divelements.SandGrid.Specialized.GridHyperlinkColumn();
            this.labelUpc = new System.Windows.Forms.Label();
            this.isbnLabel = new System.Windows.Forms.Label();
            this.imageUrlLabel = new System.Windows.Forms.Label();
            this.thumbnailUrlLabel = new System.Windows.Forms.Label();
            this.imageUrl = new System.Windows.Forms.TextBox();
            this.thumbnailUrl = new System.Windows.Forms.TextBox();
            this.isbn = new System.Windows.Forms.TextBox();
            this.upc = new System.Windows.Forms.TextBox();
            this.addAttribute = new ShipWorks.UI.Controls.LinkControl();
            this.description = new System.Windows.Forms.TextBox();
            this.weight = new ShipWorks.UI.Controls.WeightControl();
            this.cost = new ShipWorks.UI.Controls.MoneyTextBox();
            this.location = new System.Windows.Forms.TextBox();
            this.sku = new System.Windows.Forms.TextBox();
            this.status = new ShipWorks.UI.Controls.LinkControl();
            this.price = new ShipWorks.UI.Controls.MoneyTextBox();
            this.quantity = new System.Windows.Forms.TextBox();
            this.code = new System.Windows.Forms.TextBox();
            this.name = new System.Windows.Forms.TextBox();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.harmonizedCode = new System.Windows.Forms.TextBox();
            this.length = new ShipWorks.UI.Controls.PromptTextBox();
            this.width = new ShipWorks.UI.Controls.PromptTextBox();
            this.height = new ShipWorks.UI.Controls.PromptTextBox();
            this.labelHarmonizedCode = new System.Windows.Forms.Label();
            this.dimsLabel = new System.Windows.Forms.Label();
            this.dimUnitLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(248, 639);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 20;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(329, 639);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 21;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(64, 9);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(38, 13);
            this.labelName.TabIndex = 2;
            this.labelName.Text = "Name:";
            // 
            // labelCode
            // 
            this.labelCode.AutoSize = true;
            this.labelCode.Location = new System.Drawing.Point(66, 36);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(36, 13);
            this.labelCode.TabIndex = 3;
            this.labelCode.Text = "Code:";
            // 
            // labelQuantity
            // 
            this.labelQuantity.AutoSize = true;
            this.labelQuantity.Location = new System.Drawing.Point(49, 63);
            this.labelQuantity.Name = "labelQuantity";
            this.labelQuantity.Size = new System.Drawing.Size(53, 13);
            this.labelQuantity.TabIndex = 4;
            this.labelQuantity.Text = "Quantity:";
            // 
            // labelPrice
            // 
            this.labelPrice.AutoSize = true;
            this.labelPrice.Location = new System.Drawing.Point(68, 90);
            this.labelPrice.Name = "labelPrice";
            this.labelPrice.Size = new System.Drawing.Size(34, 13);
            this.labelPrice.TabIndex = 5;
            this.labelPrice.Text = "Price:";
            // 
            // labelSku
            // 
            this.labelSku.AutoSize = true;
            this.labelSku.Location = new System.Drawing.Point(72, 151);
            this.labelSku.Name = "labelSku";
            this.labelSku.Size = new System.Drawing.Size(30, 13);
            this.labelSku.TabIndex = 6;
            this.labelSku.Text = "SKU:";
            // 
            // labelLocation
            // 
            this.labelLocation.AutoSize = true;
            this.labelLocation.Location = new System.Drawing.Point(51, 232);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(51, 13);
            this.labelLocation.TabIndex = 7;
            this.labelLocation.Text = "Location:";
            // 
            // labelCost
            // 
            this.labelCost.AutoSize = true;
            this.labelCost.Location = new System.Drawing.Point(47, 340);
            this.labelCost.Name = "labelCost";
            this.labelCost.Size = new System.Drawing.Size(55, 13);
            this.labelCost.TabIndex = 8;
            this.labelCost.Text = "Unit Cost:";
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(39, 421);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(64, 13);
            this.labelDescription.TabIndex = 9;
            this.labelDescription.Text = "Description:";
            // 
            // labelWeight
            // 
            this.labelWeight.AutoSize = true;
            this.labelWeight.Location = new System.Drawing.Point(57, 287);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(45, 13);
            this.labelWeight.TabIndex = 10;
            this.labelWeight.Text = "Weight:";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(33, 119);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(69, 13);
            this.labelStatus.TabIndex = 11;
            this.labelStatus.Text = "Local Status:";
            // 
            // edge2
            // 
            this.edge2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edge2.AutoSize = false;
            this.edge2.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.edge2.Location = new System.Drawing.Point(41, 490);
            this.edge2.Name = "edge2";
            this.edge2.Size = new System.Drawing.Size(380, 1);
            this.edge2.Text = "kryptonBorderEdge1";
            // 
            // edge1
            // 
            this.edge1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edge1.AutoSize = false;
            this.edge1.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.edge1.Location = new System.Drawing.Point(40, 140);
            this.edge1.Name = "edge1";
            this.edge1.Size = new System.Drawing.Size(380, 1);
            this.edge1.Text = "kryptonBorderEdge1";
            // 
            // labelAttributes
            // 
            this.labelAttributes.AutoSize = true;
            this.labelAttributes.Location = new System.Drawing.Point(43, 500);
            this.labelAttributes.Name = "labelAttributes";
            this.labelAttributes.Size = new System.Drawing.Size(59, 13);
            this.labelAttributes.TabIndex = 23;
            this.labelAttributes.Text = "Attributes:";
            // 
            // attributeGrid
            // 
            this.attributeGrid.AllowMultipleSelection = false;
            this.attributeGrid.ColumnClickBehavior = Divelements.SandGrid.ColumnClickBehavior.None;
            this.attributeGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnDescription,
            this.gridColumnPrice1,
            this.gridLinkEdit,
            this.gridLinkDelete});
            this.attributeGrid.EnableSearching = false;
            this.attributeGrid.Location = new System.Drawing.Point(109, 497);
            this.attributeGrid.Name = "attributeGrid";
            this.attributeGrid.Renderer = windowsXPRenderer1;
            this.attributeGrid.Size = new System.Drawing.Size(300, 111);
            this.attributeGrid.TabIndex = 18;
            this.attributeGrid.RowActivated += new Divelements.SandGrid.GridRowEventHandler(this.OnRowActivated);
            // 
            // gridColumnDescription
            // 
            this.gridColumnDescription.AllowReorder = false;
            this.gridColumnDescription.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnDescription.Clickable = false;
            this.gridColumnDescription.HeaderText = "Name";
            this.gridColumnDescription.Width = 199;
            // 
            // gridColumnPrice1
            // 
            this.gridColumnPrice1.AllowReorder = false;
            this.gridColumnPrice1.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnPrice1.AutoSizeIncludeHeader = true;
            this.gridColumnPrice1.Clickable = false;
            this.gridColumnPrice1.HeaderText = "Price";
            this.gridColumnPrice1.Width = 32;
            // 
            // gridLinkEdit
            // 
            this.gridLinkEdit.AllowReorder = false;
            this.gridLinkEdit.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridLinkEdit.AutoSizeIncludeHeader = true;
            this.gridLinkEdit.Clickable = false;
            this.gridLinkEdit.HeaderText = "Edit";
            this.gridLinkEdit.LinkHotColor = System.Drawing.Color.Blue;
            this.gridLinkEdit.Width = 26;
            // 
            // gridLinkDelete
            // 
            this.gridLinkDelete.AllowReorder = false;
            this.gridLinkDelete.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridLinkDelete.AutoSizeIncludeHeader = true;
            this.gridLinkDelete.Clickable = false;
            this.gridLinkDelete.HeaderText = "Delete";
            this.gridLinkDelete.LinkHotColor = System.Drawing.Color.Blue;
            this.gridLinkDelete.Width = 39;
            // 
            // labelUpc
            // 
            this.labelUpc.AutoSize = true;
            this.labelUpc.Location = new System.Drawing.Point(71, 178);
            this.labelUpc.Name = "labelUpc";
            this.labelUpc.Size = new System.Drawing.Size(31, 13);
            this.labelUpc.TabIndex = 25;
            this.labelUpc.Text = "UPC:";
            // 
            // isbnLabel
            // 
            this.isbnLabel.AutoSize = true;
            this.isbnLabel.Location = new System.Drawing.Point(68, 205);
            this.isbnLabel.Name = "isbnLabel";
            this.isbnLabel.Size = new System.Drawing.Size(34, 13);
            this.isbnLabel.TabIndex = 27;
            this.isbnLabel.Text = "ISBN:";
            // 
            // imageUrlLabel
            // 
            this.imageUrlLabel.AutoSize = true;
            this.imageUrlLabel.Location = new System.Drawing.Point(45, 394);
            this.imageUrlLabel.Name = "imageUrlLabel";
            this.imageUrlLabel.Size = new System.Drawing.Size(57, 13);
            this.imageUrlLabel.TabIndex = 31;
            this.imageUrlLabel.Text = "Image Url:";
            // 
            // thumbnailUrlLabel
            // 
            this.thumbnailUrlLabel.AutoSize = true;
            this.thumbnailUrlLabel.Location = new System.Drawing.Point(27, 367);
            this.thumbnailUrlLabel.Name = "thumbnailUrlLabel";
            this.thumbnailUrlLabel.Size = new System.Drawing.Size(75, 13);
            this.thumbnailUrlLabel.TabIndex = 29;
            this.thumbnailUrlLabel.Text = "Thumbnail Url:";
            // 
            // imageUrl
            // 
            this.imageUrl.Location = new System.Drawing.Point(109, 391);
            this.imageUrl.Name = "imageUrl";
            this.imageUrl.Size = new System.Drawing.Size(301, 21);
            this.imageUrl.TabIndex = 16;
            // 
            // thumbnailUrl
            // 
            this.thumbnailUrl.Location = new System.Drawing.Point(109, 364);
            this.thumbnailUrl.Name = "thumbnailUrl";
            this.thumbnailUrl.Size = new System.Drawing.Size(301, 21);
            this.thumbnailUrl.TabIndex = 15;
            // 
            // isbn
            // 
            this.isbn.Location = new System.Drawing.Point(109, 202);
            this.fieldLengthProvider.SetMaxLengthSource(this.isbn, ShipWorks.Data.Utility.EntityFieldLengthSource.OrderItemIsbn);
            this.isbn.Name = "isbn";
            this.isbn.Size = new System.Drawing.Size(159, 21);
            this.isbn.TabIndex = 7;
            // 
            // upc
            // 
            this.upc.Location = new System.Drawing.Point(109, 175);
            this.fieldLengthProvider.SetMaxLengthSource(this.upc, ShipWorks.Data.Utility.EntityFieldLengthSource.OrderItemUpc);
            this.upc.Name = "upc";
            this.upc.Size = new System.Drawing.Size(159, 21);
            this.upc.TabIndex = 6;
            // 
            // addAttribute
            // 
            this.addAttribute.AutoSize = true;
            this.addAttribute.Cursor = System.Windows.Forms.Cursors.Hand;
            this.addAttribute.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.addAttribute.ForeColor = System.Drawing.Color.Blue;
            this.addAttribute.Location = new System.Drawing.Point(340, 611);
            this.addAttribute.Name = "addAttribute";
            this.addAttribute.Size = new System.Drawing.Size(72, 13);
            this.addAttribute.TabIndex = 19;
            this.addAttribute.Text = "Add Attribute";
            this.addAttribute.Click += new System.EventHandler(this.OnAddAttribute);
            // 
            // description
            // 
            this.description.AcceptsReturn = true;
            this.description.Location = new System.Drawing.Point(109, 418);
            this.description.Multiline = true;
            this.description.Name = "description";
            this.description.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.description.Size = new System.Drawing.Size(301, 66);
            this.description.TabIndex = 17;
            // 
            // weight
            // 
            this.weight.AutoSize = true;
            this.weight.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.weight.BackColor = System.Drawing.Color.Transparent;
            this.weight.ConfigureTelemetryEntityCounts = null;
            this.weight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.weight.Location = new System.Drawing.Point(109, 283);
            this.weight.Name = "weight";
            this.weight.RangeMax = 300D;
            this.weight.RangeMin = 0D;
            this.weight.ShowShortcutInfo = true;
            this.weight.Size = new System.Drawing.Size(269, 24);
            this.weight.TabIndex = 10;
            this.weight.Weight = 0D;
            // 
            // cost
            // 
            this.cost.Amount = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            this.cost.IgnoreSet = false;
            this.cost.Location = new System.Drawing.Point(109, 337);
            this.cost.Name = "cost";
            this.cost.Size = new System.Drawing.Size(100, 21);
            this.cost.TabIndex = 14;
            this.cost.Text = "$0.00";
            // 
            // location
            // 
            this.location.Location = new System.Drawing.Point(109, 229);
            this.fieldLengthProvider.SetMaxLengthSource(this.location, ShipWorks.Data.Utility.EntityFieldLengthSource.OrderItemLocation);
            this.location.Name = "location";
            this.location.Size = new System.Drawing.Size(159, 21);
            this.location.TabIndex = 8;
            // 
            // sku
            // 
            this.sku.Location = new System.Drawing.Point(109, 148);
            this.fieldLengthProvider.SetMaxLengthSource(this.sku, ShipWorks.Data.Utility.EntityFieldLengthSource.OrderItemSku);
            this.sku.Name = "sku";
            this.sku.Size = new System.Drawing.Size(159, 21);
            this.sku.TabIndex = 5;
            // 
            // status
            // 
            this.status.AutoSize = true;
            this.status.Cursor = System.Windows.Forms.Cursors.Hand;
            this.status.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.status.ForeColor = System.Drawing.Color.Blue;
            this.status.Location = new System.Drawing.Point(106, 119);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(38, 13);
            this.status.TabIndex = 4;
            this.status.Text = "Status";
            this.status.Click += new System.EventHandler(this.OnLinkLocalStatus);
            // 
            // price
            // 
            this.price.Amount = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            this.price.IgnoreSet = false;
            this.price.Location = new System.Drawing.Point(109, 87);
            this.price.Name = "price";
            this.price.Size = new System.Drawing.Size(100, 21);
            this.price.TabIndex = 3;
            this.price.Text = "$0.00";
            // 
            // quantity
            // 
            this.quantity.Location = new System.Drawing.Point(109, 60);
            this.quantity.Name = "quantity";
            this.quantity.Size = new System.Drawing.Size(67, 21);
            this.quantity.TabIndex = 2;
            // 
            // code
            // 
            this.code.Location = new System.Drawing.Point(109, 33);
            this.fieldLengthProvider.SetMaxLengthSource(this.code, ShipWorks.Data.Utility.EntityFieldLengthSource.OrderItemCode);
            this.code.Name = "code";
            this.code.Size = new System.Drawing.Size(129, 21);
            this.code.TabIndex = 1;
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(109, 6);
            this.fieldLengthProvider.SetMaxLengthSource(this.name, ShipWorks.Data.Utility.EntityFieldLengthSource.OrderItemName);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(207, 21);
            this.name.TabIndex = 0;
            // 
            // harmonizedCode
            // 
            this.harmonizedCode.Location = new System.Drawing.Point(109, 256);
            this.fieldLengthProvider.SetMaxLengthSource(this.harmonizedCode, ShipWorks.Data.Utility.EntityFieldLengthSource.OrderItemHarmonizedCode);
            this.harmonizedCode.Name = "harmonizedCode";
            this.harmonizedCode.Size = new System.Drawing.Size(159, 21);
            this.harmonizedCode.TabIndex = 9;
            // 
            // length
            // 
            this.length.Location = new System.Drawing.Point(109, 310);
            this.fieldLengthProvider.SetMaxLengthSource(this.length, ShipWorks.Data.Utility.EntityFieldLengthSource.OrderItemIsbn);
            this.length.Name = "length";
            this.length.PromptColor = System.Drawing.SystemColors.GrayText;
            this.length.PromptText = "L";
            this.length.Size = new System.Drawing.Size(49, 21);
            this.length.TabIndex = 11;
            // 
            // width
            // 
            this.width.Location = new System.Drawing.Point(164, 310);
            this.fieldLengthProvider.SetMaxLengthSource(this.width, ShipWorks.Data.Utility.EntityFieldLengthSource.OrderItemIsbn);
            this.width.Name = "width";
            this.width.PromptColor = System.Drawing.SystemColors.GrayText;
            this.width.PromptText = "W";
            this.width.Size = new System.Drawing.Size(49, 21);
            this.width.TabIndex = 12;
            // 
            // height
            // 
            this.height.Location = new System.Drawing.Point(219, 310);
            this.fieldLengthProvider.SetMaxLengthSource(this.height, ShipWorks.Data.Utility.EntityFieldLengthSource.OrderItemIsbn);
            this.height.Name = "height";
            this.height.PromptColor = System.Drawing.SystemColors.GrayText;
            this.height.PromptText = "H";
            this.height.Size = new System.Drawing.Size(49, 21);
            this.height.TabIndex = 13;
            // 
            // labelHarmonizedCode
            // 
            this.labelHarmonizedCode.AutoSize = true;
            this.labelHarmonizedCode.Location = new System.Drawing.Point(7, 259);
            this.labelHarmonizedCode.Name = "labelHarmonizedCode";
            this.labelHarmonizedCode.Size = new System.Drawing.Size(95, 13);
            this.labelHarmonizedCode.TabIndex = 34;
            this.labelHarmonizedCode.Text = "Harmonized Code:";
            // 
            // dimsLabel
            // 
            this.dimsLabel.AutoSize = true;
            this.dimsLabel.Location = new System.Drawing.Point(38, 313);
            this.dimsLabel.Name = "dimsLabel";
            this.dimsLabel.Size = new System.Drawing.Size(64, 13);
            this.dimsLabel.TabIndex = 38;
            this.dimsLabel.Text = "Dimensions:";
            // 
            // dimUnitLabel
            // 
            this.dimUnitLabel.AutoSize = true;
            this.dimUnitLabel.Location = new System.Drawing.Point(274, 313);
            this.dimUnitLabel.Name = "dimUnitLabel";
            this.dimUnitLabel.Size = new System.Drawing.Size(79, 13);
            this.dimUnitLabel.TabIndex = 43;
            this.dimUnitLabel.Text = "in.  (L x W x H)";
            // 
            // EditItemDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(416, 674);
            this.Controls.Add(this.dimUnitLabel);
            this.Controls.Add(this.height);
            this.Controls.Add(this.width);
            this.Controls.Add(this.length);
            this.Controls.Add(this.dimsLabel);
            this.Controls.Add(this.harmonizedCode);
            this.Controls.Add(this.labelHarmonizedCode);
            this.Controls.Add(this.imageUrl);
            this.Controls.Add(this.imageUrlLabel);
            this.Controls.Add(this.thumbnailUrl);
            this.Controls.Add(this.thumbnailUrlLabel);
            this.Controls.Add(this.isbn);
            this.Controls.Add(this.isbnLabel);
            this.Controls.Add(this.upc);
            this.Controls.Add(this.labelUpc);
            this.Controls.Add(this.addAttribute);
            this.Controls.Add(this.attributeGrid);
            this.Controls.Add(this.labelAttributes);
            this.Controls.Add(this.description);
            this.Controls.Add(this.weight);
            this.Controls.Add(this.cost);
            this.Controls.Add(this.location);
            this.Controls.Add(this.sku);
            this.Controls.Add(this.edge1);
            this.Controls.Add(this.edge2);
            this.Controls.Add(this.status);
            this.Controls.Add(this.price);
            this.Controls.Add(this.quantity);
            this.Controls.Add(this.code);
            this.Controls.Add(this.name);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelWeight);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.labelCost);
            this.Controls.Add(this.labelLocation);
            this.Controls.Add(this.labelSku);
            this.Controls.Add(this.labelPrice);
            this.Controls.Add(this.labelQuantity);
            this.Controls.Add(this.labelCode);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditItemDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Item";
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelCode;
        private System.Windows.Forms.Label labelQuantity;
        private System.Windows.Forms.Label labelPrice;
        private System.Windows.Forms.Label labelSku;
        private System.Windows.Forms.Label labelLocation;
        private System.Windows.Forms.Label labelCost;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Label labelWeight;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.TextBox code;
        private System.Windows.Forms.TextBox quantity;
        private ShipWorks.UI.Controls.MoneyTextBox price;
        private ShipWorks.UI.Controls.LinkControl status;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge edge2;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge edge1;
        private System.Windows.Forms.TextBox sku;
        private System.Windows.Forms.TextBox location;
        private ShipWorks.UI.Controls.MoneyTextBox cost;
        private ShipWorks.UI.Controls.WeightControl weight;
        private System.Windows.Forms.TextBox description;
        private System.Windows.Forms.Label labelAttributes;
        private Divelements.SandGrid.SandGrid attributeGrid;
        private Divelements.SandGrid.GridColumn gridColumnDescription;
        private Divelements.SandGrid.Specialized.GridHyperlinkColumn gridLinkEdit;
        private Divelements.SandGrid.Specialized.GridHyperlinkColumn gridLinkDelete;
        private ShipWorks.UI.Controls.LinkControl addAttribute;
        private Divelements.SandGrid.GridColumn gridColumnPrice1;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private System.Windows.Forms.TextBox upc;
        private System.Windows.Forms.Label labelUpc;
        private System.Windows.Forms.TextBox isbn;
        private System.Windows.Forms.Label isbnLabel;
        private System.Windows.Forms.TextBox imageUrl;
        private System.Windows.Forms.Label imageUrlLabel;
        private System.Windows.Forms.TextBox thumbnailUrl;
        private System.Windows.Forms.Label thumbnailUrlLabel;
        private System.Windows.Forms.TextBox harmonizedCode;
        private System.Windows.Forms.Label labelHarmonizedCode;
        private PromptTextBox length;
        private System.Windows.Forms.Label dimsLabel;
        private PromptTextBox width;
        private PromptTextBox height;
        private System.Windows.Forms.Label dimUnitLabel;
    }
}