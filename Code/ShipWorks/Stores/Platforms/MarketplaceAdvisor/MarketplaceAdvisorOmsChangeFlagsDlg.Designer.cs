namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor
{
    partial class MarketplaceAdvisorOmsChangeFlagsDlg
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer5 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer6 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.gridFlagsOn = new Divelements.SandGrid.SandGrid();
            this.gridColumnFlag = new Divelements.SandGrid.GridColumn();
            this.labelTurnOn = new System.Windows.Forms.Label();
            this.labelClear = new System.Windows.Forms.Label();
            this.gridFlagsOff = new Divelements.SandGrid.SandGrid();
            this.gridColumn1 = new Divelements.SandGrid.GridColumn();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok.Location = new System.Drawing.Point(187, 302);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 0;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(268, 302);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // gridFlagsOn
            // 
            this.gridFlagsOn.CheckBoxes = true;
            this.gridFlagsOn.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnFlag});
            this.gridFlagsOn.Location = new System.Drawing.Point(16, 30);
            this.gridFlagsOn.Name = "gridFlagsOn";
            this.gridFlagsOn.Renderer = windowsXPRenderer5;
            this.gridFlagsOn.ShowColumnHeaders = false;
            this.gridFlagsOn.Size = new System.Drawing.Size(326, 113);
            this.gridFlagsOn.TabIndex = 14;
            // 
            // gridColumnFlag
            // 
            this.gridColumnFlag.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnFlag.HeaderText = "Flag";
            this.gridColumnFlag.Width = 322;
            // 
            // labelTurnOn
            // 
            this.labelTurnOn.AutoSize = true;
            this.labelTurnOn.Location = new System.Drawing.Point(13, 13);
            this.labelTurnOn.Name = "labelTurnOn";
            this.labelTurnOn.Size = new System.Drawing.Size(168, 13);
            this.labelTurnOn.TabIndex = 15;
            this.labelTurnOn.Text = "Turn on these flags for all orders:";
            // 
            // labelClear
            // 
            this.labelClear.AutoSize = true;
            this.labelClear.Location = new System.Drawing.Point(13, 160);
            this.labelClear.Name = "labelClear";
            this.labelClear.Size = new System.Drawing.Size(156, 13);
            this.labelClear.TabIndex = 17;
            this.labelClear.Text = "Clear these flags for all orders:";
            // 
            // gridFlagsOff
            // 
            this.gridFlagsOff.CheckBoxes = true;
            this.gridFlagsOff.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumn1});
            this.gridFlagsOff.Location = new System.Drawing.Point(16, 177);
            this.gridFlagsOff.Name = "gridFlagsOff";
            this.gridFlagsOff.Renderer = windowsXPRenderer6;
            this.gridFlagsOff.ShowColumnHeaders = false;
            this.gridFlagsOff.Size = new System.Drawing.Size(326, 113);
            this.gridFlagsOff.TabIndex = 16;
            // 
            // gridColumn1
            // 
            this.gridColumn1.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumn1.HeaderText = "Flag";
            this.gridColumn1.Width = 322;
            // 
            // MarketplaceAdvisorOmsChangeFlagsDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(355, 337);
            this.Controls.Add(this.labelClear);
            this.Controls.Add(this.gridFlagsOff);
            this.Controls.Add(this.labelTurnOn);
            this.Controls.Add(this.gridFlagsOn);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MarketplaceAdvisorOmsChangeFlagsDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change Order Flags";
            this.Shown += new System.EventHandler(this.OnShown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private Divelements.SandGrid.SandGrid gridFlagsOn;
        private Divelements.SandGrid.GridColumn gridColumnFlag;
        private System.Windows.Forms.Label labelTurnOn;
        private System.Windows.Forms.Label labelClear;
        private Divelements.SandGrid.SandGrid gridFlagsOff;
        private Divelements.SandGrid.GridColumn gridColumn1;
    }
}
