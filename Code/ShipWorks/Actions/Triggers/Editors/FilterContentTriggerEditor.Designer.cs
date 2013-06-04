namespace ShipWorks.Actions.Triggers.Editors
{
    partial class FilterContentTriggerEditor
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
            this.labelWhen = new System.Windows.Forms.Label();
            this.labelDirection = new System.Windows.Forms.Label();
            this.filterTarget = new System.Windows.Forms.Label();
            this.filterComboBox = new ShipWorks.Filters.Controls.FilterComboBox();
            this.SuspendLayout();
            // 
            // labelWhen
            // 
            this.labelWhen.AutoSize = true;
            this.labelWhen.Location = new System.Drawing.Point(3, 3);
            this.labelWhen.Name = "labelWhen";
            this.labelWhen.Size = new System.Drawing.Size(50, 13);
            this.labelWhen.TabIndex = 0;
            this.labelWhen.Text = "When an";
            // 
            // labelDirection
            // 
            this.labelDirection.AutoSize = true;
            this.labelDirection.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelDirection.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelDirection.ForeColor = System.Drawing.Color.Blue;
            this.labelDirection.Location = new System.Drawing.Point(84, 3);
            this.labelDirection.Name = "labelDirection";
            this.labelDirection.Size = new System.Drawing.Size(38, 13);
            this.labelDirection.TabIndex = 2;
            this.labelDirection.Text = "enters";
            this.labelDirection.Click += new System.EventHandler(this.OnClickLabelDirection);
            // 
            // filterTarget
            // 
            this.filterTarget.AutoSize = true;
            this.filterTarget.Cursor = System.Windows.Forms.Cursors.Hand;
            this.filterTarget.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.filterTarget.ForeColor = System.Drawing.Color.Blue;
            this.filterTarget.Location = new System.Drawing.Point(52, 3);
            this.filterTarget.Name = "filterTarget";
            this.filterTarget.Size = new System.Drawing.Size(35, 13);
            this.filterTarget.TabIndex = 1;
            this.filterTarget.Text = "Order";
            this.filterTarget.Click += new System.EventHandler(this.OnClickFilterTarget);
            // 
            // filterComboBox
            // 
            this.filterComboBox.AllowQuickFilter = true;
            this.filterComboBox.DropDownHeight = 300;
            this.filterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.filterComboBox.FormattingEnabled = true;
            this.filterComboBox.IntegralHeight = false;
            this.filterComboBox.Location = new System.Drawing.Point(127, 0);
            this.filterComboBox.Name = "filterComboBox";
            this.filterComboBox.Size = new System.Drawing.Size(237, 21);
            this.filterComboBox.TabIndex = 3;
            this.filterComboBox.SelectedFilterNodeChanged += new System.EventHandler(this.OnFilterNodeChanged);
            // 
            // FilterContentTriggerEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.filterComboBox);
            this.Controls.Add(this.filterTarget);
            this.Controls.Add(this.labelDirection);
            this.Controls.Add(this.labelWhen);
            this.Name = "FilterContentTriggerEditor";
            this.Size = new System.Drawing.Size(377, 24);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelWhen;
        private System.Windows.Forms.Label labelDirection;
        private System.Windows.Forms.Label filterTarget;
        private ShipWorks.Filters.Controls.FilterComboBox filterComboBox;
    }
}
