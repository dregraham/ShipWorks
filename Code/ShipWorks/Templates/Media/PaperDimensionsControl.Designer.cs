namespace ShipWorks.Templates.Media
{
    partial class PaperDimensionsControl
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
            this.labelInch1 = new System.Windows.Forms.Label();
            this.labelInch2 = new System.Windows.Forms.Label();
            this.paperWidth = new System.Windows.Forms.NumericUpDown();
            this.paperHeight = new System.Windows.Forms.NumericUpDown();
            this.paperDimensions = new System.Windows.Forms.ComboBox();
            this.labelSheetDimension = new System.Windows.Forms.Label();
            this.labelX = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize) (this.paperWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.paperHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // labelInch1
            // 
            this.labelInch1.Location = new System.Drawing.Point(217, 35);
            this.labelInch1.Name = "labelInch1";
            this.labelInch1.Size = new System.Drawing.Size(8, 10);
            this.labelInch1.TabIndex = 6;
            this.labelInch1.Text = "\"";
            // 
            // labelInch2
            // 
            this.labelInch2.Location = new System.Drawing.Point(135, 35);
            this.labelInch2.Name = "labelInch2";
            this.labelInch2.Size = new System.Drawing.Size(8, 12);
            this.labelInch2.TabIndex = 3;
            this.labelInch2.Text = "\"";
            // 
            // paperWidth
            // 
            this.paperWidth.DecimalPlaces = 2;
            this.paperWidth.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.paperWidth.Location = new System.Drawing.Point(79, 35);
            this.paperWidth.Name = "paperWidth";
            this.paperWidth.Size = new System.Drawing.Size(56, 21);
            this.paperWidth.TabIndex = 2;
            this.paperWidth.ValueChanged += new System.EventHandler(this.OnChangePaperDimensionValue);
            // 
            // paperHeight
            // 
            this.paperHeight.DecimalPlaces = 2;
            this.paperHeight.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.paperHeight.Location = new System.Drawing.Point(161, 35);
            this.paperHeight.Name = "paperHeight";
            this.paperHeight.Size = new System.Drawing.Size(56, 21);
            this.paperHeight.TabIndex = 5;
            this.paperHeight.ValueChanged += new System.EventHandler(this.OnChangePaperDimensionValue);
            // 
            // paperDimensions
            // 
            this.paperDimensions.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.paperDimensions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.paperDimensions.Location = new System.Drawing.Point(3, 3);
            this.paperDimensions.Name = "paperDimensions";
            this.paperDimensions.Size = new System.Drawing.Size(250, 21);
            this.paperDimensions.TabIndex = 0;
            this.paperDimensions.SelectedIndexChanged += new System.EventHandler(this.OnChangePaperDimensions);
            // 
            // labelSheetDimension
            // 
            this.labelSheetDimension.Location = new System.Drawing.Point(3, 37);
            this.labelSheetDimension.Name = "labelSheetDimension";
            this.labelSheetDimension.Size = new System.Drawing.Size(78, 16);
            this.labelSheetDimension.TabIndex = 1;
            this.labelSheetDimension.Text = "Dimensions:";
            // 
            // labelX
            // 
            this.labelX.Location = new System.Drawing.Point(145, 37);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(14, 16);
            this.labelX.TabIndex = 4;
            this.labelX.Text = "x";
            // 
            // PaperDimensionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelInch1);
            this.Controls.Add(this.labelInch2);
            this.Controls.Add(this.paperWidth);
            this.Controls.Add(this.paperHeight);
            this.Controls.Add(this.paperDimensions);
            this.Controls.Add(this.labelSheetDimension);
            this.Controls.Add(this.labelX);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "PaperDimensionsControl";
            this.Size = new System.Drawing.Size(256, 63);
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize) (this.paperWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.paperHeight)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelInch1;
        private System.Windows.Forms.Label labelInch2;
        private System.Windows.Forms.NumericUpDown paperWidth;
        private System.Windows.Forms.NumericUpDown paperHeight;
        private System.Windows.Forms.ComboBox paperDimensions;
        private System.Windows.Forms.Label labelSheetDimension;
        private System.Windows.Forms.Label labelX;
    }
}
