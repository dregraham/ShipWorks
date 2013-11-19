using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    partial class FedExPackagePriorityAlertsControl
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
            this.priorityAlertContentDetail = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.priorityAlertEnhancementType = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // labelPriorityAlert
            // 
            this.labelPriorityAlert.AutoSize = true;
            this.labelPriorityAlert.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPriorityAlert.Location = new System.Drawing.Point(6, 6);
            this.labelPriorityAlert.Name = "labelPriorityAlert";
            this.labelPriorityAlert.Size = new System.Drawing.Size(104, 13);
            this.labelPriorityAlert.TabIndex = 0;
            this.labelPriorityAlert.Text = "FedEx Priority Alert:";
            this.labelPriorityAlert.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lalbelPriorityAlertContentDetail
            // 
            this.lalbelPriorityAlertContentDetail.Location = new System.Drawing.Point(40, 30);
            this.lalbelPriorityAlertContentDetail.Name = "lalbelPriorityAlertContentDetail";
            this.lalbelPriorityAlertContentDetail.Size = new System.Drawing.Size(68, 37);
            this.lalbelPriorityAlertContentDetail.TabIndex = 2;
            this.lalbelPriorityAlertContentDetail.Text = "Additional alert details:";
            this.lalbelPriorityAlertContentDetail.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // priorityAlertContentDetail
            // 
            this.priorityAlertContentDetail.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.priorityAlertContentDetail.ForeColor = System.Drawing.SystemColors.WindowText;
            this.priorityAlertContentDetail.Location = new System.Drawing.Point(114, 30);
            this.fieldLengthProvider.SetMaxLengthSource(this.priorityAlertContentDetail, ShipWorks.Data.Utility.EntityFieldLengthSource.FedExPackagePriorityAlertContentDetail);
            this.priorityAlertContentDetail.Multiline = true;
            this.priorityAlertContentDetail.Name = "priorityAlertContentDetail";
            this.priorityAlertContentDetail.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.priorityAlertContentDetail.Size = new System.Drawing.Size(211, 95);
            this.priorityAlertContentDetail.TabIndex = 3;
            // 
            // priorityAlertEnhancementType
            // 
            this.priorityAlertEnhancementType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.priorityAlertEnhancementType.FormattingEnabled = true;
            this.priorityAlertEnhancementType.Location = new System.Drawing.Point(114, 3);
            this.priorityAlertEnhancementType.Name = "priorityAlertEnhancementType";
            this.priorityAlertEnhancementType.PromptText = "(Multiple Values)";
            this.priorityAlertEnhancementType.Size = new System.Drawing.Size(211, 21);
            this.priorityAlertEnhancementType.TabIndex = 1;
            this.priorityAlertEnhancementType.SelectedIndexChanged += new System.EventHandler(this.OnPriorityAlertEnhancementTypeChanged);
            // 
            // FedExPackagePriorityAlertsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lalbelPriorityAlertContentDetail);
            this.Controls.Add(this.priorityAlertContentDetail);
            this.Controls.Add(this.priorityAlertEnhancementType);
            this.Controls.Add(this.labelPriorityAlert);
            this.Name = "FedExPackagePriorityAlertsControl";
            this.Size = new System.Drawing.Size(334, 138);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private System.Windows.Forms.Label labelPriorityAlert;
        private UI.Controls.MultiValueComboBox priorityAlertEnhancementType;
        private UI.Controls.MultiValueTextBox priorityAlertContentDetail;
        private System.Windows.Forms.Label lalbelPriorityAlertContentDetail;
    }
}
