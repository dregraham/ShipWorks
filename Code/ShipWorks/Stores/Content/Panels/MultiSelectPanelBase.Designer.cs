namespace ShipWorks.Stores.Content.Panels
{
    partial class MultiSelectPanelBase
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
            this.groupBy = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // addLink
            // 
            this.addLink.Location = new System.Drawing.Point(232, 30);
            // 
            // entityGrid
            // 
            this.entityGrid.Size = new System.Drawing.Size(262, 25);
            this.entityGrid.SortChanged += new Divelements.SandGrid.GridEventHandler(this.OnGridSorted);
            // 
            // groupBy
            // 
            this.groupBy.AutoSize = true;
            this.groupBy.ForeColor = System.Drawing.Color.DimGray;
            this.groupBy.Location = new System.Drawing.Point(2, 29);
            this.groupBy.Name = "groupBy";
            this.groupBy.Size = new System.Drawing.Size(99, 17);
            this.groupBy.TabIndex = 4;
            this.groupBy.Text = "Group by order";
            this.groupBy.UseVisualStyleBackColor = true;
            this.groupBy.CheckedChanged += new System.EventHandler(this.OnGroupByChanged);
            // 
            // MultiSelectPanelBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBy);
            this.Name = "MultiSelectPanelBase";
            this.Controls.SetChildIndex(this.groupBy, 0);
            this.Controls.SetChildIndex(this.entityGrid, 0);
            this.Controls.SetChildIndex(this.addLink, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox groupBy;

    }
}
