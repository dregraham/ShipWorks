namespace ShipWorks.Actions.Tasks.Common.Editors
{
    partial class HitUrlTaskEditor
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
            this.verbLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // verbLabel
            // 
            this.verbLabel.AutoSize = true;
            this.verbLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.verbLabel.ForeColor = System.Drawing.Color.Blue;
            this.verbLabel.Location = new System.Drawing.Point(37, 50);
            this.verbLabel.Name = "verbLabel";
            this.verbLabel.Size = new System.Drawing.Size(26, 13);
            this.verbLabel.TabIndex = 0;
            this.verbLabel.Text = "GET";
            this.verbLabel.Click += new System.EventHandler(this.OnClickVerbLabel);
            // 
            // HitUrlTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.verbLabel);
            this.Name = "HitUrlTaskEditor";
            this.Size = new System.Drawing.Size(413, 235);
            this.Controls.SetChildIndex(this.verbLabel, 0);
            this.Controls.SetChildIndex(this.labelTemplate, 0);
            this.Controls.SetChildIndex(this.templateCombo, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label verbLabel;
    }
}
