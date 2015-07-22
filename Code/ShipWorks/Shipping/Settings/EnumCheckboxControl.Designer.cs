namespace ShipWorks.Shipping.Settings
{
    partial class EnumCheckBoxControl<T>
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
            this.selectedEnums = new System.Windows.Forms.CheckedListBox();
            this.description = new System.Windows.Forms.Label();
            this.title = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // selectedServices
            // 
            this.selectedEnums.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectedEnums.CheckOnClick = true;
            this.selectedEnums.FormattingEnabled = true;
            this.selectedEnums.Items.AddRange(new object[] {
            "Priority Mail",
            "Priority Mail Express",
            "First Class",
            "Library Mail",
            "Media Mail",
            "International Priority",
            "International First",
            "International Express"});
            this.selectedEnums.Location = new System.Drawing.Point(18, 49);
            this.selectedEnums.Name = "selectedServices";
            this.selectedEnums.Size = new System.Drawing.Size(358, 164);
            this.selectedEnums.TabIndex = 0;
            // 
            // label1
            // 
            this.description.Location = new System.Drawing.Point(15, 17);
            this.description.Name = "label1";
            this.description.Size = new System.Drawing.Size(343, 27);
            this.description.TabIndex = 2;
            this.description.Text = "INSERT DESCRIPTION HERE";
            // 
            // label2
            // 
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title.Location = new System.Drawing.Point(0, 0);
            this.title.Name = "label2";
            this.title.Size = new System.Drawing.Size(110, 13);
            this.title.TabIndex = 3;
            this.title.Text = "Available";
            // 
            // EnumCheckboxControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.title);
            this.Controls.Add(this.description);
            this.Controls.Add(this.selectedEnums);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "EnumCheckboxControl";
            this.Size = new System.Drawing.Size(379, 215);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.CheckedListBox selectedEnums;
        private System.Windows.Forms.Label description;
        private System.Windows.Forms.Label title;
    }
}
