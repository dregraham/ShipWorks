namespace ShipWorks.UI.Controls
{
    partial class TimeZoneSelection
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
            this.timeZone = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // timeZone
            // 
            this.timeZone.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.timeZone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.timeZone.FormattingEnabled = true;
            this.timeZone.Location = new System.Drawing.Point(24, 37);
            this.timeZone.Name = "timeZone";
            this.timeZone.Size = new System.Drawing.Size(442, 21);
            this.timeZone.TabIndex = 26;
            // 
            // TimeZoneSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.timeZone);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "TimeZoneSelection";
            this.Size = new System.Drawing.Size(469, 67);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.ComboBox timeZone;


    }
}
