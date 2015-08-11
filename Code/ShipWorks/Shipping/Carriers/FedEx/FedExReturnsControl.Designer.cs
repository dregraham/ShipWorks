using ShipWorks.UI.Controls;
namespace ShipWorks.Shipping.Carriers.FedEx
{
    partial class FedExReturnsControl
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
            this.returnService = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelFedExReturnType = new System.Windows.Forms.Label();
            this.labelRmaNumber = new System.Windows.Forms.Label();
            this.rmaNumber = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.rmaReason = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelRmaReason = new System.Windows.Forms.Label();
            this.labelSaturdayPickup = new System.Windows.Forms.Label();
            this.saturdayPickup = new System.Windows.Forms.CheckBox();
            this.returnsClearance = new System.Windows.Forms.CheckBox();
            this.returnsClearanceLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // returnService
            // 
            this.returnService.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.returnService.FormattingEnabled = true;
            this.returnService.Location = new System.Drawing.Point(114, 3);
            this.returnService.Name = "returnService";
            this.returnService.PromptText = "(Multiple Values)";
            this.returnService.Size = new System.Drawing.Size(207, 21);
            this.returnService.TabIndex = 8;
            // 
            // labelFedExReturnType
            // 
            this.labelFedExReturnType.AutoSize = true;
            this.labelFedExReturnType.BackColor = System.Drawing.Color.White;
            this.labelFedExReturnType.Location = new System.Drawing.Point(3, 6);
            this.labelFedExReturnType.Name = "labelFedExReturnType";
            this.labelFedExReturnType.Size = new System.Drawing.Size(107, 13);
            this.labelFedExReturnType.TabIndex = 7;
            this.labelFedExReturnType.Text = "FedEx Return Type :";
            // 
            // labelRmaNumber
            // 
            this.labelRmaNumber.AutoSize = true;
            this.labelRmaNumber.Location = new System.Drawing.Point(37, 34);
            this.labelRmaNumber.Name = "labelRmaNumber";
            this.labelRmaNumber.Size = new System.Drawing.Size(73, 13);
            this.labelRmaNumber.TabIndex = 9;
            this.labelRmaNumber.Text = "RMA Number:";
            // 
            // rmaNumber
            // 
            this.rmaNumber.Location = new System.Drawing.Point(114, 31);
            this.rmaNumber.MaxLength = 30;
            this.rmaNumber.Name = "rmaNumber";
            this.rmaNumber.Size = new System.Drawing.Size(207, 21);
            this.rmaNumber.TabIndex = 10;
            // 
            // rmaReason
            // 
            this.rmaReason.Location = new System.Drawing.Point(114, 58);
            this.rmaReason.MaxLength = 60;
            this.rmaReason.Name = "rmaReason";
            this.rmaReason.Size = new System.Drawing.Size(207, 21);
            this.rmaReason.TabIndex = 12;
            // 
            // labelRmaReason
            // 
            this.labelRmaReason.AutoSize = true;
            this.labelRmaReason.Location = new System.Drawing.Point(38, 61);
            this.labelRmaReason.Name = "labelRmaReason";
            this.labelRmaReason.Size = new System.Drawing.Size(72, 13);
            this.labelRmaReason.TabIndex = 11;
            this.labelRmaReason.Text = "RMA Reason:";
            // 
            // labelSaturdayPickup
            // 
            this.labelSaturdayPickup.AutoSize = true;
            this.labelSaturdayPickup.Location = new System.Drawing.Point(22, 89);
            this.labelSaturdayPickup.Name = "labelSaturdayPickup";
            this.labelSaturdayPickup.Size = new System.Drawing.Size(88, 13);
            this.labelSaturdayPickup.TabIndex = 13;
            this.labelSaturdayPickup.Text = "Saturday Pickup:";
            // 
            // saturdayPickup
            // 
            this.saturdayPickup.AutoSize = true;
            this.saturdayPickup.Location = new System.Drawing.Point(114, 88);
            this.saturdayPickup.Name = "saturdayPickup";
            this.saturdayPickup.Size = new System.Drawing.Size(103, 17);
            this.saturdayPickup.TabIndex = 14;
            this.saturdayPickup.Text = "Saturday Pickup";
            this.saturdayPickup.UseVisualStyleBackColor = true;
            // 
            // returnsClearance
            // 
            this.returnsClearance.AutoSize = true;
            this.returnsClearance.Location = new System.Drawing.Point(114, 111);
            this.returnsClearance.Name = "returnsClearance";
            this.returnsClearance.Size = new System.Drawing.Size(115, 17);
            this.returnsClearance.TabIndex = 16;
            this.returnsClearance.Text = "Returns Clearance";
            this.returnsClearance.UseVisualStyleBackColor = true;
            // 
            // returnsClearanceLabel
            // 
            this.returnsClearanceLabel.AutoSize = true;
            this.returnsClearanceLabel.Location = new System.Drawing.Point(10, 111);
            this.returnsClearanceLabel.Name = "returnsClearanceLabel";
            this.returnsClearanceLabel.Size = new System.Drawing.Size(100, 13);
            this.returnsClearanceLabel.TabIndex = 15;
            this.returnsClearanceLabel.Text = "Returns Clearance:";
            // 
            // FedExReturnsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.returnsClearance);
            this.Controls.Add(this.returnsClearanceLabel);
            this.Controls.Add(this.saturdayPickup);
            this.Controls.Add(this.labelSaturdayPickup);
            this.Controls.Add(this.rmaReason);
            this.Controls.Add(this.labelRmaReason);
            this.Controls.Add(this.rmaNumber);
            this.Controls.Add(this.labelRmaNumber);
            this.Controls.Add(this.returnService);
            this.Controls.Add(this.labelFedExReturnType);
            this.Name = "FedExReturnsControl";
            this.Size = new System.Drawing.Size(325, 137);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UI.Controls.MultiValueComboBox returnService;
        private System.Windows.Forms.Label labelFedExReturnType;
        private System.Windows.Forms.Label labelRmaNumber;
        private MultiValueTextBox rmaNumber;
        private MultiValueTextBox rmaReason;
        private System.Windows.Forms.Label labelRmaReason;
        private System.Windows.Forms.Label labelSaturdayPickup;
        private System.Windows.Forms.CheckBox saturdayPickup;
        private System.Windows.Forms.CheckBox returnsClearance;
        private System.Windows.Forms.Label returnsClearanceLabel;

    }
}
