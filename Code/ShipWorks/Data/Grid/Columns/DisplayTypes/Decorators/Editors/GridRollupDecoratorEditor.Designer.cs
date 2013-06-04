namespace ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators.Editors
{
    partial class GridRollupDecoratorEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridRollupDecoratorEditor));
            this.labelMultipleIdentical = new System.Windows.Forms.Label();
            this.zeroItems = new System.Windows.Forms.TextBox();
            this.labelMultipleItems = new System.Windows.Forms.Label();
            this.multipleItems = new System.Windows.Forms.TextBox();
            this.multipleIdentical = new System.Windows.Forms.TextBox();
            this.labelZeroItems = new System.Windows.Forms.Label();
            this.infotipMultipleItems = new ShipWorks.UI.Controls.InfoTip();
            this.infotipMultipleIdentical = new ShipWorks.UI.Controls.InfoTip();
            this.SuspendLayout();
            // 
            // labelMultipleIdentical
            // 
            this.labelMultipleIdentical.AutoSize = true;
            this.labelMultipleIdentical.Location = new System.Drawing.Point(4, 47);
            this.labelMultipleIdentical.Name = "labelMultipleIdentical";
            this.labelMultipleIdentical.Size = new System.Drawing.Size(160, 13);
            this.labelMultipleIdentical.TabIndex = 43;
            this.labelMultipleIdentical.Text = "Show multiple identical items as:";
            // 
            // zeroItems
            // 
            this.zeroItems.Location = new System.Drawing.Point(21, 101);
            this.zeroItems.Name = "zeroItems";
            this.zeroItems.Size = new System.Drawing.Size(149, 21);
            this.zeroItems.TabIndex = 46;
            // 
            // labelMultipleItems
            // 
            this.labelMultipleItems.AutoSize = true;
            this.labelMultipleItems.Location = new System.Drawing.Point(4, 7);
            this.labelMultipleItems.Name = "labelMultipleItems";
            this.labelMultipleItems.Size = new System.Drawing.Size(118, 13);
            this.labelMultipleItems.TabIndex = 41;
            this.labelMultipleItems.Text = "Show multiple items as:";
            // 
            // multipleItems
            // 
            this.multipleItems.Location = new System.Drawing.Point(21, 23);
            this.multipleItems.Name = "multipleItems";
            this.multipleItems.Size = new System.Drawing.Size(149, 21);
            this.multipleItems.TabIndex = 42;
            // 
            // multipleIdentical
            // 
            this.multipleIdentical.Location = new System.Drawing.Point(21, 63);
            this.multipleIdentical.Name = "multipleIdentical";
            this.multipleIdentical.Size = new System.Drawing.Size(149, 21);
            this.multipleIdentical.TabIndex = 44;
            // 
            // labelZeroItems
            // 
            this.labelZeroItems.AutoSize = true;
            this.labelZeroItems.Location = new System.Drawing.Point(4, 85);
            this.labelZeroItems.Name = "labelZeroItems";
            this.labelZeroItems.Size = new System.Drawing.Size(103, 13);
            this.labelZeroItems.TabIndex = 45;
            this.labelZeroItems.Text = "Show zero items as:";
            // 
            // infotipMultipleItems
            // 
            this.infotipMultipleItems.Caption = "You can use the # sign as a placeholder for the actual item count.";
            this.infotipMultipleItems.Location = new System.Drawing.Point(175, 28);
            this.infotipMultipleItems.Name = "infotipMultipleItems";
            this.infotipMultipleItems.Size = new System.Drawing.Size(12, 12);
            this.infotipMultipleItems.TabIndex = 49;
            this.infotipMultipleItems.Title = "Multiple Items";
            // 
            // infotipMultipleIdentical
            // 
            this.infotipMultipleIdentical.Caption = resources.GetString("infotipMultipleIdentical.Caption");
            this.infotipMultipleIdentical.Location = new System.Drawing.Point(175, 68);
            this.infotipMultipleIdentical.Name = "infotipMultipleIdentical";
            this.infotipMultipleIdentical.Size = new System.Drawing.Size(12, 12);
            this.infotipMultipleIdentical.TabIndex = 50;
            this.infotipMultipleIdentical.Title = "Multiple Identical Items";
            // 
            // GridRollupDecoratorEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.infotipMultipleIdentical);
            this.Controls.Add(this.infotipMultipleItems);
            this.Controls.Add(this.labelMultipleIdentical);
            this.Controls.Add(this.zeroItems);
            this.Controls.Add(this.labelMultipleItems);
            this.Controls.Add(this.multipleItems);
            this.Controls.Add(this.multipleIdentical);
            this.Controls.Add(this.labelZeroItems);
            this.Name = "GridRollupDecoratorEditor";
            this.Size = new System.Drawing.Size(220, 129);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelMultipleIdentical;
        private System.Windows.Forms.TextBox zeroItems;
        private System.Windows.Forms.Label labelMultipleItems;
        private System.Windows.Forms.TextBox multipleItems;
        private System.Windows.Forms.TextBox multipleIdentical;
        private System.Windows.Forms.Label labelZeroItems;
        private UI.Controls.InfoTip infotipMultipleItems;
        private UI.Controls.InfoTip infotipMultipleIdentical;

    }
}
