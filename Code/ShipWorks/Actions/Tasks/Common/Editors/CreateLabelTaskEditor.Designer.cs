namespace ShipWorks.Actions.Tasks.Common.Editors
{
    partial class CreateLabelTaskEditor
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
            this.allowMultiShipments = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // allowMultiShipments
            // 
            this.allowMultiShipments.AutoSize = true;
            this.allowMultiShipments.Location = new System.Drawing.Point(3, 7);
            this.allowMultiShipments.Name = "allowMultiShipments";
            this.allowMultiShipments.Size = new System.Drawing.Size(356, 17);
            this.allowMultiShipments.TabIndex = 0;
            this.allowMultiShipments.Text = "Create multiple labels for orders with multiple unprocessed shipments";
            // 
            // CreateLabelTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.allowMultiShipments);
            this.Name = "CreateLabelTaskEditor";
            this.Size = new System.Drawing.Size(370, 31);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox allowMultiShipments;
    }
}
