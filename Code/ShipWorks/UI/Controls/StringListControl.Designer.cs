namespace ShipWorks.UI.Controls
{
    partial class StringListControl
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
            this.panelValues = new System.Windows.Forms.Panel();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.toolStripAddRule = new System.Windows.Forms.ToolStrip();
            this.addValueLine = new System.Windows.Forms.ToolStripButton();
            this.panelBottom.SuspendLayout();
            this.toolStripAddRule.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelValues
            // 
            this.panelValues.Location = new System.Drawing.Point(6, 0);
            this.panelValues.Name = "panelValues";
            this.panelValues.Size = new System.Drawing.Size(466, 10);
            this.panelValues.TabIndex = 10;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.toolStripAddRule);
            this.panelBottom.Location = new System.Drawing.Point(0, 16);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(471, 35);
            this.panelBottom.TabIndex = 9;
            // 
            // toolStripAddRule
            // 
            this.toolStripAddRule.BackColor = System.Drawing.Color.Transparent;
            this.toolStripAddRule.CanOverflow = false;
            this.toolStripAddRule.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripAddRule.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStripAddRule.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripAddRule.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addValueLine});
            this.toolStripAddRule.Location = new System.Drawing.Point(6, 5);
            this.toolStripAddRule.Name = "toolStripAddRule";
            this.toolStripAddRule.Padding = new System.Windows.Forms.Padding(0);
            this.toolStripAddRule.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripAddRule.Size = new System.Drawing.Size(101, 25);
            this.toolStripAddRule.Stretch = true;
            this.toolStripAddRule.TabIndex = 38;
            // 
            // addValueLine
            // 
            this.addValueLine.Image = global::ShipWorks.Properties.Resources.add16;
            this.addValueLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addValueLine.Name = "addValueLine";
            this.addValueLine.Size = new System.Drawing.Size(99, 22);
            this.addValueLine.Text = "Add Attribute";
            this.addValueLine.Click += new System.EventHandler(this.OnAddValue);
            // 
            // StringListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.panelValues);
            this.Controls.Add(this.panelBottom);
            this.Name = "StringListControl";
            this.Size = new System.Drawing.Size(568, 68);
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.toolStripAddRule.ResumeLayout(false);
            this.toolStripAddRule.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripButton addValueLine;
        private System.Windows.Forms.Panel panelValues;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.ToolStrip toolStripAddRule;
    }
}
