namespace ShipWorks.Stores.Content.Controls
{
    partial class InvoiceControl
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
            this.chargesControl = new ShipWorks.Stores.Content.Panels.OrderChargesPanel();
            this.itemsControl = new ShipWorks.Stores.Content.Panels.OrderItemsPanel();
            this.panelOrderTotal = new System.Windows.Forms.Panel();
            this.kryptonBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.orderTotal = new System.Windows.Forms.Label();
            this.labelOrderTotal = new System.Windows.Forms.Label();
            this.panelOrderTotal.SuspendLayout();
            this.SuspendLayout();
            // 
            // chargesControl
            // 
            this.chargesControl.BackColor = System.Drawing.Color.White;
            this.chargesControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.chargesControl.Location = new System.Drawing.Point(210, 69);
            this.chargesControl.Name = "chargesControl";
            this.chargesControl.Size = new System.Drawing.Size(413, 63);
            this.chargesControl.TabIndex = 1;
            this.chargesControl.IdealSizeChanged += new System.EventHandler(this.OnOrderDetailIdealSizeChanged);
            this.chargesControl.ChargesChanged += new System.EventHandler(this.OnChargesChanged);
            // 
            // itemsControl
            // 
            this.itemsControl.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.itemsControl.BackColor = System.Drawing.Color.White;
            this.itemsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.itemsControl.Location = new System.Drawing.Point(0, 0);
            this.itemsControl.Name = "itemsControl";
            this.itemsControl.Size = new System.Drawing.Size(626, 63);
            this.itemsControl.TabIndex = 0;
            this.itemsControl.ItemsChanged += new System.EventHandler(this.OnItemsChanged);
            this.itemsControl.IdealSizeChanged += new System.EventHandler(this.OnOrderDetailIdealSizeChanged);
            // 
            // panelOrderTotal
            // 
            this.panelOrderTotal.BackColor = System.Drawing.Color.White;
            this.panelOrderTotal.Controls.Add(this.kryptonBorderEdge);
            this.panelOrderTotal.Controls.Add(this.orderTotal);
            this.panelOrderTotal.Controls.Add(this.labelOrderTotal);
            this.panelOrderTotal.Location = new System.Drawing.Point(375, 138);
            this.panelOrderTotal.Name = "panelOrderTotal";
            this.panelOrderTotal.Size = new System.Drawing.Size(239, 25);
            this.panelOrderTotal.TabIndex = 0;
            // 
            // kryptonBorderEdge
            // 
            this.kryptonBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonBorderEdge.Location = new System.Drawing.Point(0, 0);
            this.kryptonBorderEdge.Name = "kryptonBorderEdge";
            this.kryptonBorderEdge.Size = new System.Drawing.Size(239, 1);
            this.kryptonBorderEdge.TabIndex = 3;
            this.kryptonBorderEdge.Text = "kryptonBorderEdge1";
            // 
            // orderTotal
            // 
            this.orderTotal.AutoSize = true;
            this.orderTotal.Location = new System.Drawing.Point(117, 5);
            this.orderTotal.Name = "orderTotal";
            this.orderTotal.Size = new System.Drawing.Size(47, 13);
            this.orderTotal.TabIndex = 1;
            this.orderTotal.Text = "$132.46";
            // 
            // labelOrderTotal
            // 
            this.labelOrderTotal.AutoSize = true;
            this.labelOrderTotal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelOrderTotal.Location = new System.Drawing.Point(8, 5);
            this.labelOrderTotal.Name = "labelOrderTotal";
            this.labelOrderTotal.Size = new System.Drawing.Size(74, 13);
            this.labelOrderTotal.TabIndex = 0;
            this.labelOrderTotal.Text = "Order Total:";
            // 
            // InvoiceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.chargesControl);
            this.Controls.Add(this.itemsControl);
            this.Controls.Add(this.panelOrderTotal);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "InvoiceControl";
            this.Size = new System.Drawing.Size(626, 200);
            this.Resize += new System.EventHandler(this.OnResize);
            this.panelOrderTotal.ResumeLayout(false);
            this.panelOrderTotal.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.Stores.Content.Panels.OrderChargesPanel chargesControl;
        private ShipWorks.Stores.Content.Panels.OrderItemsPanel itemsControl;
        private System.Windows.Forms.Panel panelOrderTotal;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge;
        private System.Windows.Forms.Label orderTotal;
        private System.Windows.Forms.Label labelOrderTotal;
    }
}
