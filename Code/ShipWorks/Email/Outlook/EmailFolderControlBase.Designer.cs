namespace ShipWorks.Email.Outlook
{
    partial class EmailFolderControlBase
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
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer1 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            this.panelGridArea = new System.Windows.Forms.Panel();
            this.entityGrid = new ShipWorks.Data.Grid.Paging.PagedEntityGrid();
            this.kryptonBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.panelTools = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.labelGridSettings = new System.Windows.Forms.Label();
            this.labelActions = new System.Windows.Forms.Label();
            this.labelManage = new System.Windows.Forms.Label();
            this.manageAccounts = new System.Windows.Forms.Button();
            this.panelMessageControls = new System.Windows.Forms.Panel();
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.panelCore = new System.Windows.Forms.Panel();
            this.panelGridArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.panelTools)).BeginInit();
            this.panelTools.SuspendLayout();
            this.panelMessageControls.SuspendLayout();
            this.panelSidebar.SuspendLayout();
            this.panelCore.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelGridArea
            // 
            this.panelGridArea.BackColor = System.Drawing.Color.White;
            this.panelGridArea.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelGridArea.Controls.Add(this.entityGrid);
            this.panelGridArea.Controls.Add(this.kryptonBorderEdge);
            this.panelGridArea.Controls.Add(this.panelTools);
            this.panelGridArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGridArea.Location = new System.Drawing.Point(0, 0);
            this.panelGridArea.Name = "panelGridArea";
            this.panelGridArea.Size = new System.Drawing.Size(576, 500);
            this.panelGridArea.TabIndex = 5;
            // 
            // entityGrid
            // 
            this.entityGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.entityGrid.DetailViewSettings = null;
            this.entityGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.entityGrid.EnableSearching = false;
            this.entityGrid.GridLines = Divelements.SandGrid.GridLinesDisplayType.Both;
            this.entityGrid.LiveResize = false;
            this.entityGrid.Location = new System.Drawing.Point(0, 0);
            this.entityGrid.Name = "entityGrid";
            this.entityGrid.NullRepresentation = "";
            this.entityGrid.Renderer = windowsXPRenderer1;
            this.entityGrid.RowHighlightType = Divelements.SandGrid.RowHighlightType.Full;
            this.entityGrid.ShadeAlternateRows = true;
            this.entityGrid.Size = new System.Drawing.Size(572, 469);
            this.entityGrid.StretchPrimaryGrid = false;
            this.entityGrid.TabIndex = 0;
            // 
            // kryptonBorderEdge
            // 
            this.kryptonBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.kryptonBorderEdge.Location = new System.Drawing.Point(0, 469);
            this.kryptonBorderEdge.Name = "kryptonBorderEdge";
            this.kryptonBorderEdge.Size = new System.Drawing.Size(572, 1);
            this.kryptonBorderEdge.TabIndex = 2;
            this.kryptonBorderEdge.Text = "kryptonBorderEdge1";
            // 
            // panelTools
            // 
            this.panelTools.Controls.Add(this.labelGridSettings);
            this.panelTools.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelTools.Location = new System.Drawing.Point(0, 470);
            this.panelTools.Name = "panelTools";
            this.panelTools.PanelBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridHeaderColumnSheet;
            this.panelTools.Size = new System.Drawing.Size(572, 26);
            this.panelTools.TabIndex = 1;
            // 
            // labelGridSettings
            // 
            this.labelGridSettings.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelGridSettings.AutoSize = true;
            this.labelGridSettings.BackColor = System.Drawing.Color.Transparent;
            this.labelGridSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelGridSettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelGridSettings.ForeColor = System.Drawing.Color.Blue;
            this.labelGridSettings.Location = new System.Drawing.Point(499, 6);
            this.labelGridSettings.Name = "labelGridSettings";
            this.labelGridSettings.Size = new System.Drawing.Size(68, 13);
            this.labelGridSettings.TabIndex = 12;
            this.labelGridSettings.Text = "Grid Settings";
            this.labelGridSettings.Click += new System.EventHandler(this.OnGridSettings);
            // 
            // labelActions
            // 
            this.labelActions.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelActions.AutoSize = true;
            this.labelActions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelActions.Location = new System.Drawing.Point(2, 1);
            this.labelActions.Name = "labelActions";
            this.labelActions.Size = new System.Drawing.Size(63, 13);
            this.labelActions.TabIndex = 10;
            this.labelActions.Text = "Messages";
            // 
            // labelManage
            // 
            this.labelManage.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelManage.AutoSize = true;
            this.labelManage.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelManage.Location = new System.Drawing.Point(2, 3);
            this.labelManage.Name = "labelManage";
            this.labelManage.Size = new System.Drawing.Size(52, 13);
            this.labelManage.TabIndex = 0;
            this.labelManage.Text = "Manage";
            // 
            // manageAccounts
            // 
            this.manageAccounts.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.manageAccounts.Image = global::ShipWorks.Properties.Resources.mail_server16;
            this.manageAccounts.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.manageAccounts.Location = new System.Drawing.Point(5, 19);
            this.manageAccounts.Name = "manageAccounts";
            this.manageAccounts.Size = new System.Drawing.Size(115, 23);
            this.manageAccounts.TabIndex = 1;
            this.manageAccounts.Text = "Accounts...";
            this.manageAccounts.UseVisualStyleBackColor = true;
            this.manageAccounts.Click += new System.EventHandler(this.OnManageAccounts);
            // 
            // panelMessageControls
            // 
            this.panelMessageControls.Controls.Add(this.labelActions);
            this.panelMessageControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMessageControls.Location = new System.Drawing.Point(0, 0);
            this.panelMessageControls.Name = "panelMessageControls";
            this.panelMessageControls.Size = new System.Drawing.Size(124, 88);
            this.panelMessageControls.TabIndex = 0;
            // 
            // panelSidebar
            // 
            this.panelSidebar.Controls.Add(this.panelCore);
            this.panelSidebar.Controls.Add(this.panelMessageControls);
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelSidebar.Location = new System.Drawing.Point(576, 0);
            this.panelSidebar.Name = "panelSidebar";
            this.panelSidebar.Size = new System.Drawing.Size(124, 500);
            this.panelSidebar.TabIndex = 15;
            // 
            // panelCore
            // 
            this.panelCore.Controls.Add(this.labelManage);
            this.panelCore.Controls.Add(this.manageAccounts);
            this.panelCore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCore.Location = new System.Drawing.Point(0, 88);
            this.panelCore.Name = "panelCore";
            this.panelCore.Size = new System.Drawing.Size(124, 412);
            this.panelCore.TabIndex = 15;
            // 
            // EmailFolderControlBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelGridArea);
            this.Controls.Add(this.panelSidebar);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "EmailFolderControlBase";
            this.Size = new System.Drawing.Size(700, 500);
            this.panelGridArea.ResumeLayout(false);
            this.panelGridArea.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.panelTools)).EndInit();
            this.panelTools.ResumeLayout(false);
            this.panelTools.PerformLayout();
            this.panelMessageControls.ResumeLayout(false);
            this.panelMessageControls.PerformLayout();
            this.panelSidebar.ResumeLayout(false);
            this.panelCore.ResumeLayout(false);
            this.panelCore.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelGridArea;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel panelTools;
        private System.Windows.Forms.Label labelGridSettings;
        private System.Windows.Forms.Label labelActions;
        private System.Windows.Forms.Label labelManage;
        private System.Windows.Forms.Button manageAccounts;
        protected ShipWorks.Data.Grid.Paging.PagedEntityGrid entityGrid;
        protected System.Windows.Forms.Panel panelMessageControls;
        private System.Windows.Forms.Panel panelCore;
        private System.Windows.Forms.Panel panelSidebar;
    }
}
