namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor.CoreExtensions.Actions
{
    partial class MarketplaceAdvisorChangeFlagsEditor
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
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer4 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            this.labelClear = new System.Windows.Forms.Label();
            this.gridFlagsOff = new Divelements.SandGrid.SandGrid();
            this.gridColumn1 = new Divelements.SandGrid.GridColumn();
            this.labelTurnOn = new System.Windows.Forms.Label();
            this.gridFlagsOn = new Divelements.SandGrid.SandGrid();
            this.gridColumnFlag = new Divelements.SandGrid.GridColumn();
            this.SuspendLayout();
            // 
            // labelClear
            // 
            this.labelClear.AutoSize = true;
            this.labelClear.Location = new System.Drawing.Point(215, 8);
            this.labelClear.Name = "labelClear";
            this.labelClear.Size = new System.Drawing.Size(92, 13);
            this.labelClear.TabIndex = 21;
            this.labelClear.Text = "Clear these flags:";
            // 
            // gridFlagsOff
            // 
            this.gridFlagsOff.CheckBoxes = true;
            this.gridFlagsOff.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumn1});
            this.gridFlagsOff.Location = new System.Drawing.Point(218, 24);
            this.gridFlagsOff.Name = "gridFlagsOff";
            this.gridFlagsOff.Renderer = windowsXPRenderer3;
            this.gridFlagsOff.ShowColumnHeaders = false;
            this.gridFlagsOff.Size = new System.Drawing.Size(189, 113);
            this.gridFlagsOff.TabIndex = 20;
            this.gridFlagsOff.AfterCheck += new Divelements.SandGrid.GridRowCheckEventHandler(this.OnCheckChanged);
            // 
            // gridColumn1
            // 
            this.gridColumn1.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumn1.HeaderText = "Flag";
            this.gridColumn1.Width = 185;
            // 
            // labelTurnOn
            // 
            this.labelTurnOn.AutoSize = true;
            this.labelTurnOn.Location = new System.Drawing.Point(6, 7);
            this.labelTurnOn.Name = "labelTurnOn";
            this.labelTurnOn.Size = new System.Drawing.Size(104, 13);
            this.labelTurnOn.TabIndex = 19;
            this.labelTurnOn.Text = "Turn on these flags:";
            // 
            // gridFlagsOn
            // 
            this.gridFlagsOn.CheckBoxes = true;
            this.gridFlagsOn.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnFlag});
            this.gridFlagsOn.Location = new System.Drawing.Point(9, 24);
            this.gridFlagsOn.Name = "gridFlagsOn";
            this.gridFlagsOn.Renderer = windowsXPRenderer4;
            this.gridFlagsOn.ShowColumnHeaders = false;
            this.gridFlagsOn.Size = new System.Drawing.Size(189, 113);
            this.gridFlagsOn.TabIndex = 18;
            this.gridFlagsOn.AfterCheck += new Divelements.SandGrid.GridRowCheckEventHandler(this.OnCheckChanged);
            // 
            // gridColumnFlag
            // 
            this.gridColumnFlag.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnFlag.HeaderText = "Flag";
            this.gridColumnFlag.Width = 185;
            // 
            // MarketplaceAdvisorChangeFlagsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelClear);
            this.Controls.Add(this.gridFlagsOff);
            this.Controls.Add(this.labelTurnOn);
            this.Controls.Add(this.gridFlagsOn);
            this.Name = "MarketplaceAdvisorChangeFlagsEditor";
            this.Size = new System.Drawing.Size(415, 158);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelClear;
        private Divelements.SandGrid.SandGrid gridFlagsOff;
        private Divelements.SandGrid.GridColumn gridColumn1;
        private System.Windows.Forms.Label labelTurnOn;
        private Divelements.SandGrid.SandGrid gridFlagsOn;
        private Divelements.SandGrid.GridColumn gridColumnFlag;
    }
}
