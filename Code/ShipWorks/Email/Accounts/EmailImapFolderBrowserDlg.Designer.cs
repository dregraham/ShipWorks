namespace ShipWorks.Email.Accounts
{
    partial class EmailImapFolderBrowserDlg
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
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer1 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.sandGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnFilterNode = new Divelements.SandGrid.GridColumn();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(152, 332);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 0;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(233, 332);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // sandGrid
            // 
            this.sandGrid.AllowMultipleSelection = false;
            this.sandGrid.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sandGrid.ColumnClickBehavior = Divelements.SandGrid.ColumnClickBehavior.None;
            this.sandGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnFilterNode});
            this.sandGrid.EmptyText = "Loading...";
            this.sandGrid.EmptyTextForeColor = System.Drawing.SystemColors.GrayText;
            this.sandGrid.ImageTextSeparation = 5;
            this.sandGrid.Location = new System.Drawing.Point(8, 8);
            this.sandGrid.Name = "sandGrid";
            this.sandGrid.NullRepresentation = "";
            this.sandGrid.Renderer = windowsXPRenderer1;
            this.sandGrid.RowDragBehavior = Divelements.SandGrid.RowDragBehavior.None;
            this.sandGrid.RowHighlightType = Divelements.SandGrid.RowHighlightType.PrimaryColumnOnly;
            this.sandGrid.ShowColumnHeaders = false;
            this.sandGrid.ShowTreeButtons = true;
            this.sandGrid.Size = new System.Drawing.Size(300, 318);
            this.sandGrid.TabIndex = 2;
            this.sandGrid.WhitespaceClickBehavior = Divelements.SandGrid.WhitespaceClickBehavior.None;
            // 
            // gridColumnFilterNode
            // 
            this.gridColumnFilterNode.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnFilterNode.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnFilterNode.Width = 0;
            // 
            // EmailImapFolderBrowserDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(320, 367);
            this.Controls.Add(this.sandGrid);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EmailImapFolderBrowserDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Folder";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private Divelements.SandGrid.SandGrid sandGrid;
        private Divelements.SandGrid.GridColumn gridColumnFilterNode;
    }
}