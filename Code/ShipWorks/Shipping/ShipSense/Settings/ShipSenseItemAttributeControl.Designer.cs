namespace ShipWorks.Shipping.ShipSense.Settings
{
    partial class ShipSenseItemAttributeControl
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
            this.labelName = new System.Windows.Forms.Label();
            this.toolStripDelete = new System.Windows.Forms.ToolStrip();
            this.buttonDelete = new System.Windows.Forms.ToolStripButton();
            this.attribute = new System.Windows.Forms.TextBox();
            this.toolStripDelete.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(26, 9);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(83, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Attribute name:";
            // 
            // toolStripDelete
            // 
            this.toolStripDelete.BackColor = System.Drawing.Color.Transparent;
            this.toolStripDelete.CanOverflow = false;
            this.toolStripDelete.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripDelete.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStripDelete.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripDelete.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonDelete});
            this.toolStripDelete.Location = new System.Drawing.Point(0, 4);
            this.toolStripDelete.Name = "toolStripDelete";
            this.toolStripDelete.Padding = new System.Windows.Forms.Padding(0);
            this.toolStripDelete.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripDelete.Size = new System.Drawing.Size(25, 25);
            this.toolStripDelete.Stretch = true;
            this.toolStripDelete.TabIndex = 1;
            // 
            // buttonDelete
            // 
            this.buttonDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonDelete.Image = global::ShipWorks.Properties.Resources.delete16;
            this.buttonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(23, 22);
            this.buttonDelete.Text = "Remove Line";
            this.buttonDelete.Click += new System.EventHandler(this.OnDelete);
            // 
            // attribute
            // 
            this.attribute.Location = new System.Drawing.Point(112, 6);
            this.attribute.Name = "attribute";
            this.attribute.Size = new System.Drawing.Size(185, 21);
            this.attribute.TabIndex = 2;
            // 
            // ShipSenseItemAttributeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.attribute);
            this.Controls.Add(this.toolStripDelete);
            this.Controls.Add(this.labelName);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ShipSenseItemAttributeControl";
            this.Size = new System.Drawing.Size(312, 33);
            this.toolStripDelete.ResumeLayout(false);
            this.toolStripDelete.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.ToolStrip toolStripDelete;
        private System.Windows.Forms.ToolStripButton buttonDelete;
        private System.Windows.Forms.TextBox attribute;
    }
}
