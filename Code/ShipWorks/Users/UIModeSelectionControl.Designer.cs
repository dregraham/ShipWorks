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
            this.uiModeInstructions = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // orderLookup
            // 
            this.orderLookup.AutoSize = true;
            this.orderLookup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.orderLookup.Location = new System.Drawing.Point(13, 16);
            this.orderLookup.Name = "orderLookup";
            this.orderLookup.Size = new System.Drawing.Size(202, 17);
            this.orderLookup.TabIndex = 0;
            this.orderLookup.TabStop = true;
            this.orderLookup.Text = "Lookup and ship orders one at a time";
            this.orderLookup.UseVisualStyleBackColor = true;
            // 
            // batch
            // 
            this.batch.AutoSize = true;
            this.batch.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.batch.Location = new System.Drawing.Point(13, 39);
            this.batch.Name = "batch";
            this.batch.Size = new System.Drawing.Size(172, 17);
            this.batch.TabIndex = 1;
            this.batch.TabStop = true;
            this.batch.Text = "Edit and ship orders in batches";
            this.batch.UseVisualStyleBackColor = true;
            // 
            // uiModeInstructions
            // 
            this.uiModeInstructions.AutoSize = true;
            this.uiModeInstructions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiModeInstructions.Location = new System.Drawing.Point(0, 0);
            this.uiModeInstructions.Name = "uiModeInstructions";
            this.uiModeInstructions.Size = new System.Drawing.Size(157, 13);
            this.uiModeInstructions.TabIndex = 2;
            this.uiModeInstructions.Text = "How do you prefer to ship?";
            // 
            // UIModeSelectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.uiModeInstructions);
            this.Controls.Add(this.batch);
            this.Controls.Add(this.orderLookup);
            this.Name = "UIModeSelectionControl";
            this.Size = new System.Drawing.Size(235, 81);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton orderLookup;
        private System.Windows.Forms.RadioButton batch;
        private System.Windows.Forms.Label uiModeInstructions;
    }
}
