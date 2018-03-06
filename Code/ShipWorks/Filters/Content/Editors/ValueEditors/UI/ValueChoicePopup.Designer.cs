using System.Drawing;
using System.Windows.Forms;

namespace ShipWorks.Filters.Content.Editors.ValueEditors.UI
{
    public partial class ValueChoicePopup<T>
    {
        /// <summary>
        /// Initialize Component
        /// </summary>
        private void InitializeComponent()
        {
            this.statusPanel = new Panel();
            this.selectAllLink = new Label();
            this.needsAttentionLabel = new Label();
            this.selectNoneLink = new Label();
            this.statusPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // statusPanel
            //
            this.statusPanel.BackColor = SystemColors.ControlLightLight;
            this.statusPanel.Controls.Add(this.selectAllLink);
            this.statusPanel.Controls.Add(this.needsAttentionLabel);
            this.statusPanel.Controls.Add(this.selectNoneLink);
            this.statusPanel.Location = new Point(0, 0);
            this.statusPanel.Name = "statusPanel";
            this.statusPanel.Size = new Size(130, 306);
            this.statusPanel.TabIndex = 7;
            this.statusPanel.Visible = false;
            this.statusPanel.AutoScroll = true;
            //
            // selectAllLink
            //
            this.selectAllLink.AutoSize = true;
            this.selectAllLink.Cursor = Cursors.Hand;
            this.selectAllLink.Font = new Font("Tahoma", 8.25F, FontStyle.Underline, GraphicsUnit.Point, ((byte) (0)));
            this.selectAllLink.ForeColor = Color.Blue;
            this.selectAllLink.Location = new Point(3, 3);
            this.selectAllLink.Name = "selectAllLink";
            this.selectAllLink.Size = new Size(101, 13);
            this.selectAllLink.TabIndex = 1;
            this.selectAllLink.Text = "Select All";
            this.selectAllLink.Click += OnReadyToGoLabelClicked;
            //
            // selectNoneLink
            //
            this.selectNoneLink.AutoSize = true;
            this.selectNoneLink.Cursor = Cursors.Hand;
            this.selectNoneLink.Font = new Font("Tahoma", 8.25F, FontStyle.Underline, GraphicsUnit.Point, ((byte) (0)));
            this.selectNoneLink.ForeColor = Color.Blue;
            this.selectNoneLink.Location = new Point(3, 26);
            this.selectNoneLink.Name = "selectNoneLink";
            this.selectNoneLink.Size = new Size(103, 13);
            this.selectNoneLink.TabIndex = 1;
            this.selectNoneLink.Text = "Select None";
            this.selectNoneLink.Click += OnNotValidatedClicked;
            //
            // AddressValidationStatusPopup
            //
            this.Controls.Add(this.statusPanel);
            this.Size = new Size(316, 21);
            this.statusPanel.ResumeLayout(false);
            this.statusPanel.PerformLayout();
            this.ResumeLayout(false);
        }

        private bool ignoreChanged;
        private Panel statusPanel;
        private Label selectAllLink;
        private Label needsAttentionLabel;
        private Label selectNoneLink;
    }
}
