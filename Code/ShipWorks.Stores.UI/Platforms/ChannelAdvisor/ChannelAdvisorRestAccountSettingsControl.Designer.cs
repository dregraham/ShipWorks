namespace ShipWorks.Stores.UI.Platforms.ChannelAdvisor
{
    partial class ChannelAdvisorRestAccountSettingsControl
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
            this.restAuthHost = new System.Windows.Forms.Integration.ElementHost();
            this.channelAdvisorRestAuthorizationControl = new ShipWorks.Stores.UI.Platforms.ChannelAdvisor.ChannelAdvisorRestAuthorizationControl();
            this.SuspendLayout();
            // 
            // elementHost1
            // 
            this.restAuthHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.restAuthHost.Location = new System.Drawing.Point(0, 0);
            this.restAuthHost.Name = "elementHost1";
            this.restAuthHost.Size = new System.Drawing.Size(492, 140);
            this.restAuthHost.TabIndex = 0;
            this.restAuthHost.Text = "elementHost1";
            this.restAuthHost.Child = this.channelAdvisorRestAuthorizationControl;
            // 
            // ChannelAdvisorRestAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.restAuthHost);
            this.Name = "ChannelAdvisorRestAccountSettingsControl";
            this.Size = new System.Drawing.Size(492, 140);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost restAuthHost;
        private UI.Platforms.ChannelAdvisor.ChannelAdvisorRestAuthorizationControl channelAdvisorRestAuthorizationControl;
    }
}
