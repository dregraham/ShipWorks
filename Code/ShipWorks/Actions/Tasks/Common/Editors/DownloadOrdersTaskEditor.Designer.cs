namespace ShipWorks.Actions.Tasks.Common.Editors
{
    partial class DownloadOrdersTaskEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DownloadOrdersTaskEditor));
            this.taskDescription = new System.Windows.Forms.Label();
            this.storeCheckBoxPanel = new ShipWorks.Actions.UI.StoreCheckBoxPanel();
            this.SuspendLayout();
            // 
            // taskDescription
            // 
            this.taskDescription.AutoSize = true;
            this.taskDescription.Location = new System.Drawing.Point(-3, 0);
            this.taskDescription.Name = "taskDescription";
            this.taskDescription.Size = new System.Drawing.Size(172, 13);
            this.taskDescription.TabIndex = 0;
            this.taskDescription.Text = "Download orders for these stores:";
            // 
            // storeCheckBoxPanel
            // 
            this.storeCheckBoxPanel.Location = new System.Drawing.Point(14, 17);
            this.storeCheckBoxPanel.Name = "storeCheckBoxPanel";
            this.storeCheckBoxPanel.SelectedStores = ((System.Collections.Generic.IEnumerable<ShipWorks.Data.Model.EntityClasses.StoreEntity>)(resources.GetObject("storeCheckBoxPanel.SelectedStores")));
            this.storeCheckBoxPanel.Size = new System.Drawing.Size(335, 57);
            this.storeCheckBoxPanel.TabIndex = 1;
            // 
            // DownloadOrdersTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.storeCheckBoxPanel);
            this.Controls.Add(this.taskDescription);
            this.Name = "DownloadOrdersTaskEditor";
            this.Size = new System.Drawing.Size(362, 75);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label taskDescription;
        private UI.StoreCheckBoxPanel storeCheckBoxPanel;
    }
}
