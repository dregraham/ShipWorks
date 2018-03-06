namespace ShipWorks.Shipping.Settings.Printing
{
    partial class PrintOutputGroupControl
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
            this.borderLeft = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.borderBottom = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.toolStripDeleteRename = new System.Windows.Forms.ToolStrip();
            this.delete = new System.Windows.Forms.ToolStripButton();
            this.rename = new System.Windows.Forms.ToolStripButton();
            this.toolStripAdd = new System.Windows.Forms.ToolStrip();
            this.add = new System.Windows.Forms.ToolStripButton();
            this.labeName = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.toolStripDeleteRename.SuspendLayout();
            this.toolStripAdd.SuspendLayout();
            this.SuspendLayout();
            // 
            // borderLeft
            // 
            this.borderLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.borderLeft.AutoSize = false;
            this.borderLeft.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.borderLeft.Location = new System.Drawing.Point(4, 28);
            this.borderLeft.Name = "borderLeft";
            this.borderLeft.Size = new System.Drawing.Size(1, 83);
            this.borderLeft.Text = "kryptonBorderEdge4";
            // 
            // borderBottom
            // 
            this.borderBottom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.borderBottom.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.borderBottom.Location = new System.Drawing.Point(4, 111);
            this.borderBottom.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.borderBottom.Name = "borderBottom";
            this.borderBottom.Size = new System.Drawing.Size(567, 1);
            this.borderBottom.Text = "kryptonBorderEdge1";
            // 
            // toolStripDeleteRename
            // 
            this.toolStripDeleteRename.BackColor = System.Drawing.Color.Transparent;
            this.toolStripDeleteRename.CanOverflow = false;
            this.toolStripDeleteRename.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripDeleteRename.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStripDeleteRename.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripDeleteRename.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.delete,
            this.rename});
            this.toolStripDeleteRename.Location = new System.Drawing.Point(94, 4);
            this.toolStripDeleteRename.Name = "toolStripDeleteRename";
            this.toolStripDeleteRename.Padding = new System.Windows.Forms.Padding(0);
            this.toolStripDeleteRename.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripDeleteRename.Size = new System.Drawing.Size(48, 25);
            this.toolStripDeleteRename.Stretch = true;
            this.toolStripDeleteRename.TabIndex = 1;
            // 
            // delete
            // 
            this.delete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.delete.Image = global::ShipWorks.Properties.Resources.delete16;
            this.delete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(23, 22);
            this.delete.Text = "Delete";
            this.delete.Click += new System.EventHandler(this.OnDeleteGroup);
            // 
            // rename
            // 
            this.rename.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.rename.Image = global::ShipWorks.Properties.Resources.rename;
            this.rename.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.rename.Name = "rename";
            this.rename.Size = new System.Drawing.Size(23, 22);
            this.rename.Text = "Rename";
            this.rename.Click += new System.EventHandler(this.OnRenameGroup);
            // 
            // toolStripAdd
            // 
            this.toolStripAdd.BackColor = System.Drawing.Color.Transparent;
            this.toolStripAdd.CanOverflow = false;
            this.toolStripAdd.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripAdd.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStripAdd.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripAdd.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.add});
            this.toolStripAdd.Location = new System.Drawing.Point(8, 84);
            this.toolStripAdd.Name = "toolStripAdd";
            this.toolStripAdd.Padding = new System.Windows.Forms.Padding(0);
            this.toolStripAdd.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripAdd.Size = new System.Drawing.Size(82, 25);
            this.toolStripAdd.Stretch = true;
            this.toolStripAdd.TabIndex = 4;
            // 
            // add
            // 
            this.add.Image = global::ShipWorks.Properties.Resources.add16;
            this.add.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(49, 22);
            this.add.Text = "Add";
            this.add.Click += new System.EventHandler(this.OnAddRule);
            // 
            // labeName
            // 
            this.labeName.AutoSize = true;
            this.labeName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labeName.Location = new System.Drawing.Point(0, 9);
            this.labeName.Name = "labeName";
            this.labeName.Size = new System.Drawing.Size(88, 13);
            this.labeName.TabIndex = 0;
            this.labeName.Text = "Shipping Label";
            // 
            // panelMain
            // 
            this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMain.Location = new System.Drawing.Point(6, 32);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(564, 49);
            this.panelMain.TabIndex = 3;
            // 
            // PrintOutputGroupControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.borderLeft);
            this.Controls.Add(this.borderBottom);
            this.Controls.Add(this.toolStripDeleteRename);
            this.Controls.Add(this.toolStripAdd);
            this.Controls.Add(this.labeName);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PrintOutputGroupControl";
            this.Size = new System.Drawing.Size(570, 125);
            this.toolStripDeleteRename.ResumeLayout(false);
            this.toolStripDeleteRename.PerformLayout();
            this.toolStripAdd.ResumeLayout(false);
            this.toolStripAdd.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge borderLeft;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge borderBottom;
        private System.Windows.Forms.ToolStrip toolStripDeleteRename;
        private System.Windows.Forms.ToolStripButton delete;
        private System.Windows.Forms.ToolStripButton rename;
        private System.Windows.Forms.ToolStrip toolStripAdd;
        private System.Windows.Forms.ToolStripButton add;
        private System.Windows.Forms.Label labeName;
        private System.Windows.Forms.Panel panelMain;
    }
}
