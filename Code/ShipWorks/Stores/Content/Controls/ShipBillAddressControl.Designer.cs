namespace ShipWorks.Stores.Content.Controls
{
    partial class ShipBillAddressControl
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
            this.tableLayoutPanelAddress = new System.Windows.Forms.TableLayoutPanel();
            this.panelBillToHeader = new System.Windows.Forms.Panel();
            this.linkCopyFromShipping = new System.Windows.Forms.Label();
            this.labeladdressBilling = new System.Windows.Forms.Label();
            this.panelShipToHeader = new System.Windows.Forms.Panel();
            this.linkCopyFromBilling = new System.Windows.Forms.Label();
            this.labelAddressShipping = new System.Windows.Forms.Label();
            this.themeBorderPanelShipping = new ShipWorks.UI.Controls.ThemeBorderPanel();
            this.personShipping = new ShipWorks.Data.Controls.PersonControl();
            this.themeBorderPanelBilling = new ShipWorks.UI.Controls.ThemeBorderPanel();
            this.personBilling = new ShipWorks.Data.Controls.PersonControl();
            this.tableLayoutPanelAddress.SuspendLayout();
            this.panelBillToHeader.SuspendLayout();
            this.panelShipToHeader.SuspendLayout();
            this.themeBorderPanelShipping.SuspendLayout();
            this.themeBorderPanelBilling.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelAddress
            // 
            this.tableLayoutPanelAddress.ColumnCount = 2;
            this.tableLayoutPanelAddress.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelAddress.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelAddress.Controls.Add(this.panelBillToHeader, 1, 0);
            this.tableLayoutPanelAddress.Controls.Add(this.panelShipToHeader, 0, 0);
            this.tableLayoutPanelAddress.Controls.Add(this.themeBorderPanelShipping, 0, 1);
            this.tableLayoutPanelAddress.Controls.Add(this.themeBorderPanelBilling, 1, 1);
            this.tableLayoutPanelAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelAddress.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelAddress.Name = "tableLayoutPanelAddress";
            this.tableLayoutPanelAddress.RowCount = 2;
            this.tableLayoutPanelAddress.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanelAddress.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelAddress.Size = new System.Drawing.Size(629, 467);
            this.tableLayoutPanelAddress.TabIndex = 2;
            // 
            // panelBillToHeader
            // 
            this.panelBillToHeader.Controls.Add(this.linkCopyFromShipping);
            this.panelBillToHeader.Controls.Add(this.labeladdressBilling);
            this.panelBillToHeader.Location = new System.Drawing.Point(314, 0);
            this.panelBillToHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelBillToHeader.Name = "panelBillToHeader";
            this.panelBillToHeader.Size = new System.Drawing.Size(200, 16);
            this.panelBillToHeader.TabIndex = 19;
            // 
            // linkCopyFromShipping
            // 
            this.linkCopyFromShipping.AutoSize = true;
            this.linkCopyFromShipping.BackColor = System.Drawing.Color.Transparent;
            this.linkCopyFromShipping.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkCopyFromShipping.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.linkCopyFromShipping.ForeColor = System.Drawing.Color.Blue;
            this.linkCopyFromShipping.Location = new System.Drawing.Point(40, 0);
            this.linkCopyFromShipping.Name = "linkCopyFromShipping";
            this.linkCopyFromShipping.Size = new System.Drawing.Size(107, 13);
            this.linkCopyFromShipping.TabIndex = 1;
            this.linkCopyFromShipping.Text = "(Copy from shipping)";
            this.linkCopyFromShipping.Click += new System.EventHandler(this.OnCopyFromShipping);
            // 
            // labeladdressBilling
            // 
            this.labeladdressBilling.AutoSize = true;
            this.labeladdressBilling.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labeladdressBilling.Location = new System.Drawing.Point(0, 0);
            this.labeladdressBilling.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labeladdressBilling.Name = "labeladdressBilling";
            this.labeladdressBilling.Size = new System.Drawing.Size(40, 13);
            this.labeladdressBilling.TabIndex = 4;
            this.labeladdressBilling.Text = "Bill To";
            // 
            // panelShipToHeader
            // 
            this.panelShipToHeader.Controls.Add(this.linkCopyFromBilling);
            this.panelShipToHeader.Controls.Add(this.labelAddressShipping);
            this.panelShipToHeader.Location = new System.Drawing.Point(0, 0);
            this.panelShipToHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelShipToHeader.Name = "panelShipToHeader";
            this.panelShipToHeader.Size = new System.Drawing.Size(308, 13);
            this.panelShipToHeader.TabIndex = 20;
            // 
            // linkCopyFromBilling
            // 
            this.linkCopyFromBilling.AutoSize = true;
            this.linkCopyFromBilling.BackColor = System.Drawing.Color.Transparent;
            this.linkCopyFromBilling.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkCopyFromBilling.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.linkCopyFromBilling.ForeColor = System.Drawing.Color.Blue;
            this.linkCopyFromBilling.Location = new System.Drawing.Point(47, 0);
            this.linkCopyFromBilling.Name = "linkCopyFromBilling";
            this.linkCopyFromBilling.Size = new System.Drawing.Size(94, 13);
            this.linkCopyFromBilling.TabIndex = 0;
            this.linkCopyFromBilling.Text = "(Copy from billing)";
            this.linkCopyFromBilling.Click += new System.EventHandler(this.OnCopyFromBilling);
            // 
            // labelAddressShipping
            // 
            this.labelAddressShipping.AutoSize = true;
            this.labelAddressShipping.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelAddressShipping.Location = new System.Drawing.Point(0, 0);
            this.labelAddressShipping.Margin = new System.Windows.Forms.Padding(0);
            this.labelAddressShipping.Name = "labelAddressShipping";
            this.labelAddressShipping.Size = new System.Drawing.Size(48, 13);
            this.labelAddressShipping.TabIndex = 3;
            this.labelAddressShipping.Text = "Ship To";
            // 
            // themeBorderPanelShipping
            // 
            this.themeBorderPanelShipping.BackColor = System.Drawing.Color.White;
            this.themeBorderPanelShipping.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.themeBorderPanelShipping.Controls.Add(this.personShipping);
            this.themeBorderPanelShipping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.themeBorderPanelShipping.Location = new System.Drawing.Point(0, 16);
            this.themeBorderPanelShipping.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.themeBorderPanelShipping.Name = "themeBorderPanelShipping";
            this.themeBorderPanelShipping.Size = new System.Drawing.Size(312, 451);
            this.themeBorderPanelShipping.TabIndex = 1;
            // 
            // personShipping
            // 
            this.personShipping.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.personShipping.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.personShipping.Location = new System.Drawing.Point(3, 3);
            this.personShipping.Name = "personShipping";
            this.personShipping.Size = new System.Drawing.Size(290, 434);
            this.personShipping.TabIndex = 0;
            // 
            // themeBorderPanelBilling
            // 
            this.themeBorderPanelBilling.BackColor = System.Drawing.Color.White;
            this.themeBorderPanelBilling.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.themeBorderPanelBilling.Controls.Add(this.personBilling);
            this.themeBorderPanelBilling.Dock = System.Windows.Forms.DockStyle.Fill;
            this.themeBorderPanelBilling.Location = new System.Drawing.Point(316, 16);
            this.themeBorderPanelBilling.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.themeBorderPanelBilling.Name = "themeBorderPanelBilling";
            this.themeBorderPanelBilling.Size = new System.Drawing.Size(313, 451);
            this.themeBorderPanelBilling.TabIndex = 2;
            // 
            // personBilling
            // 
            this.personBilling.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.personBilling.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.personBilling.Location = new System.Drawing.Point(4, 3);
            this.personBilling.Name = "personBilling";
            this.personBilling.Size = new System.Drawing.Size(290, 434);
            this.personBilling.TabIndex = 0;
            // 
            // ShipBillAddressControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelAddress);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "ShipBillAddressControl";
            this.Size = new System.Drawing.Size(629, 467);
            this.tableLayoutPanelAddress.ResumeLayout(false);
            this.panelBillToHeader.ResumeLayout(false);
            this.panelBillToHeader.PerformLayout();
            this.panelShipToHeader.ResumeLayout(false);
            this.panelShipToHeader.PerformLayout();
            this.themeBorderPanelShipping.ResumeLayout(false);
            this.themeBorderPanelBilling.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelAddress;
        private System.Windows.Forms.Panel panelBillToHeader;
        private System.Windows.Forms.Label linkCopyFromShipping;
        private System.Windows.Forms.Label labeladdressBilling;
        private System.Windows.Forms.Panel panelShipToHeader;
        private System.Windows.Forms.Label linkCopyFromBilling;
        private System.Windows.Forms.Label labelAddressShipping;
        private ShipWorks.UI.Controls.ThemeBorderPanel themeBorderPanelShipping;
        private ShipWorks.Data.Controls.PersonControl personShipping;
        private ShipWorks.UI.Controls.ThemeBorderPanel themeBorderPanelBilling;
        private ShipWorks.Data.Controls.PersonControl personBilling;
    }
}
