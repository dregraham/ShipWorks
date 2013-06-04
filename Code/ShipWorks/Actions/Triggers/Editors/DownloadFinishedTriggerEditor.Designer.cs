namespace ShipWorks.Actions.Triggers.Editors
{
    partial class DownloadFinishedTriggerEditor
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
            this.restrictResult = new System.Windows.Forms.CheckBox();
            this.restrictResultCombo = new System.Windows.Forms.ComboBox();
            this.onlyNewOrders = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // restrictResult
            // 
            this.restrictResult.AutoSize = true;
            this.restrictResult.Location = new System.Drawing.Point(3, 3);
            this.restrictResult.Name = "restrictResult";
            this.restrictResult.Size = new System.Drawing.Size(116, 17);
            this.restrictResult.TabIndex = 0;
            this.restrictResult.Text = "Only if the result is";
            this.restrictResult.UseVisualStyleBackColor = true;
            // 
            // restrictResultCombo
            // 
            this.restrictResultCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.restrictResultCombo.FormattingEnabled = true;
            this.restrictResultCombo.Location = new System.Drawing.Point(116, 1);
            this.restrictResultCombo.Name = "restrictResultCombo";
            this.restrictResultCombo.Size = new System.Drawing.Size(131, 21);
            this.restrictResultCombo.TabIndex = 1;
            // 
            // onlyNewOrders
            // 
            this.onlyNewOrders.AutoSize = true;
            this.onlyNewOrders.Location = new System.Drawing.Point(3, 26);
            this.onlyNewOrders.Name = "onlyNewOrders";
            this.onlyNewOrders.Size = new System.Drawing.Size(206, 17);
            this.onlyNewOrders.TabIndex = 2;
            this.onlyNewOrders.Text = "Only if new orders were downloaded.";
            this.onlyNewOrders.UseVisualStyleBackColor = true;
            // 
            // DownloadFinishedTriggerEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.onlyNewOrders);
            this.Controls.Add(this.restrictResultCombo);
            this.Controls.Add(this.restrictResult);
            this.Name = "DownloadFinishedTriggerEditor";
            this.Size = new System.Drawing.Size(312, 51);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox restrictResult;
        private System.Windows.Forms.ComboBox restrictResultCombo;
        private System.Windows.Forms.CheckBox onlyNewOrders;
    }
}
