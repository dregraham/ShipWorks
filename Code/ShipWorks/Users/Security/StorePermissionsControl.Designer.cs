namespace ShipWorks.Users.Security
{
    partial class StorePermissionsControl
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
            this.labelOrders = new System.Windows.Forms.Label();
            this.manageOrders = new System.Windows.Forms.CheckBox();
            this.editOrderStatus = new System.Windows.Forms.CheckBox();
            this.viewPaymentDetails = new System.Windows.Forms.CheckBox();
            this.editOrderNotes = new System.Windows.Forms.CheckBox();
            this.voidShipments = new System.Windows.Forms.CheckBox();
            this.labelShipments = new System.Windows.Forms.Label();
            this.prepareShipments = new System.Windows.Forms.CheckBox();
            this.labelEmail = new System.Windows.Forms.Label();
            this.sendEmail = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // labelOrders
            // 
            this.labelOrders.AutoSize = true;
            this.labelOrders.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelOrders.Location = new System.Drawing.Point(2, 0);
            this.labelOrders.Name = "labelOrders";
            this.labelOrders.Size = new System.Drawing.Size(45, 13);
            this.labelOrders.TabIndex = 0;
            this.labelOrders.Text = "Orders";
            // 
            // manageOrders
            // 
            this.manageOrders.AutoSize = true;
            this.manageOrders.Location = new System.Drawing.Point(19, 17);
            this.manageOrders.Name = "manageOrders";
            this.manageOrders.Size = new System.Drawing.Size(142, 17);
            this.manageOrders.TabIndex = 1;
            this.manageOrders.Text = "Create, edit, and delete";
            this.manageOrders.UseVisualStyleBackColor = true;
            // 
            // editOrderStatus
            // 
            this.editOrderStatus.AutoSize = true;
            this.editOrderStatus.Location = new System.Drawing.Point(19, 36);
            this.editOrderStatus.Name = "editOrderStatus";
            this.editOrderStatus.Size = new System.Drawing.Size(77, 17);
            this.editOrderStatus.TabIndex = 2;
            this.editOrderStatus.Text = "Edit status";
            this.editOrderStatus.UseVisualStyleBackColor = true;
            // 
            // viewPaymentDetails
            // 
            this.viewPaymentDetails.AutoSize = true;
            this.viewPaymentDetails.Location = new System.Drawing.Point(19, 75);
            this.viewPaymentDetails.Name = "viewPaymentDetails";
            this.viewPaymentDetails.Size = new System.Drawing.Size(172, 17);
            this.viewPaymentDetails.TabIndex = 4;
            this.viewPaymentDetails.Text = "View sensitive payment details";
            this.viewPaymentDetails.UseVisualStyleBackColor = true;
            // 
            // editOrderNotes
            // 
            this.editOrderNotes.AutoSize = true;
            this.editOrderNotes.Location = new System.Drawing.Point(19, 56);
            this.editOrderNotes.Name = "editOrderNotes";
            this.editOrderNotes.Size = new System.Drawing.Size(74, 17);
            this.editOrderNotes.TabIndex = 3;
            this.editOrderNotes.Text = "Edit notes";
            this.editOrderNotes.UseVisualStyleBackColor = true;
            // 
            // voidShipments
            // 
            this.voidShipments.AutoSize = true;
            this.voidShipments.Location = new System.Drawing.Point(19, 132);
            this.voidShipments.Name = "voidShipments";
            this.voidShipments.Size = new System.Drawing.Size(100, 17);
            this.voidShipments.TabIndex = 7;
            this.voidShipments.Text = "Void and delete";
            this.voidShipments.UseVisualStyleBackColor = true;
            // 
            // labelShipments
            // 
            this.labelShipments.AutoSize = true;
            this.labelShipments.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelShipments.Location = new System.Drawing.Point(2, 96);
            this.labelShipments.Name = "labelShipments";
            this.labelShipments.Size = new System.Drawing.Size(67, 13);
            this.labelShipments.TabIndex = 5;
            this.labelShipments.Text = "Shipments";
            // 
            // prepareShipments
            // 
            this.prepareShipments.AutoSize = true;
            this.prepareShipments.Location = new System.Drawing.Point(19, 113);
            this.prepareShipments.Name = "prepareShipments";
            this.prepareShipments.Size = new System.Drawing.Size(149, 17);
            this.prepareShipments.TabIndex = 6;
            this.prepareShipments.Text = "Create, edit, and process";
            this.prepareShipments.UseVisualStyleBackColor = true;
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelEmail.Location = new System.Drawing.Point(2, 153);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(37, 13);
            this.labelEmail.TabIndex = 8;
            this.labelEmail.Text = "Email";
            // 
            // sendEmail
            // 
            this.sendEmail.AutoSize = true;
            this.sendEmail.Location = new System.Drawing.Point(19, 171);
            this.sendEmail.Name = "sendEmail";
            this.sendEmail.Size = new System.Drawing.Size(133, 17);
            this.sendEmail.TabIndex = 9;
            this.sendEmail.Text = "Create and send email";
            this.sendEmail.UseVisualStyleBackColor = true;
            // 
            // StorePermissionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.sendEmail);
            this.Controls.Add(this.labelEmail);
            this.Controls.Add(this.labelOrders);
            this.Controls.Add(this.manageOrders);
            this.Controls.Add(this.editOrderStatus);
            this.Controls.Add(this.viewPaymentDetails);
            this.Controls.Add(this.editOrderNotes);
            this.Controls.Add(this.voidShipments);
            this.Controls.Add(this.labelShipments);
            this.Controls.Add(this.prepareShipments);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "StorePermissionsControl";
            this.Size = new System.Drawing.Size(197, 195);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelOrders;
        private System.Windows.Forms.CheckBox manageOrders;
        private System.Windows.Forms.CheckBox editOrderStatus;
        private System.Windows.Forms.CheckBox viewPaymentDetails;
        private System.Windows.Forms.CheckBox editOrderNotes;
        private System.Windows.Forms.CheckBox voidShipments;
        private System.Windows.Forms.Label labelShipments;
        private System.Windows.Forms.CheckBox prepareShipments;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.CheckBox sendEmail;
    }
}
