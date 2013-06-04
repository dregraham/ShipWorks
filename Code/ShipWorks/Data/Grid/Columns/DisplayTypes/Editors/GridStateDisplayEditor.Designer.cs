namespace ShipWorks.Data.Grid.Columns.DisplayTypes.Editors
{
    partial class GridStateDisplayEditor
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
            this.label2 = new System.Windows.Forms.Label();
            this.formatList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Format:";
            // 
            // formatList
            // 
            this.formatList.FormattingEnabled = true;
            this.formatList.Items.AddRange(new object[] {
            "Missouri",
            "Missouri (MO)",
            "MO",
            "MO (Missouri)"});
            this.formatList.Location = new System.Drawing.Point(6, 53);
            this.formatList.Name = "formatList";
            this.formatList.Size = new System.Drawing.Size(211, 56);
            this.formatList.TabIndex = 8;
            // 
            // GridStateDisplayEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.formatList);
            this.Name = "GridStateDisplayEditor";
            this.Size = new System.Drawing.Size(220, 117);
            this.Controls.SetChildIndex(this.formatList, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox formatList;
    }
}
