namespace ShipWorks.Filters.Controls
{
    partial class FilterConditionControl
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
            this.panelFolderCondition = new System.Windows.Forms.Panel();
            this.useFolderCondition = new System.Windows.Forms.CheckBox();
            this.conditionEditor = new ShipWorks.Filters.Controls.FilterDefinitionEditor();
            this.infoTip1 = new ShipWorks.UI.Controls.InfoTip();
            this.panelFolderCondition.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelFolderCondition
            // 
            this.panelFolderCondition.Controls.Add(this.infoTip1);
            this.panelFolderCondition.Controls.Add(this.useFolderCondition);
            this.panelFolderCondition.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFolderCondition.Location = new System.Drawing.Point(0, 0);
            this.panelFolderCondition.Name = "panelFolderCondition";
            this.panelFolderCondition.Size = new System.Drawing.Size(609, 27);
            this.panelFolderCondition.TabIndex = 0;
            // 
            // useFolderCondition
            // 
            this.useFolderCondition.AutoSize = true;
            this.useFolderCondition.Checked = true;
            this.useFolderCondition.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useFolderCondition.Location = new System.Drawing.Point(11, 5);
            this.useFolderCondition.Name = "useFolderCondition";
            this.useFolderCondition.Size = new System.Drawing.Size(281, 17);
            this.useFolderCondition.TabIndex = 1;
            this.useFolderCondition.Text = "Use a condition to restrict the contents of this folder.";
            this.useFolderCondition.UseVisualStyleBackColor = true;
            this.useFolderCondition.CheckedChanged += new System.EventHandler(this.OnChangeUseFolderCondition);
            // 
            // conditionEditor
            // 
            this.conditionEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.conditionEditor.Location = new System.Drawing.Point(0, 27);
            this.conditionEditor.Name = "conditionEditor";
            this.conditionEditor.Size = new System.Drawing.Size(609, 407);
            this.conditionEditor.TabIndex = 1;
            // 
            // infoTip1
            // 
            this.infoTip1.Caption = "Folders can have a condition that restricts the items that will be in them.  Any " +
                "filters in this folder will show only items that match both the filter condition" +
                " and the folder condition. ";
            this.infoTip1.Location = new System.Drawing.Point(289, 6);
            this.infoTip1.Name = "infoTip1";
            this.infoTip1.Size = new System.Drawing.Size(12, 12);
            this.infoTip1.TabIndex = 21;
            this.infoTip1.Title = "Folder Condition";
            // 
            // FilterConditionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.conditionEditor);
            this.Controls.Add(this.panelFolderCondition);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "FilterConditionControl";
            this.Size = new System.Drawing.Size(609, 434);
            this.panelFolderCondition.ResumeLayout(false);
            this.panelFolderCondition.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.Filters.Controls.FilterDefinitionEditor conditionEditor;
        private System.Windows.Forms.Panel panelFolderCondition;
        private System.Windows.Forms.CheckBox useFolderCondition;
        private UI.Controls.InfoTip infoTip1;
    }
}
