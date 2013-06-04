namespace ShipWorks.Data.Grid.Columns.DisplayTypes.Editors
{
    partial class GridEmailDisplayEditor
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
            this.labelDisplay = new System.Windows.Forms.Label();
            this.nameOnly = new System.Windows.Forms.RadioButton();
            this.addressOnly = new System.Windows.Forms.RadioButton();
            this.nameAndAddress = new System.Windows.Forms.RadioButton();
            this.labeNameOnly = new System.Windows.Forms.Label();
            this.labelNameAndAddress = new System.Windows.Forms.Label();
            this.labelAddressOnly = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelDisplay
            // 
            this.labelDisplay.AutoSize = true;
            this.labelDisplay.Location = new System.Drawing.Point(3, 31);
            this.labelDisplay.Name = "labelDisplay";
            this.labelDisplay.Size = new System.Drawing.Size(45, 13);
            this.labelDisplay.TabIndex = 2;
            this.labelDisplay.Text = "Display:";
            // 
            // nameOnly
            // 
            this.nameOnly.AutoSize = true;
            this.nameOnly.Location = new System.Drawing.Point(19, 48);
            this.nameOnly.Name = "nameOnly";
            this.nameOnly.Size = new System.Drawing.Size(75, 17);
            this.nameOnly.TabIndex = 3;
            this.nameOnly.TabStop = true;
            this.nameOnly.Text = "Name only";
            this.nameOnly.UseVisualStyleBackColor = true;
            this.nameOnly.CheckedChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // addressOnly
            // 
            this.addressOnly.AutoSize = true;
            this.addressOnly.Location = new System.Drawing.Point(19, 110);
            this.addressOnly.Name = "addressOnly";
            this.addressOnly.Size = new System.Drawing.Size(87, 17);
            this.addressOnly.TabIndex = 7;
            this.addressOnly.TabStop = true;
            this.addressOnly.Text = "Address only";
            this.addressOnly.UseVisualStyleBackColor = true;
            this.addressOnly.CheckedChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // nameAndAddress
            // 
            this.nameAndAddress.AutoSize = true;
            this.nameAndAddress.Location = new System.Drawing.Point(19, 79);
            this.nameAndAddress.Name = "nameAndAddress";
            this.nameAndAddress.Size = new System.Drawing.Size(114, 17);
            this.nameAndAddress.TabIndex = 5;
            this.nameAndAddress.TabStop = true;
            this.nameAndAddress.Text = "Name and address";
            this.nameAndAddress.UseVisualStyleBackColor = true;
            this.nameAndAddress.CheckedChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // labeNameOnly
            // 
            this.labeNameOnly.AutoSize = true;
            this.labeNameOnly.ForeColor = System.Drawing.Color.Gray;
            this.labeNameOnly.Location = new System.Drawing.Point(35, 63);
            this.labeNameOnly.Name = "labeNameOnly";
            this.labeNameOnly.Size = new System.Drawing.Size(60, 13);
            this.labeNameOnly.TabIndex = 4;
            this.labeNameOnly.Text = "Brian Smith";
            // 
            // labelNameAndAddress
            // 
            this.labelNameAndAddress.AutoSize = true;
            this.labelNameAndAddress.ForeColor = System.Drawing.Color.Gray;
            this.labelNameAndAddress.Location = new System.Drawing.Point(35, 94);
            this.labelNameAndAddress.Name = "labelNameAndAddress";
            this.labelNameAndAddress.Size = new System.Drawing.Size(168, 13);
            this.labelNameAndAddress.TabIndex = 6;
            this.labelNameAndAddress.Text = "Brian Smith [brian@example.com]";
            // 
            // labelAddressOnly
            // 
            this.labelAddressOnly.AutoSize = true;
            this.labelAddressOnly.ForeColor = System.Drawing.Color.Gray;
            this.labelAddressOnly.Location = new System.Drawing.Point(35, 125);
            this.labelAddressOnly.Name = "labelAddressOnly";
            this.labelAddressOnly.Size = new System.Drawing.Size(104, 13);
            this.labelAddressOnly.TabIndex = 8;
            this.labelAddressOnly.Text = "brian@example.com";
            // 
            // GridEmailDisplayEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelAddressOnly);
            this.Controls.Add(this.labelNameAndAddress);
            this.Controls.Add(this.labeNameOnly);
            this.Controls.Add(this.nameAndAddress);
            this.Controls.Add(this.addressOnly);
            this.Controls.Add(this.nameOnly);
            this.Controls.Add(this.labelDisplay);
            this.Name = "GridEmailDisplayEditor";
            this.Size = new System.Drawing.Size(220, 155);
            this.Controls.SetChildIndex(this.labelDisplay, 0);
            this.Controls.SetChildIndex(this.nameOnly, 0);
            this.Controls.SetChildIndex(this.addressOnly, 0);
            this.Controls.SetChildIndex(this.nameAndAddress, 0);
            this.Controls.SetChildIndex(this.labeNameOnly, 0);
            this.Controls.SetChildIndex(this.labelNameAndAddress, 0);
            this.Controls.SetChildIndex(this.labelAddressOnly, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelDisplay;
        private System.Windows.Forms.RadioButton nameOnly;
        private System.Windows.Forms.RadioButton addressOnly;
        private System.Windows.Forms.RadioButton nameAndAddress;
        private System.Windows.Forms.Label labeNameOnly;
        private System.Windows.Forms.Label labelNameAndAddress;
        private System.Windows.Forms.Label labelAddressOnly;

    }
}
