using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ShipWorks.ApplicationCore.Dashboard
{
    /// <summary>
    /// A single line in the dashboard area that gets displayed in the top of the MainForm.
    /// </summary>
    [ToolboxItem(false)]
    public partial class DashboardBar : UserControl
    {
        public event EventHandler Dismissed;

        /// <summary>
        /// Constructor
        /// </summary>
        public DashboardBar()
        {
            InitializeComponent();

            this.Dock = DockStyle.Top;
        }

        /// <summary>
        /// The icon image displayed before the primary text
        /// </summary>
        public Image Image
        {
            get
            {
                return image.Image;
            }
            set
            {
                image.Image = value;
            }
        }

        /// <summary>
        /// The primary, short, bold text displayed.
        /// </summary>
        public string PrimaryText
        {
            get
            {
                return labelPrimary.Text;
            }
            set
            {
                labelPrimary.Text = value;

                UpdatePositions();
            }
        }

        /// <summary>
        /// The secondary longer text displayed next to the primary.
        /// </summary>
        public string SecondaryText
        {
            get
            {
                return labelSecondary.Text;
            }
            set
            {
                labelSecondary.Text = value;

                UpdatePositions();
            }
        }

        /// <summary>
        /// Apply the given list of actions to the dashboard bar.  Any previous actions are cleared.
        /// </summary>
        public void ApplyActions(ICollection<DashboardAction> actions)
        {
            panelActions.Controls.Clear();

            // Add each action as links
            foreach (DashboardAction action in actions)
            {
                if (action.LinkArea.IsEmpty)
                {
                    AddActionLabel(action.Text);
                }
                else
                {
                    // If there is any text before the link, add it as a label
                    if (action.LinkArea.Start > 0)
                    {
                        AddActionLabel(action.Text.Substring(0, action.LinkArea.Start));
                    }

                    // Add the link
                    AddActionLink(action.Text.Substring(action.LinkArea.Start, action.LinkArea.Length), action);

                    int indexAfterLink = action.LinkArea.Start + action.LinkArea.Length;

                    // Any text after the link, add as a label
                    if (indexAfterLink < action.Text.Length)
                    {
                        AddActionLabel(action.Text.Substring(indexAfterLink));
                    }
                }
            }

            UpdatePositions();
        }

        /// <summary>
        /// Add a link that executes the specified action
        /// </summary>
        private void AddActionLink(string text, DashboardAction action)
        {
            Label label = CreateActionLabel(text);

            label.ForeColor = linkDismiss.ForeColor;
            label.Font = linkDismiss.Font;
            label.Cursor = Cursors.Hand;

            label.Tag = action;
            label.Click += new EventHandler(OnClickActionLink);

            panelActions.Controls.Add(label);
            label.BringToFront();
        }

        /// <summary>
        /// Add a label to the action area
        /// </summary>
        private void AddActionLabel(string text)
        {
            Label label = CreateActionLabel(text);
            label.ForeColor = labelSecondary.ForeColor;

            panelActions.Controls.Add(label);
            label.BringToFront();
        }

        /// <summary>
        /// Action link has been clicked.
        /// </summary>
        void OnClickActionLink(object sender, EventArgs e)
        {
            Label label = (Label) sender;
            DashboardAction action = (DashboardAction) label.Tag;

            action.Execute(this);
        }

        /// <summary>
        /// Create a basic action label with the specified text
        /// </summary>
        private Label CreateActionLabel(string text)
        {
            Label label = new Label();
            label.BackColor = Color.Transparent;
            label.Text = text.Trim();
            label.Top = 2;
            label.Padding = new Padding(0);
            label.Margin = new Padding(0);
            label.AutoSize = true;
            label.Left = GetNextActionPosition();

            return label;
        }

        /// <summary>
        /// Get the coordinate of the next action item.
        /// </summary>
        private int GetNextActionPosition()
        {
            if (panelActions.Controls.Count == 0)
            {
                return 0;
            }

            return panelActions.Controls[0].Right - 4;
        }

        /// <summary>
        /// Indicates if the dismiss link is visibile
        /// </summary>
        public bool CanUserDismiss
        {
            get
            {
                return linkDismiss.Visible;
            }
            set
            {
                linkDismiss.Visible = value;
            }
        }

        /// <summary>
        /// Dismiss the dashboard bar
        /// </summary>
        public void Dismiss() =>
            OnDismiss(this, EventArgs.Empty);

        /// <summary>
        /// Update the positions of all the controls
        /// </summary>
        private void UpdatePositions()
        {
            labelSecondary.Left = labelPrimary.Right - 2;
            panelActions.Left = labelSecondary.Right;
        }

        /// <summary>
        /// User has clicked to dismiss this dashboard item
        /// </summary>
        private void OnDismiss(object sender, EventArgs e) =>
            Dismissed?.Invoke(this, EventArgs.Empty);
    }
}
