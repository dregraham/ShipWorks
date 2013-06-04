namespace ShipWorks.Stores.Platforms.Miva.WizardPages
{
    partial class MivaOptionsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MivaOptionsPage));
            this.label1 = new System.Windows.Forms.Label();
            this.labelManualOrders = new System.Windows.Forms.Label();
            this.labelManualOrderInfo = new System.Windows.Forms.Label();
            this.livaManualOrderNumbers = new System.Windows.Forms.CheckBox();
            this.sebenzaOptions = new ShipWorks.Stores.Platforms.Miva.MivaSebenzaOptionsControl();
            this.label2 = new System.Windows.Forms.Label();
            this.orderStatusControl = new ShipWorks.Stores.Platforms.Miva.MivaOrderStatusControl();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(19, 206);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Sebenza Modules";
            // 
            // labelManualOrders
            // 
            this.labelManualOrders.AutoSize = true;
            this.labelManualOrders.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelManualOrders.Location = new System.Drawing.Point(19, 12);
            this.labelManualOrders.Name = "labelManualOrders";
            this.labelManualOrders.Size = new System.Drawing.Size(89, 13);
            this.labelManualOrders.TabIndex = 0;
            this.labelManualOrders.Text = "Manual Orders";
            // 
            // labelManualOrderInfo
            // 
            this.labelManualOrderInfo.Location = new System.Drawing.Point(34, 32);
            this.labelManualOrderInfo.Name = "labelManualOrderInfo";
            this.labelManualOrderInfo.Size = new System.Drawing.Size(451, 43);
            this.labelManualOrderInfo.TabIndex = 1;
            this.labelManualOrderInfo.Text = resources.GetString("labelManualOrderInfo.Text");
            // 
            // livaManualOrderNumbers
            // 
            this.livaManualOrderNumbers.AutoSize = true;
            this.livaManualOrderNumbers.Location = new System.Drawing.Point(37, 79);
            this.livaManualOrderNumbers.Name = "livaManualOrderNumbers";
            this.livaManualOrderNumbers.Size = new System.Drawing.Size(319, 17);
            this.livaManualOrderNumbers.TabIndex = 2;
            this.livaManualOrderNumbers.Text = "Get and reserve manual order numbers from my online store.";
            this.livaManualOrderNumbers.UseVisualStyleBackColor = true;
            // 
            // sebenzaOptions
            // 
            this.sebenzaOptions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sebenzaOptions.Location = new System.Drawing.Point(37, 222);
            this.sebenzaOptions.Name = "sebenzaOptions";
            this.sebenzaOptions.Size = new System.Drawing.Size(406, 72);
            this.sebenzaOptions.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(19, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Online Order Status";
            // 
            // orderStatusControl
            // 
            this.orderStatusControl.Location = new System.Drawing.Point(22, 129);
            this.orderStatusControl.Name = "orderStatusControl";
            this.orderStatusControl.Size = new System.Drawing.Size(404, 78);
            this.orderStatusControl.TabIndex = 8;
            // 
            // MivaOptionsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.orderStatusControl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.sebenzaOptions);
            this.Controls.Add(this.livaManualOrderNumbers);
            this.Controls.Add(this.labelManualOrderInfo);
            this.Controls.Add(this.labelManualOrders);
            this.Controls.Add(this.label1);
            this.Name = "MivaOptionsPage";
            this.Size = new System.Drawing.Size(500, 276);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelManualOrders;
        private System.Windows.Forms.Label labelManualOrderInfo;
        private System.Windows.Forms.CheckBox livaManualOrderNumbers;
        private MivaSebenzaOptionsControl sebenzaOptions;
        private System.Windows.Forms.Label label2;
        private MivaOrderStatusControl orderStatusControl;
    }
}
