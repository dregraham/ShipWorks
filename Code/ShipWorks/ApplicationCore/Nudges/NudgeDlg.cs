using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Nudges.Buttons;

namespace ShipWorks.ApplicationCore.Nudges
{
    /// <summary>
    /// A dialog for displaying nudges where the content is obtained by displaying the content
    /// of a nudge's content URI.
    /// </summary>
    public partial class NudgeDlg : Form
    {
        private readonly Nudge nudge;

        /// <summary>
        /// Initializes a new instance of the <see cref="NudgeDlg"/> class.
        /// </summary>
        /// <param name="nudge">The nudge.</param>
        public NudgeDlg(Nudge nudge)
        {
            InitializeComponent();

            this.nudge = nudge;

            if (!nudge.ContentDimensions.IsEmpty)
            {
                // Use the dimensions of the nudge to set the size of the dialog (with some padding)
                // to account for the location of the browser control and the other controls within 
                // the dialog
                Width = nudge.ContentDimensions.Width + 35;
                Height = nudge.ContentDimensions.Height + 95;
            }

        }

        /// <summary>
        /// Called when the form is loaded.
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            // Default the dialog's title to ShipWorks if a name was not provided
            this.Text = string.IsNullOrWhiteSpace(nudge.Name) ? "ShipWorks" : nudge.Name;

            browser.Navigate(nudge.ContentUri);
            AddButtons();
        }

        /// <summary>
        /// Adds the nudge's buttons to the option panel.
        /// </summary>
        private void AddButtons()
        {
            IEnumerable<NudgeOptionButton> nudgeButtons = nudge.CreateButtons();

            // Note the width of the widest button
            int maxWidth = nudgeButtons.ToList().Max(b => b.Width);

            // The option panel has a flow direction of right to left to have them
            // be aligned along the right side of the dialog, so add the 
            // buttons starting in reverse order
            foreach (NudgeOptionButton button in nudgeButtons.Reverse())
            {
                // Make all the buttons the same width as the widest button
                button.Width = maxWidth;
                optionPanel.Controls.Add(button);
            }
        }
    }
}
