namespace ShipWorks.Shipping.Settings.Origin
{
    partial class ShipmentOriginControl
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
            this.originCombo = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.titleOrigin = new System.Windows.Forms.Label();
            this.personControl = new ShipWorks.Data.Controls.PersonControl();
            this.labelOrigin = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // originCombo
            // 
            this.originCombo.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.originCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.originCombo.FormattingEnabled = true;
            this.originCombo.Location = new System.Drawing.Point(79, 20);
            this.originCombo.Name = "originCombo";
            this.originCombo.Size = new System.Drawing.Size(248, 21);
            this.originCombo.TabIndex = 2;
            this.originCombo.SelectedIndexChanged += new System.EventHandler(this.OnChangeSender);
            // 
            // titleOrigin
            // 
            this.titleOrigin.AutoSize = true;
            this.titleOrigin.BackColor = System.Drawing.Color.Transparent;
            this.titleOrigin.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.titleOrigin.Location = new System.Drawing.Point(3, 2);
            this.titleOrigin.Name = "titleOrigin";
            this.titleOrigin.Size = new System.Drawing.Size(40, 13);
            this.titleOrigin.TabIndex = 0;
            this.titleOrigin.Text = "Origin";
            // 
            // personControl
            // 
            this.personControl.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.personControl.BackColor = System.Drawing.Color.Transparent;
            this.personControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.personControl.Location = new System.Drawing.Point(3, 41);
            this.personControl.Name = "personControl";
            this.personControl.Size = new System.Drawing.Size(338, 414);
            this.personControl.TabIndex = 3;
            this.personControl.ContentChanged += new System.EventHandler(this.OnPersonContentChanged);
            this.personControl.DestinationChanged += new System.EventHandler(this.OnDestinationChanged);
            // 
            // labelOrigin
            // 
            this.labelOrigin.Location = new System.Drawing.Point(6, 23);
            this.labelOrigin.Name = "labelOrigin";
            this.labelOrigin.Size = new System.Drawing.Size(65, 13);
            this.labelOrigin.TabIndex = 1;
            this.labelOrigin.Text = "Origin:";
            this.labelOrigin.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ShipmentOriginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.labelOrigin);
            this.Controls.Add(this.originCombo);
            this.Controls.Add(this.titleOrigin);
            this.Controls.Add(this.personControl);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "ShipmentOriginControl";
            this.Size = new System.Drawing.Size(341, 465);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ShipWorks.UI.Controls.MultiValueComboBox originCombo;
        private System.Windows.Forms.Label titleOrigin;
        private ShipWorks.Data.Controls.PersonControl personControl;
        private System.Windows.Forms.Label labelOrigin;
    }
}
