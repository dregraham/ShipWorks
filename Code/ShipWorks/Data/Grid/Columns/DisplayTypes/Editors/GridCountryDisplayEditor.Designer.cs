namespace ShipWorks.Data.Grid.Columns.DisplayTypes.Editors
{
    partial class GridCountryDisplayEditor
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
            this.showFlag = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Format:";
            // 
            // formatList
            // 
            this.formatList.FormattingEnabled = true;
            this.formatList.Location = new System.Drawing.Point(6, 53);
            this.formatList.Name = "formatList";
            this.formatList.Size = new System.Drawing.Size(211, 56);
            this.formatList.TabIndex = 5;
            // 
            // showFlag
            // 
            this.showFlag.AutoSize = true;
            this.showFlag.Location = new System.Drawing.Point(6, 116);
            this.showFlag.Name = "showFlag";
            this.showFlag.Size = new System.Drawing.Size(113, 17);
            this.showFlag.TabIndex = 6;
            this.showFlag.Text = "Show country flag";
            this.showFlag.UseVisualStyleBackColor = true;
            // 
            // GridCountryDisplayEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.showFlag);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.formatList);
            this.Name = "GridCountryDisplayEditor";
            this.Size = new System.Drawing.Size(220, 139);
            this.Controls.SetChildIndex(this.formatList, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.showFlag, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox formatList;
        private System.Windows.Forms.CheckBox showFlag;
    }
}
