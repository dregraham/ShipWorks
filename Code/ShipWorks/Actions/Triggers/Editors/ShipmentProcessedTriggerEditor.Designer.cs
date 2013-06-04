namespace ShipWorks.Actions.Triggers.Editors
{
    partial class ShipmentProcessedTriggerEditor
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
            this.restrictType = new System.Windows.Forms.CheckBox();
            this.shipmentType = new System.Windows.Forms.ComboBox();
            this.restrictReturns = new System.Windows.Forms.CheckBox();
            this.standardReturnType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // restrictType
            // 
            this.restrictType.AutoSize = true;
            this.restrictType.Location = new System.Drawing.Point(3, 3);
            this.restrictType.Name = "restrictType";
            this.restrictType.Size = new System.Drawing.Size(105, 17);
            this.restrictType.TabIndex = 1;
            this.restrictType.Text = "Only when using";
            this.restrictType.UseVisualStyleBackColor = true;
            // 
            // shipmentType
            // 
            this.shipmentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.shipmentType.FormattingEnabled = true;
            this.shipmentType.Location = new System.Drawing.Point(106, 1);
            this.shipmentType.Name = "shipmentType";
            this.shipmentType.Size = new System.Drawing.Size(185, 21);
            this.shipmentType.TabIndex = 2;
            // 
            // restrictReturns
            // 
            this.restrictReturns.AutoSize = true;
            this.restrictReturns.Location = new System.Drawing.Point(3, 30);
            this.restrictReturns.Name = "restrictReturns";
            this.restrictReturns.Size = new System.Drawing.Size(65, 17);
            this.restrictReturns.TabIndex = 3;
            this.restrictReturns.Text = "Only for";
            this.restrictReturns.UseVisualStyleBackColor = true;
            // 
            // standardReturnType
            // 
            this.standardReturnType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.standardReturnType.FormattingEnabled = true;
            this.standardReturnType.Items.AddRange(new object[] {
            "Normal shipments",
            "Return shipments"});
            this.standardReturnType.Location = new System.Drawing.Point(69, 28);
            this.standardReturnType.Name = "standardReturnType";
            this.standardReturnType.Size = new System.Drawing.Size(149, 21);
            this.standardReturnType.TabIndex = 4;
            // 
            // ShipmentProcessedTriggerEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.standardReturnType);
            this.Controls.Add(this.restrictReturns);
            this.Controls.Add(this.shipmentType);
            this.Controls.Add(this.restrictType);
            this.Name = "ShipmentProcessedTriggerEditor";
            this.Size = new System.Drawing.Size(305, 54);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox restrictType;
        private System.Windows.Forms.ComboBox shipmentType;
        private System.Windows.Forms.CheckBox restrictReturns;
        private System.Windows.Forms.ComboBox standardReturnType;

    }
}
