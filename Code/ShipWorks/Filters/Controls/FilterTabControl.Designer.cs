namespace ShipWorks.Filters.Controls
{
    partial class FilterTabControl
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
            this.filterTabs = new System.Windows.Forms.TabControl();
            this.orderFiltersTab = new System.Windows.Forms.TabPage();
            this.orderFilterTree = new ShipWorks.Filters.Controls.FilterTree();
            this.customerFiltersTab = new System.Windows.Forms.TabPage();
            this.customerFilterTree = new ShipWorks.Filters.Controls.FilterTree();
            this.filterTabs.SuspendLayout();
            this.orderFiltersTab.SuspendLayout();
            this.customerFiltersTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // filterTabs
            // 
            this.filterTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filterTabs.Controls.Add(this.orderFiltersTab);
            this.filterTabs.Controls.Add(this.customerFiltersTab);
            this.filterTabs.Location = new System.Drawing.Point(0, 0);
            this.filterTabs.Margin = new System.Windows.Forms.Padding(0);
            this.filterTabs.Name = "filterTabs";
            this.filterTabs.Padding = new System.Drawing.Point(0, 0);
            this.filterTabs.SelectedIndex = 0;
            this.filterTabs.Size = new System.Drawing.Size(253, 316);
            this.filterTabs.TabIndex = 0;
            this.filterTabs.SelectedIndexChanged += new System.EventHandler(this.OnFilterTabSelectedIndexChanged);
            // 
            // orderFiltersTab
            // 
            this.orderFiltersTab.Controls.Add(this.orderFilterTree);
            this.orderFiltersTab.Location = new System.Drawing.Point(4, 22);
            this.orderFiltersTab.Name = "orderFiltersTab";
            this.orderFiltersTab.Padding = new System.Windows.Forms.Padding(3);
            this.orderFiltersTab.Size = new System.Drawing.Size(245, 290);
            this.orderFiltersTab.TabIndex = 0;
            this.orderFiltersTab.Text = "Orders";
            this.orderFiltersTab.UseVisualStyleBackColor = true;
            // 
            // orderFilterTree
            // 
            this.orderFilterTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.orderFilterTree.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.orderFilterTree.HotTrackNode = null;
            this.orderFilterTree.Location = new System.Drawing.Point(3, 3);
            this.orderFilterTree.Name = "orderFilterTree";
            this.orderFilterTree.Size = new System.Drawing.Size(239, 284);
            this.orderFilterTree.TabIndex = 0;
            // 
            // customerFiltersTab
            // 
            this.customerFiltersTab.Controls.Add(this.customerFilterTree);
            this.customerFiltersTab.Location = new System.Drawing.Point(4, 22);
            this.customerFiltersTab.Name = "customerFiltersTab";
            this.customerFiltersTab.Padding = new System.Windows.Forms.Padding(3);
            this.customerFiltersTab.Size = new System.Drawing.Size(245, 290);
            this.customerFiltersTab.TabIndex = 1;
            this.customerFiltersTab.Text = "Customers";
            this.customerFiltersTab.UseVisualStyleBackColor = true;
            // 
            // customerFilterTree
            // 
            this.customerFilterTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.customerFilterTree.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customerFilterTree.HotTrackNode = null;
            this.customerFilterTree.Location = new System.Drawing.Point(3, 3);
            this.customerFilterTree.Name = "customerFilterTree";
            this.customerFilterTree.Size = new System.Drawing.Size(239, 284);
            this.customerFilterTree.TabIndex = 1;
            // 
            // FilterTabControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.filterTabs);
            this.Name = "FilterTabControl";
            this.Size = new System.Drawing.Size(253, 316);
            this.filterTabs.ResumeLayout(false);
            this.orderFiltersTab.ResumeLayout(false);
            this.customerFiltersTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl filterTabs;
        private System.Windows.Forms.TabPage orderFiltersTab;
        private System.Windows.Forms.TabPage customerFiltersTab;
        private FilterTree orderFilterTree;
        private FilterTree customerFilterTree;
    }
}
