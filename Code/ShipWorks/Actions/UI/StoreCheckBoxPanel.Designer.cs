namespace ShipWorks.Actions.UI
{
    partial class StoreCheckBoxPanel
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
            this.panelStores = new System.Windows.Forms.Panel();
            this.checkBoxSample1 = new System.Windows.Forms.CheckBox();
            this.checkBoxSample2 = new System.Windows.Forms.CheckBox();
            this.panelStores.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelStores
            // 
            this.panelStores.Controls.Add(this.checkBoxSample1);
            this.panelStores.Controls.Add(this.checkBoxSample2);
            this.panelStores.Location = new System.Drawing.Point(0, 0);
            this.panelStores.Name = "panelStores";
            this.panelStores.Size = new System.Drawing.Size(488, 45);
            this.panelStores.TabIndex = 4;
            // 
            // checkBoxSample1
            // 
            this.checkBoxSample1.AutoSize = true;
            this.checkBoxSample1.Location = new System.Drawing.Point(3, 3);
            this.checkBoxSample1.Name = "checkBoxSample1";
            this.checkBoxSample1.Size = new System.Drawing.Size(82, 17);
            this.checkBoxSample1.TabIndex = 2;
            this.checkBoxSample1.Text = "ElectroGear";
            this.checkBoxSample1.UseVisualStyleBackColor = true;
            // 
            // checkBoxSample2
            // 
            this.checkBoxSample2.AutoSize = true;
            this.checkBoxSample2.Location = new System.Drawing.Point(3, 23);
            this.checkBoxSample2.Name = "checkBoxSample2";
            this.checkBoxSample2.Size = new System.Drawing.Size(135, 17);
            this.checkBoxSample2.TabIndex = 1;
            this.checkBoxSample2.Text = "Liznobber Incorporated";
            this.checkBoxSample2.UseVisualStyleBackColor = true;
            // 
            // StoreCheckBoxPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelStores);
            this.Name = "StoreCheckBoxPanel";
            this.Size = new System.Drawing.Size(496, 53);
            this.panelStores.ResumeLayout(false);
            this.panelStores.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelStores;
        private System.Windows.Forms.CheckBox checkBoxSample1;
        private System.Windows.Forms.CheckBox checkBoxSample2;
    }
}
