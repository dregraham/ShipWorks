namespace ShipWorks.Actions.Triggers.Editors
{
    partial class UserInitiatedTriggerEditor
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
            this.showOnRibbon = new System.Windows.Forms.CheckBox();
            this.labelButtonImage = new System.Windows.Forms.Label();
            this.buttonImage = new System.Windows.Forms.PictureBox();
            this.linkButtonImageSelect = new ShipWorks.UI.Controls.LinkControl();
            this.labelShowOn = new System.Windows.Forms.Label();
            this.showOnOrdersMenu = new System.Windows.Forms.CheckBox();
            this.showOnCustomersMenu = new System.Windows.Forms.CheckBox();
            this.labelRequire = new System.Windows.Forms.Label();
            this.comboRequire = new System.Windows.Forms.ComboBox();
            this.panelBottom = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.buttonImage)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // showOnRibbon
            // 
            this.showOnRibbon.AutoSize = true;
            this.showOnRibbon.Location = new System.Drawing.Point(59, 9);
            this.showOnRibbon.Name = "showOnRibbon";
            this.showOnRibbon.Size = new System.Drawing.Size(193, 17);
            this.showOnRibbon.TabIndex = 0;
            this.showOnRibbon.Text = "The \'Home\' tab of the main window";
            this.showOnRibbon.UseVisualStyleBackColor = true;
            // 
            // labelButtonImage
            // 
            this.labelButtonImage.AutoSize = true;
            this.labelButtonImage.Location = new System.Drawing.Point(23, 3);
            this.labelButtonImage.Name = "labelButtonImage";
            this.labelButtonImage.Size = new System.Drawing.Size(41, 13);
            this.labelButtonImage.TabIndex = 3;
            this.labelButtonImage.Text = "Image:";
            // 
            // buttonImage
            // 
            this.buttonImage.Image = global::ShipWorks.Properties.Resources.gear_run32;
            this.buttonImage.Location = new System.Drawing.Point(66, 3);
            this.buttonImage.Name = "buttonImage";
            this.buttonImage.Size = new System.Drawing.Size(32, 32);
            this.buttonImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.buttonImage.TabIndex = 4;
            this.buttonImage.TabStop = false;
            // 
            // linkButtonImageSelect
            // 
            this.linkButtonImageSelect.AutoSize = true;
            this.linkButtonImageSelect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkButtonImageSelect.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkButtonImageSelect.ForeColor = System.Drawing.Color.Blue;
            this.linkButtonImageSelect.Location = new System.Drawing.Point(104, 3);
            this.linkButtonImageSelect.Name = "linkButtonImageSelect";
            this.linkButtonImageSelect.Size = new System.Drawing.Size(43, 13);
            this.linkButtonImageSelect.TabIndex = 5;
            this.linkButtonImageSelect.Text = "Choose";
            this.linkButtonImageSelect.Click += new System.EventHandler(this.OnChooseImage);
            // 
            // labelShowOn
            // 
            this.labelShowOn.AutoSize = true;
            this.labelShowOn.Location = new System.Drawing.Point(6, 9);
            this.labelShowOn.Name = "labelShowOn";
            this.labelShowOn.Size = new System.Drawing.Size(52, 13);
            this.labelShowOn.TabIndex = 15;
            this.labelShowOn.Text = "Show on:";
            // 
            // showOnOrdersMenu
            // 
            this.showOnOrdersMenu.AutoSize = true;
            this.showOnOrdersMenu.Location = new System.Drawing.Point(59, 32);
            this.showOnOrdersMenu.Name = "showOnOrdersMenu";
            this.showOnOrdersMenu.Size = new System.Drawing.Size(208, 17);
            this.showOnOrdersMenu.TabIndex = 16;
            this.showOnOrdersMenu.Text = "The right-click menu of the orders grid";
            this.showOnOrdersMenu.UseVisualStyleBackColor = true;
            // 
            // showOnCustomersMenu
            // 
            this.showOnCustomersMenu.AutoSize = true;
            this.showOnCustomersMenu.Location = new System.Drawing.Point(59, 55);
            this.showOnCustomersMenu.Name = "showOnCustomersMenu";
            this.showOnCustomersMenu.Size = new System.Drawing.Size(226, 17);
            this.showOnCustomersMenu.TabIndex = 17;
            this.showOnCustomersMenu.Text = "The right-click menu of the customers grid";
            this.showOnCustomersMenu.UseVisualStyleBackColor = true;
            // 
            // labelRequire
            // 
            this.labelRequire.AutoSize = true;
            this.labelRequire.Location = new System.Drawing.Point(6, 85);
            this.labelRequire.Name = "labelRequire";
            this.labelRequire.Size = new System.Drawing.Size(48, 13);
            this.labelRequire.TabIndex = 18;
            this.labelRequire.Text = "Require:";
            // 
            // comboRequire
            // 
            this.comboRequire.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboRequire.FormattingEnabled = true;
            this.comboRequire.Location = new System.Drawing.Point(59, 82);
            this.comboRequire.Name = "comboRequire";
            this.comboRequire.Size = new System.Drawing.Size(193, 21);
            this.comboRequire.TabIndex = 19;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.labelShowOn);
            this.panelBottom.Controls.Add(this.comboRequire);
            this.panelBottom.Controls.Add(this.showOnRibbon);
            this.panelBottom.Controls.Add(this.labelRequire);
            this.panelBottom.Controls.Add(this.showOnOrdersMenu);
            this.panelBottom.Controls.Add(this.showOnCustomersMenu);
            this.panelBottom.Location = new System.Drawing.Point(5, 36);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(567, 112);
            this.panelBottom.TabIndex = 20;
            // 
            // UserInititatedTriggerEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.linkButtonImageSelect);
            this.Controls.Add(this.buttonImage);
            this.Controls.Add(this.labelButtonImage);
            this.Name = "UserInititatedTriggerEditor";
            this.Size = new System.Drawing.Size(567, 149);
            ((System.ComponentModel.ISupportInitialize)(this.buttonImage)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox showOnRibbon;
        private System.Windows.Forms.Label labelButtonImage;
        private System.Windows.Forms.PictureBox buttonImage;
        private ShipWorks.UI.Controls.LinkControl linkButtonImageSelect;
        private System.Windows.Forms.Label labelShowOn;
        private System.Windows.Forms.CheckBox showOnOrdersMenu;
        private System.Windows.Forms.CheckBox showOnCustomersMenu;
        private System.Windows.Forms.Label labelRequire;
        private System.Windows.Forms.ComboBox comboRequire;
        private System.Windows.Forms.Panel panelBottom;
    }
}
