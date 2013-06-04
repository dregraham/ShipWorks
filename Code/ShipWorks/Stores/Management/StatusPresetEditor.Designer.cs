namespace ShipWorks.Stores.Management
{
    partial class StatusPresetEditor
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Cracker Jack");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("Backordered");
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("Ready");
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("Shipped");
            this.defaultStatus = new System.Windows.Forms.ComboBox();
            this.labelDefaultStatusNote = new System.Windows.Forms.Label();
            this.labelDefaultStatus = new System.Windows.Forms.Label();
            this.listStatusStore = new System.Windows.Forms.ListView();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.labelThisStoreOnly = new System.Windows.Forms.Label();
            this.labelAllStores = new System.Windows.Forms.Label();
            this.listStatusGlobal = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.editStatusStore = new System.Windows.Forms.Button();
            this.deleteStatusStore = new System.Windows.Forms.Button();
            this.newStatusStore = new System.Windows.Forms.Button();
            this.editStatusGlobal = new System.Windows.Forms.Button();
            this.deleteStatusGlobal = new System.Windows.Forms.Button();
            this.newStatusGlobal = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // defaultStatus
            // 
            this.defaultStatus.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.defaultStatus.FormattingEnabled = true;
            this.defaultStatus.Location = new System.Drawing.Point(7, 292);
            this.defaultStatus.Name = "defaultStatus";
            this.defaultStatus.Size = new System.Drawing.Size(205, 21);
            this.defaultStatus.TabIndex = 11;
            // 
            // labelDefaultStatusNote
            // 
            this.labelDefaultStatusNote.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelDefaultStatusNote.ForeColor = System.Drawing.Color.DimGray;
            this.labelDefaultStatusNote.Location = new System.Drawing.Point(6, 316);
            this.labelDefaultStatusNote.Name = "labelDefaultStatusNote";
            this.labelDefaultStatusNote.Size = new System.Drawing.Size(409, 36);
            this.labelDefaultStatusNote.TabIndex = 12;
            this.labelDefaultStatusNote.Text = "This status is applied to orders the first time they are downloaded, or when they" +
                " are first manually created. Leave blank to have no default applied.";
            // 
            // labelDefaultStatus
            // 
            this.labelDefaultStatus.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelDefaultStatus.AutoSize = true;
            this.labelDefaultStatus.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelDefaultStatus.Location = new System.Drawing.Point(4, 274);
            this.labelDefaultStatus.Name = "labelDefaultStatus";
            this.labelDefaultStatus.Size = new System.Drawing.Size(88, 13);
            this.labelDefaultStatus.TabIndex = 10;
            this.labelDefaultStatus.Text = "Default Status";
            // 
            // listStatusStore
            // 
            this.listStatusStore.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.listStatusStore.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.listStatusStore.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listStatusStore.HideSelection = false;
            this.listStatusStore.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.listStatusStore.Location = new System.Drawing.Point(7, 178);
            this.listStatusStore.Name = "listStatusStore";
            this.listStatusStore.Size = new System.Drawing.Size(205, 82);
            this.listStatusStore.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listStatusStore.TabIndex = 6;
            this.listStatusStore.UseCompatibleStateImageBehavior = false;
            this.listStatusStore.View = System.Windows.Forms.View.Details;
            this.listStatusStore.ItemActivate += new System.EventHandler(this.OnEditPreset);
            this.listStatusStore.SelectedIndexChanged += new System.EventHandler(this.OnStoreSelectionChanged);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 180;
            // 
            // labelThisStoreOnly
            // 
            this.labelThisStoreOnly.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelThisStoreOnly.AutoSize = true;
            this.labelThisStoreOnly.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelThisStoreOnly.Location = new System.Drawing.Point(4, 162);
            this.labelThisStoreOnly.Name = "labelThisStoreOnly";
            this.labelThisStoreOnly.Size = new System.Drawing.Size(92, 13);
            this.labelThisStoreOnly.TabIndex = 5;
            this.labelThisStoreOnly.Text = "This Store Only";
            // 
            // labelAllStores
            // 
            this.labelAllStores.AutoSize = true;
            this.labelAllStores.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelAllStores.Location = new System.Drawing.Point(4, 6);
            this.labelAllStores.Name = "labelAllStores";
            this.labelAllStores.Size = new System.Drawing.Size(61, 13);
            this.labelAllStores.TabIndex = 0;
            this.labelAllStores.Text = "All Stores";
            // 
            // listStatusGlobal
            // 
            this.listStatusGlobal.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.listStatusGlobal.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listStatusGlobal.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listStatusGlobal.HideSelection = false;
            this.listStatusGlobal.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem2,
            listViewItem3,
            listViewItem4});
            this.listStatusGlobal.Location = new System.Drawing.Point(7, 22);
            this.listStatusGlobal.MultiSelect = false;
            this.listStatusGlobal.Name = "listStatusGlobal";
            this.listStatusGlobal.Size = new System.Drawing.Size(205, 130);
            this.listStatusGlobal.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listStatusGlobal.TabIndex = 1;
            this.listStatusGlobal.UseCompatibleStateImageBehavior = false;
            this.listStatusGlobal.View = System.Windows.Forms.View.Details;
            this.listStatusGlobal.ItemActivate += new System.EventHandler(this.OnEditPreset);
            this.listStatusGlobal.SelectedIndexChanged += new System.EventHandler(this.OnGlobalSelectionChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 180;
            // 
            // editStatusStore
            // 
            this.editStatusStore.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.editStatusStore.Image = global::ShipWorks.Properties.Resources.edit16;
            this.editStatusStore.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.editStatusStore.Location = new System.Drawing.Point(218, 208);
            this.editStatusStore.Name = "editStatusStore";
            this.editStatusStore.Size = new System.Drawing.Size(108, 23);
            this.editStatusStore.TabIndex = 8;
            this.editStatusStore.Text = "Edit";
            this.editStatusStore.UseVisualStyleBackColor = true;
            this.editStatusStore.Click += new System.EventHandler(this.OnEditPreset);
            // 
            // deleteStatusStore
            // 
            this.deleteStatusStore.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.deleteStatusStore.Image = global::ShipWorks.Properties.Resources.delete16;
            this.deleteStatusStore.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteStatusStore.Location = new System.Drawing.Point(218, 237);
            this.deleteStatusStore.Name = "deleteStatusStore";
            this.deleteStatusStore.Size = new System.Drawing.Size(108, 23);
            this.deleteStatusStore.TabIndex = 9;
            this.deleteStatusStore.Text = "Delete";
            this.deleteStatusStore.UseVisualStyleBackColor = true;
            this.deleteStatusStore.Click += new System.EventHandler(this.OnDeleteGlobalPreset);
            // 
            // newStatusStore
            // 
            this.newStatusStore.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.newStatusStore.Image = global::ShipWorks.Properties.Resources.rename;
            this.newStatusStore.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.newStatusStore.Location = new System.Drawing.Point(218, 179);
            this.newStatusStore.Name = "newStatusStore";
            this.newStatusStore.Size = new System.Drawing.Size(108, 23);
            this.newStatusStore.TabIndex = 7;
            this.newStatusStore.Text = "New";
            this.newStatusStore.UseVisualStyleBackColor = true;
            this.newStatusStore.Click += new System.EventHandler(this.OnNewPreset);
            // 
            // editStatusGlobal
            // 
            this.editStatusGlobal.Image = global::ShipWorks.Properties.Resources.edit16;
            this.editStatusGlobal.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.editStatusGlobal.Location = new System.Drawing.Point(218, 52);
            this.editStatusGlobal.Name = "editStatusGlobal";
            this.editStatusGlobal.Size = new System.Drawing.Size(108, 23);
            this.editStatusGlobal.TabIndex = 3;
            this.editStatusGlobal.Text = "Edit";
            this.editStatusGlobal.UseVisualStyleBackColor = true;
            this.editStatusGlobal.Click += new System.EventHandler(this.OnEditPreset);
            // 
            // deleteStatusGlobal
            // 
            this.deleteStatusGlobal.Image = global::ShipWorks.Properties.Resources.delete16;
            this.deleteStatusGlobal.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deleteStatusGlobal.Location = new System.Drawing.Point(218, 81);
            this.deleteStatusGlobal.Name = "deleteStatusGlobal";
            this.deleteStatusGlobal.Size = new System.Drawing.Size(108, 23);
            this.deleteStatusGlobal.TabIndex = 4;
            this.deleteStatusGlobal.Text = "Delete";
            this.deleteStatusGlobal.UseVisualStyleBackColor = true;
            this.deleteStatusGlobal.Click += new System.EventHandler(this.OnDeleteGlobalPreset);
            // 
            // newStatusGlobal
            // 
            this.newStatusGlobal.Image = global::ShipWorks.Properties.Resources.rename;
            this.newStatusGlobal.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.newStatusGlobal.Location = new System.Drawing.Point(218, 23);
            this.newStatusGlobal.Name = "newStatusGlobal";
            this.newStatusGlobal.Size = new System.Drawing.Size(108, 23);
            this.newStatusGlobal.TabIndex = 2;
            this.newStatusGlobal.Text = "New";
            this.newStatusGlobal.UseVisualStyleBackColor = true;
            this.newStatusGlobal.Click += new System.EventHandler(this.OnNewPreset);
            // 
            // StatusPresetEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.defaultStatus);
            this.Controls.Add(this.labelDefaultStatusNote);
            this.Controls.Add(this.labelDefaultStatus);
            this.Controls.Add(this.editStatusStore);
            this.Controls.Add(this.deleteStatusStore);
            this.Controls.Add(this.newStatusStore);
            this.Controls.Add(this.listStatusStore);
            this.Controls.Add(this.labelThisStoreOnly);
            this.Controls.Add(this.editStatusGlobal);
            this.Controls.Add(this.deleteStatusGlobal);
            this.Controls.Add(this.labelAllStores);
            this.Controls.Add(this.newStatusGlobal);
            this.Controls.Add(this.listStatusGlobal);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "StatusPresetEditor";
            this.Size = new System.Drawing.Size(407, 356);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox defaultStatus;
        private System.Windows.Forms.Label labelDefaultStatusNote;
        private System.Windows.Forms.Label labelDefaultStatus;
        private System.Windows.Forms.Button editStatusStore;
        private System.Windows.Forms.Button deleteStatusStore;
        private System.Windows.Forms.Button newStatusStore;
        private System.Windows.Forms.ListView listStatusStore;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label labelThisStoreOnly;
        private System.Windows.Forms.Button editStatusGlobal;
        private System.Windows.Forms.Button deleteStatusGlobal;
        private System.Windows.Forms.Label labelAllStores;
        private System.Windows.Forms.Button newStatusGlobal;
        private System.Windows.Forms.ListView listStatusGlobal;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}
