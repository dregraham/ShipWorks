using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    partial class FedExProfilePackagePriorityAlertsControl
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
            this.labelPriorityAlert = new System.Windows.Forms.Label();
            this.lalbelPriorityAlertContentDetail = new System.Windows.Forms.Label();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.priorityAlertContentDetail = new System.Windows.Forms.TextBox();
            this.priorityAlertEnhancementType = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // labelPriorityAlert
            // 
            this.labelPriorityAlert.AutoSize = true;
            this.labelPriorityAlert.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPriorityAlert.Location = new System.Drawing.Point(7, 6);
            this.labelPriorityAlert.Name = "labelPriorityAlert";
            this.labelPriorityAlert.Size = new System.Drawing.Size(71, 13);
            this.labelPriorityAlert.TabIndex = 2;
            this.labelPriorityAlert.Text = "Priority Alert:";
            this.labelPriorityAlert.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lalbelPriorityAlertContentDetail
            // 
            this.lalbelPriorityAlertContentDetail.Location = new System.Drawing.Point(10, 30);
            this.lalbelPriorityAlertContentDetail.Name = "lalbelPriorityAlertContentDetail";
            this.lalbelPriorityAlertContentDetail.Size = new System.Drawing.Size(68, 37);
            this.lalbelPriorityAlertContentDetail.TabIndex = 3;
            this.lalbelPriorityAlertContentDetail.Text = "Additional alert details:";
            this.lalbelPriorityAlertContentDetail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // priorityAlertContentDetail
            // 
            this.priorityAlertContentDetail.Location = new System.Drawing.Point(84, 30);
            this.fieldLengthProvider.SetMaxLengthSource(this.priorityAlertContentDetail, ShipWorks.Data.Utility.EntityFieldLengthSource.FedExPackagePriorityAlertContentDetail);
            this.priorityAlertContentDetail.Multiline = true;
            this.priorityAlertContentDetail.Name = "priorityAlertContentDetail";
            this.priorityAlertContentDetail.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.priorityAlertContentDetail.Size = new System.Drawing.Size(211, 95);
            this.priorityAlertContentDetail.TabIndex = 1;
            // 
            // priorityAlertEnhancementType
            // 
            this.priorityAlertEnhancementType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.priorityAlertEnhancementType.FormattingEnabled = true;
            this.priorityAlertEnhancementType.Location = new System.Drawing.Point(84, 3);
            this.priorityAlertEnhancementType.Name = "priorityAlertEnhancementType";
            this.priorityAlertEnhancementType.Size = new System.Drawing.Size(211, 21);
            this.priorityAlertEnhancementType.TabIndex = 0;
            // 
            // FedExProfilePackagePriorityAlertsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.priorityAlertEnhancementType);
            this.Controls.Add(this.priorityAlertContentDetail);
            this.Controls.Add(this.lalbelPriorityAlertContentDetail);
            this.Controls.Add(this.labelPriorityAlert);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FedExProfilePackagePriorityAlertsControl";
            this.Size = new System.Drawing.Size(309, 138);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private System.Windows.Forms.Label labelPriorityAlert;
        private System.Windows.Forms.Label lalbelPriorityAlertContentDetail;
        private System.Windows.Forms.TextBox priorityAlertContentDetail;
        private System.Windows.Forms.ComboBox priorityAlertEnhancementType;
    }
}
