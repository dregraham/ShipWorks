namespace ShipWorks.Stores.Platforms.Miva
{
    partial class MivaManualOrderSettingsControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MivaManualOrderSettingsControl));
            this.radioLive = new System.Windows.Forms.RadioButton();
            this.radioLocal = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // labelInfo1
            // 
            this.labelInfo1.Text = resources.GetString("labelInfo1.Text");
            // 
            // labelInfo2
            // 
            this.labelInfo2.Location = new System.Drawing.Point(30, 100);
            this.labelInfo2.TabIndex = 3;
            // 
            // labelPrefix
            // 
            this.labelPrefix.Location = new System.Drawing.Point(63, 137);
            this.labelPrefix.TabIndex = 4;
            // 
            // labelPostfix
            // 
            this.labelPostfix.Location = new System.Drawing.Point(58, 162);
            this.labelPostfix.TabIndex = 6;
            // 
            // prefix
            // 
            this.prefix.Location = new System.Drawing.Point(108, 134);
            // 
            // postfix
            // 
            this.postfix.Location = new System.Drawing.Point(108, 159);
            this.postfix.TabIndex = 7;
            // 
            // labelExample
            // 
            this.labelExample.Location = new System.Drawing.Point(51, 187);
            this.labelExample.TabIndex = 8;
            // 
            // example
            // 
            this.example.Location = new System.Drawing.Point(108, 184);
            this.example.TabIndex = 9;
            // 
            // radioLive
            // 
            this.radioLive.AutoSize = true;
            this.radioLive.Location = new System.Drawing.Point(13, 50);
            this.radioLive.Name = "radioLive";
            this.radioLive.Size = new System.Drawing.Size(391, 17);
            this.radioLive.TabIndex = 1;
            this.radioLive.TabStop = true;
            this.radioLive.Text = "Get and reserve manual order numbers from my online Miva Merchant store.";
            this.radioLive.UseVisualStyleBackColor = true;
            this.radioLive.CheckedChanged += new System.EventHandler(this.OnOptionChanged);
            // 
            // radioLocal
            // 
            this.radioLocal.AutoSize = true;
            this.radioLocal.Location = new System.Drawing.Point(13, 75);
            this.radioLocal.Name = "radioLocal";
            this.radioLocal.Size = new System.Drawing.Size(221, 17);
            this.radioLocal.TabIndex = 2;
            this.radioLocal.TabStop = true;
            this.radioLocal.Text = "Use the next highest local order number.";
            this.radioLocal.UseVisualStyleBackColor = true;
            this.radioLocal.CheckedChanged += new System.EventHandler(this.OnOptionChanged);
            // 
            // MivaManualOrderSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radioLive);
            this.Controls.Add(this.radioLocal);
            this.Name = "MivaManualOrderSettingsControl";
            this.Size = new System.Drawing.Size(481, 210);
            this.Controls.SetChildIndex(this.radioLocal, 0);
            this.Controls.SetChildIndex(this.radioLive, 0);
            this.Controls.SetChildIndex(this.labelInfo1, 0);
            this.Controls.SetChildIndex(this.labelInfo2, 0);
            this.Controls.SetChildIndex(this.labelPrefix, 0);
            this.Controls.SetChildIndex(this.labelPostfix, 0);
            this.Controls.SetChildIndex(this.prefix, 0);
            this.Controls.SetChildIndex(this.postfix, 0);
            this.Controls.SetChildIndex(this.labelExample, 0);
            this.Controls.SetChildIndex(this.example, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioLive;
        private System.Windows.Forms.RadioButton radioLocal;
    }
}
