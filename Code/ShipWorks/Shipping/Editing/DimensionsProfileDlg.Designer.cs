namespace ShipWorks.Shipping.Editing
{
    partial class DimensionsProfileDlg
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.labelName = new System.Windows.Forms.Label();
            this.name = new System.Windows.Forms.TextBox();
            this.length = new System.Windows.Forms.TextBox();
            this.labelLength = new System.Windows.Forms.Label();
            this.width = new System.Windows.Forms.TextBox();
            this.labelWidth = new System.Windows.Forms.Label();
            this.height = new System.Windows.Forms.TextBox();
            this.labelHeight = new System.Windows.Forms.Label();
            this.labelWeight = new System.Windows.Forms.Label();
            this.labelLengthUnits = new System.Windows.Forms.Label();
            this.labelWidthUnits = new System.Windows.Forms.Label();
            this.labelHeightUnits = new System.Windows.Forms.Label();
            this.weight = new ShipWorks.UI.Controls.WeightControl();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(60, 147);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 13;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(141, 147);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 14;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(18, 9);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(34, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Name";
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(58, 6);
            this.fieldLengthProvider.SetMaxLengthSource(this.name, ShipWorks.Data.Utility.EntityFieldLengthSource.DimensionsProfileName);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(155, 21);
            this.name.TabIndex = 1;
            // 
            // length
            // 
            this.length.Location = new System.Drawing.Point(58, 32);
            this.length.Name = "length";
            this.length.Size = new System.Drawing.Size(55, 21);
            this.length.TabIndex = 3;
            // 
            // labelLength
            // 
            this.labelLength.AutoSize = true;
            this.labelLength.Location = new System.Drawing.Point(12, 35);
            this.labelLength.Name = "labelLength";
            this.labelLength.Size = new System.Drawing.Size(40, 13);
            this.labelLength.TabIndex = 2;
            this.labelLength.Text = "Length";
            // 
            // width
            // 
            this.width.Location = new System.Drawing.Point(58, 59);
            this.width.Name = "width";
            this.width.Size = new System.Drawing.Size(55, 21);
            this.width.TabIndex = 6;
            // 
            // labelWidth
            // 
            this.labelWidth.AutoSize = true;
            this.labelWidth.Location = new System.Drawing.Point(17, 62);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(35, 13);
            this.labelWidth.TabIndex = 5;
            this.labelWidth.Text = "Width";
            // 
            // height
            // 
            this.height.Location = new System.Drawing.Point(58, 86);
            this.height.Name = "height";
            this.height.Size = new System.Drawing.Size(55, 21);
            this.height.TabIndex = 9;
            // 
            // labelHeight
            // 
            this.labelHeight.AutoSize = true;
            this.labelHeight.Location = new System.Drawing.Point(14, 89);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(38, 13);
            this.labelHeight.TabIndex = 8;
            this.labelHeight.Text = "Height";
            // 
            // labelWeight
            // 
            this.labelWeight.AutoSize = true;
            this.labelWeight.Location = new System.Drawing.Point(12, 116);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(41, 13);
            this.labelWeight.TabIndex = 11;
            this.labelWeight.Text = "Weight";
            // 
            // labelLengthUnits
            // 
            this.labelLengthUnits.AutoSize = true;
            this.labelLengthUnits.Location = new System.Drawing.Point(114, 35);
            this.labelLengthUnits.Name = "labelLengthUnits";
            this.labelLengthUnits.Size = new System.Drawing.Size(15, 13);
            this.labelLengthUnits.TabIndex = 4;
            this.labelLengthUnits.Text = "in";
            // 
            // labelWidthUnits
            // 
            this.labelWidthUnits.AutoSize = true;
            this.labelWidthUnits.Location = new System.Drawing.Point(114, 62);
            this.labelWidthUnits.Name = "labelWidthUnits";
            this.labelWidthUnits.Size = new System.Drawing.Size(15, 13);
            this.labelWidthUnits.TabIndex = 7;
            this.labelWidthUnits.Text = "in";
            // 
            // labelHeightUnits
            // 
            this.labelHeightUnits.AutoSize = true;
            this.labelHeightUnits.Location = new System.Drawing.Point(114, 89);
            this.labelHeightUnits.Name = "labelHeightUnits";
            this.labelHeightUnits.Size = new System.Drawing.Size(15, 13);
            this.labelHeightUnits.TabIndex = 10;
            this.labelHeightUnits.Text = "in";
            // 
            // weight
            // 
            this.weight.BackColor = System.Drawing.Color.Transparent;
            this.weight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.weight.Location = new System.Drawing.Point(58, 113);
            this.weight.Name = "weight";
            this.weight.RangeMax = 300D;
            this.weight.RangeMin = 0D;
            this.weight.ShowWeighButton = false;
            this.weight.Size = new System.Drawing.Size(98, 21);
            this.weight.TabIndex = 12;
            this.weight.Weight = 0D;
            // 
            // DimensionsProfileDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(228, 182);
            this.Controls.Add(this.labelHeightUnits);
            this.Controls.Add(this.labelWidthUnits);
            this.Controls.Add(this.labelLengthUnits);
            this.Controls.Add(this.weight);
            this.Controls.Add(this.labelWeight);
            this.Controls.Add(this.height);
            this.Controls.Add(this.labelHeight);
            this.Controls.Add(this.width);
            this.Controls.Add(this.labelWidth);
            this.Controls.Add(this.length);
            this.Controls.Add(this.labelLength);
            this.Controls.Add(this.name);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DimensionsProfileDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dimensions Profile";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.TextBox length;
        private System.Windows.Forms.Label labelLength;
        private System.Windows.Forms.TextBox width;
        private System.Windows.Forms.Label labelWidth;
        private System.Windows.Forms.TextBox height;
        private System.Windows.Forms.Label labelHeight;
        private System.Windows.Forms.Label labelWeight;
        private ShipWorks.UI.Controls.WeightControl weight;
        private System.Windows.Forms.Label labelLengthUnits;
        private System.Windows.Forms.Label labelWidthUnits;
        private System.Windows.Forms.Label labelHeightUnits;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
    }
}