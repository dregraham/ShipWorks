namespace ShipWorks.Filters.Content.Editors.ValueEditors
{
    partial class DateValueEditor
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
            this.labelAnd = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.absolutePanel = new System.Windows.Forms.Panel();
            this.value2 = new System.Windows.Forms.DateTimePicker();
            this.value1 = new System.Windows.Forms.DateTimePicker();
            this.labelOperator = new ShipWorks.Filters.Content.Editors.ChoiceLabel();
            this.withinPanel = new System.Windows.Forms.Panel();
            this.withinRangeType = new ShipWorks.Filters.Content.Editors.ChoiceLabel();
            this.withinUnits = new System.Windows.Forms.ComboBox();
            this.withinAmount = new System.Windows.Forms.TextBox();
            this.relativePanel = new System.Windows.Forms.Panel();
            this.relativeUnits = new System.Windows.Forms.ComboBox();
            this.panelDateControls = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize) (this.errorProvider)).BeginInit();
            this.absolutePanel.SuspendLayout();
            this.withinPanel.SuspendLayout();
            this.relativePanel.SuspendLayout();
            this.panelDateControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelAnd
            // 
            this.labelAnd.AutoSize = true;
            this.labelAnd.ForeColor = System.Drawing.Color.DimGray;
            this.labelAnd.Location = new System.Drawing.Point(100, 5);
            this.labelAnd.Name = "labelAnd";
            this.labelAnd.Size = new System.Drawing.Size(25, 13);
            this.labelAnd.TabIndex = 2;
            this.labelAnd.Text = "and";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // absolutePanel
            // 
            this.absolutePanel.Controls.Add(this.value2);
            this.absolutePanel.Controls.Add(this.value1);
            this.absolutePanel.Controls.Add(this.labelAnd);
            this.absolutePanel.Location = new System.Drawing.Point(410, 0);
            this.absolutePanel.Name = "absolutePanel";
            this.absolutePanel.Size = new System.Drawing.Size(228, 26);
            this.absolutePanel.TabIndex = 4;
            // 
            // value2
            // 
            this.value2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.value2.Location = new System.Drawing.Point(127, 2);
            this.value2.Name = "value2";
            this.value2.Size = new System.Drawing.Size(93, 21);
            this.value2.TabIndex = 0;
            this.value2.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidateControls);
            // 
            // value1
            // 
            this.value1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.value1.Location = new System.Drawing.Point(3, 2);
            this.value1.Name = "value1";
            this.value1.Size = new System.Drawing.Size(93, 21);
            this.value1.TabIndex = 1;
            this.value1.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidateControls);
            // 
            // labelOperator
            // 
            this.labelOperator.AutoSize = true;
            this.labelOperator.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelOperator.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelOperator.ForeColor = System.Drawing.Color.Green;
            this.labelOperator.Location = new System.Drawing.Point(3, 6);
            this.labelOperator.Name = "labelOperator";
            this.labelOperator.Size = new System.Drawing.Size(91, 13);
            this.labelOperator.TabIndex = 1;
            this.labelOperator.Text = "Is Within the Last";
            this.labelOperator.SelectedValueChanged += new System.EventHandler(this.OnChangeOperator);
            // 
            // withinPanel
            // 
            this.withinPanel.Controls.Add(this.withinRangeType);
            this.withinPanel.Controls.Add(this.withinUnits);
            this.withinPanel.Controls.Add(this.withinAmount);
            this.withinPanel.Location = new System.Drawing.Point(211, 0);
            this.withinPanel.Name = "withinPanel";
            this.withinPanel.Size = new System.Drawing.Size(196, 26);
            this.withinPanel.TabIndex = 3;
            // 
            // withinInclusive
            // 
            this.withinRangeType.AutoSize = true;
            this.withinRangeType.Cursor = System.Windows.Forms.Cursors.Hand;
            this.withinRangeType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.withinRangeType.ForeColor = System.Drawing.Color.Green;
            this.withinRangeType.Location = new System.Drawing.Point(145, 6);
            this.withinRangeType.Name = "withinInclusive";
            this.withinRangeType.Size = new System.Drawing.Size(50, 13);
            this.withinRangeType.TabIndex = 1;
            this.withinRangeType.Text = "Including";
            this.withinRangeType.SelectedValueChanged += new System.EventHandler(this.OnChangeWithinInclusive);
            // 
            // withinUnits
            // 
            this.withinUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.withinUnits.FormattingEnabled = true;
            this.withinUnits.Location = new System.Drawing.Point(55, 3);
            this.withinUnits.Name = "withinUnits";
            this.withinUnits.Size = new System.Drawing.Size(87, 21);
            this.withinUnits.TabIndex = 2;
            this.withinUnits.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidateControls);
            // 
            // withinAmount
            // 
            this.withinAmount.Location = new System.Drawing.Point(0, 3);
            this.withinAmount.Name = "withinAmount";
            this.withinAmount.Size = new System.Drawing.Size(50, 21);
            this.withinAmount.TabIndex = 3;
            this.withinAmount.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidateWithinAmount);
            // 
            // relativePanel
            // 
            this.relativePanel.Controls.Add(this.relativeUnits);
            this.relativePanel.Location = new System.Drawing.Point(100, 0);
            this.relativePanel.Name = "relativePanel";
            this.relativePanel.Size = new System.Drawing.Size(97, 26);
            this.relativePanel.TabIndex = 2;
            // 
            // relativeUnits
            // 
            this.relativeUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.relativeUnits.FormattingEnabled = true;
            this.relativeUnits.Location = new System.Drawing.Point(3, 3);
            this.relativeUnits.Name = "relativeUnits";
            this.relativeUnits.Size = new System.Drawing.Size(87, 21);
            this.relativeUnits.TabIndex = 0;
            this.relativeUnits.Validating += new System.ComponentModel.CancelEventHandler(this.OnValidateControls);
            // 
            // panelDateControls
            // 
            this.panelDateControls.Controls.Add(this.labelOperator);
            this.panelDateControls.Controls.Add(this.relativePanel);
            this.panelDateControls.Controls.Add(this.absolutePanel);
            this.panelDateControls.Controls.Add(this.withinPanel);
            this.panelDateControls.Location = new System.Drawing.Point(0, 0);
            this.panelDateControls.Name = "panelDateControls";
            this.panelDateControls.Size = new System.Drawing.Size(767, 27);
            this.panelDateControls.TabIndex = 0;
            // 
            // DateValueEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelDateControls);
            this.Name = "DateValueEditor";
            this.Size = new System.Drawing.Size(769, 27);
            ((System.ComponentModel.ISupportInitialize) (this.errorProvider)).EndInit();
            this.absolutePanel.ResumeLayout(false);
            this.absolutePanel.PerformLayout();
            this.withinPanel.ResumeLayout(false);
            this.withinPanel.PerformLayout();
            this.relativePanel.ResumeLayout(false);
            this.panelDateControls.ResumeLayout(false);
            this.panelDateControls.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelAnd;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Panel absolutePanel;
        private ChoiceLabel labelOperator;
        private System.Windows.Forms.DateTimePicker value2;
        private System.Windows.Forms.DateTimePicker value1;
        private System.Windows.Forms.Panel withinPanel;
        private System.Windows.Forms.ComboBox withinUnits;
        private System.Windows.Forms.TextBox withinAmount;
        private System.Windows.Forms.Panel relativePanel;
        private System.Windows.Forms.ComboBox relativeUnits;
        private ChoiceLabel withinRangeType;
        protected System.Windows.Forms.Panel panelDateControls;
    }
}
