namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    partial class NumericValueEditor<T>
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
            this.components = new System.ComponentModel.Container();
            this.value1 = new System.Windows.Forms.TextBox();
            this.labelAnd = new System.Windows.Forms.Label();
            this.value2 = new System.Windows.Forms.TextBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.valuePanel = new System.Windows.Forms.Panel();
            this.labelOperator = new ShipWorks.Filters.Content.Editors.ChoiceLabel();
            ((System.ComponentModel.ISupportInitialize) (this.errorProvider)).BeginInit();
            this.valuePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // value1
            // 
            this.value1.Location = new System.Drawing.Point(1, 2);
            this.value1.Name = "value1";
            this.value1.Size = new System.Drawing.Size(93, 21);
            this.value1.TabIndex = 0;
            this.value1.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidatingValue1);
            // 
            // labelAnd
            // 
            this.labelAnd.AutoSize = true;
            this.labelAnd.ForeColor = System.Drawing.Color.DimGray;
            this.labelAnd.Location = new System.Drawing.Point(96, 5);
            this.labelAnd.Name = "labelAnd";
            this.labelAnd.Size = new System.Drawing.Size(25, 13);
            this.labelAnd.TabIndex = 1;
            this.labelAnd.Text = "and";
            // 
            // value2
            // 
            this.value2.Location = new System.Drawing.Point(124, 2);
            this.value2.Name = "value2";
            this.value2.Size = new System.Drawing.Size(93, 21);
            this.value2.TabIndex = 2;
            this.value2.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidatingValue2);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // valuePanel
            // 
            this.valuePanel.Controls.Add(this.value1);
            this.valuePanel.Controls.Add(this.labelAnd);
            this.valuePanel.Controls.Add(this.value2);
            this.valuePanel.Location = new System.Drawing.Point(145, 1);
            this.valuePanel.Name = "valuePanel";
            this.valuePanel.Size = new System.Drawing.Size(228, 26);
            this.valuePanel.TabIndex = 1;
            // 
            // labelOperator
            // 
            this.labelOperator.AutoSize = true;
            this.labelOperator.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelOperator.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelOperator.ForeColor = System.Drawing.Color.Green;
            this.labelOperator.Location = new System.Drawing.Point(3, 6);
            this.labelOperator.Name = "labelOperator";
            this.labelOperator.Size = new System.Drawing.Size(140, 13);
            this.labelOperator.TabIndex = 0;
            this.labelOperator.Text = "Is Greater Than or Equal To";
            this.labelOperator.SelectedValueChanged += new System.EventHandler(this.OnChangeOperator);
            // 
            // NumericValueEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelOperator);
            this.Controls.Add(this.valuePanel);
            this.Name = "NumericValueEditor";
            this.Size = new System.Drawing.Size(414, 27);
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize) (this.errorProvider)).EndInit();
            this.valuePanel.ResumeLayout(false);
            this.valuePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox value1;
        private System.Windows.Forms.Label labelAnd;
        private System.Windows.Forms.TextBox value2;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Panel valuePanel;
        private ChoiceLabel labelOperator;
    }
}
