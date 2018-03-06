namespace ShipWorks.Shipping.Settings.Printing
{
    partial class PrintOutputControl
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
            this.panelMain = new System.Windows.Forms.Panel();
            this.toolStripAddPrintOutput = new System.Windows.Forms.ToolStrip();
            this.add = new System.Windows.Forms.ToolStripButton();
            this.labelInfo = new System.Windows.Forms.Label();
            this.printActionBox = new System.Windows.Forms.CheckBox();
            this.labelActionPrinting = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.installMissingLink = new ShipWorks.UI.Controls.LinkControl();
            this.toolStripAddPrintOutput.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMain.Location = new System.Drawing.Point(16, 95);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(558, 59);
            this.panelMain.TabIndex = 1;
            // 
            // toolStripAddPrintOutput
            // 
            this.toolStripAddPrintOutput.BackColor = System.Drawing.Color.Transparent;
            this.toolStripAddPrintOutput.CanOverflow = false;
            this.toolStripAddPrintOutput.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripAddPrintOutput.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStripAddPrintOutput.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripAddPrintOutput.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.add});
            this.toolStripAddPrintOutput.Location = new System.Drawing.Point(16, 159);
            this.toolStripAddPrintOutput.Name = "toolStripAddPrintOutput";
            this.toolStripAddPrintOutput.Padding = new System.Windows.Forms.Padding(0);
            this.toolStripAddPrintOutput.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStripAddPrintOutput.Size = new System.Drawing.Size(87, 25);
            this.toolStripAddPrintOutput.Stretch = true;
            this.toolStripAddPrintOutput.TabIndex = 2;
            // 
            // add
            // 
            this.add.Image = global::ShipWorks.Properties.Resources.add16;
            this.add.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(85, 22);
            this.add.Text = "Add Group";
            this.add.Click += new System.EventHandler(this.OnAddGroup);
            // 
            // labelInfo
            // 
            this.labelInfo.Location = new System.Drawing.Point(17, 74);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(424, 19);
            this.labelInfo.TabIndex = 0;
            this.labelInfo.Text = "ShipWorks will print using the first template in each group that satisfies the co" +
    "ndition.";
            // 
            // printActionBox
            // 
            this.printActionBox.AutoSize = true;
            this.printActionBox.Checked = true;
            this.printActionBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.printActionBox.Location = new System.Drawing.Point(20, 25);
            this.printActionBox.Name = "printActionBox";
            this.printActionBox.Size = new System.Drawing.Size(230, 17);
            this.printActionBox.TabIndex = 3;
            this.printActionBox.Text = "Automatically print labels after processing.";
            this.printActionBox.UseVisualStyleBackColor = true;
            // 
            // labelActionPrinting
            // 
            this.labelActionPrinting.AutoSize = true;
            this.labelActionPrinting.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelActionPrinting.Location = new System.Drawing.Point(5, 6);
            this.labelActionPrinting.Name = "labelActionPrinting";
            this.labelActionPrinting.Size = new System.Drawing.Size(113, 13);
            this.labelActionPrinting.TabIndex = 4;
            this.labelActionPrinting.Text = "Automatic Printing";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Output Rules";
            // 
            // installMissingLink
            // 
            this.installMissingLink.AutoSize = true;
            this.installMissingLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.installMissingLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.installMissingLink.ForeColor = System.Drawing.Color.Blue;
            this.installMissingLink.Location = new System.Drawing.Point(88, 54);
            this.installMissingLink.Name = "installMissingLink";
            this.installMissingLink.Size = new System.Drawing.Size(116, 13);
            this.installMissingLink.TabIndex = 6;
            this.installMissingLink.Text = "Install Missing Defaults";
            this.installMissingLink.Click += new System.EventHandler(this.OnInstallMissingClick);
            // 
            // PrintOutputControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.installMissingLink);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelActionPrinting);
            this.Controls.Add(this.printActionBox);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.toolStripAddPrintOutput);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PrintOutputControl";
            this.Size = new System.Drawing.Size(584, 214);
            this.toolStripAddPrintOutput.ResumeLayout(false);
            this.toolStripAddPrintOutput.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.ToolStrip toolStripAddPrintOutput;
        private System.Windows.Forms.ToolStripButton add;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.CheckBox printActionBox;
        private System.Windows.Forms.Label labelActionPrinting;
        private System.Windows.Forms.Label label1;
        private UI.Controls.LinkControl installMissingLink;

    }
}
