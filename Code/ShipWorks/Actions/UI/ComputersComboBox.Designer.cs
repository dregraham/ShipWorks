using System.Windows.Forms;

namespace ShipWorks.Actions.UI
{
    partial class ComputersComboBox
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
            this.checkBoxPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // checkBoxPanel
            // 
            this.checkBoxPanel.AutoScroll = true;
            this.checkBoxPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.checkBoxPanel.Location = new System.Drawing.Point(0, 0);
            this.checkBoxPanel.Name = "checkBoxPanel";
            this.checkBoxPanel.Size = new System.Drawing.Size(130, 306);
            this.checkBoxPanel.TabIndex = 0;
            this.checkBoxPanel.Visible = false;
            this.checkBoxPanel.WrapContents = false;
            // 
            // ComputersComboBox
            // 
            this.Controls.Add(this.checkBoxPanel);
            this.Name = "ComputerComboPopup";
            this.Size = new System.Drawing.Size(316, 21);
            this.ResumeLayout(false);

        }

        #endregion

        FlowLayoutPanel checkBoxPanel;
    }
}
