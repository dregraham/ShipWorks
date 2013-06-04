namespace ShipWorks.Data.Grid.Columns.DisplayTypes.Editors
{
    partial class GridMoneyDisplayEditor
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
            this.showCurrency = new System.Windows.Forms.CheckBox();
            this.showThousands = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // showCurrency
            // 
            this.showCurrency.AutoSize = true;
            this.showCurrency.Location = new System.Drawing.Point(6, 36);
            this.showCurrency.Name = "showCurrency";
            this.showCurrency.Size = new System.Drawing.Size(133, 17);
            this.showCurrency.TabIndex = 4;
            this.showCurrency.Text = "Show currency symbol";
            this.showCurrency.UseVisualStyleBackColor = true;
            // 
            // showThousands
            // 
            this.showThousands.AutoSize = true;
            this.showThousands.Location = new System.Drawing.Point(6, 59);
            this.showThousands.Name = "showThousands";
            this.showThousands.Size = new System.Drawing.Size(155, 17);
            this.showThousands.TabIndex = 5;
            this.showThousands.Text = "Show thousands separator";
            this.showThousands.UseVisualStyleBackColor = true;
            // 
            // GridMoneyDisplayEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.showThousands);
            this.Controls.Add(this.showCurrency);
            this.Name = "GridMoneyDisplayEditor";
            this.Controls.SetChildIndex(this.showCurrency, 0);
            this.Controls.SetChildIndex(this.showThousands, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox showCurrency;
        private System.Windows.Forms.CheckBox showThousands;
    }
}
