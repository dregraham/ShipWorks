namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor
{
    partial class MarketplaceAdvisorOmsFlagsControl
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
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer3 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            this.sandGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnFlag = new Divelements.SandGrid.GridColumn();
            this.labelNoteInfo = new System.Windows.Forms.Label();
            this.labelNote = new System.Windows.Forms.Label();
            this.labelFlagsInfo3 = new System.Windows.Forms.Label();
            this.labelFlagsInfo2 = new System.Windows.Forms.Label();
            this.labelFlagsInfo1 = new System.Windows.Forms.Label();
            this.labelFlags = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // sandGrid
            // 
            this.sandGrid.CheckBoxes = true;
            this.sandGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnFlag});
            this.sandGrid.Location = new System.Drawing.Point(27, 65);
            this.sandGrid.Name = "sandGrid";
            this.sandGrid.Renderer = windowsXPRenderer3;
            this.sandGrid.ShowColumnHeaders = false;
            this.sandGrid.Size = new System.Drawing.Size(326, 93);
            this.sandGrid.TabIndex = 13;
            // 
            // gridColumnFlag
            // 
            this.gridColumnFlag.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnFlag.HeaderText = "Flag";
            this.gridColumnFlag.Width = 322;
            // 
            // labelNoteInfo
            // 
            this.labelNoteInfo.Location = new System.Drawing.Point(44, 172);
            this.labelNoteInfo.Name = "labelNoteInfo";
            this.labelNoteInfo.Size = new System.Drawing.Size(423, 33);
            this.labelNoteInfo.TabIndex = 12;
            this.labelNoteInfo.Text = "ShipWorks only downloads each order once.  Once an order is downloaded, it is not" +
                " downloaded again even if the flags or any other properties of the order change." +
                "";
            // 
            // labelNote
            // 
            this.labelNote.AutoSize = true;
            this.labelNote.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelNote.Location = new System.Drawing.Point(3, 172);
            this.labelNote.Name = "labelNote";
            this.labelNote.Size = new System.Drawing.Size(36, 13);
            this.labelNote.TabIndex = 11;
            this.labelNote.Text = "Note:";
            // 
            // labelFlagsInfo3
            // 
            this.labelFlagsInfo3.AutoSize = true;
            this.labelFlagsInfo3.Location = new System.Drawing.Point(224, 39);
            this.labelFlagsInfo3.Name = "labelFlagsInfo3";
            this.labelFlagsInfo3.Size = new System.Drawing.Size(129, 13);
            this.labelFlagsInfo3.TabIndex = 10;
            this.labelFlagsInfo3.Text = "of the following flags set:";
            // 
            // labelFlagsInfo2
            // 
            this.labelFlagsInfo2.AutoSize = true;
            this.labelFlagsInfo2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelFlagsInfo2.Location = new System.Drawing.Point(204, 39);
            this.labelFlagsInfo2.Name = "labelFlagsInfo2";
            this.labelFlagsInfo2.Size = new System.Drawing.Size(20, 13);
            this.labelFlagsInfo2.TabIndex = 9;
            this.labelFlagsInfo2.Text = "all";
            // 
            // labelFlagsInfo1
            // 
            this.labelFlagsInfo1.AutoSize = true;
            this.labelFlagsInfo1.Location = new System.Drawing.Point(0, 39);
            this.labelFlagsInfo1.Name = "labelFlagsInfo1";
            this.labelFlagsInfo1.Size = new System.Drawing.Size(207, 13);
            this.labelFlagsInfo1.TabIndex = 8;
            this.labelFlagsInfo1.Text = "ShipWorks will download orders that have";
            // 
            // labelFlags
            // 
            this.labelFlags.Location = new System.Drawing.Point(0, 2);
            this.labelFlags.Name = "labelFlags";
            this.labelFlags.Size = new System.Drawing.Size(481, 33);
            this.labelFlags.TabIndex = 7;
            this.labelFlags.Text = "ShipWorks downloads MarketplaceAdvisor orders based on flags.  Select the flags that Shi" +
                "pWorks should look for when downloading orders.";
            // 
            // MarketplaceAdvisorOmsFlagsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sandGrid);
            this.Controls.Add(this.labelNoteInfo);
            this.Controls.Add(this.labelNote);
            this.Controls.Add(this.labelFlagsInfo3);
            this.Controls.Add(this.labelFlagsInfo2);
            this.Controls.Add(this.labelFlagsInfo1);
            this.Controls.Add(this.labelFlags);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "MarketplaceAdvisorOmsFlagsControl";
            this.Size = new System.Drawing.Size(486, 218);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Divelements.SandGrid.SandGrid sandGrid;
        private Divelements.SandGrid.GridColumn gridColumnFlag;
        private System.Windows.Forms.Label labelNoteInfo;
        private System.Windows.Forms.Label labelNote;
        private System.Windows.Forms.Label labelFlagsInfo3;
        private System.Windows.Forms.Label labelFlagsInfo2;
        private System.Windows.Forms.Label labelFlagsInfo1;
        private System.Windows.Forms.Label labelFlags;
    }
}
