namespace ShipWorks.Shipping.Carriers.FedEx
{
    partial class FedExLtlFreightControl
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
            this.totalHandlingUnits = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelTotalHandlingUnits = new System.Windows.Forms.Label();
            this.labelRole = new System.Windows.Forms.Label();
            this.role = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelCollectTerms = new System.Windows.Forms.Label();
            this.collectTerms = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.freightClass = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelFreightClass = new System.Windows.Forms.Label();
            this.callBeforeDelivery = new System.Windows.Forms.CheckBox();
            this.freezableProtection = new System.Windows.Forms.CheckBox();
            this.limitedAccessPickup = new System.Windows.Forms.CheckBox();
            this.limitedAccessDelivery = new System.Windows.Forms.CheckBox();
            this.poison = new System.Windows.Forms.CheckBox();
            this.food = new System.Windows.Forms.CheckBox();
            this.doNotStackPallets = new System.Windows.Forms.CheckBox();
            this.doNotBreakDownPallets = new System.Windows.Forms.CheckBox();
            this.topLoad = new System.Windows.Forms.CheckBox();
            this.extremeLength = new System.Windows.Forms.CheckBox();
            this.liftgateAtDelivery = new System.Windows.Forms.CheckBox();
            this.liftgateAtPickup = new System.Windows.Forms.CheckBox();
            this.insideDelivery = new System.Windows.Forms.CheckBox();
            this.insidePickup = new System.Windows.Forms.CheckBox();
            this.freightGuarantee = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // totalHandlingUnits
            // 
            this.totalHandlingUnits.Location = new System.Drawing.Point(113, 94);
            this.totalHandlingUnits.Name = "totalHandlingUnits";
            this.totalHandlingUnits.Size = new System.Drawing.Size(51, 20);
            this.totalHandlingUnits.TabIndex = 3;
            // 
            // labelTotalHandlingUnits
            // 
            this.labelTotalHandlingUnits.AutoSize = true;
            this.labelTotalHandlingUnits.BackColor = System.Drawing.Color.White;
            this.labelTotalHandlingUnits.Location = new System.Drawing.Point(-1, 97);
            this.labelTotalHandlingUnits.Name = "labelTotalHandlingUnits";
            this.labelTotalHandlingUnits.Size = new System.Drawing.Size(106, 13);
            this.labelTotalHandlingUnits.TabIndex = 6;
            this.labelTotalHandlingUnits.Text = "Total Handling Units:";
            // 
            // labelRole
            // 
            this.labelRole.AutoSize = true;
            this.labelRole.BackColor = System.Drawing.Color.Transparent;
            this.labelRole.Location = new System.Drawing.Point(73, 6);
            this.labelRole.Name = "labelRole";
            this.labelRole.Size = new System.Drawing.Size(32, 13);
            this.labelRole.TabIndex = 26;
            this.labelRole.Text = "Role:";
            // 
            // role
            // 
            this.role.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.role.FormattingEnabled = true;
            this.role.Location = new System.Drawing.Point(113, 3);
            this.role.Name = "role";
            this.role.PromptText = "(Multiple Values)";
            this.role.Size = new System.Drawing.Size(106, 21);
            this.role.TabIndex = 1;
            // 
            // labelCollectTerms
            // 
            this.labelCollectTerms.AutoSize = true;
            this.labelCollectTerms.BackColor = System.Drawing.Color.Transparent;
            this.labelCollectTerms.Location = new System.Drawing.Point(4, 36);
            this.labelCollectTerms.Name = "labelCollectTerms";
            this.labelCollectTerms.Size = new System.Drawing.Size(101, 13);
            this.labelCollectTerms.TabIndex = 28;
            this.labelCollectTerms.Text = "Collect Terms Type:";
            // 
            // collectTerms
            // 
            this.collectTerms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.collectTerms.FormattingEnabled = true;
            this.collectTerms.Location = new System.Drawing.Point(113, 34);
            this.collectTerms.Name = "collectTerms";
            this.collectTerms.PromptText = "(Multiple Values)";
            this.collectTerms.Size = new System.Drawing.Size(106, 21);
            this.collectTerms.TabIndex = 2;
            // 
            // freightClass
            // 
            this.freightClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.freightClass.FormattingEnabled = true;
            this.freightClass.Location = new System.Drawing.Point(113, 63);
            this.freightClass.Name = "freightClass";
            this.freightClass.PromptText = "(Multiple Values)";
            this.freightClass.Size = new System.Drawing.Size(106, 21);
            this.freightClass.TabIndex = 3;
            // 
            // labelFreightClass
            // 
            this.labelFreightClass.AutoSize = true;
            this.labelFreightClass.BackColor = System.Drawing.Color.Transparent;
            this.labelFreightClass.Location = new System.Drawing.Point(35, 66);
            this.labelFreightClass.Name = "labelFreightClass";
            this.labelFreightClass.Size = new System.Drawing.Size(70, 13);
            this.labelFreightClass.TabIndex = 30;
            this.labelFreightClass.Text = "Freight Class:";
            // 
            // callBeforeDelivery
            // 
            this.callBeforeDelivery.AutoSize = true;
            this.callBeforeDelivery.Location = new System.Drawing.Point(9, 129);
            this.callBeforeDelivery.Name = "callBeforeDelivery";
            this.callBeforeDelivery.Size = new System.Drawing.Size(115, 17);
            this.callBeforeDelivery.TabIndex = 31;
            this.callBeforeDelivery.Text = "Call before delivery";
            this.callBeforeDelivery.UseVisualStyleBackColor = true;
            // 
            // freezableProtection
            // 
            this.freezableProtection.AutoSize = true;
            this.freezableProtection.Location = new System.Drawing.Point(146, 129);
            this.freezableProtection.Name = "freezableProtection";
            this.freezableProtection.Size = new System.Drawing.Size(122, 17);
            this.freezableProtection.TabIndex = 32;
            this.freezableProtection.Text = "Freezable protection";
            this.freezableProtection.UseVisualStyleBackColor = true;
            // 
            // limitedAccessPickup
            // 
            this.limitedAccessPickup.AutoSize = true;
            this.limitedAccessPickup.Location = new System.Drawing.Point(9, 152);
            this.limitedAccessPickup.Name = "limitedAccessPickup";
            this.limitedAccessPickup.Size = new System.Drawing.Size(131, 17);
            this.limitedAccessPickup.TabIndex = 33;
            this.limitedAccessPickup.Text = "Limited access pickup";
            this.limitedAccessPickup.UseVisualStyleBackColor = true;
            // 
            // limitedAccessDelivery
            // 
            this.limitedAccessDelivery.AutoSize = true;
            this.limitedAccessDelivery.Location = new System.Drawing.Point(146, 152);
            this.limitedAccessDelivery.Name = "limitedAccessDelivery";
            this.limitedAccessDelivery.Size = new System.Drawing.Size(135, 17);
            this.limitedAccessDelivery.TabIndex = 34;
            this.limitedAccessDelivery.Text = "Limited access delivery";
            this.limitedAccessDelivery.UseVisualStyleBackColor = true;
            // 
            // poison
            // 
            this.poison.AutoSize = true;
            this.poison.Location = new System.Drawing.Point(9, 176);
            this.poison.Name = "poison";
            this.poison.Size = new System.Drawing.Size(58, 17);
            this.poison.TabIndex = 35;
            this.poison.Text = "Poison";
            this.poison.UseVisualStyleBackColor = true;
            // 
            // food
            // 
            this.food.AutoSize = true;
            this.food.Location = new System.Drawing.Point(146, 176);
            this.food.Name = "food";
            this.food.Size = new System.Drawing.Size(50, 17);
            this.food.TabIndex = 36;
            this.food.Text = "Food";
            this.food.UseVisualStyleBackColor = true;
            // 
            // doNotStackPallets
            // 
            this.doNotStackPallets.AutoSize = true;
            this.doNotStackPallets.Location = new System.Drawing.Point(9, 200);
            this.doNotStackPallets.Name = "doNotStackPallets";
            this.doNotStackPallets.Size = new System.Drawing.Size(120, 17);
            this.doNotStackPallets.TabIndex = 37;
            this.doNotStackPallets.Text = "Do not stack pallets";
            this.doNotStackPallets.UseVisualStyleBackColor = true;
            // 
            // doNotBreakDownPallets
            // 
            this.doNotBreakDownPallets.AutoSize = true;
            this.doNotBreakDownPallets.Location = new System.Drawing.Point(146, 200);
            this.doNotBreakDownPallets.Name = "doNotBreakDownPallets";
            this.doNotBreakDownPallets.Size = new System.Drawing.Size(150, 17);
            this.doNotBreakDownPallets.TabIndex = 38;
            this.doNotBreakDownPallets.Text = "Do not break down pallets";
            this.doNotBreakDownPallets.UseVisualStyleBackColor = true;
            // 
            // topLoad
            // 
            this.topLoad.AutoSize = true;
            this.topLoad.Location = new System.Drawing.Point(9, 224);
            this.topLoad.Name = "topLoad";
            this.topLoad.Size = new System.Drawing.Size(68, 17);
            this.topLoad.TabIndex = 39;
            this.topLoad.Text = "Top load";
            this.topLoad.UseVisualStyleBackColor = true;
            // 
            // extremeLength
            // 
            this.extremeLength.AutoSize = true;
            this.extremeLength.Location = new System.Drawing.Point(146, 224);
            this.extremeLength.Name = "extremeLength";
            this.extremeLength.Size = new System.Drawing.Size(96, 17);
            this.extremeLength.TabIndex = 40;
            this.extremeLength.Text = "Extreme length";
            this.extremeLength.UseVisualStyleBackColor = true;
            // 
            // liftgateAtDelivery
            // 
            this.liftgateAtDelivery.AutoSize = true;
            this.liftgateAtDelivery.Location = new System.Drawing.Point(9, 248);
            this.liftgateAtDelivery.Name = "liftgateAtDelivery";
            this.liftgateAtDelivery.Size = new System.Drawing.Size(112, 17);
            this.liftgateAtDelivery.TabIndex = 41;
            this.liftgateAtDelivery.Text = "Liftgate at delivery";
            this.liftgateAtDelivery.UseVisualStyleBackColor = true;
            // 
            // liftgateAtPickup
            // 
            this.liftgateAtPickup.AutoSize = true;
            this.liftgateAtPickup.Location = new System.Drawing.Point(146, 248);
            this.liftgateAtPickup.Name = "liftgateAtPickup";
            this.liftgateAtPickup.Size = new System.Drawing.Size(108, 17);
            this.liftgateAtPickup.TabIndex = 42;
            this.liftgateAtPickup.Text = "Liftgate at pickup";
            this.liftgateAtPickup.UseVisualStyleBackColor = true;
            // 
            // insideDelivery
            // 
            this.insideDelivery.AutoSize = true;
            this.insideDelivery.Location = new System.Drawing.Point(9, 272);
            this.insideDelivery.Name = "insideDelivery";
            this.insideDelivery.Size = new System.Drawing.Size(93, 17);
            this.insideDelivery.TabIndex = 43;
            this.insideDelivery.Text = "Inside delivery";
            this.insideDelivery.UseVisualStyleBackColor = true;
            // 
            // insidePickup
            // 
            this.insidePickup.AutoSize = true;
            this.insidePickup.Location = new System.Drawing.Point(146, 272);
            this.insidePickup.Name = "insidePickup";
            this.insidePickup.Size = new System.Drawing.Size(89, 17);
            this.insidePickup.TabIndex = 44;
            this.insidePickup.Text = "Inside pickup";
            this.insidePickup.UseVisualStyleBackColor = true;
            // 
            // freightGuarantee
            // 
            this.freightGuarantee.AutoSize = true;
            this.freightGuarantee.Location = new System.Drawing.Point(9, 296);
            this.freightGuarantee.Name = "freightGuarantee";
            this.freightGuarantee.Size = new System.Drawing.Size(109, 17);
            this.freightGuarantee.TabIndex = 45;
            this.freightGuarantee.Text = "Freight guarantee";
            this.freightGuarantee.UseVisualStyleBackColor = true;
            // 
            // FedExLtlFreightControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.freightGuarantee);
            this.Controls.Add(this.insidePickup);
            this.Controls.Add(this.insideDelivery);
            this.Controls.Add(this.liftgateAtPickup);
            this.Controls.Add(this.liftgateAtDelivery);
            this.Controls.Add(this.extremeLength);
            this.Controls.Add(this.topLoad);
            this.Controls.Add(this.doNotBreakDownPallets);
            this.Controls.Add(this.doNotStackPallets);
            this.Controls.Add(this.food);
            this.Controls.Add(this.poison);
            this.Controls.Add(this.limitedAccessDelivery);
            this.Controls.Add(this.limitedAccessPickup);
            this.Controls.Add(this.freezableProtection);
            this.Controls.Add(this.callBeforeDelivery);
            this.Controls.Add(this.freightClass);
            this.Controls.Add(this.labelFreightClass);
            this.Controls.Add(this.collectTerms);
            this.Controls.Add(this.labelCollectTerms);
            this.Controls.Add(this.role);
            this.Controls.Add(this.labelRole);
            this.Controls.Add(this.totalHandlingUnits);
            this.Controls.Add(this.labelTotalHandlingUnits);
            this.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.Name = "FedExLtlFreightControl";
            this.Size = new System.Drawing.Size(487, 319);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private UI.Controls.MultiValueTextBox totalHandlingUnits;
        private System.Windows.Forms.Label labelTotalHandlingUnits;
        private System.Windows.Forms.Label labelRole;
        private UI.Controls.MultiValueComboBox role;
        private System.Windows.Forms.Label labelCollectTerms;
        private UI.Controls.MultiValueComboBox collectTerms;
        private UI.Controls.MultiValueComboBox freightClass;
        private System.Windows.Forms.Label labelFreightClass;
        private System.Windows.Forms.CheckBox callBeforeDelivery;
        private System.Windows.Forms.CheckBox freezableProtection;
        private System.Windows.Forms.CheckBox limitedAccessPickup;
        private System.Windows.Forms.CheckBox limitedAccessDelivery;
        private System.Windows.Forms.CheckBox poison;
        private System.Windows.Forms.CheckBox food;
        private System.Windows.Forms.CheckBox doNotStackPallets;
        private System.Windows.Forms.CheckBox doNotBreakDownPallets;
        private System.Windows.Forms.CheckBox topLoad;
        private System.Windows.Forms.CheckBox extremeLength;
        private System.Windows.Forms.CheckBox liftgateAtDelivery;
        private System.Windows.Forms.CheckBox liftgateAtPickup;
        private System.Windows.Forms.CheckBox insideDelivery;
        private System.Windows.Forms.CheckBox insidePickup;
        private System.Windows.Forms.CheckBox freightGuarantee;
    }
}
