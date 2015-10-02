using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    partial class FimsOptionsControl
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
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory1 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            this.airWaybillLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.airWaybill = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.referenceCustomer = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // airWaybillLabel
            // 
            this.airWaybillLabel.AutoSize = true;
            this.airWaybillLabel.Location = new System.Drawing.Point(14, 6);
            this.airWaybillLabel.Name = "airWaybillLabel";
            this.airWaybillLabel.Size = new System.Drawing.Size(59, 13);
            this.airWaybillLabel.TabIndex = 0;
            this.airWaybillLabel.Text = "Air Waybill:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Reference #:";
            // 
            // airWaybill
            // 
            this.airWaybill.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.airWaybill.Location = new System.Drawing.Point(79, 3);
            this.fieldLengthProvider.SetMaxLengthSource(this.airWaybill, ShipWorks.Data.Utility.EntityFieldLengthSource.FedExShipmentFimsAirWaybill);
            this.airWaybill.Name = "airWaybill";
            this.airWaybill.Size = new System.Drawing.Size(197, 20);
            this.airWaybill.TabIndex = 1;
            // 
            // referenceCustomer
            // 
            this.referenceCustomer.Location = new System.Drawing.Point(79, 32);
            this.referenceCustomer.MaxLength = 32767;
            this.fieldLengthProvider.SetMaxLengthSource(this.referenceCustomer, ShipWorks.Data.Utility.EntityFieldLengthSource.FedExReferenceCustomer);
            this.referenceCustomer.Name = "referenceCustomer";
            this.referenceCustomer.Size = new System.Drawing.Size(197, 21);
            this.referenceCustomer.TabIndex = 4;
            this.referenceCustomer.TokenSuggestionFactory = commonTokenSuggestionsFactory1;
            // 
            // FimsOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.referenceCustomer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.airWaybill);
            this.Controls.Add(this.airWaybillLabel);
            this.Name = "FimsOptionsControl";
            this.Size = new System.Drawing.Size(281, 58);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label airWaybillLabel;
        private MultiValueTextBox airWaybill;
        private EntityFieldLengthProvider fieldLengthProvider;
        private System.Windows.Forms.Label label1;
        private Templates.Tokens.TemplateTokenTextBox referenceCustomer;
    }
}
