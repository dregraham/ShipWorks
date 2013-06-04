namespace ShipWorks.Filters.Content.Editors
{
    partial class ConditionLineControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConditionLineControl));
            this.toolbarDelete = new System.Windows.Forms.ToolStrip();
            this.buttonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolBarAdd = new System.Windows.Forms.ToolStrip();
            this.buttonAddCondition = new System.Windows.Forms.ToolStripButton();
            this.buttonAddGroup = new System.Windows.Forms.ToolStripButton();
            this.toolbarDelete.SuspendLayout();
            this.toolBarAdd.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolbarDelete
            // 
            this.toolbarDelete.BackColor = System.Drawing.Color.Transparent;
            this.toolbarDelete.CanOverflow = false;
            this.toolbarDelete.Dock = System.Windows.Forms.DockStyle.None;
            this.toolbarDelete.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolbarDelete.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolbarDelete.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonDelete});
            this.toolbarDelete.Location = new System.Drawing.Point(0, 0);
            this.toolbarDelete.Name = "toolbarDelete";
            this.toolbarDelete.Padding = new System.Windows.Forms.Padding(0);
            this.toolbarDelete.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolbarDelete.Size = new System.Drawing.Size(25, 25);
            this.toolbarDelete.Stretch = true;
            this.toolbarDelete.TabIndex = 0;
            // 
            // buttonDelete
            // 
            this.buttonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonDelete.Image = global::ShipWorks.Properties.Resources.delete16;
            this.buttonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(23, 22);
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.Click += new System.EventHandler(this.OnDelete);
            // 
            // toolBarAdd
            // 
            this.toolBarAdd.BackColor = System.Drawing.Color.Transparent;
            this.toolBarAdd.CanOverflow = false;
            this.toolBarAdd.Dock = System.Windows.Forms.DockStyle.None;
            this.toolBarAdd.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolBarAdd.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolBarAdd.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonAddGroup,
            this.buttonAddCondition});
            this.toolBarAdd.Location = new System.Drawing.Point(177, 1);
            this.toolBarAdd.Name = "toolBarAdd";
            this.toolBarAdd.Padding = new System.Windows.Forms.Padding(0);
            this.toolBarAdd.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolBarAdd.Size = new System.Drawing.Size(79, 25);
            this.toolBarAdd.Stretch = true;
            this.toolBarAdd.TabIndex = 1;
            // 
            // buttonAddCondition
            // 
            this.buttonAddCondition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonAddCondition.Image = ((System.Drawing.Image) (resources.GetObject("buttonAddCondition.Image")));
            this.buttonAddCondition.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonAddCondition.Name = "buttonAddCondition";
            this.buttonAddCondition.Size = new System.Drawing.Size(23, 22);
            this.buttonAddCondition.Text = "Add Condition";
            this.buttonAddCondition.Click += new System.EventHandler(this.OnAddCondition);
            // 
            // buttonAddGroup
            // 
            this.buttonAddGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonAddGroup.Image = global::ShipWorks.Properties.Resources.branch_element_bottom;
            this.buttonAddGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonAddGroup.Name = "buttonAddGroup";
            this.buttonAddGroup.Size = new System.Drawing.Size(23, 22);
            this.buttonAddGroup.Text = "Add Branch";
            this.buttonAddGroup.Click += new System.EventHandler(this.OnAddGroup);
            // 
            // ConditionLineControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolBarAdd);
            this.Controls.Add(this.toolbarDelete);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "ConditionLineControl";
            this.Size = new System.Drawing.Size(292, 26);
            this.Load += new System.EventHandler(this.OnLoad);
            this.toolbarDelete.ResumeLayout(false);
            this.toolbarDelete.PerformLayout();
            this.toolBarAdd.ResumeLayout(false);
            this.toolBarAdd.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolbarDelete;
        private System.Windows.Forms.ToolStripButton buttonDelete;
        private System.Windows.Forms.ToolStrip toolBarAdd;
        private System.Windows.Forms.ToolStripButton buttonAddCondition;
        private System.Windows.Forms.ToolStripButton buttonAddGroup;
    }
}
