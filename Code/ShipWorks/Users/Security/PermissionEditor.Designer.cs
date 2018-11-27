namespace ShipWorks.Users.Security
{
    partial class PermissionEditor
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
            this.panelStores = new System.Windows.Forms.Panel();
            this.labelManagement = new System.Windows.Forms.Label();
            this.manageActions = new System.Windows.Forms.CheckBox();
            this.manageFilters = new System.Windows.Forms.CheckBox();
            this.manageTemplates = new System.Windows.Forms.CheckBox();
            this.panelCustomers = new System.Windows.Forms.Panel();
            this.infoTipEmailCustomers = new ShipWorks.UI.Controls.InfoTip();
            this.infoTipCustomerNotes = new ShipWorks.UI.Controls.InfoTip();
            this.infoTipDeleteCustomers = new ShipWorks.UI.Controls.InfoTip();
            this.infotipCreateCustomers = new ShipWorks.UI.Controls.InfoTip();
            this.label1 = new System.Windows.Forms.Label();
            this.customerNotesPicture = new System.Windows.Forms.PictureBox();
            this.customerNotesLabel = new System.Windows.Forms.Label();
            this.customerDeletePicture = new System.Windows.Forms.PictureBox();
            this.customerCreatePicture = new System.Windows.Forms.PictureBox();
            this.customerEmailPicture = new System.Windows.Forms.PictureBox();
            this.customerDeleteLabel = new System.Windows.Forms.Label();
            this.customerCreateLabel = new System.Windows.Forms.Label();
            this.customerEmailLabel = new System.Windows.Forms.Label();
            this.labelCustomers = new System.Windows.Forms.Label();
            this.manageProducts = new System.Windows.Forms.CheckBox();
            this.panelCustomers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customerNotesPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerDeletePicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerCreatePicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerEmailPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // panelStores
            // 
            this.panelStores.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelStores.Location = new System.Drawing.Point(3, 197);
            this.panelStores.Name = "panelStores";
            this.panelStores.Size = new System.Drawing.Size(287, 119);
            this.panelStores.TabIndex = 5;
            // 
            // labelManagement
            // 
            this.labelManagement.AutoSize = true;
            this.labelManagement.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelManagement.Location = new System.Drawing.Point(5, 6);
            this.labelManagement.Name = "labelManagement";
            this.labelManagement.Size = new System.Drawing.Size(82, 13);
            this.labelManagement.TabIndex = 0;
            this.labelManagement.Text = "Management";
            // 
            // manageActions
            // 
            this.manageActions.AutoSize = true;
            this.manageActions.Location = new System.Drawing.Point(22, 23);
            this.manageActions.Name = "manageActions";
            this.manageActions.Size = new System.Drawing.Size(179, 17);
            this.manageActions.TabIndex = 1;
            this.manageActions.Text = "Create, edit, and delete actions";
            this.manageActions.UseVisualStyleBackColor = true;
            // 
            // manageFilters
            // 
            this.manageFilters.AutoSize = true;
            this.manageFilters.Location = new System.Drawing.Point(22, 42);
            this.manageFilters.Name = "manageFilters";
            this.manageFilters.Size = new System.Drawing.Size(172, 17);
            this.manageFilters.TabIndex = 2;
            this.manageFilters.Text = "Create, edit, and delete filters";
            this.manageFilters.UseVisualStyleBackColor = true;
            // 
            // manageTemplates
            // 
            this.manageTemplates.AutoSize = true;
            this.manageTemplates.Location = new System.Drawing.Point(22, 61);
            this.manageTemplates.Name = "manageTemplates";
            this.manageTemplates.Size = new System.Drawing.Size(192, 17);
            this.manageTemplates.TabIndex = 3;
            this.manageTemplates.Text = "Create, edit, and delete templates";
            this.manageTemplates.UseVisualStyleBackColor = true;
            // 
            // panelCustomers
            // 
            this.panelCustomers.Controls.Add(this.infoTipEmailCustomers);
            this.panelCustomers.Controls.Add(this.infoTipCustomerNotes);
            this.panelCustomers.Controls.Add(this.infoTipDeleteCustomers);
            this.panelCustomers.Controls.Add(this.infotipCreateCustomers);
            this.panelCustomers.Controls.Add(this.label1);
            this.panelCustomers.Controls.Add(this.customerNotesPicture);
            this.panelCustomers.Controls.Add(this.customerNotesLabel);
            this.panelCustomers.Controls.Add(this.customerDeletePicture);
            this.panelCustomers.Controls.Add(this.customerCreatePicture);
            this.panelCustomers.Controls.Add(this.customerEmailPicture);
            this.panelCustomers.Controls.Add(this.customerDeleteLabel);
            this.panelCustomers.Controls.Add(this.customerCreateLabel);
            this.panelCustomers.Controls.Add(this.customerEmailLabel);
            this.panelCustomers.Controls.Add(this.labelCustomers);
            this.panelCustomers.Location = new System.Drawing.Point(7, 101);
            this.panelCustomers.Name = "panelCustomers";
            this.panelCustomers.Size = new System.Drawing.Size(287, 94);
            this.panelCustomers.TabIndex = 4;
            // 
            // infoTipEmailCustomers
            // 
            this.infoTipEmailCustomers.Caption = "A user can send email to customers when he can send email for orders for every st" +
    "ore.";
            this.infoTipEmailCustomers.Location = new System.Drawing.Point(172, 74);
            this.infoTipEmailCustomers.Name = "infoTipEmailCustomers";
            this.infoTipEmailCustomers.Size = new System.Drawing.Size(12, 12);
            this.infoTipEmailCustomers.TabIndex = 48;
            this.infoTipEmailCustomers.Title = "Send Customer Email";
            // 
            // infoTipCustomerNotes
            // 
            this.infoTipCustomerNotes.Caption = "A user can edit customer notes if he can edit notes for orders of at least one st" +
    "ore.";
            this.infoTipCustomerNotes.Location = new System.Drawing.Point(153, 56);
            this.infoTipCustomerNotes.Name = "infoTipCustomerNotes";
            this.infoTipCustomerNotes.Size = new System.Drawing.Size(12, 12);
            this.infoTipCustomerNotes.TabIndex = 47;
            this.infoTipCustomerNotes.Title = "Edit Customer Notes";
            // 
            // infoTipDeleteCustomers
            // 
            this.infoTipDeleteCustomers.Caption = "A user can delete customers when he can delete orders for every store.";
            this.infoTipDeleteCustomers.Location = new System.Drawing.Point(139, 37);
            this.infoTipDeleteCustomers.Name = "infoTipDeleteCustomers";
            this.infoTipDeleteCustomers.Size = new System.Drawing.Size(12, 12);
            this.infoTipDeleteCustomers.TabIndex = 46;
            this.infoTipDeleteCustomers.Title = "Delete Customers";
            // 
            // infotipCreateCustomers
            // 
            this.infotipCreateCustomers.Caption = "A user can create customers when he can create orders for at least one store.";
            this.infotipCreateCustomers.Location = new System.Drawing.Point(182, 20);
            this.infotipCreateCustomers.Name = "infotipCreateCustomers";
            this.infotipCreateCustomers.Size = new System.Drawing.Size(12, 12);
            this.infotipCreateCustomers.TabIndex = 45;
            this.infotipCreateCustomers.Title = "Create Customers";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label1.Location = new System.Drawing.Point(66, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 13);
            this.label1.TabIndex = 40;
            this.label1.Text = "(Derived from subsequent settings)";
            // 
            // customerNotesPicture
            // 
            this.customerNotesPicture.Image = global::ShipWorks.Properties.Resources.forbidden;
            this.customerNotesPicture.Location = new System.Drawing.Point(13, 54);
            this.customerNotesPicture.Name = "customerNotesPicture";
            this.customerNotesPicture.Size = new System.Drawing.Size(16, 16);
            this.customerNotesPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.customerNotesPicture.TabIndex = 39;
            this.customerNotesPicture.TabStop = false;
            // 
            // customerNotesLabel
            // 
            this.customerNotesLabel.AutoSize = true;
            this.customerNotesLabel.Location = new System.Drawing.Point(31, 55);
            this.customerNotesLabel.Name = "customerNotesLabel";
            this.customerNotesLabel.Size = new System.Drawing.Size(121, 13);
            this.customerNotesLabel.TabIndex = 2;
            this.customerNotesLabel.Text = "{0} edit customer notes";
            // 
            // customerDeletePicture
            // 
            this.customerDeletePicture.Image = global::ShipWorks.Properties.Resources.forbidden;
            this.customerDeletePicture.Location = new System.Drawing.Point(13, 36);
            this.customerDeletePicture.Name = "customerDeletePicture";
            this.customerDeletePicture.Size = new System.Drawing.Size(16, 16);
            this.customerDeletePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.customerDeletePicture.TabIndex = 36;
            this.customerDeletePicture.TabStop = false;
            // 
            // customerCreatePicture
            // 
            this.customerCreatePicture.Image = global::ShipWorks.Properties.Resources.check16;
            this.customerCreatePicture.Location = new System.Drawing.Point(13, 18);
            this.customerCreatePicture.Name = "customerCreatePicture";
            this.customerCreatePicture.Size = new System.Drawing.Size(16, 16);
            this.customerCreatePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.customerCreatePicture.TabIndex = 35;
            this.customerCreatePicture.TabStop = false;
            // 
            // customerEmailPicture
            // 
            this.customerEmailPicture.Image = global::ShipWorks.Properties.Resources.check16;
            this.customerEmailPicture.Location = new System.Drawing.Point(13, 71);
            this.customerEmailPicture.Name = "customerEmailPicture";
            this.customerEmailPicture.Size = new System.Drawing.Size(16, 16);
            this.customerEmailPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.customerEmailPicture.TabIndex = 34;
            this.customerEmailPicture.TabStop = false;
            // 
            // customerDeleteLabel
            // 
            this.customerDeleteLabel.AutoSize = true;
            this.customerDeleteLabel.Location = new System.Drawing.Point(31, 37);
            this.customerDeleteLabel.Name = "customerDeleteLabel";
            this.customerDeleteLabel.Size = new System.Drawing.Size(108, 13);
            this.customerDeleteLabel.TabIndex = 1;
            this.customerDeleteLabel.Text = "{0} delete customers";
            // 
            // customerCreateLabel
            // 
            this.customerCreateLabel.AutoSize = true;
            this.customerCreateLabel.Location = new System.Drawing.Point(31, 19);
            this.customerCreateLabel.Name = "customerCreateLabel";
            this.customerCreateLabel.Size = new System.Drawing.Size(151, 13);
            this.customerCreateLabel.TabIndex = 0;
            this.customerCreateLabel.Text = "{0} create and edit customers";
            // 
            // customerEmailLabel
            // 
            this.customerEmailLabel.AutoSize = true;
            this.customerEmailLabel.Location = new System.Drawing.Point(31, 73);
            this.customerEmailLabel.Name = "customerEmailLabel";
            this.customerEmailLabel.Size = new System.Drawing.Size(141, 13);
            this.customerEmailLabel.TabIndex = 3;
            this.customerEmailLabel.Text = "{0} send email to customers";
            // 
            // labelCustomers
            // 
            this.labelCustomers.AutoSize = true;
            this.labelCustomers.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCustomers.Location = new System.Drawing.Point(-2, 2);
            this.labelCustomers.Name = "labelCustomers";
            this.labelCustomers.Size = new System.Drawing.Size(68, 13);
            this.labelCustomers.TabIndex = 23;
            this.labelCustomers.Text = "Customers";
            // 
            // manageProducts
            // 
            this.manageProducts.AutoSize = true;
            this.manageProducts.Location = new System.Drawing.Point(22, 80);
            this.manageProducts.Name = "manageProducts";
            this.manageProducts.Size = new System.Drawing.Size(146, 17);
            this.manageProducts.TabIndex = 6;
            this.manageProducts.Text = "Create and edit products";
            this.manageProducts.UseVisualStyleBackColor = true;
            // 
            // PermissionEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.manageProducts);
            this.Controls.Add(this.panelCustomers);
            this.Controls.Add(this.panelStores);
            this.Controls.Add(this.labelManagement);
            this.Controls.Add(this.manageActions);
            this.Controls.Add(this.manageFilters);
            this.Controls.Add(this.manageTemplates);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PermissionEditor";
            this.Size = new System.Drawing.Size(297, 330);
            this.panelCustomers.ResumeLayout(false);
            this.panelCustomers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customerNotesPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerDeletePicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerCreatePicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customerEmailPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelStores;
        private System.Windows.Forms.Label labelManagement;
        private System.Windows.Forms.CheckBox manageActions;
        private System.Windows.Forms.CheckBox manageFilters;
        private System.Windows.Forms.CheckBox manageTemplates;
        private System.Windows.Forms.Panel panelCustomers;
        private System.Windows.Forms.Label labelCustomers;
        private System.Windows.Forms.Label customerCreateLabel;
        private System.Windows.Forms.Label customerEmailLabel;
        private System.Windows.Forms.Label customerDeleteLabel;
        private System.Windows.Forms.PictureBox customerEmailPicture;
        private System.Windows.Forms.PictureBox customerDeletePicture;
        private System.Windows.Forms.PictureBox customerCreatePicture;
        private System.Windows.Forms.PictureBox customerNotesPicture;
        private System.Windows.Forms.Label customerNotesLabel;
        private System.Windows.Forms.Label label1;
        private UI.Controls.InfoTip infoTipEmailCustomers;
        private UI.Controls.InfoTip infoTipCustomerNotes;
        private UI.Controls.InfoTip infoTipDeleteCustomers;
        private UI.Controls.InfoTip infotipCreateCustomers;
        private System.Windows.Forms.CheckBox manageProducts;
    }
}
