namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    partial class EndiciaAccountManagerControl
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
            this.components = new System.ComponentModel.Container();
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer1 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            this.sandGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnName = new Divelements.SandGrid.GridColumn();
            this.gridColumnPostage = new Divelements.SandGrid.GridColumn();
            this.add = new System.Windows.Forms.Button();
            this.remove = new System.Windows.Forms.Button();
            this.details = new System.Windows.Forms.Button();
            this.editionGuiHelper = new ShipWorks.Editions.EditionGuiHelper(this.components);
            this.SuspendLayout();
            // 
            // sandGrid
            // 
            this.sandGrid.AllowMultipleSelection = false;
            this.sandGrid.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sandGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnName,
            this.gridColumnPostage});
            this.sandGrid.CommitOnLoseFocus = true;
            this.sandGrid.ImageTextSeparation = 1;
            this.sandGrid.Location = new System.Drawing.Point(3, 3);
            this.sandGrid.Name = "sandGrid";
            this.sandGrid.Renderer = windowsXPRenderer1;
            this.sandGrid.RowDragBehavior = Divelements.SandGrid.RowDragBehavior.None;
            this.sandGrid.Size = new System.Drawing.Size(344, 154);
            this.sandGrid.TabIndex = 0;
            this.sandGrid.WhitespaceClickBehavior = Divelements.SandGrid.WhitespaceClickBehavior.None;
            this.sandGrid.SelectionChanged += new Divelements.SandGrid.SelectionChangedEventHandler(this.OnChangeSelectedAccount);
            this.sandGrid.RowActivated += new Divelements.SandGrid.GridRowEventHandler(this.OnActivate);
            // 
            // gridColumnName
            // 
            this.gridColumnName.AllowReorder = false;
            this.gridColumnName.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnName.Clickable = false;
            this.gridColumnName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnName.HeaderText = "Name";
            this.gridColumnName.MinimumWidth = 50;
            this.gridColumnName.Width = 240;
            // 
            // gridColumnPostage
            // 
            this.gridColumnPostage.Clickable = false;
            this.gridColumnPostage.HeaderText = "Postage Balance";
            // 
            // add
            // 
            this.add.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.add.Image = global::ShipWorks.Properties.Resources.add16;
            this.add.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.add.Location = new System.Drawing.Point(353, 33);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(94, 23);
            this.add.TabIndex = 2;
            this.add.Text = "Add";
            this.add.UseVisualStyleBackColor = true;
            this.add.Click += new System.EventHandler(this.OnAddAccount);
            // 
            // remove
            // 
            this.remove.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.remove.Image = global::ShipWorks.Properties.Resources.delete16;
            this.remove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.remove.Location = new System.Drawing.Point(353, 62);
            this.remove.Name = "remove";
            this.remove.Size = new System.Drawing.Size(94, 23);
            this.remove.TabIndex = 3;
            this.remove.Text = "Remove";
            this.remove.UseVisualStyleBackColor = true;
            this.remove.Click += new System.EventHandler(this.OnRemove);
            // 
            // details
            // 
            this.details.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.details.Image = global::ShipWorks.Properties.Resources.edit16;
            this.details.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.details.Location = new System.Drawing.Point(353, 3);
            this.details.Name = "details";
            this.details.Size = new System.Drawing.Size(94, 23);
            this.details.TabIndex = 1;
            this.details.Text = "Edit";
            this.details.UseVisualStyleBackColor = true;
            this.details.Click += new System.EventHandler(this.OnEdit);
            // 
            // EndiciaAccountManagerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sandGrid);
            this.Controls.Add(this.add);
            this.Controls.Add(this.remove);
            this.Controls.Add(this.details);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "EndiciaAccountManagerControl";
            this.Size = new System.Drawing.Size(450, 160);
            this.ResumeLayout(false);

        }

        #endregion

        private Divelements.SandGrid.SandGrid sandGrid;
        private Divelements.SandGrid.GridColumn gridColumnName;
        private Divelements.SandGrid.GridColumn gridColumnPostage;
        private System.Windows.Forms.Button add;
        private System.Windows.Forms.Button remove;
        private System.Windows.Forms.Button details;
        private Editions.EditionGuiHelper editionGuiHelper;
    }
}
