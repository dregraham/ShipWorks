namespace ShipWorks.Data.Import.Spreadsheet.Editing
{
    partial class GenericSpreadsheetMapChooserControl
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
            this.labelConfigurationInfo = new System.Windows.Forms.Label();
            this.labelConfiguration = new System.Windows.Forms.Label();
            this.labelMapName = new System.Windows.Forms.Label();
            this.mapName = new System.Windows.Forms.TextBox();
            this.newMap1 = new System.Windows.Forms.Button();
            this.panelExists = new System.Windows.Forms.Panel();
            this.edit = new System.Windows.Forms.Button();
            this.linkLoadMap = new ShipWorks.UI.Controls.LinkControl();
            this.linkSaveMap = new ShipWorks.UI.Controls.LinkControl();
            this.panelNone = new System.Windows.Forms.Panel();
            this.loadMap = new System.Windows.Forms.Button();
            this.newMap2 = new System.Windows.Forms.Button();
            this.panelExists.SuspendLayout();
            this.panelNone.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelConfigurationInfo
            // 
            this.labelConfigurationInfo.Location = new System.Drawing.Point(26, 35);
            this.labelConfigurationInfo.Name = "labelConfigurationInfo";
            this.labelConfigurationInfo.Size = new System.Drawing.Size(464, 22);
            this.labelConfigurationInfo.TabIndex = 3;
            this.labelConfigurationInfo.Text = "ShipWorks uses a map to control how data is loaded from your imported files into " +
                "ShipWorks.";
            // 
            // labelConfiguration
            // 
            this.labelConfiguration.AutoSize = true;
            this.labelConfiguration.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelConfiguration.Location = new System.Drawing.Point(14, 13);
            this.labelConfiguration.Name = "labelConfiguration";
            this.labelConfiguration.Size = new System.Drawing.Size(74, 13);
            this.labelConfiguration.TabIndex = 2;
            this.labelConfiguration.Text = "Import Map";
            // 
            // labelMapName
            // 
            this.labelMapName.AutoSize = true;
            this.labelMapName.Location = new System.Drawing.Point(6, 9);
            this.labelMapName.Name = "labelMapName";
            this.labelMapName.Size = new System.Drawing.Size(60, 13);
            this.labelMapName.TabIndex = 4;
            this.labelMapName.Text = "Map name:";
            // 
            // mapName
            // 
            this.mapName.Location = new System.Drawing.Point(72, 6);
            this.mapName.Name = "mapName";
            this.mapName.ReadOnly = true;
            this.mapName.Size = new System.Drawing.Size(242, 21);
            this.mapName.TabIndex = 5;
            // 
            // newMap1
            // 
            this.newMap1.Location = new System.Drawing.Point(401, 6);
            this.newMap1.Name = "newMap1";
            this.newMap1.Size = new System.Drawing.Size(75, 23);
            this.newMap1.TabIndex = 7;
            this.newMap1.Text = "New Map...";
            this.newMap1.UseVisualStyleBackColor = true;
            this.newMap1.Click += new System.EventHandler(this.OnCreateMap);
            // 
            // panelExists
            // 
            this.panelExists.Controls.Add(this.edit);
            this.panelExists.Controls.Add(this.labelMapName);
            this.panelExists.Controls.Add(this.mapName);
            this.panelExists.Controls.Add(this.linkLoadMap);
            this.panelExists.Controls.Add(this.newMap1);
            this.panelExists.Controls.Add(this.linkSaveMap);
            this.panelExists.Location = new System.Drawing.Point(30, 55);
            this.panelExists.Name = "panelExists";
            this.panelExists.Size = new System.Drawing.Size(540, 61);
            this.panelExists.TabIndex = 19;
            // 
            // edit
            // 
            this.edit.Location = new System.Drawing.Point(320, 6);
            this.edit.Name = "edit";
            this.edit.Size = new System.Drawing.Size(75, 23);
            this.edit.TabIndex = 19;
            this.edit.Text = "Edit Map...";
            this.edit.UseVisualStyleBackColor = true;
            this.edit.Click += new System.EventHandler(this.OnEditMap);
            // 
            // linkLoadMap
            // 
            this.linkLoadMap.AutoSize = true;
            this.linkLoadMap.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkLoadMap.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkLoadMap.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.linkLoadMap.Location = new System.Drawing.Point(249, 33);
            this.linkLoadMap.Name = "linkLoadMap";
            this.linkLoadMap.Size = new System.Drawing.Size(65, 13);
            this.linkLoadMap.TabIndex = 18;
            this.linkLoadMap.Text = "Load map...";
            this.linkLoadMap.Click += new System.EventHandler(this.OnLoadMap);
            // 
            // linkSaveMap
            // 
            this.linkSaveMap.AutoSize = true;
            this.linkSaveMap.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkSaveMap.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkSaveMap.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.linkSaveMap.Location = new System.Drawing.Point(179, 33);
            this.linkSaveMap.Name = "linkSaveMap";
            this.linkSaveMap.Size = new System.Drawing.Size(66, 13);
            this.linkSaveMap.TabIndex = 16;
            this.linkSaveMap.Text = "Save map...";
            this.linkSaveMap.Click += new System.EventHandler(this.OnSaveMap);
            // 
            // panelNone
            // 
            this.panelNone.Controls.Add(this.loadMap);
            this.panelNone.Controls.Add(this.newMap2);
            this.panelNone.Location = new System.Drawing.Point(30, 131);
            this.panelNone.Name = "panelNone";
            this.panelNone.Size = new System.Drawing.Size(540, 77);
            this.panelNone.TabIndex = 20;
            // 
            // loadMap
            // 
            this.loadMap.Location = new System.Drawing.Point(90, 5);
            this.loadMap.Name = "loadMap";
            this.loadMap.Size = new System.Drawing.Size(75, 23);
            this.loadMap.TabIndex = 9;
            this.loadMap.Text = "Load Map...";
            this.loadMap.UseVisualStyleBackColor = true;
            this.loadMap.Click += new System.EventHandler(this.OnLoadMap);
            // 
            // newMap2
            // 
            this.newMap2.Location = new System.Drawing.Point(9, 5);
            this.newMap2.Name = "newMap2";
            this.newMap2.Size = new System.Drawing.Size(75, 23);
            this.newMap2.TabIndex = 8;
            this.newMap2.Text = "New Map...";
            this.newMap2.UseVisualStyleBackColor = true;
            this.newMap2.Click += new System.EventHandler(this.OnCreateMap);
            // 
            // GenericSpreadsheetMapChooserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelNone);
            this.Controls.Add(this.panelExists);
            this.Controls.Add(this.labelConfigurationInfo);
            this.Controls.Add(this.labelConfiguration);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "GenericSpreadsheetMapChooserControl";
            this.Size = new System.Drawing.Size(602, 277);
            this.panelExists.ResumeLayout(false);
            this.panelExists.PerformLayout();
            this.panelNone.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelConfigurationInfo;
        private System.Windows.Forms.Label labelMapName;
        private System.Windows.Forms.TextBox mapName;
        private System.Windows.Forms.Button newMap1;
        private UI.Controls.LinkControl linkLoadMap;
        private UI.Controls.LinkControl linkSaveMap;
        private System.Windows.Forms.Panel panelExists;
        private System.Windows.Forms.Panel panelNone;
        private System.Windows.Forms.Button loadMap;
        private System.Windows.Forms.Button newMap2;
        private System.Windows.Forms.Button edit;
        protected System.Windows.Forms.Label labelConfiguration;
    }
}
