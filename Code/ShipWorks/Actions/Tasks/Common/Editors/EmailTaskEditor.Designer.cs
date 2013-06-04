namespace ShipWorks.Actions.Tasks.Common.Editors
{
    partial class EmailTaskEditor
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
            this.labelDelayDelivery = new System.Windows.Forms.Label();
            this.delayDelivery = new System.Windows.Forms.CheckBox();
            this.delayTimeOfDay = new System.Windows.Forms.DateTimePicker();
            this.delayTimeLink = new System.Windows.Forms.Label();
            this.labelAt = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelDelayDelivery
            // 
            this.labelDelayDelivery.AutoSize = true;
            this.labelDelayDelivery.Location = new System.Drawing.Point(1, 32);
            this.labelDelayDelivery.Name = "labelDelayDelivery";
            this.labelDelayDelivery.Size = new System.Drawing.Size(79, 13);
            this.labelDelayDelivery.TabIndex = 2;
            this.labelDelayDelivery.Text = "Delay delivery:";
            // 
            // delayDelivery
            // 
            this.delayDelivery.AutoSize = true;
            this.delayDelivery.Location = new System.Drawing.Point(81, 32);
            this.delayDelivery.Name = "delayDelivery";
            this.delayDelivery.Size = new System.Drawing.Size(50, 17);
            this.delayDelivery.TabIndex = 3;
            this.delayDelivery.Text = "Send";
            this.delayDelivery.UseVisualStyleBackColor = true;
            this.delayDelivery.CheckedChanged += new System.EventHandler(this.OnChangeDelayDelivery);
            // 
            // delayTimeOfDay
            // 
            this.delayTimeOfDay.CustomFormat = "h:mm tt";
            this.delayTimeOfDay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.delayTimeOfDay.Location = new System.Drawing.Point(228, 28);
            this.delayTimeOfDay.Name = "delayTimeOfDay";
            this.delayTimeOfDay.ShowUpDown = true;
            this.delayTimeOfDay.Size = new System.Drawing.Size(82, 21);
            this.delayTimeOfDay.TabIndex = 6;
            this.delayTimeOfDay.Value = new System.DateTime(2009, 1, 29, 8, 0, 0, 0);
            this.delayTimeOfDay.ValueChanged += new System.EventHandler(this.OnChangeDelayTimeOfDay);
            // 
            // delayTimeLink
            // 
            this.delayTimeLink.AutoSize = true;
            this.delayTimeLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.delayTimeLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.delayTimeLink.ForeColor = System.Drawing.Color.Blue;
            this.delayTimeLink.Location = new System.Drawing.Point(125, 33);
            this.delayTimeLink.Name = "delayTimeLink";
            this.delayTimeLink.Size = new System.Drawing.Size(87, 13);
            this.delayTimeLink.TabIndex = 4;
            this.delayTimeLink.Text = "2 days from now";
            this.delayTimeLink.Click += new System.EventHandler(this.OnClickLinkDelayTime);
            // 
            // labelAt
            // 
            this.labelAt.AutoSize = true;
            this.labelAt.Location = new System.Drawing.Point(209, 33);
            this.labelAt.Name = "labelAt";
            this.labelAt.Size = new System.Drawing.Size(17, 13);
            this.labelAt.TabIndex = 5;
            this.labelAt.Text = "at";
            // 
            // EmailTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelAt);
            this.Controls.Add(this.delayTimeLink);
            this.Controls.Add(this.delayTimeOfDay);
            this.Controls.Add(this.labelDelayDelivery);
            this.Controls.Add(this.delayDelivery);
            this.Name = "EmailTaskEditor";
            this.Size = new System.Drawing.Size(427, 57);
            this.Controls.SetChildIndex(this.delayDelivery, 0);
            this.Controls.SetChildIndex(this.labelDelayDelivery, 0);
            this.Controls.SetChildIndex(this.labelTemplate, 0);
            this.Controls.SetChildIndex(this.templateCombo, 0);
            this.Controls.SetChildIndex(this.delayTimeOfDay, 0);
            this.Controls.SetChildIndex(this.delayTimeLink, 0);
            this.Controls.SetChildIndex(this.labelAt, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelDelayDelivery;
        private System.Windows.Forms.CheckBox delayDelivery;
        private System.Windows.Forms.DateTimePicker delayTimeOfDay;
        private System.Windows.Forms.Label delayTimeLink;
        private System.Windows.Forms.Label labelAt;
    }
}
