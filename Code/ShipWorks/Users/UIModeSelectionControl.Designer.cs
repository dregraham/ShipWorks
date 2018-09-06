namespace ShipWorks.Users
{
    partial class UIModeSelectionControl
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
            this.orderLookup = new System.Windows.Forms.RadioButton();
            this.batch = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            
            // 
            // batch
            // 
            this.batch.AutoSize = true;
            this.batch.Checked = true;
            this.batch.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.batch.Location = new System.Drawing.Point(13, 3);
            this.batch.Name = "batch";
            this.batch.Size = new System.Drawing.Size(172, 17);
            this.batch.TabIndex = 0;
            this.batch.TabStop = true;
            this.batch.Text = "Edit and ship orders in batches";
            this.batch.UseVisualStyleBackColor = true;
            // 
            // orderLookup
            // 
            this.orderLookup.AutoSize = true;
            this.orderLookup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.orderLookup.Location = new System.Drawing.Point(13, 26);
            this.orderLookup.Name = "orderLookup";
            this.orderLookup.Size = new System.Drawing.Size(202, 17);
            this.orderLookup.TabIndex = 1;
            this.orderLookup.TabStop = true;
            this.orderLookup.Text = "Lookup and ship orders one at a time";
            this.orderLookup.UseVisualStyleBackColor = true;
            // 
            // UIModeSelectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.batch);
            this.Controls.Add(this.orderLookup);
            this.Name = "UIModeSelectionControl";
            this.Size = new System.Drawing.Size(235, 48);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton orderLookup;
        private System.Windows.Forms.RadioButton batch;
    }
}
