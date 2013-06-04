namespace ShipWorks.Actions.Triggers.Editors
{
    partial class OrderDownloadedTriggerEditor
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
            this.downloadRestriction = new System.Windows.Forms.CheckBox();
            this.downloadRestrictionCombo = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // downloadRestriction
            // 
            this.downloadRestriction.AutoSize = true;
            this.downloadRestriction.Location = new System.Drawing.Point(3, 3);
            this.downloadRestriction.Name = "downloadRestriction";
            this.downloadRestriction.Size = new System.Drawing.Size(105, 17);
            this.downloadRestriction.TabIndex = 0;
            this.downloadRestriction.Text = "Only if the order";
            this.downloadRestriction.UseVisualStyleBackColor = true;
            // 
            // downloadRestrictionCombo
            // 
            this.downloadRestrictionCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.downloadRestrictionCombo.FormattingEnabled = true;
            this.downloadRestrictionCombo.Items.AddRange(new object[] {
            "was downloaded for the first time",
            "has been downloaded before"});
            this.downloadRestrictionCombo.Location = new System.Drawing.Point(105, 1);
            this.downloadRestrictionCombo.Name = "downloadRestrictionCombo";
            this.downloadRestrictionCombo.Size = new System.Drawing.Size(190, 21);
            this.downloadRestrictionCombo.TabIndex = 1;
            // 
            // OrderDownloadedTriggerEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.downloadRestrictionCombo);
            this.Controls.Add(this.downloadRestriction);
            this.Name = "OrderDownloadedTriggerEditor";
            this.Size = new System.Drawing.Size(381, 30);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox downloadRestriction;
        private System.Windows.Forms.ComboBox downloadRestrictionCombo;
    }
}
